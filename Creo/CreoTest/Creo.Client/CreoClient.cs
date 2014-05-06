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
            get { return "PREO"; }
        }

        public bool CheckApplicationIsRun()
        {
            return Tools.ApplicationIsRun("xtop.exe") || Tools.ApplicationIsRun("pro_comm_msg.exe");
        }

        public System.Collections.ArrayList GetBom()
        {
            DocumentProperty doc = DocumentProperty.LoadXml(BomFilePath);
            return doc.ConvertToBOM();
        }

        public System.Collections.ArrayList GetBomByFileName(string Filename)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentFileName()
        {
            DocumentProperty doc = DocumentProperty.LoadXml(BomFilePath);
            if (doc != null && doc.Count > 0)
            {
                return doc.First<PartProperty>()[""];
            }
            return "";
        }

        public string GetCurrentFilePath()
        {
            DocumentProperty doc = DocumentProperty.LoadXml(BomFilePath);
            if (doc != null && doc.Count > 0)
            {
                return doc.First<PartProperty>()[""];
            }
            return "";
        }

        public string GetCurrentFileProperty()
        {
            return "";
        }

        public string GetCurrentFullPath()
        {
            throw new NotImplementedException();
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
