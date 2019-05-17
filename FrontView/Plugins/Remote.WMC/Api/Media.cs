using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.WMC.Api
{
	
	public class Media
	{
		// ATTRIBUTES
		[XmlAttribute("aspectRatio")]
		public decimal aspectRatio  { get; set; }
		
		[XmlAttribute("audioChannels")]
		public int audioChannels  { get; set; }
		
		[XmlAttribute("audioCodec")]
		public string audioCodec { get; set; }
		
		[XmlAttribute("bitrate")]
		public int bitrate  { get; set; }
		
		[XmlAttribute("container")]
		public string container { get; set; }
		
		[XmlAttribute("duration")]
		public int duration  { get; set; }
		
		[XmlAttribute("height")]
		public int height  { get; set; }
		
		[XmlAttribute("id")]
		public int id  { get; set; }
		
		[XmlAttribute("title")]
		public string title { get; set; }
		
		[XmlAttribute("videoCodec")]
		public string videoCodec { get; set; }
		
		[XmlAttribute("videoFrameRate")]
		public string videoFrameRate { get; set; }
		
		[XmlAttribute("videoProfile")]
		public string videoProfile { get; set; }
		
		[XmlAttribute("videoResolution")]
		public int videoResolution  { get; set; }
		
		[XmlAttribute("width")]
		public int width  { get; set; }
		
		[XmlAttribute("audioProfile")]
		public string audioProfile { get; set; }
		
		// ELEMENTS
		[XmlElement("Part")]
		public Part Part { get; set; }
		
		// CONSTRUCTOR
		public Media()
		{}
	}
}
