using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kingdee.PLM.Integration.Client.Common.Abstract;
using Intgration.Common;

namespace Creo.Client
{
    public class CreoClient : IIntegration
    {
        static string BomFilePath;
        
        static CreoClient()
        {
            BomFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ptc\\output.txt";
        }

        public string CadType
        {
            get { return "PROE"; }
        }

        public bool CheckApplicationIsRun()
        {
            return Tools.ApplicationIsRun("xtop.exe") || Tools.ApplicationIsRun("pro_comm_msg.exe");
        }

        public System.Collections.ArrayList GetBom()
        {
            DocumentProperty doc = DocumentProperty.LoadXml(BomFilePath);

            Dictionary<string, string> guids = new Dictionary<string, string>(doc.Count);
            string guid = "", fileid = "";
            foreach (PartProperty item in doc)
            {
                fileid = item[Key.Fileid];
                if (!guids.TryGetValue(fileid, out guid))
                {
                    guid = Guid.NewGuid().ToString();
                    guids.Add(fileid, guid);
                }
                item[Key.VerId] = guid;
            }
            guid = ""; fileid = "";
            foreach (PartProperty item in doc)
            {
                foreach (var child in item.ChildRelation.Concat(item.ObjectRelation))
                {
                    fileid = child[Key.Fileid];
                    child[Key.VerId] = guids[fileid];
                }
            }

            return doc.ConvertToBOM();
        }

        public System.Collections.ArrayList GetBomByFileName(string Filename)
        {
            throw new NotImplementedException();
        }
        
        public string GetCurrentFileName()
        {
            return Tools.GetFileContentByContent(BomFilePath, "<key><string>filename</string></key>", "<value><string>(.*)</string></value>");
        }

        public string GetCurrentFilePath()
        {
            return Tools.GetFileContentByContent(BomFilePath, "<key><string>filepath</string></key>", "<value><string>(.*)</string></value>").TrimEnd('\\');
        }

        public string GetCurrentFileProperty()
        {
            return "";
        }

        public string GetCurrentFullPath()
        {
            return GetCurrentFilePath() + "\\" + GetCurrentFileName();
        }

        public string GetFileDesCription(string FullPath)
        {
            throw new NotImplementedException();
        }

        public bool OpenFile(string FullPath)
        {
            throw new NotImplementedException();
        }

        public bool SetPropertyInfo(string FullPath, string PropString)
        {
            throw new NotImplementedException();
        }

        public bool WriteFileDesCription(string FullPath, string PropString)
        {
            throw new NotImplementedException();
        }
    }
}
