using Fiddler;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml;

namespace FireFiddler
{
    /// <summary>
    /// 条件 接口
    /// </summary>
    public interface IRule
    {
        bool Execute(Session session);
        /// <summary>
        /// 是否有效
        /// </summary>
        bool Disabled { get; }
    }

    /// <summary>
    /// Path条件
    /// </summary>
    public class PathRule : IRule, IXmlSerializable
    {
        public PathRule() { }

        private bool mDisabled = false;
        /// <summary>
        /// 部分路径
        /// </summary>
        public string Path { get; set; }
        public bool Execute(Session session)
        {
            return session.url.IndexOf(Path, StringComparison.OrdinalIgnoreCase) > -1;
        }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Disabled
        {
            get { return mDisabled; }
            set { mDisabled = value; }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement("PathRule");
            reader.ReadStartElement();
            this.Disabled = reader.ReadContentAsBoolean();
            reader.ReadEndElement();
            reader.ReadStartElement();
            this.Path = reader.ReadContentAsString();
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("PathRule");
            writer.WriteElementString("Disabled", this.Disabled.ToString().ToLower());
            writer.WriteElementString("Path", this.Path);
            writer.WriteEndElement();
        }
    }

    /// <summary>
    /// Host条件
    /// </summary>
    public class HostRule : IRule, IXmlSerializable
    {
        private bool mDisabled = false;

        public HostRule() { }

        /// <summary>
        /// host
        /// </summary>
        public string Host { get; set; }
        public bool Execute(Session session)
        {
            return session.hostname == Host;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Disabled
        {
            get { return mDisabled; }
            set { mDisabled = value; }
        }
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement("HostRule");
            reader.ReadStartElement();
            this.Disabled = reader.ReadContentAsBoolean();
            reader.ReadEndElement();
            reader.ReadStartElement();
            this.Host = reader.ReadContentAsString();
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("HostRule");
            writer.WriteElementString("Disabled", this.Disabled.ToString().ToLower());
            writer.WriteElementString("Host", this.Host);
            writer.WriteEndElement();
        }
    }

    /// <summary>
    /// RegExp条件
    /// </summary>
    public class RegExpRule : IRule, IXmlSerializable
    {
        private bool mDisabled = false;
        public RegExpRule() { }

        /// <summary>
        /// 模式
        /// </summary>
        public string Pattern { get; set; }

        public bool Execute(Session session)
        {
            Regex reg = new Regex(Pattern);
            return reg.IsMatch(session.url);
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Disabled
        {
            get { return mDisabled; }
            set { mDisabled = value; }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement("RegExpRule");
            reader.ReadStartElement();
            this.Disabled = reader.ReadContentAsBoolean();
            reader.ReadEndElement();
            reader.ReadStartElement();
            this.Pattern = reader.ReadContentAsString();
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("RegExpRule");
            writer.WriteElementString("Disabled", this.Disabled.ToString().ToLower());
            writer.WriteElementString("Pattern", this.Pattern);
            writer.WriteEndElement();
        }
    }
}
