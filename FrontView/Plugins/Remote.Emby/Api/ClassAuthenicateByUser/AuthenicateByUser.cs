using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.AuthenicateByUser
{
    public class Root
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users", IsNullable = false)]
        public partial class AuthenticationResult
        {

            private string accessTokenField;

            private string serverIdField;

            private AuthenticationResultSessionInfo sessionInfoField;

            private AuthenticationResultUser userField;

            /// <remarks/>
            public string AccessToken
            {
                get
                {
                    return this.accessTokenField;
                }
                set
                {
                    this.accessTokenField = value;
                }
            }

            /// <remarks/>
            public string ServerId
            {
                get
                {
                    return this.serverIdField;
                }
                set
                {
                    this.serverIdField = value;
                }
            }

            /// <remarks/>
            public AuthenticationResultSessionInfo SessionInfo
            {
                get
                {
                    return this.sessionInfoField;
                }
                set
                {
                    this.sessionInfoField = value;
                }
            }

            /// <remarks/>
            public AuthenticationResultUser User
            {
                get
                {
                    return this.userField;
                }
                set
                {
                    this.userField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
        public partial class AuthenticationResultSessionInfo
        {

            private object additionalUsersField;

            private object appIconUrlField;

            private string applicationVersionField;

            private string clientField;

            private string deviceIdField;

            private string deviceNameField;

            private string idField;

            private System.DateTime lastActivityDateField;

            private object nowPlayingItemField;

            private object nowViewingItemField;

            private PlayState playStateField;

            private object playableMediaTypesField;

            private object queueableMediaTypesField;

            private object supportedCommandsField;

            private bool supportsRemoteControlField;

            private object transcodingInfoField;

            private string userIdField;

            private string userNameField;

            private object userPrimaryImageTagField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public object AdditionalUsers
            {
                get
                {
                    return this.additionalUsersField;
                }
                set
                {
                    this.additionalUsersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session", IsNullable = true)]
            public object AppIconUrl
            {
                get
                {
                    return this.appIconUrlField;
                }
                set
                {
                    this.appIconUrlField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public string ApplicationVersion
            {
                get
                {
                    return this.applicationVersionField;
                }
                set
                {
                    this.applicationVersionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public string Client
            {
                get
                {
                    return this.clientField;
                }
                set
                {
                    this.clientField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public string DeviceId
            {
                get
                {
                    return this.deviceIdField;
                }
                set
                {
                    this.deviceIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public string DeviceName
            {
                get
                {
                    return this.deviceNameField;
                }
                set
                {
                    this.deviceNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public System.DateTime LastActivityDate
            {
                get
                {
                    return this.lastActivityDateField;
                }
                set
                {
                    this.lastActivityDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session", IsNullable = true)]
            public object NowPlayingItem
            {
                get
                {
                    return this.nowPlayingItemField;
                }
                set
                {
                    this.nowPlayingItemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session", IsNullable = true)]
            public object NowViewingItem
            {
                get
                {
                    return this.nowViewingItemField;
                }
                set
                {
                    this.nowViewingItemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public PlayState PlayState
            {
                get
                {
                    return this.playStateField;
                }
                set
                {
                    this.playStateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public object PlayableMediaTypes
            {
                get
                {
                    return this.playableMediaTypesField;
                }
                set
                {
                    this.playableMediaTypesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public object QueueableMediaTypes
            {
                get
                {
                    return this.queueableMediaTypesField;
                }
                set
                {
                    this.queueableMediaTypesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public object SupportedCommands
            {
                get
                {
                    return this.supportedCommandsField;
                }
                set
                {
                    this.supportedCommandsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public bool SupportsRemoteControl
            {
                get
                {
                    return this.supportsRemoteControlField;
                }
                set
                {
                    this.supportsRemoteControlField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session", IsNullable = true)]
            public object TranscodingInfo
            {
                get
                {
                    return this.transcodingInfoField;
                }
                set
                {
                    this.transcodingInfoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public string UserId
            {
                get
                {
                    return this.userIdField;
                }
                set
                {
                    this.userIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
            public string UserName
            {
                get
                {
                    return this.userNameField;
                }
                set
                {
                    this.userNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session", IsNullable = true)]
            public object UserPrimaryImageTag
            {
                get
                {
                    return this.userPrimaryImageTagField;
                }
                set
                {
                    this.userPrimaryImageTagField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Session", IsNullable = false)]
        public partial class PlayState
        {

            private object audioStreamIndexField;

            private bool canSeekField;

            private bool isMutedField;

            private bool isPausedField;

            private object mediaSourceIdField;

            private object playMethodField;

            private object positionTicksField;

            private string repeatModeField;

            private object subtitleStreamIndexField;

            private object volumeLevelField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object AudioStreamIndex
            {
                get
                {
                    return this.audioStreamIndexField;
                }
                set
                {
                    this.audioStreamIndexField = value;
                }
            }

            /// <remarks/>
            public bool CanSeek
            {
                get
                {
                    return this.canSeekField;
                }
                set
                {
                    this.canSeekField = value;
                }
            }

            /// <remarks/>
            public bool IsMuted
            {
                get
                {
                    return this.isMutedField;
                }
                set
                {
                    this.isMutedField = value;
                }
            }

            /// <remarks/>
            public bool IsPaused
            {
                get
                {
                    return this.isPausedField;
                }
                set
                {
                    this.isPausedField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object MediaSourceId
            {
                get
                {
                    return this.mediaSourceIdField;
                }
                set
                {
                    this.mediaSourceIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object PlayMethod
            {
                get
                {
                    return this.playMethodField;
                }
                set
                {
                    this.playMethodField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object PositionTicks
            {
                get
                {
                    return this.positionTicksField;
                }
                set
                {
                    this.positionTicksField = value;
                }
            }

            /// <remarks/>
            public string RepeatMode
            {
                get
                {
                    return this.repeatModeField;
                }
                set
                {
                    this.repeatModeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object SubtitleStreamIndex
            {
                get
                {
                    return this.subtitleStreamIndexField;
                }
                set
                {
                    this.subtitleStreamIndexField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public object VolumeLevel
            {
                get
                {
                    return this.volumeLevelField;
                }
                set
                {
                    this.volumeLevelField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
        public partial class AuthenticationResultUser
        {

            private Configuration configurationField;

            private object connectLinkTypeField;

            private object connectUserIdField;

            private object connectUserNameField;

            private bool hasConfiguredEasyPasswordField;

            private bool hasConfiguredPasswordField;

            private bool hasPasswordField;

            private string idField;

            private System.DateTime lastActivityDateField;

            private System.DateTime lastLoginDateField;

            private string nameField;

            private object offlinePasswordField;

            private object offlinePasswordSaltField;

            private Policy policyField;

            private object primaryImageAspectRatioField;

            private object primaryImageTagField;

            private string serverIdField;

            private object serverNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public Configuration Configuration
            {
                get
                {
                    return this.configurationField;
                }
                set
                {
                    this.configurationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = true)]
            public object ConnectLinkType
            {
                get
                {
                    return this.connectLinkTypeField;
                }
                set
                {
                    this.connectLinkTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = true)]
            public object ConnectUserId
            {
                get
                {
                    return this.connectUserIdField;
                }
                set
                {
                    this.connectUserIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = true)]
            public object ConnectUserName
            {
                get
                {
                    return this.connectUserNameField;
                }
                set
                {
                    this.connectUserNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public bool HasConfiguredEasyPassword
            {
                get
                {
                    return this.hasConfiguredEasyPasswordField;
                }
                set
                {
                    this.hasConfiguredEasyPasswordField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public bool HasConfiguredPassword
            {
                get
                {
                    return this.hasConfiguredPasswordField;
                }
                set
                {
                    this.hasConfiguredPasswordField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public bool HasPassword
            {
                get
                {
                    return this.hasPasswordField;
                }
                set
                {
                    this.hasPasswordField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public System.DateTime LastActivityDate
            {
                get
                {
                    return this.lastActivityDateField;
                }
                set
                {
                    this.lastActivityDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public System.DateTime LastLoginDate
            {
                get
                {
                    return this.lastLoginDateField;
                }
                set
                {
                    this.lastLoginDateField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = true)]
            public object OfflinePassword
            {
                get
                {
                    return this.offlinePasswordField;
                }
                set
                {
                    this.offlinePasswordField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = true)]
            public object OfflinePasswordSalt
            {
                get
                {
                    return this.offlinePasswordSaltField;
                }
                set
                {
                    this.offlinePasswordSaltField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public Policy Policy
            {
                get
                {
                    return this.policyField;
                }
                set
                {
                    this.policyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = true)]
            public object PrimaryImageAspectRatio
            {
                get
                {
                    return this.primaryImageAspectRatioField;
                }
                set
                {
                    this.primaryImageAspectRatioField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = true)]
            public object PrimaryImageTag
            {
                get
                {
                    return this.primaryImageTagField;
                }
                set
                {
                    this.primaryImageTagField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
            public string ServerId
            {
                get
                {
                    return this.serverIdField;
                }
                set
                {
                    this.serverIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = true)]
            public object ServerName
            {
                get
                {
                    return this.serverNameField;
                }
                set
                {
                    this.serverNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = false)]
        public partial class Configuration
        {

            private string audioLanguagePreferenceField;

            private bool displayCollectionsViewField;

            private bool displayFoldersViewField;

            private bool displayMissingEpisodesField;

            private bool displayUnairedEpisodesField;

            private bool enableCinemaModeField;

            private bool enableLocalPasswordField;

            private object excludeFoldersFromGroupingField;

            private bool groupMoviesIntoBoxSetsField;

            private object groupedFoldersField;

            private bool hidePlayedInLatestField;

            private bool includeTrailersInSuggestionsField;

            private object latestItemsExcludesField;

            private object orderedViewsField;

            private object plainFolderViewsField;

            private bool playDefaultAudioTrackField;

            private string subtitleLanguagePreferenceField;

            private string subtitleModeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public string AudioLanguagePreference
            {
                get
                {
                    return this.audioLanguagePreferenceField;
                }
                set
                {
                    this.audioLanguagePreferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool DisplayCollectionsView
            {
                get
                {
                    return this.displayCollectionsViewField;
                }
                set
                {
                    this.displayCollectionsViewField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool DisplayFoldersView
            {
                get
                {
                    return this.displayFoldersViewField;
                }
                set
                {
                    this.displayFoldersViewField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool DisplayMissingEpisodes
            {
                get
                {
                    return this.displayMissingEpisodesField;
                }
                set
                {
                    this.displayMissingEpisodesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool DisplayUnairedEpisodes
            {
                get
                {
                    return this.displayUnairedEpisodesField;
                }
                set
                {
                    this.displayUnairedEpisodesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool EnableCinemaMode
            {
                get
                {
                    return this.enableCinemaModeField;
                }
                set
                {
                    this.enableCinemaModeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool EnableLocalPassword
            {
                get
                {
                    return this.enableLocalPasswordField;
                }
                set
                {
                    this.enableLocalPasswordField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration", IsNullable = true)]
            public object ExcludeFoldersFromGrouping
            {
                get
                {
                    return this.excludeFoldersFromGroupingField;
                }
                set
                {
                    this.excludeFoldersFromGroupingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool GroupMoviesIntoBoxSets
            {
                get
                {
                    return this.groupMoviesIntoBoxSetsField;
                }
                set
                {
                    this.groupMoviesIntoBoxSetsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public object GroupedFolders
            {
                get
                {
                    return this.groupedFoldersField;
                }
                set
                {
                    this.groupedFoldersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool HidePlayedInLatest
            {
                get
                {
                    return this.hidePlayedInLatestField;
                }
                set
                {
                    this.hidePlayedInLatestField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool IncludeTrailersInSuggestions
            {
                get
                {
                    return this.includeTrailersInSuggestionsField;
                }
                set
                {
                    this.includeTrailersInSuggestionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public object LatestItemsExcludes
            {
                get
                {
                    return this.latestItemsExcludesField;
                }
                set
                {
                    this.latestItemsExcludesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public object OrderedViews
            {
                get
                {
                    return this.orderedViewsField;
                }
                set
                {
                    this.orderedViewsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public object PlainFolderViews
            {
                get
                {
                    return this.plainFolderViewsField;
                }
                set
                {
                    this.plainFolderViewsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public bool PlayDefaultAudioTrack
            {
                get
                {
                    return this.playDefaultAudioTrackField;
                }
                set
                {
                    this.playDefaultAudioTrackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public string SubtitleLanguagePreference
            {
                get
                {
                    return this.subtitleLanguagePreferenceField;
                }
                set
                {
                    this.subtitleLanguagePreferenceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Configuration")]
            public string SubtitleMode
            {
                get
                {
                    return this.subtitleModeField;
                }
                set
                {
                    this.subtitleModeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Dto", IsNullable = false)]
        public partial class Policy
        {

            private object accessSchedulesField;

            private object blockUnratedItemsField;

            private object blockedChannelsField;

            private object blockedMediaFoldersField;

            private object blockedTagsField;

            private bool enableAllChannelsField;

            private bool enableAllDevicesField;

            private bool enableAllFoldersField;

            private bool enableAudioPlaybackTranscodingField;

            private bool enableContentDeletionField;

            private bool enableContentDownloadingField;

            private bool enableLiveTvAccessField;

            private bool enableLiveTvManagementField;

            private bool enableMediaPlaybackField;

            private bool enablePublicSharingField;

            private bool enableRemoteControlOfOtherUsersField;

            private bool enableSharedDeviceControlField;

            private bool enableSyncField;

            private bool enableSyncTranscodingField;

            private bool enableUserPreferenceAccessField;

            private bool enableVideoPlaybackTranscodingField;

            private object enabledChannelsField;

            private object enabledDevicesField;

            private object enabledFoldersField;

            private byte invalidLoginAttemptCountField;

            private bool isAdministratorField;

            private bool isDisabledField;

            private bool isHiddenField;

            private object maxParentalRatingField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object AccessSchedules
            {
                get
                {
                    return this.accessSchedulesField;
                }
                set
                {
                    this.accessSchedulesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object BlockUnratedItems
            {
                get
                {
                    return this.blockUnratedItemsField;
                }
                set
                {
                    this.blockUnratedItemsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users", IsNullable = true)]
            public object BlockedChannels
            {
                get
                {
                    return this.blockedChannelsField;
                }
                set
                {
                    this.blockedChannelsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users", IsNullable = true)]
            public object BlockedMediaFolders
            {
                get
                {
                    return this.blockedMediaFoldersField;
                }
                set
                {
                    this.blockedMediaFoldersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object BlockedTags
            {
                get
                {
                    return this.blockedTagsField;
                }
                set
                {
                    this.blockedTagsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableAllChannels
            {
                get
                {
                    return this.enableAllChannelsField;
                }
                set
                {
                    this.enableAllChannelsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableAllDevices
            {
                get
                {
                    return this.enableAllDevicesField;
                }
                set
                {
                    this.enableAllDevicesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableAllFolders
            {
                get
                {
                    return this.enableAllFoldersField;
                }
                set
                {
                    this.enableAllFoldersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableAudioPlaybackTranscoding
            {
                get
                {
                    return this.enableAudioPlaybackTranscodingField;
                }
                set
                {
                    this.enableAudioPlaybackTranscodingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableContentDeletion
            {
                get
                {
                    return this.enableContentDeletionField;
                }
                set
                {
                    this.enableContentDeletionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableContentDownloading
            {
                get
                {
                    return this.enableContentDownloadingField;
                }
                set
                {
                    this.enableContentDownloadingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableLiveTvAccess
            {
                get
                {
                    return this.enableLiveTvAccessField;
                }
                set
                {
                    this.enableLiveTvAccessField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableLiveTvManagement
            {
                get
                {
                    return this.enableLiveTvManagementField;
                }
                set
                {
                    this.enableLiveTvManagementField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableMediaPlayback
            {
                get
                {
                    return this.enableMediaPlaybackField;
                }
                set
                {
                    this.enableMediaPlaybackField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnablePublicSharing
            {
                get
                {
                    return this.enablePublicSharingField;
                }
                set
                {
                    this.enablePublicSharingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableRemoteControlOfOtherUsers
            {
                get
                {
                    return this.enableRemoteControlOfOtherUsersField;
                }
                set
                {
                    this.enableRemoteControlOfOtherUsersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableSharedDeviceControl
            {
                get
                {
                    return this.enableSharedDeviceControlField;
                }
                set
                {
                    this.enableSharedDeviceControlField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableSync
            {
                get
                {
                    return this.enableSyncField;
                }
                set
                {
                    this.enableSyncField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableSyncTranscoding
            {
                get
                {
                    return this.enableSyncTranscodingField;
                }
                set
                {
                    this.enableSyncTranscodingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableUserPreferenceAccess
            {
                get
                {
                    return this.enableUserPreferenceAccessField;
                }
                set
                {
                    this.enableUserPreferenceAccessField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool EnableVideoPlaybackTranscoding
            {
                get
                {
                    return this.enableVideoPlaybackTranscodingField;
                }
                set
                {
                    this.enableVideoPlaybackTranscodingField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object EnabledChannels
            {
                get
                {
                    return this.enabledChannelsField;
                }
                set
                {
                    this.enabledChannelsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object EnabledDevices
            {
                get
                {
                    return this.enabledDevicesField;
                }
                set
                {
                    this.enabledDevicesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public object EnabledFolders
            {
                get
                {
                    return this.enabledFoldersField;
                }
                set
                {
                    this.enabledFoldersField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public byte InvalidLoginAttemptCount
            {
                get
                {
                    return this.invalidLoginAttemptCountField;
                }
                set
                {
                    this.invalidLoginAttemptCountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool IsAdministrator
            {
                get
                {
                    return this.isAdministratorField;
                }
                set
                {
                    this.isAdministratorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool IsDisabled
            {
                get
                {
                    return this.isDisabledField;
                }
                set
                {
                    this.isDisabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users")]
            public bool IsHidden
            {
                get
                {
                    return this.isHiddenField;
                }
                set
                {
                    this.isHiddenField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.datacontract.org/2004/07/MediaBrowser.Model.Users", IsNullable = true)]
            public object MaxParentalRating
            {
                get
                {
                    return this.maxParentalRatingField;
                }
                set
                {
                    this.maxParentalRatingField = value;
                }
            }
        }



    }

}
