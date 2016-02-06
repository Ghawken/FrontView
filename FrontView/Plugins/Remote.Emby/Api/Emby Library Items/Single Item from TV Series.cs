﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.TVSingleItemSeries
{

    public class Rootobject
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string Id { get; set; }
        public string Etag { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastMediaAdded { get; set; }
        public bool DisplaySpecialsWithSeasons { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDownload { get; set; }
        public bool SupportsSync { get; set; }
        public bool HasSyncJob { get; set; }
        public bool IsSynced { get; set; }
        public string SortName { get; set; }
        public DateTime PremiereDate { get; set; }
        public Externalurl[] ExternalUrls { get; set; }
        public string Path { get; set; }
        public string OfficialRating { get; set; }
        public string Overview { get; set; }
        public object[] Taglines { get; set; }
        public string[] Genres { get; set; }
        public float CommunityRating { get; set; }
        public int VoteCount { get; set; }
        public long CumulativeRunTimeTicks { get; set; }
        public long RunTimeTicks { get; set; }
        public string PlayAccess { get; set; }
        public int ProductionYear { get; set; }
        public object[] RemoteTrailers { get; set; }
        public Providerids ProviderIds { get; set; }
        public bool IsFolder { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public Person[] People { get; set; }
        public Studio[] Studios { get; set; }
        public int LocalTrailerCount { get; set; }
        public Userdata UserData { get; set; }
        public int RecursiveItemCount { get; set; }
        public int ChildCount { get; set; }
        public string DisplayPreferencesId { get; set; }
        public string Status { get; set; }
        public string AirTime { get; set; }
        public string[] AirDays { get; set; }
        public string[] IndexOptions { get; set; }
        public object[] Tags { get; set; }
        public object[] Keywords { get; set; }
        public float PrimaryImageAspectRatio { get; set; }
        public Imagetags ImageTags { get; set; }
        public string[] BackdropImageTags { get; set; }
        public object[] ScreenshotImageTags { get; set; }
        public string LocationType { get; set; }
        public string HomePageUrl { get; set; }
        public object[] ProductionLocations { get; set; }
        public object[] LockedFields { get; set; }
        public bool LockData { get; set; }
    }

    public class Providerids
    {
        public string Imdb { get; set; }
        public string Zap2It { get; set; }
        public string Tmdb { get; set; }
        public string TvRage { get; set; }
        public string Tvdb { get; set; }
    }

    public class Userdata
    {
        public int PlayedPercentage { get; set; }
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
        public string Banner { get; set; }
        public string Logo { get; set; }
        public string Thumb { get; set; }
    }

    public class Externalurl
    {
        public string Name { get; set; }
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


}
