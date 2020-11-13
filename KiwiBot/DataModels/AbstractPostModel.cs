using Newtonsoft.Json;

namespace KiwiBot.DataModels
{
    abstract class AbstractPostModel
    {
        private string _tags;
        public virtual string Tags {
            get
            {
                return _tags;
            }
            set
            {
                if (_tags.Length > 200)
                    _tags = value.Substring(0,200);
            }
        }

        public virtual string FileUrl { get; set; }
    }
}
