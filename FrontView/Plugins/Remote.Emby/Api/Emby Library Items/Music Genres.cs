using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.MusicGenres
{

    public class Rootobject
    {
        public Item[] Items { get; set; }
        public int TotalRecordCount { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string Id { get; set; }
        public string PlayAccess { get; set; }
        public bool IsFolder { get; set; }
        public string Type { get; set; }
        public Imagetags ImageTags { get; set; }
        public object[] BackdropImageTags { get; set; }
        public string LocationType { get; set; }
    }

    public class Imagetags
    {
        public string Primary { get; set; }
        public string Thumb { get; set; }
    }


}
