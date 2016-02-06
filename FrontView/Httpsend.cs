using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Plugin;
using FrontView.Classes;
using FrontView.Libs;
using Setup;
using System.Windows.Automation.Peers;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Forms;

namespace FrontView
{
    public class HttpSend
    {
        public static void HttpsendgotoHttpsimple(FrontView.Classes.FrontViewConfig _config, string url)
        {

            if (url == "")
            {
                return;
            }
            
            try
            {
                if (_config.HttpUseDigest == false)
                {
                    Logger.Instance().LogDump("HttpSimpleSend", "Using Basic: Url:  " + url, true);

                    var logon = _config.HttpUser;   
                    
                    var password = _config.HttpPassword;
                    
                    

                    var Auth = "Basic";


                    Logger.Instance().LogDump("HttpSimpleSend", "Using " + Auth + " Authorisation:   URL " + url, true);

                    WebRequest request = WebRequest.Create(url);
                    request.Method = WebRequestMethods.Http.Get;
                    NetworkCredential networkCredential = new NetworkCredential(logon, password); // logon in format "domain\username"
                    CredentialCache myCredentialCache = new CredentialCache { { new Uri(url), Auth, networkCredential } };
                    request.PreAuthenticate = true;
                    request.Credentials = myCredentialCache;
                    using (WebResponse response = request.GetResponse())
                    {

                        //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                        Logger.Instance().LogDump("HttpSimpleSend", "Response: " + url + " Response: " + (((HttpWebResponse)response).StatusDescription), true);
                        //using (Stream dataStream = response.GetResponseStream())
                        // {
                        //     using (StreamReader reader = new StreamReader(dataStream))
                        //     {
                        //         string responseFromServer = reader.ReadToEnd();
                        //         Logger.Instance().LogDump("HttpSend", "url: " + url + " Response: " + responseFromServer, true);
                        //Console.WriteLine(responseFromServer);
                        //     }
                        // }
                    }
                }
                if (_config.HttpUseDigest == true)
                {
                    Uri myurl = new Uri(url);
                    string baseurl = myurl.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
                    //var baseurl = myurl.Host;
                    var dir = myurl.PathAndQuery;
                    Logger.Instance().LogDump("HttpSimpleSend", "Using Digest:  Url: " + url + " BaseURL: " + baseurl + "  Dir: " + dir, true);
                    DigestAuthFixer digest = new DigestAuthFixer(baseurl, _config.HttpUser, _config.HttpPassword);
                    string strReturn = digest.GrabResponse(dir);
                }


            }


            catch (Exception ex)
            {
                Logger.Instance().Log("HttpSimpleSend", "ERROR: For URL: " + url + "   Exception: " + ex, true);
            }


        }

        public static void HttpSendgotoHttp(string url, FrontViewConfig _config, Plugin.ApiCurrently nowPlaying)
        {
            try
            {
                //Add Variable Support to URL passing - mainly useful for filename?
                var newurl = url;

                if (url == "")
                {
                    Logger.Instance().LogDump("HttpSend", "Called - URL Empty:  URL: " + url, true);
                    return;
                }

                if (nowPlaying.FileName != null)
                {
                    newurl = url.Replace("%HTTPFILENAME%", Uri.EscapeUriString(nowPlaying.FileName));
                }
                if (nowPlaying.Artist != null)
                {
                    newurl = newurl.Replace("%HTTPARTIST%", Uri.EscapeUriString(nowPlaying.Artist));
                }
                if (nowPlaying.Album != null)
                {
                    newurl = newurl.Replace("%HTTPALBUM%", Uri.EscapeUriString(nowPlaying.Album));
                }
                if (nowPlaying.FanartURL != null)
                {
                    newurl = newurl.Replace("%HTTPFANARTURL%", Uri.EscapeUriString(nowPlaying.FanartURL));
                }
                if (nowPlaying.MediaType != null)
                {
                    newurl = newurl.Replace("%HTTPMEDIATYPE%", Uri.EscapeUriString(nowPlaying.MediaType));
                }
                if (nowPlaying.ShowTitle != null)
                {
                    newurl = newurl.Replace("%HTTPTITLE%", Uri.EscapeUriString(nowPlaying.ShowTitle));
                }
                if (nowPlaying.Plot != null)
                {
                    newurl = newurl.Replace("%HTTPPLOT%", Uri.EscapeUriString(nowPlaying.Plot));
                }


                newurl = newurl.Replace("%HTTPSEASONNO%", Uri.EscapeUriString(nowPlaying.SeasonNumber.ToString()));


                newurl = newurl.Replace("%HTTPPROGRESS%", Uri.EscapeUriString(nowPlaying.Progress.ToString()));

                newurl = newurl.Replace("%HTTPTIME%", Uri.EscapeUriString(nowPlaying.Time.ToString()));

                newurl = newurl.Replace("%HTTPEPISODENO%", Uri.EscapeUriString(nowPlaying.EpisodeNumber.ToString()));











                Logger.Instance().LogDump("HttpSend", "Variables " + url + " newURL " + newurl, true);

                url = newurl;


                if (_config.HttpUseDigest == false)
                {
                    Logger.Instance().LogDump("HttpSend", "Using Basic: Url:  " + url, true);

                    var logon = _config.HttpUser;
                    var password = _config.HttpPassword;
                    var Auth = "Basic";


                    Logger.Instance().LogDump("HttpSend", "Using " + Auth + " Authorisation:   URL " + url, true);

                    WebRequest request = WebRequest.Create(url);
                    request.Method = WebRequestMethods.Http.Get;
                    NetworkCredential networkCredential = new NetworkCredential(logon, password); // logon in format "domain\username"
                    CredentialCache myCredentialCache = new CredentialCache { { new Uri(url), Auth, networkCredential } };
                    request.PreAuthenticate = true;
                    request.Credentials = myCredentialCache;
                    using (WebResponse response = request.GetResponse())
                    {

                        //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                        Logger.Instance().LogDump("HttpSend", "Response: " + url + " Response: " + (((HttpWebResponse)response).StatusDescription), true);
                        //using (Stream dataStream = response.GetResponseStream())
                        // {
                        //     using (StreamReader reader = new StreamReader(dataStream))
                        //     {
                        //         string responseFromServer = reader.ReadToEnd();
                        //         Logger.Instance().LogDump("HttpSend", "url: " + url + " Response: " + responseFromServer, true);
                        //Console.WriteLine(responseFromServer);
                        //     }
                        // }
                    }
                }
                if (_config.HttpUseDigest == true)
                {
                    Uri myurl = new Uri(url);
                    string baseurl = myurl.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped);
                    //var baseurl = myurl.Host;
                    var dir = myurl.PathAndQuery;
                    Logger.Instance().LogDump("HttpSend", "Using Digest:  Url: " + url + " BaseURL: " + baseurl + "  Dir: " + dir, true);
                    DigestAuthFixer digest = new DigestAuthFixer(baseurl, _config.HttpUser, _config.HttpPassword);
                    string strReturn = digest.GrabResponse(dir);
                }


            }


            catch (Exception ex)
            {
                Logger.Instance().Log("HttpSend", "ERROR: For URL: " + url + "   Exception: " + ex, true);
            }


        }

    }
}
