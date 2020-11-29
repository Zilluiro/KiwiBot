using System.Collections.Generic;
using System.Xml.Serialization;

namespace KiwiBot.DataModels
{
    public class GelbooruCompatiblePostModel: BasePostModel
    {
        [XmlAttribute(AttributeName = "tags")]
        public override string Tags { get; set; }

        [XmlAttribute(AttributeName = "sample_url")]
        public override string FileUrl { get; set; }
    }
    
	[XmlRoot(ElementName="posts")]
	public class SafeBooruPostsModel 
    {
	    [XmlElement(ElementName="post")]
	    public List<GelbooruCompatiblePostModel> Posts { get; set; }
	}
}
