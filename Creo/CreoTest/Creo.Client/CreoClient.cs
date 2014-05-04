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
            //
            throw new NotImplementedException();
        }

        public System.Collections.ArrayList GetBomByFileName(string Filename)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentFileName()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentFilePath()
        {
            throw new NotImplementedException();
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
