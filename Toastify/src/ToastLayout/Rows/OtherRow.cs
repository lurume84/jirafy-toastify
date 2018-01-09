using System;
using System.Xml.Serialization;

namespace Toastify.ToastLayout.Rows
{
    [Serializable]
    [XmlRoot("OtherRow")]
    public abstract class OtherRow : Row
    {
        [XmlAttribute]
        public double MinHeight { get; set; }
    }
}