// ------------------------------------------------------------------------
//    YATSE 2 - A touch screen remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2010  Tolriq (http://yatse.leetzone.org)
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Plugin;

namespace Remote.XBMC.Camelot.XbmcHttpApi
{
    class XbmcHttpFile : IApiFile
    {
        private readonly XbmcHttp _parent;
        private const int DownloadTimeout = 30000;
        private bool _isDownloading;

        private readonly BackgroundWorker _workerDownloads;
        private readonly BackgroundWorker _workerDownload;        

        private class DownloadFileInfo
        {
            public string Filename { get; set; }
            public string Destination { get; set; }
        }

        public void StopAsync()
        {
            _parent.Log("Cancelling downloads");
            _workerDownloads.CancelAsync();
            _workerDownload.CancelAsync();
        }

        public XbmcHttpFile(XbmcHttp parent)
        {
            _parent = parent;
            _workerDownloads = new BackgroundWorker { WorkerSupportsCancellation = true };
            _workerDownload = new BackgroundWorker { WorkerSupportsCancellation = true };
        }

        public bool AsyncDownloadFinished()
        {
            return ! _isDownloading;
        }

        public void AsyncDownloadImages(ApiImageDownloadInfo[] apiImageDownloadInfos)
        {
            if (apiImageDownloadInfos.Length < 1)
                return;

            _isDownloading = true;
            var bw = new BackgroundWorker();
            bw.DoWork += AsyncImagesDownloadsWorker;
            bw.RunWorkerAsync(apiImageDownloadInfos);
        }

        public bool DownloadImages(ApiImageDownloadInfo apiImageDownloadInfo)
        {
            var res = Download(apiImageDownloadInfo.Source, apiImageDownloadInfo.Destination);
            if (res)
            {
                if (apiImageDownloadInfo.ToThumb)
                    _parent.GenerateThumb(apiImageDownloadInfo.Destination, apiImageDownloadInfo.Destination, apiImageDownloadInfo.MaxHeight);
            }
            return res;
        }

        private void AsyncImagesDownloadsWorker(object sender, DoWorkEventArgs e)
        {
            var downloadsFileInfo = (ApiImageDownloadInfo[])e.Argument;
            foreach (var downloadFileInfo in downloadsFileInfo)
            {
                if (((BackgroundWorker)sender).CancellationPending)
                {
                    _isDownloading = false;
                    e.Cancel = true;
                    return;
                }
                var res = Download(downloadFileInfo.Source, downloadFileInfo.Destination);
                if (res)
                {
                    if (downloadFileInfo.ToThumb)
                        _parent.GenerateThumb(downloadFileInfo.Destination, downloadFileInfo.Destination, downloadFileInfo.MaxHeight);
                }
            }
            _isDownloading = false;
        }

        private static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(byteArray, 0, byteArray.Length);
            }
        }

        public void AsyncDownloads(string[] fileNames, string[] destinations)
        {
            if (fileNames == null || destinations == null)
                return;
            if (fileNames.Length != destinations.Length) return;
            _isDownloading = true;
            var downloadsFileInfo = fileNames.Select((filename, index) => new DownloadFileInfo {Filename = filename, Destination = destinations[index]}).ToList();
            _workerDownloads.CancelAsync();
            _workerDownloads.DoWork += AsyncDownloadsWorker;
            _workerDownloads.RunWorkerAsync(downloadsFileInfo);
        }

        private void AsyncDownloadsWorker(object sender, DoWorkEventArgs e)
        {
            var downloadsFileInfo = (List<DownloadFileInfo>)e.Argument;
            foreach (var downloadFileInfo in downloadsFileInfo)
            {
                if (((BackgroundWorker)sender).CancellationPending)
                {
                    e.Cancel = true;
                    _isDownloading = false;
                    return;
                }
                var fileContent = Download(downloadFileInfo.Filename);
                if (fileContent == null) continue;
                ByteArrayToFile(downloadFileInfo.Destination, fileContent);
            }
            _isDownloading = false;
        }



        public void AsyncDownload(string fileName, string destination)
        {
            if (fileName == null || destination == null)
                return;
            _isDownloading = true;
            var downloadFileInfo = new DownloadFileInfo {Filename = fileName, Destination = destination};
            _workerDownload.CancelAsync();
            _workerDownload.DoWork += AsyncDownloadWorker;
            _workerDownload.RunWorkerAsync(downloadFileInfo);
        }

        private void AsyncDownloadWorker(object sender, DoWorkEventArgs e)
        {
            if (((BackgroundWorker)sender).CancellationPending)
            {
                e.Cancel = true;
                _isDownloading = false;
                return;
            }
            var downloadFileInfo = (DownloadFileInfo) e.Argument;
            var fileContent = Download(downloadFileInfo.Filename);
            if (fileContent == null) return;
            ByteArrayToFile(downloadFileInfo.Destination, fileContent);
            _isDownloading = false;
        }

        public bool Download(string fileName, string destination)
        {
            var fileContent = Download(fileName);
            if (fileContent == null) return false;
            ByteArrayToFile(destination, fileContent);
            return true;
        }

        public byte[] Download(string fileName)
        {
            var fileExist = _parent.Command("FileExists", fileName);
            if (fileExist == null) return null;
            if (fileExist[0] != "True") return null;

            HttpWebRequest request;
            byte[] fileContent = null;
            var credentials = _parent.GetCredentials();

            var uri = _parent.GetApiPath() + @"?command=FileDownload(" + fileName + @")";
            
            try
            {
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.Timeout = DownloadTimeout;
                if (credentials != null)
                    request.Credentials = credentials;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        try
                        {
                            fileContent =
                                Convert.FromBase64String(
                                    reader.ReadToEnd().Replace("<html>\n", "").Replace("\n</html>", "").Replace(
                                        "<html>", "")
                                        .Replace("</html>", ""));
                        }
                        catch(FormatException)
                        {
                            return null;
                        }
                    }
                }
                _parent.Log("DOWNLOAD : " + fileName);
            }
            catch (WebException e)
            {
                _parent.Log("ERROR - DOWNLOAD : " + fileName + " - " + e.Message);
            }
            
            return fileContent;
        }

        public void Dispose()
        {
            _workerDownload.CancelAsync();
            _workerDownload.Dispose();
            _workerDownloads.CancelAsync();
            _workerDownloads.Dispose();
        }
    }
}
