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

        private int sizeField;

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
        public int size
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

        private MediaContainerVideoDirector directorField;

        private MediaContainerVideoWriter[] writerField;

        private MediaContainerVideoUser userField;

        private MediaContainerVideoPlayer playerField;

        private uint addedAtField;

        private string artField;

        private string chapterSourceField;

        private string contentRatingField;

        private uint durationField;

        private string grandparentArtField;

        private string grandparentKeyField;

        private uint grandparentRatingKeyField;

        private string grandparentThumbField;

        private string grandparentTitleField;

        private string guidField;

        private int indexField;

        private string keyField;

        private uint lastViewedAtField;

        private bool lastViewedAtFieldSpecified;

        private int librarySectionIDField;

        private System.DateTime originallyAvailableAtField;

        private int parentIndexField;

        private string parentKeyField;

        private uint parentRatingKeyField;

        private string parentThumbField;

        private uint ratingKeyField;

        private int sessionKeyField;

        private string summaryField;

        private string thumbField;

        private string titleField;

        private string typeField;

        private uint updatedAtField;

        private uint viewOffsetField;

        private uint yearField;

        private string titleSortField;

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
        public MediaContainerVideoDirector Director
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint addedAt
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
        public uint duration
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
        public uint grandparentRatingKey
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
        public int index
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
        public uint lastViewedAt
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
        public int librarySectionID
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int parentIndex
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
        public uint parentRatingKey
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
        public uint ratingKey
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
        public int sessionKey
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
        public uint updatedAt
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
        public uint viewOffset
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
        public uint year
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoMedia
    {

        private MediaContainerVideoMediaPart partField;

        private decimal aspectRatioField;

        private int audioChannelsField;

        private string audioCodecField;

        private ushort bitrateField;

        private string containerField;

        private uint durationField;

        private ushort heightField;

        private uint idField;

        private string videoCodecField;

        private string videoFrameRateField;

        private string videoProfileField;

        private string videoResolutionField;

        private ushort widthField;

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
        public decimal aspectRatio
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int audioChannels
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
        public ushort bitrate
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
        public uint duration
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
        public ushort height
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
        public uint id
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
        public ushort width
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoMediaPart
    {

        private MediaContainerVideoMediaPartStream[] streamField;

        private string containerField;

        private uint durationField;

        private string fileField;

        private uint idField;

        private string keyField;

        private UInt64 sizeField;

        private string videoProfileField;

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
        public uint duration
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
        public uint id
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
        public UInt64 size
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoMediaPartStream
    {

        private int bitDepthField;

        private ushort bitrateField;

        private int cabacField;

        private bool cabacFieldSpecified;

        private string chromaSubsamplingField;

        private string codecField;

        private string codecIDField;

        private int defaultField;

        private uint durationField;

        private decimal frameRateField;

        private bool frameRateFieldSpecified;

        private string frameRateModeField;

        private int hasScalingMatrixField;

        private bool hasScalingMatrixFieldSpecified;

        private ushort heightField;

        private bool heightFieldSpecified;

        private uint idField;

        private int indexField;

        private string languageField;

        private string languageCodeField;

        private int levelField;

        private bool levelFieldSpecified;

        private string pixelFormatField;

        private string profileField;

        private int refFramesField;

        private bool refFramesFieldSpecified;

        private string scanTypeField;

        private int streamTypeField;

        private ushort widthField;

        private bool widthFieldSpecified;

        private string audioChannelLayoutField;

        private string bitrateModeField;

        private int channelsField;

        private bool channelsFieldSpecified;

        private int dialogNormField;

        private bool dialogNormFieldSpecified;

        private uint samplingRateField;

        private bool samplingRateFieldSpecified;

        private int selectedField;

        private bool selectedFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int bitDepth
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort bitrate
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
        public int cabac
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
        public int @default
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint duration
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
        public decimal frameRate
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
        public int hasScalingMatrix
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
        public ushort height
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
        public uint id
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
        public int index
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
        public int level
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
        public int refFrames
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
        public int streamType
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
        public ushort width
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
        public int channels
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
        public int dialogNorm
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
        public uint samplingRate
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
        public int selected
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoDirector
    {

        private uint idField;

        private string tagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
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

        private uint idField;

        private string tagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
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
    public partial class MediaContainerVideoUser
    {

        private int idField;

        private string thumbField;

        private string titleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int id
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

        private string stateField;

        private string titleField;

        private string vendorField;

        private string versionField;

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
    }







}
