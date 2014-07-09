using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LL.FrameWork.Web
{
	[XmlRoot("OutputCache")]
	public class OutputCacheConfig
	{
		[XmlArrayItem("Setting")]
		public List<OutputCacheSetting> Settings = new List<OutputCacheSetting>();
	}


	public class OutputCacheSetting : OutputCacheAttribute
	{
		[XmlAttribute]
		public string FilePath { get; set; }
	}

}
