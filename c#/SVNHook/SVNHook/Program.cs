using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SVNHook
{
    class Program
    {
        static void Main(string[] args)
        {
            //Util.Log("asdad");
            if (args != null)
            {
                //int i = 0;
                //foreach (var item in args)
                //{
                //    Util.Log(string.Concat("参数", i++, ":", item));
                //}
                if (args.Length > 1)
                {
                    Util.Log(string.Concat("参数", string.Join(" ", args)));

                    Param p = new Param(args);

                    //Util.Log("CommitPath:" + p.CommitPath);

                    foreach (var item in p.CommitFiles)
                    {
                        Util.Log("CommitFile:" + item);

                        var up = UploaderFactroy.CreateUploader(item);
                        if (up != null)
                        {
                            up.upload();
                        }
                    }
                }
                else if (args.Length > 0)
                {
                    var up = UploaderFactroy.CreateUploader(args[0]);
                    if (up != null)
                    {
                        up.upload();
                    }
                }
            }
            else
            {
                Util.Log("hi");
            }
        }
    }
}
