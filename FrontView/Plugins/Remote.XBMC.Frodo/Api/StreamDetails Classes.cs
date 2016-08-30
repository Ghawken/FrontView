using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.XBMC.Frodo.Api.StreamDetails
{


    public class Rootobject
    {
        public Audio[] audio { get; set; }
        public Subtitle[] subtitle { get; set; }
        public Video[] video { get; set; }
    }

    public class Audio
    {
        public int channels { get; set; }
        public string codec { get; set; }
        public string language { get; set; }
    }

    public class Subtitle
    {
        public string language { get; set; }
    }

    public class Video
    {
        public float aspect { get; set; }
        public string codec { get; set; }
        public int duration { get; set; }
        public int? height { get; set; }
        public string stereomode { get; set; }
        public int? width { get; set; }
    }



}