using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.MusicSongs
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
        public long RunTimeTicks { get; set; }
        public string PlayAccess { get; set; }
        public int ProductionYear { get; set; }
        public int IndexNumber { get; set; }
        public bool IsFolder { get; set; }
        public string Type { get; set; }
        public string ParentBackdropItemId { get; set; }
        public string[] ParentBackdropImageTags { get; set; }
        public Userdata UserData { get; set; }
        public string[] Artists { get; set; }
        public Artistitem[] ArtistItems { get; set; }
        public string Album { get; set; }
        public string AlbumId { get; set; }
        public string AlbumPrimaryImageTag { get; set; }
        public string AlbumArtist { get; set; }
        public Albumartist[] AlbumArtists { get; set; }
        public Imagetags ImageTags { get; set; }
        public object[] BackdropImageTags { get; set; }
        public string LocationType { get; set; }
        public string MediaType { get; set; }
        public int ParentIndexNumber { get; set; }
        public DateTime PremiereDate { get; set; }
        public string ParentLogoItemId { get; set; }
        public string ParentLogoImageTag { get; set; }
    }

    public class Userdata
    {
        public int PlaybackPositionTicks { get; set; }
        public int PlayCount { get; set; }
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public string Key { get; set; }
        public DateTime LastPlayedDate { get; set; }
    }

    public class Imagetags
    {
        public string Primary { get; set; }
    }

    public class Artistitem
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class Albumartist
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }


}
