/**************************************
* 作用：返回标限标识，该设置与系统中的
*       RGP_AuthorityDir表对应该起来
* 作者：Nick.Yan
* 日期: 2008-02-11
* 网址：www.redglove.com.cn
**************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedGlovePermission.Web
{
    public class RGP_Tag
    {
        /// <summary>
        /// 浏览权限
        /// </summary>
        public static string Browse
        {
            get { return "RGP_BROWSE"; }
        }

        /// <summary>
        /// 新增权限
        /// </summary>
        public static string Add
        {
            get { return "RGP_ADD"; }
        }

        /// <summary>
        /// 编辑权限
        /// </summary>
        public static string Edit
        {
            get { return "RGP_EDIT"; }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        public static string Delete
        {
            get { return "RGP_DELETE"; }
        }

        /// <summary>
        /// 搜索权限
        /// </summary>
        public static string Search
        {
            get { return "RGP_SEARCH"; }
        }

        /// <summary>
        /// 审核权限
        /// </summary>
        public static string Verify
        {
            get { return "RGP_VERIFY"; }
        }

        /// <summary>
        /// 移动权限
        /// </summary>
        public static string Move
        {
            get { return "RGP_MOVE"; }
        }

        /// <summary>
        /// 打印权限
        /// </summary>
        public static string Print
        {
            get { return "RGP_PRINT"; }
        }

        /// <summary>
        /// 下载权限
        /// </summary>
        public static string Download
        {
            get { return "RGP_DOWNLOAD"; }
        }

        /// <summary>
        /// 备份权限
        /// </summary>
        public static string back
        {
            get { return "RGP_BACK"; }
        }             
    }
}
