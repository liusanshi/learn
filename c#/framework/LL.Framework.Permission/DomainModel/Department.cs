using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LL.Framework.Core.Domain;

namespace LL.Framework.Permission.DomainModel
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Department : EntityBase<string>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


    }
}
