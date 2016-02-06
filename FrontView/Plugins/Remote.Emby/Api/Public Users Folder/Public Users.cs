using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.Public_Users_Folder
{

    public class Rootobject
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string ConnectUserName { get; set; }
        public string ConnectUserId { get; set; }
        public string ConnectLinkType { get; set; }
        public string Id { get; set; }
        public string PrimaryImageTag { get; set; }
        public bool HasPassword { get; set; }
        public bool HasConfiguredPassword { get; set; }
        public bool HasConfiguredEasyPassword { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public Configuration Configuration { get; set; }
        public Policy Policy { get; set; }
        public float PrimaryImageAspectRatio { get; set; }
    }

    public class Configuration
    {
        public bool PlayDefaultAudioTrack { get; set; }
        public bool DisplayMissingEpisodes { get; set; }
        public bool DisplayUnairedEpisodes { get; set; }
        public bool GroupMoviesIntoBoxSets { get; set; }
        public object[] GroupedFolders { get; set; }
        public string SubtitleMode { get; set; }
        public bool DisplayCollectionsView { get; set; }
        public bool DisplayFoldersView { get; set; }
        public bool EnableLocalPassword { get; set; }
        public object[] OrderedViews { get; set; }
        public bool IncludeTrailersInSuggestions { get; set; }
        public bool EnableCinemaMode { get; set; }
        public object[] LatestItemsExcludes { get; set; }
        public object[] PlainFolderViews { get; set; }
        public bool HidePlayedInLatest { get; set; }
        public string AudioLanguagePreference { get; set; }
        public string SubtitleLanguagePreference { get; set; }
    }

    public class Policy
    {
        public bool IsAdministrator { get; set; }
        public bool IsHidden { get; set; }
        public bool IsDisabled { get; set; }
        public object[] BlockedTags { get; set; }
        public bool EnableUserPreferenceAccess { get; set; }
        public object[] AccessSchedules { get; set; }
        public object[] BlockUnratedItems { get; set; }
        public bool EnableRemoteControlOfOtherUsers { get; set; }
        public bool EnableSharedDeviceControl { get; set; }
        public bool EnableLiveTvManagement { get; set; }
        public bool EnableLiveTvAccess { get; set; }
        public bool EnableMediaPlayback { get; set; }
        public bool EnableAudioPlaybackTranscoding { get; set; }
        public bool EnableVideoPlaybackTranscoding { get; set; }
        public bool EnableContentDeletion { get; set; }
        public bool EnableContentDownloading { get; set; }
        public bool EnableSync { get; set; }
        public bool EnableSyncTranscoding { get; set; }
        public object[] EnabledDevices { get; set; }
        public bool EnableAllDevices { get; set; }
        public object[] EnabledChannels { get; set; }
        public bool EnableAllChannels { get; set; }
        public string[] EnabledFolders { get; set; }
        public bool EnableAllFolders { get; set; }
        public int InvalidLoginAttemptCount { get; set; }
        public bool EnablePublicSharing { get; set; }
    }
}