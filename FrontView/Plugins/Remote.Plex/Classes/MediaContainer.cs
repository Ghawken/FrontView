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

        private byte sizeField;

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
        public byte size
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

        private MediaContainerVideoUser userField;

        private MediaContainerVideoPlayer playerField;

        private MediaContainerVideoTranscodeSession transcodeSessionField;

        private uint addedAtField;

        private string artField;

        private string chapterSourceField;

        private uint durationField;

        private string guidField;

        private string keyField;

        private byte librarySectionIDField;

        private ushort ratingKeyField;

        private byte sessionKeyField;

        private string summaryField;

        private string thumbField;

        private string titleField;

        private string typeField;

        private uint updatedAtField;

        private ushort viewOffsetField;

        private ushort yearField;

        private bool yearFieldSpecified;

        private string contentRatingField;

        private string grandparentArtField;

        private string grandparentKeyField;

        private ushort grandparentRatingKeyField;

        private bool grandparentRatingKeyFieldSpecified;

        private string grandparentThumbField;

        private string grandparentTitleField;

        private byte indexField;

        private bool indexFieldSpecified;

        private uint lastViewedAtField;

        private bool lastViewedAtFieldSpecified;

        private byte parentIndexField;

        private bool parentIndexFieldSpecified;

        private string parentKeyField;

        private ushort parentRatingKeyField;

        private bool parentRatingKeyFieldSpecified;

        private string parentThumbField;

        private byte viewCountField;

        private bool viewCountFieldSpecified;

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
        public byte librarySectionID
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
        public ushort ratingKey
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
        public byte sessionKey
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
        public ushort viewOffset
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
        public ushort year
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
        public ushort grandparentRatingKey
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
        public byte index
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
        public byte parentIndex
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
        public ushort parentRatingKey
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
        public byte viewCount
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoMedia
    {

        private MediaContainerVideoMediaPart partField;

        private decimal aspectRatioField;

        private byte audioChannelsField;

        private string audioCodecField;

        private string audioProfileField;

        private ushort bitrateField;

        private string containerField;

        private uint durationField;

        private ushort heightField;

        private ushort idField;

        private string videoCodecField;

        private string videoFrameRateField;

        private string videoProfileField;

        private ushort videoResolutionField;

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
        public byte audioChannels
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
        public ushort id
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
        public ushort videoResolution
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

        private string audioProfileField;

        private string containerField;

        private uint durationField;

        private string fileField;

        private ushort idField;

        private string keyField;

        private ulong sizeField;

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
        public ushort id
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
        public ulong size
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

        private byte bitDepthField;

        private bool bitDepthFieldSpecified;

        private ushort bitrateField;

        private bool bitrateFieldSpecified;

        private byte cabacField;

        private bool cabacFieldSpecified;

        private string chromaSubsamplingField;

        private string codecField;

        private string codecIDField;

        private byte defaultField;

        private bool defaultFieldSpecified;

        private uint durationField;

        private bool durationFieldSpecified;

        private decimal frameRateField;

        private bool frameRateFieldSpecified;

        private string frameRateModeField;

        private byte hasScalingMatrixField;

        private bool hasScalingMatrixFieldSpecified;

        private ushort heightField;

        private bool heightFieldSpecified;

        private ushort idField;

        private byte indexField;

        private bool indexFieldSpecified;

        private string languageField;

        private string languageCodeField;

        private byte levelField;

        private bool levelFieldSpecified;

        private string pixelFormatField;

        private string profileField;

        private byte refFramesField;

        private bool refFramesFieldSpecified;

        private string scanTypeField;

        private byte streamTypeField;

        private ushort widthField;

        private bool widthFieldSpecified;

        private string audioChannelLayoutField;

        private string bitrateModeField;

        private byte channelsField;

        private bool channelsFieldSpecified;

        private ushort samplingRateField;

        private bool samplingRateFieldSpecified;

        private byte selectedField;

        private bool selectedFieldSpecified;

        private string formatField;

        private string keyField;

        private string colorRangeField;

        private string colorSpaceField;

        private sbyte dialogNormField;

        private bool dialogNormFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte bitDepth
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
        public byte cabac
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
        public byte @default
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
        public byte hasScalingMatrix
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
        public ushort id
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
        public byte index
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
        public byte level
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
        public byte refFrames
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
        public byte streamType
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
        public byte channels
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
        public ushort samplingRate
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
        public byte selected
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
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
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
        public sbyte dialogNorm
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoUser
    {

        private byte idField;

        private bool idFieldSpecified;

        private string thumbField;

        private string titleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idSpecified
        {
            get
            {
                return this.idFieldSpecified;
            }
            set
            {
                this.idFieldSpecified = value;
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

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MediaContainerVideoTranscodeSession
    {

        private string keyField;

        private byte throttledField;

        private byte completeField;

        private decimal progressField;

        private byte speedField;

        private uint durationField;

        private ushort remainingField;

        private string contextField;

        private string videoDecisionField;

        private string audioDecisionField;

        private string protocolField;

        private string containerField;

        private string videoCodecField;

        private string audioCodecField;

        private byte audioChannelsField;

        private ushort widthField;

        private ushort heightField;

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
        public byte throttled
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
        public byte complete
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
        public decimal progress
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
        public byte speed
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
        public ushort remaining
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
        public byte audioChannels
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
    }






}
