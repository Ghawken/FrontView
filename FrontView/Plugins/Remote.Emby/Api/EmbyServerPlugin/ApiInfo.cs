using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.EmbyServerPlugin
{
    public class ApiInfo
    {

        public string PlayingClientID { get; set; }
        public string ID { get; set; }
        public string Filename { get; set; }
        public bool IsPaused { get; set; }
        public string Overview { get; set; }
        public long? TimePosition { get; set; }
        public string Album { get; set; }
        public string Director { get; set; }
        public string Rating { get; set; }
        public string Tagline { get; set; }
        public string Studio { get; set; }
        public bool IsPlaying { get; set; }
        public string MediaType { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Track { get; set; }
        public string Genre { get; set; }
        public string PrimaryItemId { get; set; }
        public string BackdropItemId { get; set; }
        public string Artist { get; set; }
        public int? EpisodeNumber { get; set; }
        public int? SeasonNumber { get; set; }
        public string ShowTitle { get; set; }
        public long? Duration { get; set; }
        public bool IsMuted { get; set; }
        public int? Volume { get; set; }
        public DateTime? AirDate { get; set; }
        public string NowViewingName { get; set; }
        public string NowViewingSeriesName { get; set; }
        public string NowViewingArtists { get; set; }
        public string NowViewingAlbum { get; set; }
        public string NowViewingMediaType { get; set; }
        public string AudioCodec { get; set; }
        public string AudioProfile { get; set; }
        public string AudioChannels { get; set; }
        public string VideoCodec { get; set; }
        public string VideoHeight { get; set; }

    }
}
