using System.Xml.Serialization;

namespace KiwiBot.DataModels
{
    public class BasePostModel
    {
        [XmlIgnore]
        private string _tags;

        [XmlIgnore]
        public virtual string Tags {
            get
            {
                return _tags;
            }
            set
            {
                _tags = (value?.Length > 200) ? value.Substring(0, 200) : value;
            }
        }

        [XmlIgnore]
        public virtual string FileUrl { get; set; }
    }
}
