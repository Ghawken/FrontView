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
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Setup;
using FrontView.Libs;
using System.Linq;
using FrontView.Classes;

namespace FrontView
{

    public class VisibilityConverter : IValueConverter
    {
        private static readonly VisibilityConverter TheInstance = new VisibilityConverter();
        private VisibilityConverter() { }
        public static VisibilityConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;

            var param = (string)parameter;
            
            if (param == "bool")
                return ((bool)value) ? Visibility.Visible : Visibility.Hidden;

            if (param == "boolinvert")
                return ((bool)value) ? Visibility.Hidden : Visibility.Visible;

            if (param == "long")
                return ((long)value > 0) ? Visibility.Visible : Visibility.Hidden;

            if (param == "longinvert")
                return ((long)value > 0) ? Visibility.Hidden : Visibility.Visible;

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class IconVisibility : IValueConverter
    {
        private static readonly IconVisibility TheInstance = new IconVisibility();
        private IconVisibility() { }
        public static IconVisibility Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;

            var param = (string)parameter;
            var check = (string)value;


            Logger.Instance().Trace("IconVisibility:", "check for: " + check + ":param:" + param);

            // check equals to MovieIcons contents eg.  DTS,h264,1080p,2:40:1 etc
            // param is value passed

            if (check.Contains(param))
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }



    public class LongDurationConverter : IValueConverter
    {
        private static readonly LongDurationConverter TheInstance = new LongDurationConverter();
        private LongDurationConverter() { }
        public static LongDurationConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;
            var val = (long) value;
            return (val > 0) ? string.Format(CultureInfo.InvariantCulture,"{0:00}:{1:00}", val / 60, val % 60) : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class SkinImagePathConverter : IValueConverter
    {
        private static readonly SkinImagePathConverter TheInstance = new SkinImagePathConverter();
        private SkinImagePathConverter() { }
        public static SkinImagePathConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;
            var path = Helper.SkinPath + (string)value + @"\Interface\" + (string)parameter + ".png";
            
            if (!File.Exists(path))
            {
                Logger.Instance().Trace("C_SkinImgPath","Missing skin image : " + path + " Using Default Skin:");
                path = Helper.SkinPath + "Default" + @"\Interface\" + (string)parameter + ".png";
                if (!File.Exists(path))
                {
                    Logger.Instance().Log("C_SkinImgPath", "Missing DEFAULT skin image : " + path );
                    return "";
                }

                return path;
            }
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
    public class SkinIconConverter : IValueConverter
    {
        private static readonly SkinIconConverter TheInstance = new SkinIconConverter();
        private SkinIconConverter() { }
        public static SkinIconConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;

            var param = (string)parameter;
            var temp = param.Split('/');

            if (temp.Length > 1)
            {
                param = temp[0];
            }

            var path = Helper.SkinPath + (string)value + @"\Icons\" + param + ".png";


            if (!File.Exists(path))
            {
                // Change to check Default Skin if missing - hopefully moving to avoid duplication of entire skin directories    

                Logger.Instance().Trace("C_SkinBrush", "Missing skin image : " + path + " Trying Default Skin");
                path = Helper.SkinPath + "Default" + @"\Icons\" + param + ".png";
                if (!File.Exists(path))
                {
                    Logger.Instance().Log("C_SkinBrush", "Missing DEFAULT skin image : " + path);
                    return new ImageBrush();
                }

            }


            if (temp.Length > 1)
            {
                if (temp[1] == "Small")
                    return new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(path)),
                        Stretch = Stretch.Uniform,
                        TileMode = TileMode.None,
                        AlignmentX = AlignmentX.Center,
                        AlignmentY = AlignmentY.Center,
                        Viewport = new Rect(10, 10, 34, 34),
                        ViewportUnits = BrushMappingMode.Absolute,
                    };
                if (temp[1] == "Fill")
                    return new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(path)),
                        Stretch = Stretch.UniformToFill,
                        AlignmentX = AlignmentX.Center,
                        AlignmentY = AlignmentY.Center
                    };
            }

            return new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(path)),
                Stretch = Stretch.Uniform,
                AlignmentX = AlignmentX.Center,
                AlignmentY = AlignmentY.Center
                
            };
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
    public class SkinBrushConverter : IValueConverter
    {
        private static readonly SkinBrushConverter TheInstance = new SkinBrushConverter();
        private SkinBrushConverter() { }
        public static SkinBrushConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;

            var param = (string) parameter;
            var temp = param.Split('/');
            
            if (temp.Length > 1)
            {
                param = temp[0];
            }

            var path = Helper.SkinPath + (string)value + @"\Interface\" + param + ".png";
            
            
            if (!File.Exists(path))
            {
            // Change to check Default Skin if missing - hopefully moving to avoid duplication of entire skin directories    
                
                Logger.Instance().Trace("C_SkinBrush", "Missing skin image : " + path + " Trying Default Skin");
                path = Helper.SkinPath + "Default" + @"\Interface\" + param + ".png";
                if (!File.Exists(path))
                {
                    Logger.Instance().Log("C_SkinBrush", "Missing DEFAULT skin image : " + path );
                    return new ImageBrush();
                }          
               
            }


            if (temp.Length > 1)
            {
                if (temp[1] == "Small")
                    return new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(path)),
                        Stretch = Stretch.Uniform,
                        TileMode = TileMode.None,
                        AlignmentX = AlignmentX.Center,
                        AlignmentY = AlignmentY.Center,
                        Viewport = new Rect(10, 10, 34, 34),
                        ViewportUnits = BrushMappingMode.Absolute,
                    };
                if (temp[1] == "Fill")
                    return new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(path)),
                        Stretch = Stretch.UniformToFill,
                        AlignmentX = AlignmentX.Center,
                        AlignmentY = AlignmentY.Center
                    };
            }

            return new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(path)),
                Stretch = Stretch.Uniform,
                AlignmentX = AlignmentX.Center,
                AlignmentY = AlignmentY.Center,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class CacheImageConverter : IValueConverter
    {
        private static readonly CacheImageConverter TheInstance = new CacheImageConverter();

        private CacheImageConverter() { }
        public static CacheImageConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;
            var param = (string)parameter;
            var path = Helper.CachePath + param + @"\" + ApiHelper.Instance().GetPluginHashFromFileName((string)value, Helper.Instance.CurrentApi) + ".jpg";

            if (File.Exists(path))
            {
                try
                {
                    return new BitmapImage(new Uri(path));
                }
                catch (Exception)
                {
                    return new BitmapImage(new Uri("pack://application:,,,/Skin/Internal/Images/Empty.png"));
                }
            }
                
            param = @"\Interface\Default_" + param.Replace("\\", "-") + ".png";
            path = Helper.SkinPath + Helper.Instance.CurrentSkin + param;
            if (File.Exists(path))
                return new BitmapImage(new Uri(path));

            Logger.Instance().Log("C_CacheImage", "Missing default Image : " + path);
            return new BitmapImage(new Uri("pack://application:,,,/Skin/Internal/Images/Empty.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class CacheImageMovieDetailsConverter : IValueConverter
    {
        private static readonly CacheImageMovieDetailsConverter TheInstance = new CacheImageMovieDetailsConverter();

        private CacheImageMovieDetailsConverter() { }
        public static CacheImageMovieDetailsConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;
            var param = (string)parameter;
            var path = "";

            if (param == @"Video\Thumbs")
            {
                path = Helper.CachePath + @"Video\Thumbs" + @"\" + ApiHelper.Instance().GetPluginHashFromFileName((string)value, Helper.Instance.CurrentApi) + ".jpg";
            }
            else
            {
                path = (string)value;
            }

            Logger.Instance().Trace("CoverART MovieDetails", "param = :" + param);
            Logger.Instance().Trace("CoverART MovieDetails", "Path = :" + path);
            Logger.Instance().Trace("CoverART MovieDetails", "Value = :" + (string)value);

            // If running Kodi check for Case and use that if exists
            if (Helper.Instance.CurrentApi == "Kodi 16" || Helper.Instance.CurrentApi == "Kodi 17")
            {
                if (Helper.Instance.UseCoverArt == true)
                {

                    var newpath = path.Remove(path.Length - 4) + "-Case.jpg";
                    if (File.Exists(newpath))
                    {
                        try
                        {
                            return new BitmapImage(new Uri(newpath));
                        }
                        catch (Exception)
                        {
                            return new BitmapImage(new Uri(path));
                        }
                    }
                }
            }

            if (File.Exists(path))
            {
                try
                {
                    return new BitmapImage(new Uri(path));
                }
                catch (Exception)
                {
                    return new BitmapImage(new Uri("pack://application:,,,/Skin/Internal/Images/Empty.png"));
                }
            }

            param = @"\Interface\Default_" + param.Replace("\\", "-") + ".png";
            path = Helper.SkinPath + Helper.Instance.CurrentSkin + param;
            if (File.Exists(path))
                return new BitmapImage(new Uri(path));

            Logger.Instance().Log("C_CacheImage", "Missing default Image : " + path);
            return new BitmapImage(new Uri("pack://application:,,,/Skin/Internal/Images/Empty.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }




    public class CacheCoverartConverter : IMultiValueConverter
    {
        private static readonly CacheCoverartConverter TheInstance = new CacheCoverartConverter();
        private CacheCoverartConverter() { }
        public static CacheCoverartConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == DependencyProperty.UnsetValue) return false;

            var param = (string)parameter;
            var path = Helper.CachePath + param + @"\" + ApiHelper.Instance().GetPluginHashFromFileName((string)values[0], Helper.Instance.CurrentApi) + ".jpg";
            var MovieIcons = (string)values[2];
            string newpath = path.Remove(path.Length - 4) + "-Case.jpg";


            Logger.Instance().Trace("CoverART", " Remote Property equals : " + Helper.Instance.CurrentApi);
            Logger.Instance().Trace("CoverART", " Remote UseCoverART equals : " + Helper.Instance.UseCoverArt);


            if (Helper.Instance.CurrentApi == "Kodi 16" || Helper.Instance.CurrentApi == "Kodi 17")
            {
                if (Helper.Instance.UseCoverArt == true)
                {
                    Logger.Instance().Trace("CoverART", " values MultiBinding equals : " + values[0] + "Skin Value1:  " + values[1] + "MovieIcons:" + values[2]);

                    Logger.Instance().Trace("CoverART", " Remote Property equals : " + Helper.Instance.CurrentApi);

                    if (File.Exists(newpath))
                    {
                        return new BitmapImage(new Uri(newpath));
                    }

                    if (File.Exists(path))
                    {
                        return ApiHelper.Instance().CoverArtTreatmentKodi(path, (string)values[1], MovieIcons);

                    }
                }

            }


            if (File.Exists(path))
            {
                try
                {
                    return new BitmapImage(new Uri(path));
                }
                catch (Exception)
                {
                    return new BitmapImage(new Uri("pack://application:,,,/Skin/Internal/Images/Empty.png"));
                }
            }

            param = @"\Interface\Default_" + param.Replace("\\", "-") + ".png";
            path = Helper.SkinPath + Helper.Instance.CurrentSkin + param;
            if (File.Exists(path))
                return new BitmapImage(new Uri(path));

            Logger.Instance().Log("C_CacheImage", "Missing default Image : " + path);
            return new BitmapImage(new Uri("pack://application:,,,/Skin/Internal/Images/Empty.png"));
        }

    /**    public object ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            string[] splitValues = ((string)value).Split(' ');
            return splitValues;
        }
            **/
        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return ((IMultiValueConverter)TheInstance).ConvertBack(value, targetTypes, parameter, culture);
        }
    }





    /// <summary>
    /// 
    /// </summary>
    public class SkinImageConverter : IValueConverter
    {
        private static readonly SkinImageConverter TheInstance = new SkinImageConverter();
        private SkinImageConverter() { }
        public static SkinImageConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;

            var img = (string)value;
            var param = (string) parameter;

            if (String.IsNullOrEmpty(param))
                return false;


            var path = Helper.SkinPath + Helper.Instance.CurrentSkin + @"\" + param + @"\" + img + ".png";


            if (!String.IsNullOrEmpty(img))
            {
                if (param == @"Interface\Icon_")
                {
                    path = Helper.SkinPath + Helper.Instance.CurrentSkin + @"\" + param + img + ".png";
                }



                if (File.Exists(path))
                {
                    return new BitmapImage(new Uri(path));
                }

                Logger.Instance().Trace("C_SkinImage", "Missing skin Image : " + path + " trying DEFAULT skin");


                path = Helper.SkinPath + "Default" + @"\" + param + @"\" + img + ".png";
                if (param == @"Interface\Icon_")
                {
                    path = Helper.SkinPath + "Default" + @"\" + param + img + ".png";
                }
                if (File.Exists(path))
                {
                    return new BitmapImage(new Uri(path));
                }

                Logger.Instance().Log("C_SkinImage", "Missing DEFAULT skin Image : " + path );

            }




            path = Helper.SkinPath + Helper.Instance.CurrentSkin + @"\" + param + @"\Default.png";

            if (File.Exists(path))
            {
                return new BitmapImage(new Uri(path));
            }
            Logger.Instance().Trace("C_SkinImage", "Default not found : " + path + " trying Default Skin");
            
            path = Helper.SkinPath + "Default" + @"\" + param + @"\Default.png";
            if (File.Exists(path))
            {
                return new BitmapImage(new Uri(path));
            }
            Logger.Instance().Log("C_SkinImage", "DEFAULT SKIN Default not found : " + path);
            
            
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class BrushOpacityConverter : IValueConverter
    {
        private static readonly BrushOpacityConverter TheInstance = new BrushOpacityConverter();
        private BrushOpacityConverter() { }
        public static BrushOpacityConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;

            var img = (string)value;
            var param = (int)parameter;

           
            Logger.Instance().Trace("BrushOpacityConverter:", "img: " + img + ":param:" + param);

            Color mycolour = new Color();
            mycolour = Color.FromRgb(0, 255, 255);
            return mycolour;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    public class SkinLogoConverter : IValueConverter
    {
        private static readonly SkinLogoConverter TheInstance = new SkinLogoConverter();
        private SkinLogoConverter() { }
        public static SkinLogoConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            
            if (value == DependencyProperty.UnsetValue) return false;


            // Check and change the filename from default to cache file first and then check if equals default cache.

            //string cacheornot = "";
            var img = (string)value;

            if (String.IsNullOrEmpty(img))
            {
                Logger.Instance().Trace("SkinLogoConverter:", "img NULL orEMPTY; returning false ");
                return false; 
                
            }


            var param = (string)parameter;
            var path = img;

            Logger.Instance().Trace("SkinLogoConverter: Base:", "img: " + img + ":param:" + param +" path :"+path);

            if (img.Contains(@"Video\Logos") == false)
            {
                path = Helper.CachePath + "Video\\Logos" + @"\" + ApiHelper.Instance().GetPluginHashFromFileName((string)value, Helper.Instance.CurrentApi) + ".jpg";
                Logger.Instance().Trace("SkinLogoConverter: Contains Video-Logo FALSE", "img: " + img + ":param:" + param + " path :" + path);
            }




            if (File.Exists(path))
            {
                try
                {
                    // Early return - obviously not using default as using Cache.
                    Logger.Instance().Trace("SkinLogoConverter: Returning FALSE:", "img: " + img + ":param:" + param + " path :" + path);
                    return false;
                }
                catch (Exception)
                {
                    Logger.Instance().Trace("SkinLogoConverter: Exception:", "img: " + img + ":param:" + param + " path :" + path);
                }
            }
            /**
            param = @"\Interface\Default_" + param.Replace("\\", "-") + ".png";
            path = Helper.SkinPath + Helper.Instance.CurrentSkin + param;

            if (File.Exists(path))
            {
                cacheornot = path;
            }
            else
            {
                cacheornot = "pack://application:,,,/Skin/Internal/Images/Empty.png";
            }
    **/
            // if gets this far - cache image does not exist
            // why?  alwways return true

            Logger.Instance().Trace("SkinLogoConverter: Returning True:", "img: " + img + ":param:" + param + " path :" + path);
            return true;


            var img2 = (string)value;
            var param2 = (string)parameter;



            string lastpart = img2.Substring(Math.Max(0, img2.Length - 23));

            Logger.Instance().Trace("SkinLogoConverter:", "img: " + img2 + ":param:" + param2 + " Last last of Filename:======"+lastpart);


            if (lastpart == "Default_Video-Logos.png")
            {
                return true;
                //using Default Video Logos png hence turn
            }
            else
            {
                return false;
                // Not Default return false..
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class SkinExtraConverter : IValueConverter
    {
        private static readonly SkinExtraConverter TheInstance = new SkinExtraConverter();
        private SkinExtraConverter() { }
        public static SkinExtraConverter Instance
        {
            get { return TheInstance; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue) return false;

            var img = (string)value;
            var param = (string)parameter;

            if (String.IsNullOrEmpty(param))
                return false;

            Logger.Instance().Trace("SkinExtraConverter:", "img: " + img + ":param:"+param);

            if (img=="V Large NowPlaying")
            {
                if (param == "scale")
                {
                    return 1.6;
                }
                if (param == "scaleneg")
                {
                    return -1.6;
                }
                if (param == "margincover")
                {
                    return new Thickness(2,0,0,256); 
                }
                if (param == "margincoverreflection")
                {
                    return new Thickness(50,0,0 ,-198 );
                }
                
            }
            else if (img == "Large NowPlaying")
            {
                if (param == "scale")
                {
                    return 1.4;
                }
                if (param == "scaleneg")
                {
                    return -1.4;
                }
                if (param == "margincover")
                {
                    return new Thickness(8, 0, 0, 216);
                }
                if (param == "margincoverreflection")
                {
                    return new Thickness(40, 0, 0, -170);
                }
            }
            else if (img == "Small NowPlaying")
            {
                if (param == "scale")
                {
                    return 1;
                }
                if (param == "scaleneg")
                {
                    return -1;
                }
                if (param == "margincover")
                {
                    return new Thickness(40, 0, 0, 116);
                }
                if (param == "margincoverreflection")
                {
                    return new Thickness(40, 0, 0, -125);
                }
            }



            if (img == "V Large")
            {
                if (param == "textsize")
                {
                    return 30;
                }
                if (param == "titlesize")
                {
                    return 50;
                }
                if (param == "logosize")
                {
                    return 200;
                }
                if (param == "boxsize")
                {
                    return 100;
                }
                if (param == "TVS00E00")
                {
                    return 18;
                }
            }
            if (img == "Large")
            {
                if (param == "textsize")
                {
                    return 22;
                }
                if (param == "titlesize")
                {
                    return 40;
                }
                if (param == "logosize")
                {
                    return 150;
                }
                if (param == "boxsize")
                {
                    return 60;
                }
                if (param == "TVS00E00")
                {
                    return 18;
                }
            }
            if (img=="Medium"  )
            {
                if (param== "logosize")
                {
                    return 120;
                }
            }
            if (img == "Small")
            {
                if (param == "textsize")
                {
                    return 18;
                }
                if (param == "titlesize")
                {
                    return 22;
                }
                if (param == "logosize")
                {
                    return 50;
                }
                if (param == "boxsize")
                {
                    return 35;
                }
                if (param == "TVS00E00")
                {
                    return 18;
                }
            }

            return 1;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
