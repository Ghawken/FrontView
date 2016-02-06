using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Plex.Api
{


    [XmlRoot("MediaContainer")]
    public class ClientsMediaContainer
	{
		// ATTRIBUTES
		[XmlAttribute("size")]
		public int size  { get; set; }
		
		// ELEMENTS
		[XmlElement("Server")]
        public List<Server> Server { get; set; }
		
		// CONSTRUCTOR
		public ClientsMediaContainer()
		{}
	}
}
