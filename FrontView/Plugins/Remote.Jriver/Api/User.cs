using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Jriver.Api
{
	
	public class User
	{
		// ATTRIBUTES
		[XmlAttribute("id")]
		public int id  { get; set; }
		
		[XmlAttribute("thumb")]
		public string thumb { get; set; }
		
		[XmlAttribute("title")]
		public string title { get; set; }
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
		public User()
		{}
	}
}
