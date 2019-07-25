using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Remote.Jriver.Api
{
    public class AllDatabase
    {
            [XmlRoot(ElementName = "Field")]
            public class Field
            {
                [XmlAttribute(AttributeName = "Name")]
                public string Name { get; set; }
                [XmlText]
                public string Text { get; set; }
            }

            [XmlRoot(ElementName = "Item")]
            public class Item
            {
                [XmlElement(ElementName = "Field")]
                public List<Field> Field { get; set; }
            }

            [XmlRoot(ElementName = "MPL")]
            public class MPL
            {
                [XmlElement(ElementName = "Item")]
                public List<Item> Item { get; set; }
                [XmlAttribute(AttributeName = "Version")]
                public string Version { get; set; }
                [XmlAttribute(AttributeName = "Title")]
                public string Title { get; set; }
                [XmlAttribute(AttributeName = "PathSeparator")]
                public string PathSeparator { get; set; }
            }

        }


    }

