using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireFiddler
{
    /// <summary>
    /// 规则类型
    /// </summary>
    public class RuleType
    {
        public string Name { get; set; }

        public string value { get; set; }
    }

    /// <summary>
    /// 规则列表
    /// </summary>
    public class RuleTypes : List<RuleType>
    {
 
    }
}
