using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Plex.Api
{


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class MediaContainer
    {

        private MediaContainerVideo[] videoField;

        private string sizeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Video")]
        public MediaContainerVideo[] Video
        {
            get
            {
                return this.videoField;
            }
            set
            {
                this.videoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideo
    {

        private MediaContainerVideoMedia mediaField;

        private MediaContainerVideoGenre[] genreField;

        private MediaContainerVideoDirector[] directorField;

        private MediaContainerVideoWriter[] writerField;

        private MediaContainerVideoCountry[] countryField;

        private MediaContainerVideoCollection[] collectionField;

        private MediaContainerVideoField[] fieldField;

        private MediaContainerVideoUser userField;

        private MediaContainerVideoPlayer playerField;

        private MediaContainerVideoSession sessionField;

        private MediaContainerVideoTranscodeSession transcodeSessionField;

        private string addedAtField;

        private string artField;

        private string contentRatingField;

        private string durationField;

        private string grandparentArtField;

        private string grandparentKeyField;

        private string grandparentRatingKeyField;

        private bool grandparentRatingKeyFieldSpecified;

        private string grandparentThumbField;

        private string grandparentTitleField;

        private string guidField;

        private string indexField;

        private bool indexFieldSpecified;

        private string keyField;

        private string lastViewedAtField;

        private bool lastViewedAtFieldSpecified;

        private string librarySectionIDField;

        private string librarySectionKeyField;

        private string parentIndexField;

        private bool parentIndexFieldSpecified;

        private string parentKeyField;

        private string parentRatingKeyField;

        private bool parentRatingKeyFieldSpecified;

        private string parentThumbField;

        private string parentTitleField;

        private string ratingKeyField;

        private string sessionKeyField;

        private string summaryField;

        private string thumbField;

        private string titleField;

        private string typeField;

        private string updatedAtField;

        private string viewOffsetField;

        private string chapterSourceField;

        private string originalTitleField;

        private System.DateTime originallyAvailableAtField;

        private bool originallyAvailableAtFieldSpecified;

        private string ratingField;

        private bool ratingFieldSpecified;

        private string studioField;

        private string taglineField;

        private string titleSortField;

        private string yearField;

        private bool yearFieldSpecified;

        private string viewCountField;

        private bool viewCountFieldSpecified;

        private string createdAtAccuracyField;

        private string createdAtTZOffsetField;

        private bool createdAtTZOffsetFieldSpecified;

        /// <remarks/>
        public MediaContainerVideoMedia Media
        {
            get
            {
                return this.mediaField;
            }
            set
            {
                this.mediaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Genre")]
        public MediaContainerVideoGenre[] Genre
        {
            get
            {
                return this.genreField;
            }
            set
            {
                this.genreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Director")]
        public MediaContainerVideoDirector[] Director
        {
            get
            {
                return this.directorField;
            }
            set
            {
                this.directorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Writer")]
        public MediaContainerVideoWriter[] Writer
        {
            get
            {
                return this.writerField;
            }
            set
            {
                this.writerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Country")]
        public MediaContainerVideoCountry[] Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Collection")]
        public MediaContainerVideoCollection[] Collection
        {
            get
            {
                return this.collectionField;
            }
            set
            {
                this.collectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Field")]
        public MediaContainerVideoField[] Field
        {
            get
            {
                return this.fieldField;
            }
            set
            {
                this.fieldField = value;
            }
        }

        /// <remarks/>
        public MediaContainerVideoUser User
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

        /// <remarks/>
        public MediaContainerVideoPlayer Player
        {
            get
            {
                return this.playerField;
            }
            set
            {
                this.playerField = value;
            }
        }

        /// <remarks/>
        public MediaContainerVideoSession Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }

        /// <remarks/>
        public MediaContainerVideoTranscodeSession TranscodeSession
        {
            get
            {
                return this.transcodeSessionField;
            }
            set
            {
                this.transcodeSessionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string addedAt
        {
            get
            {
                return this.addedAtField;
            }
            set
            {
                this.addedAtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string art
        {
            get
            {
                return this.artField;
            }
            set
            {
                this.artField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contentRating
        {
            get
            {
                return this.contentRatingField;
            }
            set
            {
                this.contentRatingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string grandparentArt
        {
            get
            {
                return this.grandparentArtField;
            }
            set
            {
                this.grandparentArtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string grandparentKey
        {
            get
            {
                return this.grandparentKeyField;
            }
            set
            {
                this.grandparentKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string grandparentRatingKey
        {
            get
            {
                return this.grandparentRatingKeyField;
            }
            set
            {
                this.grandparentRatingKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool grandparentRatingKeySpecified
        {
            get
            {
                return this.grandparentRatingKeyFieldSpecified;
            }
            set
            {
                this.grandparentRatingKeyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string grandparentThumb
        {
            get
            {
                return this.grandparentThumbField;
            }
            set
            {
                this.grandparentThumbField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string grandparentTitle
        {
            get
            {
                return this.grandparentTitleField;
            }
            set
            {
                this.grandparentTitleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string guid
        {
            get
            {
                return this.guidField;
            }
            set
            {
                this.guidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string index
        {
            get
            {
                return this.indexField;
            }
            set
            {
                this.indexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool indexSpecified
        {
            get
            {
                return this.indexFieldSpecified;
            }
            set
            {
                this.indexFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lastViewedAt
        {
            get
            {
                return this.lastViewedAtField;
            }
            set
            {
                this.lastViewedAtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastViewedAtSpecified
        {
            get
            {
                return this.lastViewedAtFieldSpecified;
            }
            set
            {
                this.lastViewedAtFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string librarySectionID
        {
            get
            {
                return this.librarySectionIDField;
            }
            set
            {
                this.librarySectionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string librarySectionKey
        {
            get
            {
                return this.librarySectionKeyField;
            }
            set
            {
                this.librarySectionKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentIndex
        {
            get
            {
                return this.parentIndexField;
            }
            set
            {
                this.parentIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool parentIndexSpecified
        {
            get
            {
                return this.parentIndexFieldSpecified;
            }
            set
            {
                this.parentIndexFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentKey
        {
            get
            {
                return this.parentKeyField;
            }
            set
            {
                this.parentKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentRatingKey
        {
            get
            {
                return this.parentRatingKeyField;
            }
            set
            {
                this.parentRatingKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool parentRatingKeySpecified
        {
            get
            {
                return this.parentRatingKeyFieldSpecified;
            }
            set
            {
                this.parentRatingKeyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentThumb
        {
            get
            {
                return this.parentThumbField;
            }
            set
            {
                this.parentThumbField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentTitle
        {
            get
            {
                return this.parentTitleField;
            }
            set
            {
                this.parentTitleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ratingKey
        {
            get
            {
                return this.ratingKeyField;
            }
            set
            {
                this.ratingKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sessionKey
        {
            get
            {
                return this.sessionKeyField;
            }
            set
            {
                this.sessionKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string thumb
        {
            get
            {
                return this.thumbField;
            }
            set
            {
                this.thumbField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string updatedAt
        {
            get
            {
                return this.updatedAtField;
            }
            set
            {
                this.updatedAtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string viewOffset
        {
            get
            {
                return this.viewOffsetField;
            }
            set
            {
                this.viewOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string chapterSource
        {
            get
            {
                return this.chapterSourceField;
            }
            set
            {
                this.chapterSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string originalTitle
        {
            get
            {
                return this.originalTitleField;
            }
            set
            {
                this.originalTitleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime originallyAvailableAt
        {
            get
            {
                return this.originallyAvailableAtField;
            }
            set
            {
                this.originallyAvailableAtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool originallyAvailableAtSpecified
        {
            get
            {
                return this.originallyAvailableAtFieldSpecified;
            }
            set
            {
                this.originallyAvailableAtFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rating
        {
            get
            {
                return this.ratingField;
            }
            set
            {
                this.ratingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ratingSpecified
        {
            get
            {
                return this.ratingFieldSpecified;
            }
            set
            {
                this.ratingFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string studio
        {
            get
            {
                return this.studioField;
            }
            set
            {
                this.studioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tagline
        {
            get
            {
                return this.taglineField;
            }
            set
            {
                this.taglineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string titleSort
        {
            get
            {
                return this.titleSortField;
            }
            set
            {
                this.titleSortField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool yearSpecified
        {
            get
            {
                return this.yearFieldSpecified;
            }
            set
            {
                this.yearFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string viewCount
        {
            get
            {
                return this.viewCountField;
            }
            set
            {
                this.viewCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool viewCountSpecified
        {
            get
            {
                return this.viewCountFieldSpecified;
            }
            set
            {
                this.viewCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string createdAtAccuracy
        {
            get
            {
                return this.createdAtAccuracyField;
            }
            set
            {
                this.createdAtAccuracyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string createdAtTZOffset
        {
            get
            {
                return this.createdAtTZOffsetField;
            }
            set
            {
                this.createdAtTZOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool createdAtTZOffsetSpecified
        {
            get
            {
                return this.createdAtTZOffsetFieldSpecified;
            }
            set
            {
                this.createdAtTZOffsetFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoMedia
    {

        private MediaContainerVideoMediaPart partField;

        private string aspectRatioField;

        private bool aspectRatioFieldSpecified;

        private string audioChannelsField;

        private string audioCodecField;

        private string bitrateField;

        private string containerField;

        private string durationField;

        private string heightField;

        private string idField;

        private string videoCodecField;

        private string videoFrameRateField;

        private string videoProfileField;

        private string videoResolutionField;

        private string widthField;

        private string selectedField;

        private string audioProfileField;

        /// <remarks/>
        public MediaContainerVideoMediaPart Part
        {
            get
            {
                return this.partField;
            }
            set
            {
                this.partField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string aspectRatio
        {
            get
            {
                return this.aspectRatioField;
            }
            set
            {
                this.aspectRatioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool aspectRatioSpecified
        {
            get
            {
                return this.aspectRatioFieldSpecified;
            }
            set
            {
                this.aspectRatioFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioChannels
        {
            get
            {
                return this.audioChannelsField;
            }
            set
            {
                this.audioChannelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioCodec
        {
            get
            {
                return this.audioCodecField;
            }
            set
            {
                this.audioCodecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bitrate
        {
            get
            {
                return this.bitrateField;
            }
            set
            {
                this.bitrateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string container
        {
            get
            {
                return this.containerField;
            }
            set
            {
                this.containerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoCodec
        {
            get
            {
                return this.videoCodecField;
            }
            set
            {
                this.videoCodecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoFrameRate
        {
            get
            {
                return this.videoFrameRateField;
            }
            set
            {
                this.videoFrameRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoProfile
        {
            get
            {
                return this.videoProfileField;
            }
            set
            {
                this.videoProfileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoResolution
        {
            get
            {
                return this.videoResolutionField;
            }
            set
            {
                this.videoResolutionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string selected
        {
            get
            {
                return this.selectedField;
            }
            set
            {
                this.selectedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioProfile
        {
            get
            {
                return this.audioProfileField;
            }
            set
            {
                this.audioProfileField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoMediaPart
    {

        private MediaContainerVideoMediaPartStream[] streamField;

        private string containerField;

        private string durationField;

        private string fileField;

        private string idField;

        private string keyField;

        private string sizeField;

        private bool sizeFieldSpecified;

        private string videoProfileField;

        private string decisionField;

        private string selectedField;

        private string audioProfileField;

        private string bitrateField;

        private bool bitrateFieldSpecified;

        private string heightField;

        private bool heightFieldSpecified;

        private string widthField;

        private bool widthFieldSpecified;

        private string packetLengthField;

        private bool packetLengthFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Stream")]
        public MediaContainerVideoMediaPartStream[] Stream
        {
            get
            {
                return this.streamField;
            }
            set
            {
                this.streamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string container
        {
            get
            {
                return this.containerField;
            }
            set
            {
                this.containerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string file
        {
            get
            {
                return this.fileField;
            }
            set
            {
                this.fileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sizeSpecified
        {
            get
            {
                return this.sizeFieldSpecified;
            }
            set
            {
                this.sizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoProfile
        {
            get
            {
                return this.videoProfileField;
            }
            set
            {
                this.videoProfileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string decision
        {
            get
            {
                return this.decisionField;
            }
            set
            {
                this.decisionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string selected
        {
            get
            {
                return this.selectedField;
            }
            set
            {
                this.selectedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioProfile
        {
            get
            {
                return this.audioProfileField;
            }
            set
            {
                this.audioProfileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bitrate
        {
            get
            {
                return this.bitrateField;
            }
            set
            {
                this.bitrateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool bitrateSpecified
        {
            get
            {
                return this.bitrateFieldSpecified;
            }
            set
            {
                this.bitrateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool widthSpecified
        {
            get
            {
                return this.widthFieldSpecified;
            }
            set
            {
                this.widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string packetLength
        {
            get
            {
                return this.packetLengthField;
            }
            set
            {
                this.packetLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool packetLengthSpecified
        {
            get
            {
                return this.packetLengthFieldSpecified;
            }
            set
            {
                this.packetLengthFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoMediaPartStream
    {

        private string bitDepthField;

        private bool bitDepthFieldSpecified;

        private string bitrateField;

        private bool bitrateFieldSpecified;

        private string cabacField;

        private bool cabacFieldSpecified;

        private string chromaSubsamplingField;

        private string codecField;

        private string codecIDField;

        private string defaultField;

        private bool defaultFieldSpecified;

        private string durationField;

        private bool durationFieldSpecified;

        private string frameRateField;

        private bool frameRateFieldSpecified;

        private string frameRateModeField;

        private string hasScalingMatrixField;

        private bool hasScalingMatrixFieldSpecified;

        private string heightField;

        private bool heightFieldSpecified;

        private string idField;

        private string indexField;

        private bool indexFieldSpecified;

        private string languageField;

        private string languageCodeField;

        private string levelField;

        private bool levelFieldSpecified;

        private string pixelFormatField;

        private string profileField;

        private string refFramesField;

        private bool refFramesFieldSpecified;

        private string scanTypeField;

        private string streamTypeField;

        private string widthField;

        private bool widthFieldSpecified;

        private string audioChannelLayoutField;

        private string bitrateModeField;

        private string channelsField;

        private bool channelsFieldSpecified;

        private string dialogNormField;

        private bool dialogNormFieldSpecified;

        private string samplingRateField;

        private bool samplingRateFieldSpecified;

        private string selectedField;

        private bool selectedFieldSpecified;

        private string decisionField;

        private string colorRangeField;

        private string colorSpaceField;

        private string streamIdentifierField;

        private bool streamIdentifierFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bitDepth
        {
            get
            {
                return this.bitDepthField;
            }
            set
            {
                this.bitDepthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool bitDepthSpecified
        {
            get
            {
                return this.bitDepthFieldSpecified;
            }
            set
            {
                this.bitDepthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bitrate
        {
            get
            {
                return this.bitrateField;
            }
            set
            {
                this.bitrateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool bitrateSpecified
        {
            get
            {
                return this.bitrateFieldSpecified;
            }
            set
            {
                this.bitrateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cabac
        {
            get
            {
                return this.cabacField;
            }
            set
            {
                this.cabacField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool cabacSpecified
        {
            get
            {
                return this.cabacFieldSpecified;
            }
            set
            {
                this.cabacFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string chromaSubsampling
        {
            get
            {
                return this.chromaSubsamplingField;
            }
            set
            {
                this.chromaSubsamplingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codec
        {
            get
            {
                return this.codecField;
            }
            set
            {
                this.codecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codecID
        {
            get
            {
                return this.codecIDField;
            }
            set
            {
                this.codecIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @default
        {
            get
            {
                return this.defaultField;
            }
            set
            {
                this.defaultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool defaultSpecified
        {
            get
            {
                return this.defaultFieldSpecified;
            }
            set
            {
                this.defaultFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool durationSpecified
        {
            get
            {
                return this.durationFieldSpecified;
            }
            set
            {
                this.durationFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string frameRate
        {
            get
            {
                return this.frameRateField;
            }
            set
            {
                this.frameRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool frameRateSpecified
        {
            get
            {
                return this.frameRateFieldSpecified;
            }
            set
            {
                this.frameRateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string frameRateMode
        {
            get
            {
                return this.frameRateModeField;
            }
            set
            {
                this.frameRateModeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string hasScalingMatrix
        {
            get
            {
                return this.hasScalingMatrixField;
            }
            set
            {
                this.hasScalingMatrixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hasScalingMatrixSpecified
        {
            get
            {
                return this.hasScalingMatrixFieldSpecified;
            }
            set
            {
                this.hasScalingMatrixFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string index
        {
            get
            {
                return this.indexField;
            }
            set
            {
                this.indexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool indexSpecified
        {
            get
            {
                return this.indexFieldSpecified;
            }
            set
            {
                this.indexFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string languageCode
        {
            get
            {
                return this.languageCodeField;
            }
            set
            {
                this.languageCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool levelSpecified
        {
            get
            {
                return this.levelFieldSpecified;
            }
            set
            {
                this.levelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pixelFormat
        {
            get
            {
                return this.pixelFormatField;
            }
            set
            {
                this.pixelFormatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string profile
        {
            get
            {
                return this.profileField;
            }
            set
            {
                this.profileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string refFrames
        {
            get
            {
                return this.refFramesField;
            }
            set
            {
                this.refFramesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool refFramesSpecified
        {
            get
            {
                return this.refFramesFieldSpecified;
            }
            set
            {
                this.refFramesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string scanType
        {
            get
            {
                return this.scanTypeField;
            }
            set
            {
                this.scanTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string streamType
        {
            get
            {
                return this.streamTypeField;
            }
            set
            {
                this.streamTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool widthSpecified
        {
            get
            {
                return this.widthFieldSpecified;
            }
            set
            {
                this.widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioChannelLayout
        {
            get
            {
                return this.audioChannelLayoutField;
            }
            set
            {
                this.audioChannelLayoutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bitrateMode
        {
            get
            {
                return this.bitrateModeField;
            }
            set
            {
                this.bitrateModeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string channels
        {
            get
            {
                return this.channelsField;
            }
            set
            {
                this.channelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool channelsSpecified
        {
            get
            {
                return this.channelsFieldSpecified;
            }
            set
            {
                this.channelsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dialogNorm
        {
            get
            {
                return this.dialogNormField;
            }
            set
            {
                this.dialogNormField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dialogNormSpecified
        {
            get
            {
                return this.dialogNormFieldSpecified;
            }
            set
            {
                this.dialogNormFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string samplingRate
        {
            get
            {
                return this.samplingRateField;
            }
            set
            {
                this.samplingRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool samplingRateSpecified
        {
            get
            {
                return this.samplingRateFieldSpecified;
            }
            set
            {
                this.samplingRateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string selected
        {
            get
            {
                return this.selectedField;
            }
            set
            {
                this.selectedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool selectedSpecified
        {
            get
            {
                return this.selectedFieldSpecified;
            }
            set
            {
                this.selectedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string decision
        {
            get
            {
                return this.decisionField;
            }
            set
            {
                this.decisionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string colorRange
        {
            get
            {
                return this.colorRangeField;
            }
            set
            {
                this.colorRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string colorSpace
        {
            get
            {
                return this.colorSpaceField;
            }
            set
            {
                this.colorSpaceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string streamIdentifier
        {
            get
            {
                return this.streamIdentifierField;
            }
            set
            {
                this.streamIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool streamIdentifierSpecified
        {
            get
            {
                return this.streamIdentifierFieldSpecified;
            }
            set
            {
                this.streamIdentifierFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoGenre
    {

        private string countField;

        private string filterField;

        private string idField;

        private string tagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string filter
        {
            get
            {
                return this.filterField;
            }
            set
            {
                this.filterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoDirector
    {

        private string countField;

        private bool countFieldSpecified;

        private string filterField;

        private string idField;

        private string tagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool countSpecified
        {
            get
            {
                return this.countFieldSpecified;
            }
            set
            {
                this.countFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string filter
        {
            get
            {
                return this.filterField;
            }
            set
            {
                this.filterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoWriter
    {

        private string filterField;

        private string idField;

        private string tagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string filter
        {
            get
            {
                return this.filterField;
            }
            set
            {
                this.filterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoCountry
    {

        private string filterField;

        private string idField;

        private string tagField;

        private string countField;

        private bool countFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string filter
        {
            get
            {
                return this.filterField;
            }
            set
            {
                this.filterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool countSpecified
        {
            get
            {
                return this.countFieldSpecified;
            }
            set
            {
                this.countFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoCollection
    {

        private string filterField;

        private string idField;

        private string tagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string filter
        {
            get
            {
                return this.filterField;
            }
            set
            {
                this.filterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tag
        {
            get
            {
                return this.tagField;
            }
            set
            {
                this.tagField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoField
    {

        private string lockedField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string locked
        {
            get
            {
                return this.lockedField;
            }
            set
            {
                this.lockedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoUser
    {

        private string idField;

        private string thumbField;

        private string titleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string thumb
        {
            get
            {
                return this.thumbField;
            }
            set
            {
                this.thumbField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoPlayer
    {

        private string addressField;

        private string deviceField;

        private string machineIdentifierField;

        private string modelField;

        private string platformField;

        private string platformVersionField;

        private string productField;

        private string profileField;

        private string remotePublicAddressField;

        private string stateField;

        private string titleField;

        private string vendorField;

        private string versionField;

        private string localField;

        private string userIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string device
        {
            get
            {
                return this.deviceField;
            }
            set
            {
                this.deviceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string machineIdentifier
        {
            get
            {
                return this.machineIdentifierField;
            }
            set
            {
                this.machineIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string platform
        {
            get
            {
                return this.platformField;
            }
            set
            {
                this.platformField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string platformVersion
        {
            get
            {
                return this.platformVersionField;
            }
            set
            {
                this.platformVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string product
        {
            get
            {
                return this.productField;
            }
            set
            {
                this.productField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string profile
        {
            get
            {
                return this.profileField;
            }
            set
            {
                this.profileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string remotePublicAddress
        {
            get
            {
                return this.remotePublicAddressField;
            }
            set
            {
                this.remotePublicAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string vendor
        {
            get
            {
                return this.vendorField;
            }
            set
            {
                this.vendorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string local
        {
            get
            {
                return this.localField;
            }
            set
            {
                this.localField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string userID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoSession
    {

        private string idField;

        private string bandwidthField;

        private string locationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bandwidth
        {
            get
            {
                return this.bandwidthField;
            }
            set
            {
                this.bandwidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoTranscodeSession
    {

        private string keyField;

        private string throttledField;

        private string completeField;

        private string progressField;

        private string speedField;

        private string durationField;

        private string remainingField;

        private string contextField;

        private string sourceVideoCodecField;

        private string sourceAudioCodecField;

        private string videoDecisionField;

        private string audioDecisionField;

        private string protocolField;

        private string containerField;

        private string videoCodecField;

        private string audioCodecField;

        private string audioChannelsField;

        private string transcodeHwRequestedField;

        private string transcodeHwFullPipelineField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string throttled
        {
            get
            {
                return this.throttledField;
            }
            set
            {
                this.throttledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string complete
        {
            get
            {
                return this.completeField;
            }
            set
            {
                this.completeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string progress
        {
            get
            {
                return this.progressField;
            }
            set
            {
                this.progressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string speed
        {
            get
            {
                return this.speedField;
            }
            set
            {
                this.speedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string remaining
        {
            get
            {
                return this.remainingField;
            }
            set
            {
                this.remainingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string context
        {
            get
            {
                return this.contextField;
            }
            set
            {
                this.contextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sourceVideoCodec
        {
            get
            {
                return this.sourceVideoCodecField;
            }
            set
            {
                this.sourceVideoCodecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sourceAudioCodec
        {
            get
            {
                return this.sourceAudioCodecField;
            }
            set
            {
                this.sourceAudioCodecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoDecision
        {
            get
            {
                return this.videoDecisionField;
            }
            set
            {
                this.videoDecisionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioDecision
        {
            get
            {
                return this.audioDecisionField;
            }
            set
            {
                this.audioDecisionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string protocol
        {
            get
            {
                return this.protocolField;
            }
            set
            {
                this.protocolField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string container
        {
            get
            {
                return this.containerField;
            }
            set
            {
                this.containerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string videoCodec
        {
            get
            {
                return this.videoCodecField;
            }
            set
            {
                this.videoCodecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioCodec
        {
            get
            {
                return this.audioCodecField;
            }
            set
            {
                this.audioCodecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string audioChannels
        {
            get
            {
                return this.audioChannelsField;
            }
            set
            {
                this.audioChannelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcodeHwRequested
        {
            get
            {
                return this.transcodeHwRequestedField;
            }
            set
            {
                this.transcodeHwRequestedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transcodeHwFullPipeline
        {
            get
            {
                return this.transcodeHwFullPipelineField;
            }
            set
            {
                this.transcodeHwFullPipelineField = value;
            }
        }
    }






}
