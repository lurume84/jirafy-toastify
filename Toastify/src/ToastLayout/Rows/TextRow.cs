using System;
using System.Windows.Media;
using System.Xml.Serialization;
using Toastify.Helpers;

namespace Toastify.ToastLayout.Rows
{
    [Serializable]
    [XmlRoot("TextRow")]
    public abstract class TextRow : Row
    {
        [XmlAttribute]
        public double FontSize { get; set; }

        [XmlAttribute("Color")]
        public string HexColor { get; private set; }

        [XmlAttribute]
        public bool DropShadow { get; set; }

        [XmlAttribute]
        public double ShadowDepth { get; set; }

        [XmlAttribute]
        public double ShadowBlur { get; set; }

        [XmlIgnore]
        public string Text { get; set; }

        [XmlIgnore]
        public Color Color
        {
            get { return string.IsNullOrWhiteSpace(this.HexColor) ? default(Color) : ColorHelper.HexToColor(this.HexColor); }
            set { this.HexColor = ColorHelper.ColorToHex(value); }
        }
    }
}