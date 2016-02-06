using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Plex.Api
{
	
	public class Part
	{
		// ATTRIBUTES
		[XmlAttribute("container")]
		public string container { get; set; }
		
		[XmlAttribute("duration")]
		public int duration  { get; set; }
		
		[XmlAttribute("file")]
		public string file { get; set; }
		
		[XmlAttribute("id")]
		public int id  { get; set; }
		
		[XmlAttribute("key")]
		public string key { get; set; }
		
		[XmlAttribute("size")]
		public int size  { get; set; }
		
		[XmlAttribute("videoProfile")]
		public string videoProfile { get; set; }
		
		[XmlAttribute("audioProfile")]
		public string audioProfile { get; set; }
		
		// ELEMENTS
		[XmlElement("Stream")]
		public List<Stream> Stream { get; set; }
		
		// CONSTRUCTOR
		public Part()
		{}
	}
}
