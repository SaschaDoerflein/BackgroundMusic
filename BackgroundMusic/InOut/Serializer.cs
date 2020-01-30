using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using BackgroundMusic.Model;

namespace BackgroundMusic.InOut
{
    public static class Serializer
    {
        public static T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }


        public static string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }



        

        private static string GetXmlStreamContent(Stream stream)
        {
            string xmlFileContent = "";

                using (StreamReader sr = new StreamReader(stream))
                {
                    xmlFileContent = sr.ReadToEnd();
                }

            return xmlFileContent;
        }
    }

}
