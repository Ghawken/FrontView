using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.MusicAlbums
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
        public int ProductionYear { get; set; }
        public bool IsFolder { get; set; }
        public string Type { get; set; }
        public string ParentBackdropItemId { get; set; }
        public string[] ParentBackdropImageTags { get; set; }
        public Userdata UserData { get; set; }
        public int RecursiveItemCount { get; set; }
        public int ChildCount { get; set; }
        public string[] Artists { get; set; }
        public Artistitem[] ArtistItems { get; set; }
        public string AlbumArtist { get; set; }
        public Albumartist[] AlbumArtists { get; set; }
        public Imagetags ImageTags { get; set; }
        public string[] BackdropImageTags { get; set; }
        public string LocationType { get; set; }
        public DateTime PremiereDate { get; set; }
        public string ParentLogoItemId { get; set; }
        public string ParentLogoImageTag { get; set; }
    }

    public class Userdata
    {
        public float PlayedPercentage { get; set; }
        public int UnplayedItemCount { get; set; }
        public int PlaybackPositionTicks { get; set; }
        public int PlayCount { get; set; }
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public string Key { get; set; }
    }

    public class Imagetags
    {
        public string Primary { get; set; }
        public string Disc { get; set; }
        public string BoxRear { get; set; }
        public string Logo { get; set; }
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
