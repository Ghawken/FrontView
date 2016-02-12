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
using System.Globalization;
using System.IO;
using System.Threading;
using Plugin;

namespace Remote.MediaPortal.iPimp.Api
{
    class MediaPortalFile : IApiFile
    {
        private readonly MediaPortal _parent;
        private bool _isDownloading;

        private readonly BackgroundWorker _workerDownloads;

        public void StopAsync()
        {
            _parent.Log("Cancelling downloads");
            _workerDownloads.CancelAsync();
        }

        public MediaPortalFile(MediaPortal parent)
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
                Thread.Sleep(100);
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

        public bool Download(string fileName, string destination)
        {
            if (String.IsNullOrEmpty(fileName))
                return false;
            if (!_parent.IsConnected())
                return false;
            try
            {
              
                var result = _parent.IPimpCommand(new CommandInfoIPimp {Action = "getfile" , Value = fileName});

                if (Convert.ToBoolean(result["result"], CultureInfo.InvariantCulture) == false)
                    return false;
                var imageData = Convert.FromBase64String(Convert.ToString(result["data"], CultureInfo.InvariantCulture));

                ByteArrayToFile(destination, imageData);

                _parent.Log("DOWNLOAD : " + fileName);
                return true;
            }
            catch (Exception exception)
            {
                _parent.Log("ERROR - DOWNLOAD : " + fileName + " - " + exception.Message);
            }
            return false;
        }


        public void Dispose()
        {
            _workerDownloads.CancelAsync();
            _workerDownloads.Dispose();
        }
    }
}
