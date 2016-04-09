using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.MusicSingleAlbumItem
{

    public class Rootobject
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string Id { get; set; }
        public string Etag { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastMediaAdded { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDownload { get; set; }
        public bool SupportsSync { get; set; }
        public bool HasSyncJob { get; set; }
        public bool IsSynced { get; set; }
        public string SortName { get; set; }
        public Externalurl[] ExternalUrls { get; set; }
        public string Path { get; set; }
        public string Overview { get; set; }
        public object[] Taglines { get; set; }
        public string[] Genres { get; set; }
        public long CumulativeRunTimeTicks { get; set; }
        public string PlayAccess { get; set; }
        public int ProductionYear { get; set; }
        public object[] RemoteTrailers { get; set; }
        public Providerids ProviderIds { get; set; }
        public bool IsFolder { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public object[] People { get; set; }
        public Studio[] Studios { get; set; }
        public string ParentBackdropItemId { get; set; }
        public string[] ParentBackdropImageTags { get; set; }
        public Userdata UserData { get; set; }
        public int RecursiveItemCount { get; set; }
        public int ChildCount { get; set; }
        public string DisplayPreferencesId { get; set; }
        public object[] Tags { get; set; }
        public object[] Keywords { get; set; }
        public float PrimaryImageAspectRatio { get; set; }
        public string[] Artists { get; set; }
        public Artistitem[] ArtistItems { get; set; }
        public string AlbumArtist { get; set; }
        public Albumartist[] AlbumArtists { get; set; }
        public Imagetags ImageTags { get; set; }
        public object[] BackdropImageTags { get; set; }
        public object[] ScreenshotImageTags { get; set; }
        public string LocationType { get; set; }
        public object[] ProductionLocations { get; set; }
        public object[] LockedFields { get; set; }
        public bool LockData { get; set; }
    }

    public class Providerids
    {
        public string MusicBrainzAlbum { get; set; }
        public string MusicBrainzReleaseGroup { get; set; }
        public string AudioDbArtist { get; set; }
        public string AudioDbAlbum { get; set; }
        public string MusicBrainzAlbumArtist { get; set; }
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
    }

    public class Externalurl
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Studio
    {
        public string Name { get; set; }
        public string Id { get; set; }
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
