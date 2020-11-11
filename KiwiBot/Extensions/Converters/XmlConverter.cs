using System.IO;
using System.Xml.Serialization;

namespace KiwiBot.Extensions.Converters
{
    class XmlConverter: IDataConverter
    {
        public string To<T>(T obj) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            
            using(StringWriter writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public T From<T>(string str) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using(StringReader reader = new StringReader(str))
            {
                return (T) xmlSerializer.Deserialize(reader);
            }
        }
    }
}
