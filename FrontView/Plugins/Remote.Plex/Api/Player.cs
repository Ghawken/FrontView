using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Plex.Api
{
	
	public class Player
	{
		// ATTRIBUTES
		[XmlAttribute("address")]
		public string address { get; set; }
		
		[XmlAttribute("machineIdentifier")]
		public string machineIdentifier { get; set; }
		
		[XmlAttribute("platform")]
		public string platform { get; set; }
		
		[XmlAttribute("product")]
		public string product { get; set; }
		
		[XmlAttribute("state")]
		public string state { get; set; }
		
		[XmlAttribute("title")]
		public string title { get; set; }
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
		public Player()
		{}
	}
}
