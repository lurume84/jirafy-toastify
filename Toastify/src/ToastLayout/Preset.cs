using log4net;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Toastify.ToastLayout.Rows;

namespace Toastify.ToastLayout
{
    [Serializable]
    [XmlRoot("Preset")]
    public class Preset
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Preset));

        private static XmlSerializer _xmlSerializer;

        public static XmlSerializer XmlSerializer
        {
            get { return _xmlSerializer ?? (_xmlSerializer = new XmlSerializer(typeof(Preset))); }
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement("Rows")]
        public ObservableCollection<Row> Rows { get; set; }

        public Preset() : this(string.Empty)
        {
        }

        public Preset(string name)
        {
            this.Name = name;
            this.Rows = new ObservableCollection<Row>();
        }

        public static void Save(Preset preset, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                XmlSerializer.Serialize(sw, preset);
            }
        }

        public static Preset Load(string filePath)
        {
            Preset file;
            using (StreamReader sr = new StreamReader(filePath))
            {
                file = XmlSerializer.Deserialize(sr) as Preset;
            }
            return file;
        }
    }
}