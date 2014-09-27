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
using System.Security.AccessControl;
using System.IO;
using Intgration.Common.Utility;
using Creo.Setup;
using System.Net;

namespace CreoTest
{
    class Program
    {
        /// <summary>
        /// 固定路径
        /// </summary>
        const string path = @"c:\test\creo";

        const string BOMJSON = "[{\"filename\":\"BCFL-029-026-0020-A-D.asm.10\",\"filesize\":\"161983\",\"fileid\":\"1\",\"filetype\":\"ASM\",\"filepath\":\"D:\\\\works\\\\ProE\\\\0516test\\\\\",\"DESCRIPTION\":\"拐角轴组\",\"MODELED_BY\":\"\",\"REMARKS\":\"\",\"PTC_COMMON_NAME\":\"\",\"PDMREV\":\"1.1+\",\"PDMDB\":\"\",\"PDMRL\":\"设计中\",\"PROI_REVISION\":\"1\",\"PROI_VERSION\":\"1+\",\"PROI_BRANCH\":\"main\",\"PROI_RELEASE\":\"设计中\",\"PTC_MODIFIED\":\"1\",\"PROI_CREATED_BY\":\"adm\",\"PROI_CREATED_ON\":\"2014/1/22 8:22:00\",\"PROI_LOCATION\":\"根文件夹/2012/6-红枣/004-002/临时\",\"MATERIAL\":\"\",\"GONGYI\":\"A\",\"MODEL_NAME\":\"BCFL-029-026-0020-A-D\",\"verid\":\"13330371-ff42-4544-8e72-9270a0c90b9b\",\"_curconfigname\":\"BCFL-029-026-0020-A-D_INST\",\"activeconfig\":\"BCFL-029-026-0020-A-D_INST\",\"child\":[{\"fileid\":\"2\",\"count\":\"1\",\"verid\":\"d6aefdd3-6f85-4455-8592-b43b65568810\"},{\"fileid\":\"3\",\"count\":\"2\",\"verid\":\"8b1753c9-b3d6-4a0e-ae8b-adaee0f52522\"},{\"fileid\":\"4\",\"count\":\"2\",\"verid\":\"da4e1c0c-cfea-4ab4-b046-d0fc775a1603\"}],\"__filemd5\":\"\"},{\"filename\":\"BCFL-029-026-0024-P-D.prt.7\",\"filesize\":\"208501\",\"fileid\":\"2\",\"filetype\":\"PRT\",\"filepath\":\"D:\\\\works\\\\ProE\\\\0516test\\\\\",\"DESCRIPTION\":\"隔板机拐弯轴\",\"MODELED_BY\":\"\",\"PAGE\":\"\",\"GONGYI\":\"MA\",\"MATERIAL\":\"SUS304\",\"REMARKS\":\"\",\"PTC_COMMON_NAME\":\"\",\"PDMREV\":\"1.1+\",\"PDMDB\":\"\",\"PDMRL\":\"设计中\",\"PROI_REVISION\":\"1\",\"PROI_VERSION\":\"1+\",\"PROI_BRANCH\":\"main\",\"PROI_RELEASE\":\"设计中\",\"PTC_MODIFIED\":\"1\",\"PROI_CREATED_BY\":\"adm\",\"PROI_CREATED_ON\":\"2014/1/22 8:22:00\",\"PROI_LOCATION\":\"根文件夹/2012/6-红枣/004-002/临时\",\"PAPER\":\"Y\",\"MODEL_NAME\":\"BCFL-029-026-0024-P-D\",\"verid\":\"d6aefdd3-6f85-4455-8592-b43b65568810\",\"_curconfigname\":\"BCFL-029-026-0024-P-D\",\"activeconfig\":\"BCFL-029-026-0024-P-D\",\"__filemd5\":\"\"},{\"filename\":\"UCPA2.prt.7\",\"filesize\":\"244871\",\"fileid\":\"3\",\"filetype\":\"PRT\",\"filepath\":\"D:\\\\works\\\\ProE\\\\0516test\\\\\",\"DESCRIPTION\":\"带座轴承\",\"MODELED_BY\":\"\",\"PAGE\":\"\",\"GONGYI\":\"FS\",\"MATERIAL\":\"\",\"REMARKS\":\"GB\",\"PTC_COMMON_NAME\":\"\",\"PDMREV\":\"1.14+\",\"PDMDB\":\"\",\"PDMRL\":\"设计中\",\"PROI_REVISION\":\"1\",\"PROI_VERSION\":\"14+\",\"PROI_BRANCH\":\"main\",\"PROI_RELEASE\":\"设计中\",\"PTC_MODIFIED\":\"1\",\"PROI_CREATED_BY\":\"adm\",\"PROI_CREATED_ON\":\"2013/10/28 14:39:08\",\"PROI_LOCATION\":\"根文件夹/Standard/带座轴承\",\"MODEL_NAME\":\"UCPA210\",\"verid\":\"8b1753c9-b3d6-4a0e-ae8b-adaee0f52522\",\"_curconfigname\":\"UCPA210\",\"activeconfig\":\"UCPA210\",\"__filemd5\":\"\"},{\"filename\":\"BCFL-029-026-0011-P-D.prt.8\",\"filesize\":\"305216\",\"fileid\":\"4\",\"filetype\":\"PRT\",\"filepath\":\"D:\\\\works\\\\ProE\\\\0516test\\\\\",\"DESCRIPTION\":\"轴承座\",\"MODELED_BY\":\"\",\"GONGYI\":\"LBA\",\"MATERIAL\":\"SUS304板\",\"L\":\"120.00\",\"W\":\"74.50\",\"T\":\"3.00\",\"REMARKS\":\"UCPA204用\",\"PDMREV\":\"1.1+\",\"PDMDB\":\"\",\"PDMRL\":\"设计中\",\"PROI_REVISION\":\"1\",\"PROI_VERSION\":\"1+\",\"PROI_BRANCH\":\"main\",\"PROI_RELEASE\":\"设计中\",\"PTC_MODIFIED\":\"1\",\"PROI_CREATED_BY\":\"adm\",\"PROI_CREATED_ON\":\"2014/1/22 8:22:00\",\"PROI_LOCATION\":\"根文件夹/2012/6-红枣/004-002/临时\",\"PTC_COMMON_NAME\":\"\",\"QTY\":\"\",\"CMASS\":\"0.00\",\"AREA\":\"0.01\",\"PTC_MATERIAL_NAME\":\"STAINLESS\",\"MODEL_NAME\":\"BCFL-029-026-0011-P-D\",\"SMT_UPDATE_BEND_ALLOW_INFO\":\"0\",\"SMT_THICKNESS\":\"3.00\",\"SMT_PART_BEND_ALLOWANCE_FACTOR\":\"0.30\",\"SMT_PART_BEND_ALLOW_FACTOR_TYPE\":\"Y FACTOR\",\"SMT_PART_BEND_TABLE_NAME\":\"BEND-TALE\",\"SMT_DFLT_BEND_RADIUS\":\"0.50\",\"SMT_DFLT_RADIUS_SIDE\":\"Inside\",\"SMT_DFLT_BEND_ANGLE\":\"90.00\",\"SMT_DFLT_CRNR_REL_TYPE\":\"V Notch\",\"SMT_DFLT_CRNR_REL_WIDTH\":\"3.00\",\"SMT_DFLT_CRNR_REL_DEPTH_TYPE\":\"Blind\",\"SMT_DFLT_CRNR_REL_DEPTH\":\"3.00\",\"SMT_DFLT_BEND_REL_TYPE\":\"Rip\",\"SMT_DFLT_BEND_REL_WIDTH\":\"3.00\",\"SMT_DFLT_BEND_REL_DEPTH_TYPE\":\"Up to Bend\",\"SMT_DFLT_BEND_REL_DEPTH\":\"6.00\",\"SMT_DFLT_BEND_REL_ANGLE\":\"45.00\",\"SMT_GAP\":\"1.50\",\"SMT_DFLT_EDGE_TREA_TYPE\":\"Open\",\"SMT_DFLT_EDGE_TREA_WIDTH\":\"-1.50\",\"SMT_DFLT_MITER_CUT_WIDTH\":\"1.50\",\"SMT_DFLT_MITER_CUT_OFFSET\":\"3.30\",\"verid\":\"da4e1c0c-cfea-4ab4-b046-d0fc775a1603\",\"_curconfigname\":\"BCFL-029-026-0011-P-D\",\"activeconfig\":\"BCFL-029-026-0011-P-D\",\"__filemd5\":\"\"}]";

        static void Main(string[] args)
        {
            var reg_prt = new System.Text.RegularExpressions.Regex(@"(.+)\.prt(?:.\d+)?$");
            var reg_asm = new System.Text.RegularExpressions.Regex(@"(.+)\.asm(?:.\d+)?$");

            Console.WriteLine(GetFileWithoutExt("GB5781"));
            Console.WriteLine(GetFileWithoutExt("GB5781.prt"));
            Console.WriteLine(GetFileWithoutExt("GB5781.prt.23"));
            Console.WriteLine(GetFileWithoutExt("M12X30_GB5781.asm"));
            Console.WriteLine(GetFileWithoutExt("M12X30_GB5781.asm.677"));

            Console.WriteLine(Path.GetFileNameWithoutExtension("GB5781"));
            Console.WriteLine(Path.GetFileNameWithoutExtension("GB5781.prt.1"));
            Console.WriteLine(Path.GetFileNameWithoutExtension("M12X30_GB5781.asm.1"));
            Console.Read();
            return;

            //var t = getTime();
            if (DateTime.Now >= new DateTime(2014, 8, 1))
            {
                Console.WriteLine("dayu");
            }

            //AssociateFieldConllection collent = new AssociateFieldConllection();
            //collent.Add(new Associatefield("MaterialCode", "MODEL_NAME", true, 0, 0, false, "物料编码", true));
            //collent.Add(new Associatefield("MaterialSpec", "SPEC", false, 0, 0, false, "物料规格", false));
            //collent.Add(new Associatefield("MaterialSubstance", "MATERIAL", false, 0, 0, false, "物料材质", false));
            //collent.Add(new Associatefield("MaterialName", "DESCRIPTION", false, 0, 0, false, "物料名称", false));
            //collent.Add(new Associatefield("MaterialDrawNumber", "DRAWNO", false, 0, 0, false, "图号", false));

            //CreoOperate op = new CreoOperate();

            //op.AppTypeName = "PROE";
            //op.IntegType = Proway.PLM.Document.IntegrationType.PROE;
            //op.CategoryId = "affba405-d687-488b-8bb7-05e9a8a986ed";
            //op.WorkId = "";
            //op.CurOperatePosition =  Proway.PLM.Document.OperatePosition.CheckIn;
            //op.FolderId = "60C4C5B7-0107-4F41-9147-F8904FAC6728";
            //op.FileTypeId = "4be5f1a4-db00-4aa4-88cf-a8a5c48a88bf";
            //op.UserId = "FFB0AC2D-C1B5-49E2-89B2-F4058523DF18";
            //op.IsCreateCopy = false;
            //op.IsOperateBOM = true;
            //op.IndexFields = collent;

            ////op.CheckDoc(BOMJSON);

            //Dictionary<string,object> dic = new Dictionary<string,object>();

            //BOMHelp.Write(dic, _.MultiConfiguration, true);

            //var aa = dic.GetValue(_.MultiConfiguration, false);

            //Console.WriteLine(aa);


            //Console.WriteLine(OSHelp.IsVistaAndSubsequentVersion);
            //Console.WriteLine(OSHelp.IsWin7);

            //AccessControl.FileInfoAccessControl(@"c:\111.txt");
            //AccessControl.DirectoryInfoAccessControl(@"c:\111");

            //var regk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\PTC");
            //Console.WriteLine(regk == null);

            //CreoClient creo = new CreoClient();
            //var aa = creo.GetBom();

            //测试版本
            //Console.WriteLine(OSHelp.GetExcutePlatform(@"D:\github\learn\Creo\CreoTest\Creo.Setup\pubilc\PLM64.dll").ToString());
            //Console.WriteLine(OSHelp.GetExcutePlatform(@"D:\github\learn\Creo\CreoTest\Creo.Setup\pubilc\PLM.dll").ToString());

            //Console.WriteLine(OSHelp.GetExcutePlatform(@"C:\Program Files\Kingdee\K3PLM\Integration\Proe\PLM.dll").ToString());

            foreach (var item in FileAndDirectoryManager.SearchFile(@"D:\MyWork\PTC\Creo 2.0\Common Files\M060\",
                new string[] { "i486_nt\\obj", "x86e_win64\\obj" }, "xtop.exe")) 
            {
                Console.WriteLine(item);
            }

            if (UACHelp.IsRunAsAdmin())
            {
                Console.WriteLine("管理员启动");
            }
            else
            {
                Console.WriteLine("非管理员启动");
            }

            //CreoSetup setup = new CreoSetup();
            //setup.SourceDefaultDir = @"D:\Program Files\kingdee\K3PLM\Integration\Integration Setup\Resources";
            //setup.UserDefaultDir = @"C:\Program Files\Kingdee\K3PLM\Integration";

            //setup.CopyMenuFile();

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

        /// <summary>
        /// 零件的名称的匹配串
        /// </summary>
        private static readonly System.Text.RegularExpressions.Regex reg_prt
            = new System.Text.RegularExpressions.Regex(@"(.+)\.prt(?:.\d+)?$"
                    , System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        /// <summary>
        /// 装配件的名称匹配串
        /// </summary>
        private static readonly System.Text.RegularExpressions.Regex reg_asm
            = new System.Text.RegularExpressions.Regex(@"(.+)\.asm(?:.\d+)?$"
                , System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);


        public static string GetFileWithoutExt(string filename)
        {
            var m = reg_prt.Match(filename);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            m = reg_asm.Match(filename);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            //System.IO.Path.GetFileNameWithoutExtension()
            return filename;
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

        /// <summary>
        /// 获取北京时间
        /// </summary>
        /// <returns></returns>
        public static DateTime getTime()
        {
            //t0 = new Date().getTime();
            //nyear = 2011;
            //nmonth = 7;
            //nday = 5;
            //nwday = 2;
            //nhrs = 17;
            //nmin = 12;
            //nsec = 12;
            DateTime dt;
            WebRequest wrt = null;
            WebResponse wrp = null;
            try
            {
                wrt = WebRequest.Create("http://www.beijing-time.org/time.asp");
                wrp = wrt.GetResponse();

                string html = string.Empty;
                using (Stream stream = wrp.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        html = sr.ReadToEnd();
                    }
                }

                string[] tempArray = html.Split(';');
                for (int i = 0; i < tempArray.Length; i++)
                {
                    tempArray[i] = tempArray[i].Replace("\r\n", "");
                }

                string year = tempArray[1].Substring(tempArray[1].IndexOf("nyear=") + 6);
                string month = tempArray[2].Substring(tempArray[2].IndexOf("nmonth=") + 7);
                string day = tempArray[3].Substring(tempArray[3].IndexOf("nday=") + 5);
                string hour = tempArray[5].Substring(tempArray[5].IndexOf("nhrs=") + 5);
                string minite = tempArray[6].Substring(tempArray[6].IndexOf("nmin=") + 5);
                string second = tempArray[7].Substring(tempArray[7].IndexOf("nsec=") + 5);
                dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second);
            }
            catch (WebException)
            {
                dt = DateTime.Now;
            }
            catch (Exception)
            {
                dt = DateTime.Now;
            }
            finally
            {
                if (wrp != null)
                    wrp.Close();
                if (wrt != null)
                    wrt.Abort();
            }
            return dt;
        }
    }
}
