using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SVNHook
{
    //C:\Users\payneliu\AppData\Local\Temp\svn9B4.tmp,
    //3,
    //C:\Users\payneliu\AppData\Local\Temp\svn9B5.tmp,
    //221137,
    //C:\Users\payneliu\AppData\Local\Temp\svn9C6.tmp,
    //E:\svn\isd_qqvipserver_rep\ClubDev1Imgcache_proj\trunk\imgcache.qq.com\htdocs\vipstyle\game\act\20150316_jjds\img

    //第一个参数表示： 提交的文件路径
    //第二个参数表示： 未知
    //第三个参数表示： mark
    //第四个参数表示： 未知
    //第五个参数表示： 未知
    //第六个参数表示： 提交目录

    class Param
    {
        /// <summary>
        /// 提交的文件路径
        /// </summary>
        public string CommitPath { get; private set; }

        /// <summary>
        /// 所有提交的文件
        /// </summary>
        public IEnumerable<string> CommitFiles
        {
            get
            {
                foreach (var item in lst_file)
                {
                    yield return item;
                }
            }
        }

        private List<string> lst_file = new List<string>();

        public Param(string[] args)
        {
            string[] data = args;
            if (args.Length == 1)
            {
                data = args[0].Split(',');
            }
            if (data.Length > 5)
            {
                this.CommitPath = data[5];

                lst_file.AddRange(GetfileContent(data[0], Encoding.UTF8));
            }
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="filepath"></param>
        private string[] GetfileContent(string filepath, Encoding coding = null)
        {
            if (coding == null)
            {
                coding = Encoding.Default;
            }
            try
            {
                return File.ReadAllLines(filepath, coding);
            }
            catch (Exception ex)
            {
                Util.LogException(ex);
            }
            return new string[0];
        }
    }
}
