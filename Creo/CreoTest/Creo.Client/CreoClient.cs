using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

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
            DealDocumentProperty(doc);
            return doc.ConvertToBOM();
        }

        /// <summary>
        /// 处理文档属性
        /// </summary>
        /// <param name="docprop"></param>
        private void DealDocumentProperty(DocumentProperty docprop)
        {
            Dictionary<string, string> guids = new Dictionary<string, string>(docprop.Count);
            string guid = "", fileid = "";
            foreach (PartProperty item in docprop)
            {
                fileid = item[Key.Fileid];
                if (!guids.TryGetValue(fileid, out guid))
                {
                    guid = Guid.NewGuid().ToString();
                    guids.Add(fileid, guid);
                }
                item[Key.VerId] = guid;
                item[Key.ActiveConfig] = item[Key.CurConfigName] = item[Key.ConfigName];
                item.Remove(Key.ConfigName);
            }
            guid = ""; fileid = "";
            foreach (PartProperty item in docprop)
            {
                foreach (var child in item.ChildRelation.Concat(item.ObjectRelation))
                {
                    fileid = child[Key.Fileid];
                    child[Key.VerId] = guids[fileid];
                }
            }
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

        /// <summary>
        /// 获取北京时间
        /// </summary>
        /// <returns></returns>
        private static DateTime getTime()
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
