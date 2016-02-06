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
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Plugin;

namespace Remote.XBMC.Camelot.Api
{
    class XbmcFile : IApiFile
    {
        private readonly Xbmc _parent;
        private const int DownloadTimeout = 30000;
        private bool _isDownloading;

        private readonly BackgroundWorker _workerDownloads;

        public void StopAsync()
        {
            _parent.Log("Cancelling downloads");
            _workerDownloads.CancelAsync();
        }

        public XbmcFile(Xbmc parent)
        {
            _parent = parent;
            _workerDownloads = new BackgroundWorker { WorkerSupportsCancellation = true };
            _workerDownloads.DoWork += AsyncImagesDownloadsWorker;
        }

        public bool AsyncDownloadFinished()
        {
            return ! _isDownloading;
        }

        public void AsyncDownloadImages(ApiImageDownloadInfo[] apiImageDownloadInfos)
        {
            if (apiImageDownloadInfos == null)
                return;
            if (apiImageDownloadInfos.Length < 1)
                return;

            _workerDownloads.CancelAsync();
            while (_workerDownloads.IsBusy)
            {
                Thread.Sleep(50);
                System.Windows.Forms.Application.DoEvents();
            }

            _isDownloading = true;
            _workerDownloads.RunWorkerAsync(apiImageDownloadInfos);
        }

        public bool DownloadImages(ApiImageDownloadInfo apiImageDownloadInfo)
        {
            if (apiImageDownloadInfo == null)
                return false;
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
                DownloadImages(downloadFileInfo);
                Thread.Sleep(200);
            }
            _isDownloading = false;
        }

        private static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(byteArray, 0, byteArray.Length);
                }
            }
            catch (Exception)
            {
            }

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
            /*var fileExist = _parent.Command("FileExists", fileName);
            if (fileExist == null) return null;
            if (fileExist[0] != "True") return null;*/
            if (!_parent.IsConnected())
                return null;

            HttpWebRequest request;
            byte[] fileContent = null;
            var credentials = _parent.GetCredentials();

            var uri = _parent.GetApiPath() + @"?command=FileDownload(" + Uri.EscapeDataString(fileName) + @";bare)";
            
            try
            {
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.Timeout = DownloadTimeout;
                if (credentials != null)
                    request.Credentials = credentials;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var respstream = response.GetResponseStream();
                    if (respstream != null)
                        using (var reader = new StreamReader(respstream, Encoding.UTF8))
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
            _workerDownloads.CancelAsync();
            _workerDownloads.Dispose();
        }
    }
}
