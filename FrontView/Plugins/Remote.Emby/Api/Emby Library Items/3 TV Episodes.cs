using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.TVEpisodes
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
        public DateTime PremiereDate { get; set; }
        public long RunTimeTicks { get; set; }
        public string PlayAccess { get; set; }
        public int ProductionYear { get; set; }
        public bool IsPlaceHolder { get; set; }
        public int IndexNumber { get; set; }
        public int ParentIndexNumber { get; set; }
        public bool IsHD { get; set; }
        public bool IsFolder { get; set; }
        public string Type { get; set; }
        public string ParentLogoItemId { get; set; }
        public string ParentBackdropItemId { get; set; }
        public string[] ParentBackdropImageTags { get; set; }
        public Userdata UserData { get; set; }
        public string SeriesName { get; set; }
        public string SeriesId { get; set; }
        public string SeasonId { get; set; }
        public string SeriesPrimaryImageTag { get; set; }
        public string VideoType { get; set; }
        public Imagetags ImageTags { get; set; }
        public object[] BackdropImageTags { get; set; }
        public string ParentLogoImageTag { get; set; }
        public string SeriesThumbImageTag { get; set; }
        public string ParentThumbItemId { get; set; }
        public string ParentThumbImageTag { get; set; }
        public string LocationType { get; set; }
        public string MediaType { get; set; }
        public float CommunityRating { get; set; }
        public bool HasSubtitles { get; set; }
        public string OfficialRating { get; set; }
        public int IndexNumberEnd { get; set; }
    }

    public class Userdata
    {
        public long PlaybackPositionTicks { get; set; }
        public int PlayCount { get; set; }
        public bool IsFavorite { get; set; }
        public bool Played { get; set; }
        public string Key { get; set; }
        public DateTime LastPlayedDate { get; set; }
        public float PlayedPercentage { get; set; }
    }

    public class Imagetags
    {
        public string Primary { get; set; }
    }

}
