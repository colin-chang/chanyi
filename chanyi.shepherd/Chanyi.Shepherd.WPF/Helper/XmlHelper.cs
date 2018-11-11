using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Chanyi.Shepherd.WPF.Helper
{
    public class XmlHelper
    {
        public static string GetDiseaseDesc(string id)
        {
            string path = "./Data/Diseases.xml";
            var node = XDocument.Load(path).Root.Elements("Disease").Where(d => d.Attribute("Id").Value == id).FirstOrDefault();
            return node == null ? null : node.Value;
        }

        public static List<string> GetNews()
        {
            string path = "./Data/Readme.xml";
            return XDocument.Load(path).Root.Element("New").Elements("Item").Select(i => i.Value).ToList();
        }
    }
}
