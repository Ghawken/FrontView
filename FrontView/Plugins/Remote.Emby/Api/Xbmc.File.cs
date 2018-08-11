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

using System.ComponentModel;
using System.Net;
using System.Threading;
using Plugin;
using System.IO;
using System.Web;

namespace Remote.Emby.Api
{
    class XbmcFile : IApiFile
    {
        private readonly Xbmc _parent;
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
            return !_isDownloading;
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

        public bool DownloadImagesNEW(ApiImageDownloadInfo apiImageDownloadInfo)
        {
            if (apiImageDownloadInfo == null)
                return false;
            
                        
            var res = DownloadRemoteImageFile(apiImageDownloadInfo.Source, apiImageDownloadInfo.Destination);
            if (res)
            {
                if (apiImageDownloadInfo.ToThumb)
                    _parent.GenerateThumb(apiImageDownloadInfo.Destination, apiImageDownloadInfo.Destination, apiImageDownloadInfo.MaxHeight);
            }
            return res;
        }

        public bool DownloadImages(ApiImageDownloadInfo apiImageDownloadInfo)
        {

            try
            {
                if (apiImageDownloadInfo == null)
                    return false;

                bool res = false;

                if (apiImageDownloadInfo.Source.Contains(_parent.IP))
                {
                    _parent.Trace("----------DOWNLOAD IMAGES: Checking for presence of Server IP Address in source to select Download Method - Server IP Found");
                    _parent.Trace("----------DOWNLOAD IMAGES: Source:" + apiImageDownloadInfo.Source.ToString() + ":Destination:"+ apiImageDownloadInfo.Destination.ToString());
                    res = DownloadRemoteImageFile(apiImageDownloadInfo.Source, apiImageDownloadInfo.Destination);
                }
                else
                {
                    res = Download(apiImageDownloadInfo.Source, apiImageDownloadInfo.Destination);
                }


                if (res)
                {
                    if (apiImageDownloadInfo.ToThumb)
                        _parent.GenerateThumb(apiImageDownloadInfo.Destination, apiImageDownloadInfo.Destination, apiImageDownloadInfo.MaxHeight);
                }
                return res;
            }
            catch (System.Exception ex)
            {
                _parent.Log("Download Image: Does not Exist: Exception Caught:" + ex);
                // Maybe add this check log first -- apiImageDownloadInfo.Source = "NONE";
                return false;
;
            }
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
            }
            _isDownloading = false;
        }

        public bool Download(string fileName, string destination)
        {
            _parent.Trace("----------DOWNLOAD IMAGES ISSUE:  fileNAME is"+fileName+"  : Destination is: "+destination);

            if (!_parent.IsConnected())
            {
                _parent.Trace(" ---------------DOWNLOAD ISSUES ISSUE:  PARENT NOT CONNECTING RETURNING FALSE");
                return false;
            }

            try
            {
                using (var client = new WebClient())
                {
                    var credentials = _parent.GetCredentials();
                    
                    
                    if (credentials != null)
                         client.Credentials = credentials;

                    client.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                    //Add Header Client - ? helps
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                    client.DownloadFile(_parent.GetDownloadPath(fileName), destination);
                }
                _parent.Trace("DOWNLOAD : " + fileName);
                return true;
            }
            catch (WebException ex)
            {
                _parent.Trace("EMBY DOWNLOAD ERROR - DOWNLOAD : " + _parent.GetDownloadPath(fileName) + " " + ex.Message);
            }
            catch (System.Exception ex)
            {
                _parent.Trace("EMBY DOWNLOAD ISSUE: EXCEPTION CAUGHT:" + ex);
            }
            return false;
        }

        //New File DOwnloaded for emby Media Server.
        private bool DownloadRemoteImageFile(string uri, string fileName)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Headers.Add("X-MediaBrowser-Token", Globals.EmbyAuthToken);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Check that the remote file was found. The ContentType
                // check is performed since a request for a non-existent
                // image file might be redirected to a 404-page, which would
                // yield the StatusCode "OK", even though the image was not
                // found.
                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect))
                {

                    // if the remote file was found, download oit
                    using (System.IO.Stream inputStream = response.GetResponseStream())
                    using (System.IO.Stream outputStream = File.OpenWrite(fileName))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (WebException ex)
            {
                _parent.Trace("Emby:   Something wrong with new one" + ex + "------------------------------- URL TRIED"+uri);
                return false;
            }
        }

        public void Dispose()
        {
            _workerDownloads.CancelAsync();
            _workerDownloads.Dispose();
        }
    }
}
