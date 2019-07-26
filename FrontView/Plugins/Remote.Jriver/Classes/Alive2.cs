using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;


namespace Remote.Jriver.Api
{

    public partial class Alive2
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = false)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Response
        {

            private ResponseItem[] itemField;

            private string statusField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Item")]
            public ResponseItem[] Item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Status
            {
                get
                {
                    return this.statusField;
                }
                set
                {
                    this.statusField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ResponseItem
        {

            private string nameField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

    }

}

