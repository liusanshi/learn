using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

using Fiddler;

namespace FireFiddler
{
    /// <summary>
    /// 过滤Session
    /// </summary>
    public class RuleList : List<IRule>, IXmlSerializable
    {
        public RuleList() { }

        /// <summary>
        /// 获取所有有效的rule
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IRule> GetRules()
        {
            return this.Where(p => p.Disabled);
        }

        /// <summary>
        /// 规则保存文件
        /// </summary>
        public static string SavePath
        {
            get 
            {
                return Path.Combine(Util.CfgPath, "RuleList.xml");
            }
        }

        /// <summary>
        /// 匹配回话
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool Match(Session session)
        {
            foreach (var item in GetRules())
            {
                if (item.Execute(session))
                {
                    return true;
                }
            }
            return false;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement("RuleList");
            while (reader.IsStartElement())
            {
                IXmlSerializable r = null;
                switch (reader.Name)
                {
                    case "RegExpRule":
                        r = new RegExpRule();
                        break;
                    case "HostRule":
                        r = new HostRule();
                        break;
                    case "PathRule":
                        r = new PathRule();
                        break;
                }
                if (r != null)
                {
                    r.ReadXml(reader.ReadSubtree());
                    this.Add((IRule)r);
                }
                reader.ReadEndElement();
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            //writer.WriteStartElement("RuleList");
            foreach (var item in this.Cast<IXmlSerializable>())
            {
                item.WriteXml(writer);
            }
            //writer.WriteEndElement();
        }

        /// <summary>
        /// 保存规则
        /// </summary>
        public string SaveXML()
        {
            using (MemoryStream ms = new MemoryStream(200)) //主要是为了 是xml使用utf编码
            {
                XmlSerializer serialzer = new XmlSerializer(typeof(RuleList));

                XmlWriterSettings xmlSet = new XmlWriterSettings();
                xmlSet.Encoding = new UTF8Encoding(false);
                xmlSet.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(ms, xmlSet))
                {
                    serialzer.Serialize(writer, this);
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static RuleList LoadString(string text)
        {
            var reader = new System.IO.StringReader(text);
            XmlSerializer serialzer = new XmlSerializer(typeof(RuleList));
            return (RuleList)serialzer.Deserialize(reader);
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="filepath"></param>
        public void Save(string filepath)
        {
            File.WriteAllText(filepath, this.SaveXML(), Encoding.UTF8);
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static RuleList LoadFile(string filepath)
        {
            string text = File.ReadAllText(filepath, Encoding.UTF8);
            var rl = RuleList.LoadString(text);
            return rl ?? new RuleList();
        }

        /// <summary>
        /// 加载文件 默认路径
        /// </summary>
        /// <returns></returns>
        public static RuleList LoadFile()
        {
            return LoadFile(SavePath);
        }
    }
}
