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

        [Serializable()]
        [System.Xml.Serialization.XmlRoot("MPL")]
        public class MPL
        {
            public MPL() { Items = new List<Item>(); }
            [XmlElement("Items")]
            public List<Item> Items { get; set; }
        }


        public class Fields
        {
            [XmlElement("Name")]
            public String Name { get; set; }
            [XmlElement("Value")]
            public String Value { get; set; }
        }

        public class Item
        {
            public Item() { Fields = new List<Fields>(); }
            [XmlElement("Items")]
            public List<Fields> Fields { get; set; }
        }




        ///// <remarks/>
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        //public class MPL
        //{

        //    private MPLItemField[][] itemField;

        //    private decimal versionField;

        //    private string titleField;

        //    private string pathSeparatorField;

        //    /// <remarks/>
        //    [System.Xml.Serialization.XmlArrayItemAttribute("Field", typeof(MPLItemField), IsNullable = false)]
        //    public MPLItemField[][] Item
        //    {
        //        get
        //        {
        //            return this.itemField;
        //        }
        //        set
        //        {
        //            this.itemField = value;
        //        }
        //    }

        //    /// <remarks/>
        //    [System.Xml.Serialization.XmlAttributeAttribute()]
        //    public decimal Version
        //    {
        //        get
        //        {
        //            return this.versionField;
        //        }
        //        set
        //        {
        //            this.versionField = value;
        //        }
        //    }

        //    /// <remarks/>
        //    [System.Xml.Serialization.XmlAttributeAttribute()]
        //    public string Title
        //    {
        //        get
        //        {
        //            return this.titleField;
        //        }
        //        set
        //        {
        //            this.titleField = value;
        //        }
        //    }

        //    /// <remarks/>
        //    [System.Xml.Serialization.XmlAttributeAttribute()]
        //    public string PathSeparator
        //    {
        //        get
        //        {
        //            return this.pathSeparatorField;
        //        }
        //        set
        //        {
        //            this.pathSeparatorField = value;
        //        }
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //public partial class MPLItemField
        //{

        //    private string nameField;

        //    private string valueField;

        //    /// <remarks/>
        //    [System.Xml.Serialization.XmlAttributeAttribute()]
        //    public string Name
        //    {
        //        get
        //        {
        //            return this.nameField;
        //        }
        //        set
        //        {
        //            this.nameField = value;
        //        }
        //    }

        //    /// <remarks/>
        //    [System.Xml.Serialization.XmlTextAttribute()]
        //    public string Value
        //    {
        //        get
        //        {
        //            return this.valueField;
        //        }
        //        set
        //        {
        //            this.valueField = value;
        //        }
        //    }
        //}



    }
}
