using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using Microsoft.Win32;

using Kingdee.PLM.Integration.Setup.Abstract;
using Kingdee.PLM.Integration.Setup.Common;
using System.Security.AccessControl;
using Intgration.Common.win;

namespace Creo.Setup
{
    public class CreoSetup : InstallInterface
    {
        private StringBuilder CheckString = new StringBuilder();
        private ProgressChanged onProgressChanged;
        private MessageChanged onMessageChanged;
        public event ProgressChanged OnProgressChanged;
        public event MessageChanged OnMessageChanged;
        /// <summary>
        /// creo 菜单dll 32位
        /// </summary>
        const string CREOPACKAGE = "PLM.dll";
        /// <summary>
        /// creo 菜单dll 64位
        /// </summary>
        const string CREOPACKAGE64 = "PLM64.dll";
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
        public string SourceDefaultDir
        {
            get;
            set;
        }
        public string UserDefaultDir
        {
            get;
            set;
        }
        public string LogFilePath
        {
            private get;
            set;
        }
        public string LanguageMark
        {
            get;
            set;
        }
        public long TotalSize
        {
            get
            {
                return 30240000L;
            }
        }
        public string Description
        {
            get
            {
                return plm.kingdee.com.MutiLanguageConvert.LoadKDString("\u3000\u3000Creo集成插件的安装将使您从Creo软件中直接执行金蝶PLM系统操作，例如：检入、检出等。");
            }
        }
        public string ModuleName
        {
            get
            {
                return plm.kingdee.com.MutiLanguageConvert.LoadKDString("Creo集成插件");
            }
        }
        public string ID
        {
            get
            {
                return "5684A405-2F3D-47c3-A6E9-F91A8F419016";
            }
        }
        public string ParentId
        {
            get
            {
                return "";
            }
        }
        public string DirName
        {
            get
            {
                return "Proe";
            }
        }
        public int NeedTime
        {
            get
            {
                return 10;
            }
        }
        public float SortIndex
        {
            get
            {
                return 3f;
            }
        }
        public float Version
        {
            get
            {
                return 1f;
            }
        }
        public bool IsNecessary
        {
            get
            {
                return false;
            }
        }

        private void CopyFile()
        {
            string str = string.Format("{0}\\{1}\\", this.SourceDefaultDir, this.DirName);
            string text = string.Format("{0}\\{1}\\", this.UserDefaultDir, this.DirName);
            string str2 = string.Format("{0}\\{1}\\", this.SourceDefaultDir, "Common\\");
            string systemPath = CommonBase.GetSystemPath();
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            string oSBit = CommonBase.GetOSBit();
            if (oSBit == "64")
            {
                if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\PTC") != null)//判断是否32位creo
                {
                    CommonBase.CopyFile(str + CREOPACKAGE, text + CREOPACKAGE);
                }
                else
                {
                    CommonBase.CopyFile(str + CREOPACKAGE64, text + CREOPACKAGE);
                }
            }
            else
            {
                CommonBase.CopyFile(str + CREOPACKAGE, text + CREOPACKAGE);
            }
            if (!File.Exists("c:\\WINDOWS\\IntegrationLogin.exe"))
                CommonBase.CopyFile(str2 + "IntegrationLogin.exe", "c:\\WINDOWS\\IntegrationLogin.exe");
            if (!File.Exists("c:\\WINDOWS\\IntegrationLogin.exe.config"))
                CommonBase.CopyFile(str2 + "IntegrationLogin.exe.config", "c:\\WINDOWS\\IntegrationLogin.exe.config");

            //给文件、文件夹设置权限
            //system32\LoginSetting.ini
            //system32\proegm.txt
            //system32\Login.Html
            //%ALLUSERSPROFILE%\PLM\LoginSetting.ini
            //c:\windows\temp\plmpdf
            foreach (var item in DealFiles())
            {
                AccessControl.FileInfoAccessControl(item);
            }
            
            this.CopyMenuFile();
        }

        private IEnumerable<string> DealFiles()
        {
            string system32path = CommonBase.GetSystemPath().TrimEnd('\\', '/');
            string system64path = CommonBase.GetSystem64Path().TrimEnd('\\', '/');
            string allusers = CommonBase.GetAllUsersDocumentPath().TrimEnd('\\', '/');
            yield return Path.Combine(system32path, "LoginSetting.ini");
            yield return Path.Combine(system32path, "proegm.txt");
            yield return Path.Combine(system32path, "Login.html");
            yield return Path.Combine(system64path, "LoginSetting.ini");
            yield return Path.Combine(allusers, "LoginSetting.ini");
        }

        private void CopyMenuFile()
        {
            //string systemLanguageID = plm.kingdee.com.MutiLanguageConvert.GetSystemLanguageID();
            string systemLanguageID = "chn";
            string str = string.Format("{0}\\{1}\\", this.SourceDefaultDir, this.DirName);
            string str2 = string.Format("{0}\\{1}\\", this.UserDefaultDir, this.DirName);
            string proeSetupPath = this.GetProeSetupPath();
            if (!Directory.Exists(str2 + "Text\\chinese_cn\\"))
            {
                Directory.CreateDirectory(str2 + "Text\\chinese_cn\\");
            }
            if (!Directory.Exists(str2 + "Text\\chinese_tw\\"))
            {
                Directory.CreateDirectory(str2 + "Text\\chinese_tw\\");
            }
            if (!Directory.Exists(str2 + "Text\\usascii\\"))
            {
                Directory.CreateDirectory(str2 + "Text\\usascii\\");
            }
            if (!Directory.Exists(proeSetupPath + "Text\\chinese_cn\\"))
            {
                Directory.CreateDirectory(proeSetupPath + "Text\\chinese_cn\\");
            }
            if (!Directory.Exists(proeSetupPath + "Text\\chinese_tw\\"))
            {
                Directory.CreateDirectory(proeSetupPath + "Text\\chinese_tw\\");
            }
            if (!Directory.Exists(proeSetupPath + "Text\\usascii\\"))
            {
                Directory.CreateDirectory(proeSetupPath + "Text\\usascii\\");
            }
            CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", str2 + "Text\\usascii\\Message.txt");
            CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", str2 + "Text\\chinese_cn\\Message.txt");
            CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", str2 + "Text\\chinese_tw\\Message.txt");
            CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", proeSetupPath + "\\Text\\usascii\\Message.txt");
            CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", proeSetupPath + "\\Text\\chinese_cn\\Message.txt");
            CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", proeSetupPath + "\\Text\\chinese_tw\\Message.txt");
            //if (systemLanguageID == "cht")
            //{
            //    CommonBase.CopyFile(str + "Text\\chinese_tw\\FMessage.txt", str2 + "Text\\usascii\\Message.txt");
            //    CommonBase.CopyFile(str + "Text\\chinese_tw\\FMessage.txt", str2 + "Text\\chinese_cn\\Message.txt");
            //    CommonBase.CopyFile(str + "Text\\chinese_tw\\FMessage.txt", str2 + "Text\\chinese_tw\\Message.txt");
            //    CommonBase.CopyFile(str + "Text\\chinese_tw\\FMessage.txt", proeSetupPath + "\\Text\\usascii\\Message.txt");
            //    CommonBase.CopyFile(str + "Text\\chinese_tw\\FMessage.txt", proeSetupPath + "\\Text\\chinese_cn\\Message.txt");
            //    CommonBase.CopyFile(str + "Text\\chinese_tw\\FMessage.txt", proeSetupPath + "\\Text\\chinese_tw\\Message.txt");
            //}
            //else
            //{
            //    if (systemLanguageID == "eng")
            //    {
            //        CommonBase.CopyFile(str + "Text\\usascii\\EMessage.txt", str2 + "Text\\usascii\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\usascii\\EMessage.txt", str2 + "Text\\chinese_cn\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\usascii\\EMessage.txt", str2 + "Text\\chinese_tw\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\usascii\\EMessage.txt", proeSetupPath + "\\Text\\usascii\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\usascii\\EMessage.txt", proeSetupPath + "\\Text\\chinese_cn\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\usascii\\EMessage.txt", proeSetupPath + "\\Text\\chinese_tw\\Message.txt");
            //    }
            //    else
            //    {
            //        CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", str2 + "Text\\usascii\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", str2 + "Text\\chinese_cn\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", str2 + "Text\\chinese_tw\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", proeSetupPath + "\\Text\\usascii\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", proeSetupPath + "\\Text\\chinese_cn\\Message.txt");
            //        CommonBase.CopyFile(str + "Text\\chinese_cn\\Message.txt", proeSetupPath + "\\Text\\chinese_tw\\Message.txt");
            //    }
            //}
        }
        private void DeleteFile()
        {
            string str = string.Format("{0}\\{1}\\", this.UserDefaultDir, this.DirName);
            CommonBase.DeleteFileNoException(str + CREOPACKAGE);
            string systemPath = CommonBase.GetSystemPath();
            CommonBase.DeleteFileNoException(systemPath + "Login.exe");
            string tagfile = this.GetProeSetupPath() + "\\text\\protk.dat";
            CommonBase.DeleteFileNoException(tagfile);
            string tagfile2 = CommonBase.GetMyDocumentPath() + "\\protk.dat";
            CommonBase.DeleteFileNoException(tagfile2);
            string tagfile3 = this.GetProeCommonFilesLocationPath() + "\\text\\protk.dat";
            CommonBase.DeleteFileNoException(tagfile3);
            string tagfile4 = CommonBase.GetAllUsersDocumentPath() + "\\protk.dat";
            CommonBase.DeleteFileNoException(tagfile4);
            string tagfile5 = this.GetProeCommonFilesLocationPathX64() + "\\text\\protk.dat";
            CommonBase.DeleteFileNoException(tagfile5);
        }

        private void WriteDateFile()
        {
            string proeSetupPath = this.GetProeSetupPath();
            if (Directory.Exists(proeSetupPath + "\\text"))
            {
                string protkPath = proeSetupPath + "\\text\\protk.dat";
                this.WriteProtkFile(protkPath);
            }
            string text = CommonBase.GetMyDocumentPath() + "\\protk.dat";
            if (Directory.Exists(text))
            {
                string protkPath = text + "\\protk.dat";
                this.WriteProtkFile(protkPath);
            }
            string proeCommonFilesLocationPath = this.GetProeCommonFilesLocationPath();
            if (Directory.Exists(proeCommonFilesLocationPath + "\\text"))
            {
                string protkPath = proeCommonFilesLocationPath + "\\text\\protk.dat";
                this.WriteProtkFile(protkPath);
            }
            string proeCommonFilesLocationPathX = this.GetProeCommonFilesLocationPathX64();
            if (Directory.Exists(proeCommonFilesLocationPathX + "\\text"))
            {
                string protkPath = proeCommonFilesLocationPathX + "\\text\\protk.dat";
                this.WriteProtkFile(protkPath);
            }
            string allUsersDocumentPath = CommonBase.GetAllUsersDocumentPath();
            if (Directory.Exists(allUsersDocumentPath))
            {
                string protkPath = allUsersDocumentPath + "\\protk.dat";
                this.WriteProtkFile(protkPath);
            }
        }
        public void WriteProtkFile(string ProtkPath)
        {
            string str = string.Format("{0}\\{1}\\", this.UserDefaultDir, this.DirName);
            try
            {
                StreamWriter streamWriter = new StreamWriter(ProtkPath, false, Encoding.Default);
                streamWriter.WriteLine("name PLMsystem");
                streamWriter.WriteLine("startup dll");
                streamWriter.WriteLine("allow_stop TRUE");
                streamWriter.WriteLine("exec_file " + str + CREOPACKAGE);
                streamWriter.WriteLine("text_dir " + str + "Text");
                streamWriter.WriteLine("revision 1151");
                streamWriter.WriteLine("end");
                streamWriter.Close();
            }
            catch
            {
            }
        }
        public string GetProeSetupPath()
        {
            string subitem = "Software\\PTC\\";
            string text = CommonBase.GetRegistUserKeyValue(subitem, "proemsg", false, false);
            try
            {
                text = text.Replace("\"", "");
                if (text != string.Empty)
                {
                    text = text.Substring(0, text.ToUpper().IndexOf("BIN\\PROE.EXE") - 1);
                }
            }
            catch
            {
                text = "";
            }
            if (string.IsNullOrEmpty(text))
            {
                subitem = "Software\\PTC\\Pro/ENGINEER";
                text = CommonBase.GetRegistMachineKeyValue(subitem, "InstallDir", false, true);
            }
            if (string.IsNullOrEmpty(text))
            {
                subitem = "Software\\PTC\\Creo Parametric";
                text = CommonBase.GetRegistMachineKeyValue(subitem, "InstallDir", false, true);
                if (!string.IsNullOrEmpty(text))
                {
                    text += "\\";
                }
            }
            if (string.IsNullOrEmpty(text))
            {
                subitem = "Software\\Wow6432Node\\PTC\\Creo Parametric";
                text = CommonBase.GetRegistMachineKeyValue(subitem, "InstallDir", false, true);
                if (!string.IsNullOrEmpty(text))
                {
                    text += "\\";
                }
            }
            return text;
        }
        public string GetProeCommonFilesLocationPath()
        {
            string subitem = "Software\\PTC\\Creo Parametric";
            string text = CommonBase.GetRegistMachineKeyValue(subitem, "CommonFilesLocation", false, true);
            if (!string.IsNullOrEmpty(text))
            {
                text += "\\";
            }
            return text;
        }
        public string GetProeCommonFilesLocationPathX64()
        {
            string subitem = "Software\\Wow6432Node\\PTC\\Creo Parametric";
            string text = CommonBase.GetRegistMachineKeyValue(subitem, "CommonFilesLocation", false, true);
            if (!string.IsNullOrEmpty(text))
            {
                text += "\\";
            }
            return text;
        }
        private ArrayList AddCheckProcess()
        {
            return new ArrayList
			{
				"PROE",
				"xtop",
				"iexplore",
				"Login"
			};
        }
        private void GetSoftOpen(ArrayList ExeName, bool IsRemove)
        {
            if (ExeName != null && ExeName.Count > 0)
            {
                for (int i = 0; i < ExeName.Count; i++)
                {
                    string text = ExeName[i].ToString();
                    bool flag = CommonBase.CheckProcessStart(text);
                    if (flag)
                    {
                        if (IsRemove)
                        {
                            this.CheckString.Append(text + plm.kingdee.com.MutiLanguageConvert.LoadKDString("正在运行,请先关闭再进行卸载。\r\n"));
                        }
                        else
                        {
                            this.CheckString.Append(text + plm.kingdee.com.MutiLanguageConvert.LoadKDString("正在运行,请先关闭再进行安装。\r\n"));
                        }
                    }
                }
            }
        }
        public bool SetupMain()
        {
            if (this.onProgressChanged != null)
            {
                this.onProgressChanged(10);
            }
            this.CopyFile();
            if (this.onProgressChanged != null)
            {
                this.onProgressChanged(30);
            }
            if (this.onProgressChanged != null)
            {
                this.onProgressChanged(60);
            }
            this.WriteDateFile();
            if (this.onProgressChanged != null)
            {
                this.onProgressChanged(99);
            }
            return true;
        }
        public bool RemoveMain()
        {
            this.DeleteFile();
            return true;
        }
        public string CheckMain()
        {
            this.CheckString.Remove(0, this.CheckString.Length);
            string proeSetupPath = this.GetProeSetupPath();
            if (proeSetupPath == string.Empty)
            {
                this.CheckString.Append(plm.kingdee.com.MutiLanguageConvert.LoadKDString("未安装CROE软件。\r\n"));
            }
            ArrayList exeName = this.AddCheckProcess();
            this.GetSoftOpen(exeName, false);
            return this.CheckString.ToString().Replace("xtop", "CROE");
        }
        public string CheckRemoveMain()
        {
            this.CheckString.Remove(0, this.CheckString.Length);
            ArrayList exeName = this.AddCheckProcess();
            this.GetSoftOpen(exeName, true);
            return this.CheckString.ToString().Replace("xtop", "CROE");
        }
        private bool GetVcredist()
        {
            return true;
        }
        public virtual CheckEnviromentStateMessage CheckEnviroment()
        {
            string text = this.CheckMain();
            CheckEnviromentStateMessage result;
            if (string.IsNullOrEmpty(text))
            {
                if (!this.GetVcredist())
                {
                    result = new CheckEnviromentStateMessage(CheckEnviromentState.Ignore, "您的操作系统可能未安装Vcredist运行库，请下载安装！！");
                }
                else
                {
                    result = new CheckEnviromentStateMessage(CheckEnviromentState.OK, "");
                }
            }
            else
            {
                result = new CheckEnviromentStateMessage(CheckEnviromentState.Wrong, text);
            }
            return result;
        }
        public virtual CheckEnviromentStateMessage CheckRemoveEnviroment()
        {
            string text = this.CheckRemoveMain();
            CheckEnviromentStateMessage result;
            if (string.IsNullOrEmpty(text))
            {
                result = new CheckEnviromentStateMessage(CheckEnviromentState.OK, "");
            }
            else
            {
                result = new CheckEnviromentStateMessage(CheckEnviromentState.Wrong, text);
            }
            return result;
        }
        public virtual bool Install()
        {
            return this.SetupMain();
        }
        public virtual bool Uninstall()
        {
            return this.RemoveMain();
        }
        public virtual bool Rollback()
        {
            return this.RemoveMain();
        }
    }
}
