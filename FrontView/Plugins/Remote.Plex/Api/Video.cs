using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Plex.Api
{
	
	public class Video
	{
		// ATTRIBUTES
		[XmlAttribute("addedAt")]
		public int addedAt  { get; set; }
		
		[XmlAttribute("art")]
		public string art { get; set; }
		
		[XmlAttribute("chapterSource")]
		public string chapterSource { get; set; }
		
		[XmlAttribute("duration")]
		public int duration  { get; set; }
		
		[XmlAttribute("grandparentArt")]
		public string grandparentArt { get; set; }
		
		[XmlAttribute("grandparentKey")]
		public string grandparentKey { get; set; }
		
		[XmlIgnore]
		public int? grandparentRatingKey { get; set; }
		[XmlAttribute("grandparentRatingKey")]
		public string grandparentRatingKeyString
		{
			get { return grandparentRatingKey==null ? "" : grandparentRatingKey.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) grandparentRatingKey = null;
				else grandparentRatingKey = int.Parse(value);
			}
		}
		
		[XmlAttribute("grandparentThumb")]
		public string grandparentThumb { get; set; }
		
		[XmlAttribute("grandparentTitle")]
		public string grandparentTitle { get; set; }
		
		[XmlAttribute("guid")]
		public string guid { get; set; }
		
		[XmlIgnore]
		public int? index { get; set; }
		[XmlAttribute("index")]
		public string indexString
		{
			get { return index==null ? "" : index.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) index = null;
				else index = int.Parse(value);
			}
		}
		
		[XmlAttribute("key")]
		public string key { get; set; }
		
		[XmlAttribute("lastViewedAt")]
		public int lastViewedAt  { get; set; }
		
		[XmlAttribute("librarySectionID")]
		public int librarySectionID  { get; set; }
		
		[XmlIgnore]
		public int? parentIndex { get; set; }
		[XmlAttribute("parentIndex")]
		public string parentIndexString
		{
			get { return parentIndex==null ? "" : parentIndex.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) parentIndex = null;
				else parentIndex = int.Parse(value);
			}
		}
		
		[XmlAttribute("parentKey")]
		public string parentKey { get; set; }
		
		[XmlIgnore]
		public int? parentRatingKey { get; set; }
		[XmlAttribute("parentRatingKey")]
		public string parentRatingKeyString
		{
			get { return parentRatingKey==null ? "" : parentRatingKey.Value.ToString(CultureInfo.InvariantCulture); }
			set
			{
				if (String.IsNullOrWhiteSpace(value)) parentRatingKey = null;
				else parentRatingKey = int.Parse(value);
			}
		}
		
		[XmlAttribute("parentThumb")]
		public string parentThumb { get; set; }
		
		[XmlAttribute("ratingKey")]
		public int ratingKey  { get; set; }
		
		[XmlAttribute("sessionKey")]
		public int sessionKey  { get; set; }
		
		[XmlAttribute("summary")]
		public string summary { get; set; }
		
		[XmlAttribute("thumb")]
		public string thumb { get; set; }
		
		[XmlAttribute("title")]
		public string title { get; set; }
		
		[XmlAttribute("type")]
		public string type { get; set; }
		
		[XmlAttribute("updatedAt")]
		public int updatedAt  { get; set; }
		
		[XmlAttribute("viewOffset")]
		public int viewOffset  { get; set; }
		
		[XmlAttribute("year")]
		public int year  { get; set; }
		
		// ELEMENTS
		[XmlElement("Media")]
		public Media Media { get; set; }
		
		[XmlElement("Writer")]
		public Writer Writer { get; set; }
		
		[XmlElement("Director")]
		public Director Director { get; set; }
		
		[XmlElement("User")]
		public User User { get; set; }
		
		[XmlElement("Player")]
		public Player Player { get; set; }
		
		[XmlElement("TranscodeSession")]
		public TranscodeSession TranscodeSession { get; set; }
		
		// CONSTRUCTOR
		public Video()
		{}
	}
}
