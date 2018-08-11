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
using System.Linq;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Plugin;
using Setup;
using FrontView.Libs;

namespace FrontView
{
    public partial class Yatse2Window
    {
        private string GetMusicThumbPath(string remotepath)
        {
            var hash = _remotePlugin.GetHashFromFileName(remotepath);
            var destFile = Helper.CachePath + @"Music\Thumbs\" + hash + ".jpg";

            if (File.Exists(destFile)) return destFile;
            
            _remote.File.DownloadImages(new ApiImageDownloadInfo { Source = remotepath, Destination = destFile, MaxHeight = (int)Height / 2, ToThumb = _config.CropCacheImage });

            if (File.Exists(destFile))
            {
                return destFile;
            }

            if (File.Exists(Helper.SkinorDefault( Helper.SkinPath ,_config.Skin , @"\Interface\Default_Music-Thumbs.png")))
            {
                return  Helper.SkinorDefault(Helper.SkinPath ,_config.Skin , @"\Interface\Default_Music-Thumbs.png");
            }

            Logger.Instance().Log("FrontView+", @"Missing skin image : \Interface\Default_Music-Thumbs.png", true);
            return "";
        }

        private string GetMusicFanartPath(string remotepath)
        {
            var hash = _remotePlugin.GetHashFromFileName(remotepath);
            var destFile = Helper.CachePath + @"Music\Fanarts\" + hash + ".jpg";
            Logger.Instance().LogDump("GetMusicFanart", "Getting Music Fanart " + destFile, true);
            if (File.Exists(destFile)) return destFile;

            _remote.File.DownloadImages(new ApiImageDownloadInfo { Source = remotepath, Destination = destFile, MaxHeight = (int)Height , ToThumb = _config.CropCacheImage });

            if (File.Exists(destFile))
            {
                return destFile;
            }

            if (File.Exists(Helper.SkinorDefault( Helper.SkinPath , _config.Skin , @"\Interface\Default_Music-Fanarts.png")))
            {
                Logger.Instance().LogDump("GetMusicFanart", "File Exists " + Helper.SkinorDefault( Helper.SkinPath , _config.Skin , @"\Interface\Default_Music-Fanarts.png"), true);
                return Helper.SkinorDefault( Helper.SkinPath , _config.Skin , @"\Interface\Default_Music-Fanarts.png");
            }

            Logger.Instance().Log("FrontView+", @"Missing skin image : \Interface\Default_Music-Fanarts.png", true);
            return "";
        }

        private string GetVideoThumbPath(string remotepath)
        {
            var hash = _remotePlugin.GetHashFromFileName(remotepath);
            var destFile = Helper.CachePath + @"Video\Thumbs\" + hash + ".jpg";

            if (File.Exists(destFile)) return destFile;

            _remote.File.DownloadImages(new ApiImageDownloadInfo { Source = remotepath, Destination = destFile, MaxHeight = (int)Height / 2, ToThumb = _config.CropCacheImage });

            // Add CoverArt treatment for Kodi
            /**
            if(_remote.GetOS()=="Xbmc"  )  //Need to add optional check to apply CoverARt
            {
                Logger.Instance().LogDump("KodiCoverArt", "destFile Exisits now changing...", true);
                destFile = CoverArtTreatmentKodi(destFile);
            }
    **/


            if (File.Exists(destFile))
            {
                return destFile;
            }



            if (File.Exists( Helper.SkinorDefault( Helper.SkinPath , _config.Skin , @"\Interface\Default_Video-Thumbs.png")))
            {
                return Helper.SkinorDefault( Helper.SkinPath, _config.Skin , @"\Interface\Default_Video-Thumbs.png");
            }
            Logger.Instance().Log("FrontView+", @"Missing skin image : \Interface\Default_Video-Thumbs.png", true);
            return "";
        }

        private string GetVideoFanartPath(string remotepath)
        {
            var hash = _remotePlugin.GetHashFromFileName(remotepath);
            var destFile = Helper.CachePath + @"Video\Fanarts\" + hash + ".jpg";

            if (File.Exists(destFile)) return destFile;

            _remote.File.DownloadImages(new ApiImageDownloadInfo { Source = remotepath, Destination = destFile, MaxHeight = (int)Height , ToThumb = _config.CropCacheImage });

            if (File.Exists(destFile))
            {
                return destFile;
            }

            if (File.Exists( Helper.SkinorDefault( Helper.SkinPath , _config.Skin , @"\Interface\Default_Video-Fanarts.png")))
            {
                return Helper.SkinorDefault(Helper.SkinPath ,_config.Skin , @"\Interface\Default_Video-Fanarts.png");
            }
            Logger.Instance().Log("FrontView+", @"Missing skin image : \Interface\Default_Video-Fanarts.png", true);
            return "";
        }

        private string GetVideoLogoPath(string remotepath)
        {
            var hash = _remotePlugin.GetHashFromFileName(remotepath);
            var destFile = Helper.CachePath + @"Video\Logos\" + hash + ".jpg";

            if (File.Exists(destFile)) return destFile;

            _remote.File.DownloadImages(new ApiImageDownloadInfo { Source = remotepath, Destination = destFile, MaxHeight = (int)Height, ToThumb = _config.CropCacheImage });

            if (File.Exists(destFile))
            {
                return destFile;
            }

            if (File.Exists(Helper.SkinorDefault(Helper.SkinPath, _config.Skin, @"\Interface\Default_Video-Logos.png")))
            {
                return Helper.SkinorDefault(Helper.SkinPath, _config.Skin, @"\Interface\Default_Video-Logos.png");
            }
            Logger.Instance().Log("FrontView+", @"Missing skin image : \Interface\Default_Video-Logos.png", true);
            return "";
        }

        private string GetRandomImagePath(string path)
        { //TODO : Add parameter to change default image
            if (string.IsNullOrEmpty(path))
                return null;
            try
            {
                var extensions = new[] { ".PNG", ".JPG", ".GIF", ".JPEG", ".BMP" };
                var opt = _config.DiaporamaSubdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var di = new DirectoryInfo(path);
                return (di.GetFiles("*.*", opt)
                    .Where(f => extensions.Contains(f.Extension.ToUpperInvariant()))
                    .OrderBy(f => Guid.NewGuid())
                    .FirstOrDefault()).FullName;
            }

            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is DirectoryNotFoundException ||
                    ex is ArgumentException ||
                    ex is SecurityException ||
                    ex is InvalidOperationException ||
                    ex is NullReferenceException)
                {
                    Logger.Instance().Log("FrontView+", "GetRandomImagePath : no images in " + path);
                    if (File.Exists(Helper.SkinorDefault(Helper.SkinPath, _config.Skin, @"\Interface\Default_Diaporama.png")))
                    {
             
                        return Helper.SkinorDefault(Helper.SkinPath , _config.Skin, @"\Interface\Default_Diaporama.png");
                    }
                    Logger.Instance().Log("FrontView+", @"Missing skin image :"+ Helper.SkinPath + _config.Skin + @"\Interface\Default_Diaporama.png", true);
                    return "";
                }
                throw;
            }
        }
        private string GetRandomImagePathNew(string path)
        { //TODO : Add parameter to change default image
            if (string.IsNullOrEmpty(path))
                return null;
            try
            {
                var extensions = new[] { ".PNG", ".JPG", ".GIF", ".JPEG", ".BMP" };
                var opt = _config.DiaporamaSubdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var di = new DirectoryInfo(path);
                //Random R = new Random();
                var rgfiles = (di.GetFiles("*.*", opt)
                    .Where(f => extensions.Contains(f.Extension.ToUpperInvariant()))
                    .OrderBy(f => Guid.NewGuid())
                    .FirstOrDefault()).FullName;
               // Logger.Instance().Log("IMAGES", "GetRandom Results " + rgfiles);
                //return rgfiles;
                
                 return (di.GetFiles("*.*", opt)
                    .Where(f => extensions.Contains(f.Extension.ToUpperInvariant()))
                    .OrderBy(f => Guid.NewGuid())
                    .FirstOrDefault()).FullName;
                
                    
            }

            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is DirectoryNotFoundException ||
                    ex is ArgumentException ||
                    ex is SecurityException ||
                    ex is UnauthorizedAccessException ||
                    ex is InvalidOperationException ||
                    ex is ArgumentException ||
                    ex is NullReferenceException)  
                {
                    Logger.Instance().Log("FrontView+", "Exception Thrown - or No Fanart Images - cancel show in " + path);                   
                    return null;
                                                            
                }
                else
                {
                    Logger.Instance().Log("FrontView+", "Exception Thrown:---------------- " + path);
                    return null;
                }
                // don't ever throw
                //throw;
            }
        }


        private void RefreshDictionaries()
        {
            Logger.Instance().Log("FrontView+", "Load dictionaries - Skin : " + _yatse2Properties.Skin + ", Lang : " + _yatse2Properties.Language);
            try
            {
                using (System.Xml.XmlReader xmlskin = System.Xml.XmlReader.Create( Helper.SkinorDefault( Helper.SkinPath , _yatse2Properties.Skin , @"\Skin.xaml")),  xmllang = System.Xml.XmlReader.Create(Helper.LangPath + _yatse2Properties.Language + @".xaml"))
                {
                    var skin =
                        (ResourceDictionary)
                        XamlReader.Load(xmlskin);
                    var lang =
                        (ResourceDictionary)
                        XamlReader.Load(xmllang);
                    Resources.MergedDictionaries.Clear();
                    Resources.MergedDictionaries.Add(skin);
                    Resources.MergedDictionaries.Add(lang);
                }
            }
            catch (XamlParseException e)
            {
                Logger.Instance().Log("FrontView+", "Error loading Dictionaries : " + e.Message, true);
                return;
            }
        }

        private ImageBrush GetSkinImageBrush(string element)
        {
            var img = new ImageBrush();
            if (File.Exists(Helper.SkinorDefault(  Helper.SkinPath , _config.Skin , @"\Interface\" + element + ".png")))
            {
                img = new ImageBrush
                          {
                              ImageSource =
                                  new BitmapImage(
                                  new Uri(Helper.SkinorDefault( Helper.SkinPath ,_config.Skin , @"\Interface\" + element + ".png"))),
                              Stretch = Stretch.Uniform,
                              AlignmentX = AlignmentX.Center,
                              AlignmentY = AlignmentY.Center
                };
            }
            else
            {
                Logger.Instance().Log("FrontView+", @"Skin image not found : \Interface\" + element + ".png", true);
            }
            return img;
        }

        private ImageBrush GetSkinImageBrushSmall(string element)
        {
            var img = new ImageBrush();
            if (File.Exists( Helper.SkinorDefault( Helper.SkinPath , _config.Skin , @"\Interface\" + element + ".png")))
            {
                img = new ImageBrush
                {
                    ImageSource =
                        new BitmapImage(
                        new Uri( Helper.SkinorDefault( Helper.SkinPath , _config.Skin , @"\Interface\" + element + ".png"))),
                    Stretch = Stretch.Uniform,
                    TileMode = TileMode.None,
                    AlignmentX = AlignmentX.Center,
                    AlignmentY = AlignmentY.Center,
                    Viewport = new Rect(10, 10, 34, 34),
                    ViewportUnits = BrushMappingMode.Absolute,
                };
            }
            else
            {
                Logger.Instance().Log("FrontView+", @"Skin image not found : \Interface\" + element + ".png", true);
            }
            return img;
        }

    }
}