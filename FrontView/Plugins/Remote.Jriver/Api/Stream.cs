using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Jriver.Api
{
	
	public class Stream
	{
		// ATTRIBUTES
		[XmlAttribute("bitDepth")]
		public int bitDepth  { get; set; }
		
		[XmlAttribute("bitrate")]
		public int bitrate  { get; set; }
		
    	[XmlAttribute("cabac")]
		public int cabac { get; set; }

		
		[XmlAttribute("chromaSubsampling")]
		public string chromaSubsampling { get; set; }
		
		[XmlAttribute("codec")]
		public string codec { get; set; }
		
		[XmlAttribute("codecID")]
		public string codecID { get; set; }
		
		[XmlAttribute("colorRange")]
		public string colorRange { get; set; }
		
		[XmlAttribute("colorSpace")]
		public string colorSpace { get; set; }
		
		[XmlAttribute("default")]
		public int defaultxxx  { get; set; }
		
		[XmlAttribute("duration")]
		public int duration  { get; set; }
		[XmlAttribute("frameRate")]
		public decimal frameRate { get; set; }
	

		
		[XmlAttribute("frameRateMode")]
		public string frameRateMode { get; set; }
		[XmlAttribute("hasScalingMatrix")]
    	public int hasScalingMatrix { get; set; }
		
        [XmlAttribute("height")]
		public int height { get; set; }

		
		[XmlAttribute("id")]
		public int id  { get; set; }
		
		[XmlAttribute("index")]
		public int index  { get; set; }
		
		[XmlAttribute("language")]
		public string language { get; set; }
		
		[XmlAttribute("languageCode")]
		public string languageCode { get; set; }
		
		[XmlIgnore]
		public int? level { get; set; }
		[XmlAttribute("level")]
		public string levelString
		{
			get { return level==null ? "" : level.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) level = null;
				else level = int.Parse(value);
			}
		}
		
		[XmlAttribute("pixelFormat")]
		public string pixelFormat { get; set; }
		
		[XmlAttribute("profile")]
		public string profile { get; set; }
		
		[XmlIgnore]
		public int? refFrames { get; set; }
		[XmlAttribute("refFrames")]
		public string refFramesString
		{
			get { return refFrames==null ? "" : refFrames.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) refFrames = null;
				else refFrames = int.Parse(value);
			}
		}
		
		[XmlAttribute("scanType")]
		public string scanType { get; set; }
		
		[XmlAttribute("streamType")]
		public int streamType  { get; set; }
		
		[XmlIgnore]
		public int? width { get; set; }
		[XmlAttribute("width")]
		public string widthString
		{
			get { return width==null ? "" : width.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) width = null;
				else width = int.Parse(value);
			}
		}
		
		[XmlAttribute("audioChannelLayout")]
		public string audioChannelLayout { get; set; }
		
		[XmlAttribute("bitrateMode")]
		public string bitrateMode { get; set; }
		
		[XmlIgnore]
		public int? channels { get; set; }
		[XmlAttribute("channels")]
		public string channelsString
		{
			get { return channels==null ? "" : channels.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) channels = null;
				else channels = int.Parse(value);
			}
		}
		
		[XmlIgnore]
		public int? dialogNorm { get; set; }
		[XmlAttribute("dialogNorm")]
		public string dialogNormString
		{
			get { return dialogNorm==null ? "" : dialogNorm.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) dialogNorm = null;
				else dialogNorm = int.Parse(value);
			}
		}
		
		[XmlIgnore]
		public int? samplingRate { get; set; }
		[XmlAttribute("samplingRate")]
		public string samplingRateString
		{
			get { return samplingRate==null ? "" : samplingRate.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) samplingRate = null;
				else samplingRate = int.Parse(value);
			}
		}
		
		[XmlIgnore]
		public int? selected { get; set; }
		[XmlAttribute("selected")]
		public string selectedString
		{
			get { return selected==null ? "" : selected.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) selected = null;
				else selected = int.Parse(value);
			}
		}
		
		[XmlIgnore]
		public int? headerStripping { get; set; }
		[XmlAttribute("headerStripping")]
		public string headerStrippingString
		{
			get { return headerStripping==null ? "" : headerStripping.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) headerStripping = null;
				else headerStripping = int.Parse(value);
			}
		}
		
		[XmlAttribute("title")]
		public string title { get; set; }
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
		public Stream()
		{}
	}
}
