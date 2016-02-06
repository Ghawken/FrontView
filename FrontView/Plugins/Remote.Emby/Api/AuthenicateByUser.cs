using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api
{
    public class Configuration
    {
        public string AudioLanguagePreference { get; set; }
        public bool PlayDefaultAudioTrack { get; set; }
        public string SubtitleLanguagePreference { get; set; }
        public bool DisplayMissingEpisodes { get; set; }
        public bool DisplayUnairedEpisodes { get; set; }
        public bool GroupMoviesIntoBoxSets { get; set; }
        public List<object> GroupedFolders { get; set; }
        public string SubtitleMode { get; set; }
        public bool DisplayCollectionsView { get; set; }
        public bool DisplayFoldersView { get; set; }
        public bool EnableLocalPassword { get; set; }
        public List<object> OrderedViews { get; set; }
        public bool IncludeTrailersInSuggestions { get; set; }
        public bool EnableCinemaMode { get; set; }
        public List<object> LatestItemsExcludes { get; set; }
        public List<object> PlainFolderViews { get; set; }
        public bool HidePlayedInLatest { get; set; }
    }

    public class Policy
    {
        public bool IsAdministrator { get; set; }
        public bool IsHidden { get; set; }
        public bool IsDisabled { get; set; }
        public List<object> BlockedTags { get; set; }
        public bool EnableUserPreferenceAccess { get; set; }
        public List<object> AccessSchedules { get; set; }
        public List<object> BlockUnratedItems { get; set; }
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
        public List<object> EnabledDevices { get; set; }
        public bool EnableAllDevices { get; set; }
        public List<object> EnabledChannels { get; set; }
        public bool EnableAllChannels { get; set; }
        public List<object> EnabledFolders { get; set; }
        public bool EnableAllFolders { get; set; }
        public int InvalidLoginAttemptCount { get; set; }
        public bool EnablePublicSharing { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string ServerId { get; set; }
        public string Id { get; set; }
        public bool HasPassword { get; set; }
        public bool HasConfiguredPassword { get; set; }
        public bool HasConfiguredEasyPassword { get; set; }
        public string LastLoginDate { get; set; }
        public string LastActivityDate { get; set; }
        public Configuration Configuration { get; set; }
        public Policy Policy { get; set; }
    }

    public class PlayState
    {
        public bool CanSeek { get; set; }
        public bool IsPaused { get; set; }
        public bool IsMuted { get; set; }
        public string RepeatMode { get; set; }
    }

    public class SessionInfo
    {
        public List<object> SupportedCommands { get; set; }
        public List<object> QueueableMediaTypes { get; set; }
        public List<object> PlayableMediaTypes { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<object> AdditionalUsers { get; set; }
        public string ApplicationVersion { get; set; }
        public string Client { get; set; }
        public string LastActivityDate { get; set; }
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public bool SupportsRemoteControl { get; set; }
        public PlayState PlayState { get; set; }
    }

    public class RootObject
    {
        public User User { get; set; }
        public SessionInfo SessionInfo { get; set; }
        public string AccessToken { get; set; }
        public string ServerId { get; set; }
    }
}