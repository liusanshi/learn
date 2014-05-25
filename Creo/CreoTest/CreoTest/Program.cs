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
