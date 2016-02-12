using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.MusicSongSingleItem
{

    public class Rootobject
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string Id { get; set; }
        public string Etag { get; set; }
        public DateTime DateCreated { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDownload { get; set; }
        public bool SupportsSync { get; set; }
        public bool HasSyncJob { get; set; }
        public bool IsSynced { get; set; }
        public string SortName { get; set; }
        public object[] ExternalUrls { get; set; }
        public Mediasource[] MediaSources { get; set; }
        public string Path { get; set; }
        public object[] Taglines { get; set; }
        public string[] Genres { get; set; }
        public long RunTimeTicks { get; set; }
        public string PlayAccess { get; set; }
        public int ProductionYear { get; set; }
        public int IndexNumber { get; set; }
        public object[] RemoteTrailers { get; set; }
        public Providerids ProviderIds { get; set; }
        public bool IsFolder { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public Person[] People { get; set; }
        public Studio[] Studios { get; set; }
        public string ParentBackdropItemId { get; set; }
        public string[] ParentBackdropImageTags { get; set; }
        public Userdata UserData { get; set; }
        public string DisplayPreferencesId { get; set; }
        public object[] Tags { get; set; }
        public object[] Keywords { get; set; }
        public string[] Artists { get; set; }
        public Artistitem[] ArtistItems { get; set; }
        public string Album { get; set; }
        public string AlbumId { get; set; }
        public string AlbumArtist { get; set; }
        public Albumartist[] AlbumArtists { get; set; }
        public Mediastream1[] MediaStreams { get; set; }
        public Imagetags ImageTags { get; set; }
        public object[] BackdropImageTags { get; set; }
        public object[] ScreenshotImageTags { get; set; }
        public string LocationType { get; set; }
        public string MediaType { get; set; }
        public object[] ProductionLocations { get; set; }
        public object[] LockedFields { get; set; }
        public bool LockData { get; set; }
    }

    public class Providerids
    {
    }

    public class Userdata
    {
        public int PlaybackPositionTicks { get; set; }
        public int PlayCount { get; set; }
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public string Key { get; set; }
    }

    public class Imagetags
    {
    }

    public class Mediasource
    {
        public string Protocol { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string Container { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
        public long RunTimeTicks { get; set; }
        public bool ReadAtNativeFramerate { get; set; }
        public bool SupportsTranscoding { get; set; }
        public bool SupportsDirectStream { get; set; }
        public bool SupportsDirectPlay { get; set; }
        public bool RequiresOpening { get; set; }
        public bool RequiresClosing { get; set; }
        public Mediastream[] MediaStreams { get; set; }
        public object[] PlayableStreamFileNames { get; set; }
        public object[] Formats { get; set; }
        public int Bitrate { get; set; }
        public Requiredhttpheaders RequiredHttpHeaders { get; set; }
    }

    public class Requiredhttpheaders
    {
    }

    public class Mediastream
    {
        public string Codec { get; set; }
        public bool IsInterlaced { get; set; }
        public string ChannelLayout { get; set; }
        public int BitRate { get; set; }
        public int Channels { get; set; }
        public int SampleRate { get; set; }
        public bool IsDefault { get; set; }
        public bool IsForced { get; set; }
        public string Type { get; set; }
        public int Index { get; set; }
        public bool IsExternal { get; set; }
        public bool IsTextSubtitleStream { get; set; }
        public bool SupportsExternalStream { get; set; }
        public int Level { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
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

    public class Mediastream1
    {
        public string Codec { get; set; }
        public bool IsInterlaced { get; set; }
        public string ChannelLayout { get; set; }
        public int BitRate { get; set; }
        public int Channels { get; set; }
        public int SampleRate { get; set; }
        public bool IsDefault { get; set; }
        public bool IsForced { get; set; }
        public string Type { get; set; }
        public int Index { get; set; }
        public bool IsExternal { get; set; }
        public bool IsTextSubtitleStream { get; set; }
        public bool SupportsExternalStream { get; set; }
        public int Level { get; set; }
    }

}
