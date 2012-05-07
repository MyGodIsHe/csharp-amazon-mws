using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Amazone.MWS
{
    /// <summary>
    /// Small wrapper around the find and findall methods of
    /// the the System.Xml.XmlDocument class.
    /// </summary>
    public class TreeWrapper
    {
        public XmlDocument doc;
        public string ns;

        public TreeWrapper(Stream stream, string ns)
        {
            this.doc = new XmlDocument();
            this.doc.Load(stream);
            this.ns = ns;
        }

        public XmlNode SelectSingleNode(string xpath)
        {
            return this.doc.SelectSingleNode(".//" + this.ns + xpath);
        }

        public XmlNodeList SelectNodes(string xpath)
        {
            return this.doc.SelectNodes(".//" + this.ns + xpath);
        }
    }
}
