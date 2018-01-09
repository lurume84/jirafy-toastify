using System;
using System.Windows.Media;
using System.Xml.Serialization;
using Toastify.Helpers;

namespace Toastify.ToastLayout.Rows
{
    [Serializable]
    [XmlRoot("ProgressBarRow")]
    public class ProgressBarRow : OtherRow
    {
        [XmlAttribute("ContainerColor")]
        public string ContainerHexColor { get; private set; }

        [XmlAttribute("BarColor")]
        public string BarHexColor { get; private set; }

        [XmlIgnore]
        public Color ContainerColor
        {
            get { return string.IsNullOrWhiteSpace(this.ContainerHexColor) ? default(Color) : ColorHelper.HexToColor(this.ContainerHexColor); }
            set { this.ContainerHexColor = ColorHelper.ColorToHex(value); }
        }

        [XmlIgnore]
        public Color BarColor
        {
            get { return string.IsNullOrWhiteSpace(this.BarHexColor) ? default(Color) : ColorHelper.HexToColor(this.BarHexColor); }
            set { this.BarHexColor = ColorHelper.ColorToHex(value); }
        }
    }
}