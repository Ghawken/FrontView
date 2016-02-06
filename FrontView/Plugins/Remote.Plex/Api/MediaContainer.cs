using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Plex.Api
{
	
	public class MediaContainer
	{
		// ATTRIBUTES
		[XmlAttribute("size")]
		public int size  { get; set; }
		
		// ELEMENTS
		[XmlElement("Video")]
		public List<Video> Video { get; set; }
		
		// CONSTRUCTOR
		public MediaContainer()
		{}
	}
}
