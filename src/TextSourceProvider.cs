using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameGen
{
    public class TextSourceProvider
    {
        public TextSource Load()
        {
            return XMLSerializer.Deserialize<TextSource>("source.xml");
        }
        public void Save(TextSource src)
        {
            XMLSerializer.Serialize<TextSource>(src, ".", "source.xml");
        }
    }

    public static class XMLSerializer
    {
        public static void Serialize<T>(this T baseType, string path, string filename)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            if (!Directory.Exists(path))
            {
                string ShortDirectory = (path.Length > 30) ? (path.Substring(0, 30) + "...") : (path);
                throw new DirectoryNotFoundException(string.Format("Directory \"{0}\" not found", ShortDirectory));
            }

            using (FileStream stream = new FileStream(Path.Combine(path, filename), FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(stream, baseType);
            }
        }

        public static T Deserialize<T>(string filename)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            if (!File.Exists(filename))
            {
                string ShortDirectory = (filename.Length > 30) ? (filename.Substring(0, 30) + "...") : (filename);
                throw new FileNotFoundException(string.Format("Directory \"{0}\" not found", ShortDirectory));
            }

            T theclass;

            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                theclass = (T)serializer.Deserialize(stream);
            }

            return theclass;
        }
    }
}
