using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kingdee.PLM.Integration.Setup.Abstract;
using Kingdee.PLM.Integration.Setup.Common;
using plm.kingdee.com;
using System.IO;
using Intgration.Common.win;

namespace Creo.Setup
{
    class Creo2PDF : CreoSetup, InstallInterface
    {
        private ProgressChanged onProgressChanged;
        private MessageChanged onMessageChanged;
        public new event ProgressChanged OnProgressChanged;
        public new event MessageChanged OnMessageChanged;
        public bool Suspend
        {
            private get;
            set;
        }
        public bool Interrupt
        {
            private get;
            set;
        }
        public new string SourceDefaultDir
        {
            get;
            set;
        }
        public new string UserDefaultDir
        {
            get;
            set;
        }
        public string LogFilePath
        {
            private get;
            set;
        }
        public new string LanguageMark
        {
            get;
            set;
        }
        public new long TotalSize
        {
            get
            {
                return 10240000L;
            }
        }
        public new string Description
        {
            get
            {
                return MutiLanguageConvert.LoadKDString("\u3000\u3000在PLM中设置了电子签名并安装该签名插件才可以使用Creo电子签名功能。");
            }
        }
        public new string ModuleName
        {
            get
            {
                return MutiLanguageConvert.LoadKDString("Creo电子签名插件");
            }
        }
        public new string ID
        {
            get
            {
                return "D19F8365-7172-4ae3-B302-09753C0263E0";
            }
        }
        public new string ParentId
        {
            get
            {
                return "5684A405-2F3D-47c3-A6E9-F91A8F419016";
            }
        }
        public new string DirName
        {
            get
            {
                return "ProeToPdf";
            }
        }
        public new int NeedTime
        {
            get
            {
                return 10;
            }
        }
        public new float SortIndex
        {
            get
            {
                return 10f;
            }
        }
        public new float Version
        {
            get
            {
                return 1f;
            }
        }
        public new bool IsNecessary
        {
            get
            {
                return false;
            }
        }
        public bool SetupMain1()
        {
            string str = string.Format("{0}\\{1}\\", this.SourceDefaultDir, this.DirName);
            string windowsPath = CommonBase.GetWindowsPath();
            CommonBase.CopyFile(str + "PlmConfig.dop", windowsPath + "\\PlmConfig.dop");

            AccessControl.DirectoryInfoAccessControl(windowsPath + @"\temp\plmpdf");

            return true;
        }
        public bool RemoveMain1()
        {
            string windowsPath = CommonBase.GetWindowsPath();
            string text = windowsPath + "\\PlmConfig.dop";
            CommonBase.DeleteFileNoException(text);
            return true;
        }

        public override CheckEnviromentStateMessage CheckEnviroment()
        {
            string text = base.CheckMain();
            if (string.IsNullOrEmpty(text))
            {
                return new CheckEnviromentStateMessage(CheckEnviromentState.OK, "");
            }
            return new CheckEnviromentStateMessage(CheckEnviromentState.Wrong, text);
        }
        public override CheckEnviromentStateMessage CheckRemoveEnviroment()
        {
            string text = base.CheckRemoveMain();
            if (string.IsNullOrEmpty(text))
            {
                return new CheckEnviromentStateMessage(CheckEnviromentState.OK, "");
            }
            return new CheckEnviromentStateMessage(CheckEnviromentState.Wrong, text);
        }
        public override bool Install()
        {
            return this.SetupMain1();
        }
        public override bool Uninstall()
        {
            return this.RemoveMain1();
        }
        public override bool Rollback()
        {
            return this.RemoveMain1();
        }
    }
}
