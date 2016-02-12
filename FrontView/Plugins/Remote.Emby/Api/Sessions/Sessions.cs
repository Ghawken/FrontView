using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.Sessions
{

    public class Rootobject
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string[] SupportedCommands { get; set; }
        public string[] QueueableMediaTypes { get; set; }
        public string[] PlayableMediaTypes { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public object[] AdditionalUsers { get; set; }
        public string ApplicationVersion { get; set; }
        public string Client { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public bool SupportsRemoteControl { get; set; }
        public Playstate PlayState { get; set; }
        public Nowplayingitem NowPlayingItem { get; set; }
        public Transcodinginfo TranscodingInfo { get; set; }
    }

    public class Playstate
    {
        public bool CanSeek { get; set; }
        public bool IsPaused { get; set; }
        public bool IsMuted { get; set; }
        public string RepeatMode { get; set; }
        public long PositionTicks { get; set; }
        public int VolumeLevel { get; set; }
        public int AudioStreamIndex { get; set; }
        public string MediaSourceId { get; set; }
        public string PlayMethod { get; set; }
        public int SubtitleStreamIndex { get; set; }
    }

    public class Nowplayingitem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string MediaType { get; set; }
        public long RunTimeTicks { get; set; }
        public string PrimaryImageTag { get; set; }
        public string PrimaryImageItemId { get; set; }
        public string LogoImageTag { get; set; }
        public string LogoItemId { get; set; }
        public string ThumbImageTag { get; set; }
        public string ThumbItemId { get; set; }
        public string BackdropImageTag { get; set; }
        public string BackdropItemId { get; set; }
        public DateTime PremiereDate { get; set; }
        public int ProductionYear { get; set; }
        public int IndexNumber { get; set; }

        // Glenn Changed Below - need tsting
        public string Album { get; set; }
        public int ParentIndexNumber { get; set; }
        public string SeriesName { get; set; }
        public object[] Artists { get; set; }
        public Mediastream[] MediaStreams { get; set; }
        public string ChapterImagesItemId { get; set; }
        public Chapter[] Chapters { get; set; }
    }

    public class Mediastream
    {
        public string Codec { get; set; }
        public bool IsInterlaced { get; set; }
        public int BitRate { get; set; }
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
        public string Language { get; set; }
        public int BitDepth { get; set; }
    }

    public class Chapter
    {
        public long StartPositionTicks { get; set; }
        public string Name { get; set; }
    }

    public class Transcodinginfo
    {
        public string AudioCodec { get; set; }
        public string VideoCodec { get; set; }
        public string Container { get; set; }
        public bool IsVideoDirect { get; set; }
        public bool IsAudioDirect { get; set; }
        public int Bitrate { get; set; }
        public float CompletionPercentage { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int AudioChannels { get; set; }
    }

}