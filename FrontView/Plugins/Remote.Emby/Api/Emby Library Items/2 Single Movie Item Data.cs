using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.SingleMovieItem
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
        public bool HasSubtitles { get; set; }
        public string AwardSummary { get; set; }
        public int Metascore { get; set; }
        public bool SupportsSync { get; set; }
        public bool HasSyncJob { get; set; }
        public bool IsSynced { get; set; }
        public string SortName { get; set; }
        public DateTime PremiereDate { get; set; }
        public Externalurl[] ExternalUrls { get; set; }
        public Mediasource[] MediaSources { get; set; }
        public int CriticRating { get; set; }
        public string CriticRatingSummary { get; set; }
        public string Path { get; set; }
        public string OfficialRating { get; set; }
        public string Overview { get; set; }
        public string ShortOverview { get; set; }
        public string[] Taglines { get; set; }
        public string[] Genres { get; set; }
        public float CommunityRating { get; set; }
        public int VoteCount { get; set; }
        public long RunTimeTicks { get; set; }
        public string PlayAccess { get; set; }
        public int ProductionYear { get; set; }
        public bool IsPlaceHolder { get; set; }
        public Remotetrailer[] RemoteTrailers { get; set; }
        public Providerids ProviderIds { get; set; }
        public bool IsHD { get; set; }
        public bool IsFolder { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public Person[] People { get; set; }
        public Studio[] Studios { get; set; }
        public int LocalTrailerCount { get; set; }
        public Userdata UserData { get; set; }
        public string DisplayPreferencesId { get; set; }
        public object[] Tags { get; set; }
        public string[] Keywords { get; set; }
        public float PrimaryImageAspectRatio { get; set; }
        public Mediastream1[] MediaStreams { get; set; }
        public string VideoType { get; set; }
        public Imagetags ImageTags { get; set; }
        public string[] BackdropImageTags { get; set; }
        public object[] ScreenshotImageTags { get; set; }
        public Chapter[] Chapters { get; set; }
        public string LocationType { get; set; }
        public string MediaType { get; set; }
        public string HomePageUrl { get; set; }
        public string[] ProductionLocations { get; set; }
        public int Budget { get; set; }
        public int Revenue { get; set; }
        public object[] LockedFields { get; set; }
        public bool LockData { get; set; }
    }

    public class Providerids
    {
        public string Imdb { get; set; }
        public string Tmdb { get; set; }
    }

    public class Userdata
    {
        public int PlaybackPositionTicks { get; set; }
        public int PlayCount { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime LastPlayedDate { get; set; }
        public bool Played { get; set; }
        public string Key { get; set; }
    }

    public class Imagetags
    {
        public string Primary { get; set; }
        public string Logo { get; set; }
        public string Thumb { get; set; }
    }

    public class Externalurl
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Mediasource
    {
        public string Protocol { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string Container { get; set; }
        public string Name { get; set; }
        public long RunTimeTicks { get; set; }
        public bool ReadAtNativeFramerate { get; set; }
        public bool SupportsTranscoding { get; set; }
        public bool SupportsDirectStream { get; set; }
        public bool SupportsDirectPlay { get; set; }
        public bool RequiresOpening { get; set; }
        public bool RequiresClosing { get; set; }
        public string VideoType { get; set; }
        public Mediastream[] MediaStreams { get; set; }
        public object[] PlayableStreamFileNames { get; set; }
        public object[] Formats { get; set; }
        public int Bitrate { get; set; }
        public Requiredhttpheaders RequiredHttpHeaders { get; set; }
        public int DefaultAudioStreamIndex { get; set; }
        public int DefaultSubtitleStreamIndex { get; set; }
    }

    public class Requiredhttpheaders
    {
    }

    public class Mediastream
    {
        public string Codec { get; set; }
        public string Language { get; set; }
        public bool IsInterlaced { get; set; }
        public int BitRate { get; set; }
        public int BitDepth { get; set; }
        public int RefFrames { get; set; }
        public bool IsDefault { get; set; }
        public bool IsForced { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public float AverageFrameRate { get; set; }
        public float RealFrameRate { get; set; }
        public string Profile { get; set; }
        public string Type { get; set; }
        public string AspectRatio { get; set; }
        public int Index { get; set; }
        public bool IsExternal { get; set; }
        public bool IsTextSubtitleStream { get; set; }
        public bool SupportsExternalStream { get; set; }
        public string PixelFormat { get; set; }
        public int Level { get; set; }
        public bool IsAnamorphic { get; set; }
        public string ChannelLayout { get; set; }
        public int Channels { get; set; }
        public int SampleRate { get; set; }
        public int Score { get; set; }
    }

    public class Remotetrailer
    {
        public string Url { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
        public string PrimaryImageTag { get; set; }
    }

    public class Studio
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class Mediastream1
    {
        public string Codec { get; set; }
        public string Language { get; set; }
        public bool IsInterlaced { get; set; }
        public int BitRate { get; set; }
        public int BitDepth { get; set; }
        public int RefFrames { get; set; }
        public bool IsDefault { get; set; }
        public bool IsForced { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public float AverageFrameRate { get; set; }
        public float RealFrameRate { get; set; }
        public string Profile { get; set; }
        public string Type { get; set; }
        public string AspectRatio { get; set; }
        public int Index { get; set; }
        public bool IsExternal { get; set; }
        public bool IsTextSubtitleStream { get; set; }
        public bool SupportsExternalStream { get; set; }
        public string PixelFormat { get; set; }
        public int Level { get; set; }
        public bool IsAnamorphic { get; set; }
        public string ChannelLayout { get; set; }
        public int Channels { get; set; }
        public int SampleRate { get; set; }
        public int Score { get; set; }
    }

    public class Chapter
    {
        public long StartPositionTicks { get; set; }
        public string Name { get; set; }
    }

}
