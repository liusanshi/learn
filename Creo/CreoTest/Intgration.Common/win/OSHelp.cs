using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intgration.Common.win
{
    /// <summary>
    /// 操作系统的帮助类
    /// </summary>
    public static class OSHelp
    {
        /// <summary>
        /// 是否win7操作系统
        /// </summary>
        /// <returns></returns>
        public static bool IsWin7
        {
            get
            {
                Version version = Environment.OSVersion.Version;
                return version.Major == 6 && version.Minor == 1;
            }
        }

        /// <summary>
        /// 是否vista以及vista后续的版本
        /// </summary>
        /// <returns></returns>
        public static bool IsVistaAndSubsequentVersion
        {
            get
            {
                Version version = Environment.OSVersion.Version;
                return version.Major >= 6;
            }
        }
    }
}
