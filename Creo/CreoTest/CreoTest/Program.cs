using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intgration.Common;

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
