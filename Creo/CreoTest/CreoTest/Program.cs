using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Diagnostics;

using Intgration.Common;
using Intgration.Common.win;
using Creo.Client;

namespace CreoTest
{
    class Program
    {
        /// <summary>
        /// 固定路径
        /// </summary>
        const string path = @"c:\test\creo";

        static void Main(string[] args)
        {

            var regk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\PTC");
            Console.WriteLine(regk == null);

            CreoClient creo = new CreoClient();
            var aa = creo.GetBom();

            if (UACHelp.IsRunAsAdmin())
            {
                Console.WriteLine("管理员启动");
            }
            else
            {
                Console.WriteLine("非管理员启动");
            }

            //ProcessStartInfo proc = new ProcessStartInfo();
            //proc.UseShellExecute = true;
            //proc.WorkingDirectory = Environment.CurrentDirectory;
            //proc.FileName = Application.ExecutablePath;
            //proc.Verb = "runas"; //使用管理员启动

            //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ptc\\output.txt";
            //DocumentProperty docprop1 = DocumentProperty.LoadXml(path, Encoding.Default);
            //Console.WriteLine(docprop1.Count);
            
            //CreoClient cc = new CreoClient();
            //Console.WriteLine(cc.GetCurrentFileName());
            //Console.WriteLine(cc.GetCurrentFilePath());
            //Console.WriteLine(cc.GetCurrentFullPath());

            //var bom = cc.GetBom();

            //var aa = bom.Count;

            Console.Read();

            return;

#if false

            DocumentProperty docprop = new DocumentProperty();

            int index = 1;

            var partprop = getPartData("00" + index, index, "asm");
            docprop.Add(partprop);

            partprop.ChildRelation.Add(new Dictionary<string, string>() { { "fileid", "3" }, { "count", "1" } });
            partprop.ChildRelation.Add(new Dictionary<string, string>() { { "fileid", "5" }, { "count", "2" } });
            partprop.ChildRelation.Add(new Dictionary<string, string>() { { "fileid", "6" }, { "count", "1" } });
            partprop.ChildRelation.Add(new Dictionary<string, string>() { { "fileid", "7" }, { "count", "1" } });

            partprop.ObjectRelation.Add(new Dictionary<string, string>() { { "fileid", "2" } });

            index++;
            partprop = getPartData("00" + index, index, "drw");//2
            docprop.Add(partprop);

            index++;
            partprop = getPartData("00" + index, index, "prt");//3
            partprop.ObjectRelation.Add(new Dictionary<string, string>() { { "fileid", "4" } });
            docprop.Add(partprop);

            index++;
            partprop = getPartData("00" + index, index, "drw");//4
            docprop.Add(partprop);

            index++;
            partprop = getPartData("00" + index, index, "prt");//5
            docprop.Add(partprop);

            index++;
            partprop = getPartData("00" + index, index, "prt");//6
            docprop.Add(partprop);

            index++;
            partprop = getPartData("00" + index, index, "asm");//7
            partprop.ChildRelation.Add(new Dictionary<string, string>() { { "fileid", "8" }, { "count", "1" } });
            partprop.ChildRelation.Add(new Dictionary<string, string>() { { "fileid", "9" }, { "count", "10" } });
            docprop.Add(partprop);

            index++;
            partprop = getPartData("00" + index, index, "prt");//8
            docprop.Add(partprop);

            index++;
            partprop = getPartData("00" + index, index, "prt");//9
            docprop.Add(partprop);

            docprop.SaveXml(@"c:\123.txt");
#endif
        }

        static PartProperty getPartData(string index, int fileid, string filetype)
        {
            PartProperty part = new PartProperty();

            part.Add("filesize", "10000");
            part.Add("filepath", path);
            part.Add("filename", index + "." + filetype.ToLower());
            part.Add("fileid", fileid + "");
            part.Add("filetype", filetype.ToUpper());

            part.Add("partname", "partname" + index);
            part.Add("partNumber", "partNumber" + index);
            part.Add("DrawNo", "DrawNo" + index);

            return part;
        }
    }
}
