using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Plex.Api
{
	
	public class TranscodeSession
	{
		// ATTRIBUTES
		[XmlAttribute("key")]
		public string key { get; set; }
		
		[XmlAttribute("throttled")]
		public int throttled  { get; set; }
		
		[XmlAttribute("progress")]
		public decimal progress  { get; set; }
		
		[XmlAttribute("speed")]
		public decimal speed  { get; set; }
		
		[XmlAttribute("duration")]
		public int duration  { get; set; }
		
		[XmlAttribute("remaining")]
		public int remaining  { get; set; }
		
		[XmlAttribute("context")]
		public string context { get; set; }
		
		[XmlAttribute("videoDecision")]
		public string videoDecision { get; set; }
		
		[XmlAttribute("audioDecision")]
		public string audioDecision { get; set; }
		
		[XmlAttribute("protocol")]
		public string protocol { get; set; }
		
		[XmlAttribute("container")]
		public string container { get; set; }
		
		[XmlAttribute("videoCodec")]
		public string videoCodec { get; set; }
		
		[XmlAttribute("audioCodec")]
		public string audioCodec { get; set; }
		
		[XmlAttribute("audioChannels")]
		public int audioChannels  { get; set; }
		
		[XmlAttribute("width")]
		public int width  { get; set; }
		
		[XmlAttribute("height")]
		public int height  { get; set; }
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
		public TranscodeSession()
		{}
	}
}
