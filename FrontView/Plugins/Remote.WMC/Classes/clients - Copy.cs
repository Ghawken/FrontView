using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;


namespace Remote.WMC.Api.Clients2
{
    public class MediaContainer
    {
        // ATTRIBUTES
        [XmlAttribute("size")]
        public int size { get; set; }

        // ELEMENTS
        [XmlElement("Server")]
        public List<Server> Server { get; set; }

        // CONSTRUCTOR
        public MediaContainer()
        { }
    }

    public class Server
    {
        // ATTRIBUTES
        [XmlAttribute("name")]
        public string name { get; set; }

        [XmlAttribute("host")]
        public string host { get; set; }

        [XmlAttribute("address")]
        public string address { get; set; }

        [XmlAttribute("port")]
        public int port { get; set; }

        [XmlAttribute("machineIdentifier")]
        public string machineIdentifier { get; set; }

        [XmlAttribute("version")]
        public string version { get; set; }

        [XmlAttribute("protocol")]
        public string protocol { get; set; }

        [XmlAttribute("product")]
        public string product { get; set; }

        [XmlAttribute("deviceClass")]
        public string deviceClass { get; set; }

        [XmlAttribute("protocolVersion")]
        public int protocolVersion { get; set; }

        [XmlAttribute("protocolCapabilities")]
        public string protocolCapabilities { get; set; }

        // ELEMENTS
        [XmlText]
        public string Value { get; set; }

        // CONSTRUCTOR
        public Server()
        { }
    }

}

