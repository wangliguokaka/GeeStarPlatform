using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// XmlSerializerHelper 的摘要说明
/// </summary>
namespace D2012.Common
{
    public class XmlSerializerHelper
    {
        public XmlSerializerHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary> 

        ///序列化

        /// </summary> 

        /// <typeparam name="T"></typeparam> 

        /// <param name="serialObject"></param> 

        /// <returns></returns> 

        public static string XmlSerializer(object serialObject) 
        {

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var serializer = new XmlSerializer(serialObject.GetType());

            var settings = new XmlWriterSettings();

            settings.OmitXmlDeclaration = true;

            settings.Encoding = Encoding.UTF8;

            using (var stream = new StringWriter())

            using (var writer = XmlWriter.Create(stream, settings))
            {

                serializer.Serialize(writer, serialObject, emptyNamepsaces);

                return stream.ToString();

            }

        }

        /// <summary> 

        ///反序列化

        /// </summary> 

        /// <typeparam name="T"></typeparam> 

        /// <param name="xml"></param> 

        /// <returns></returns> 

        public static List<string> DeserializeObject(string xml) 
        {

            using (var str = new StringReader(xml))
            {

                var xmlSerializer = new XmlSerializer(typeof(List<string>));

                var result = (List<string>)xmlSerializer.Deserialize(str);

                return result;

            }

        }

    }
}