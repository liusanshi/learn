using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Proway.PLM.Document;
using Proway.Framework;
using Proway.PLM.Document.Validate;

namespace Creo.Server
{
    /// <summary>
    /// Creo 处理类
    /// </summary>
    public class CreoOperate : DocumentOperateSW
    {

        /// <summary>
        /// 导入时的验证
        /// </summary>
        /// <param name="server"></param>
        protected override void ImportValidator(ValidateServer server)
        {
            base.ImportValidator(server);
            //验证删除
            //server.RegisterValidator(new ValidateDeleteDoc(this.dicDeleteDoc, (DocumentVersion docver, DocStruct doc) => BOMHelp.IsEquals(BOMHelp.GetFileNameNoVersion(docver.FileName), BOMHelp.GetFileNameNoVersion(doc.FileName))));
            //验证删除参数
            //server.RegisterValidator(new ValidateDocConfig(base.IndexFields));
            //验证物料是否已经被使用
            //server.RegisterValidator(new ValidateCanUseMaterial());

            var empty = new ValidateEmpty();
            server.Replace(typeof(ValidateUpdateAddin), empty);
            server.Replace(typeof(ValidateStandardParts), empty);
            server.Replace(typeof(ValidateStandardPartsState), empty);
            server.Replace(typeof(ValidateStandardPartsMaterialRepeat), empty);
        }

        /// <summary>
        /// 检入时的验证
        /// </summary>
        /// <param name="server"></param>
        protected override void CheckInValidator(ValidateServer server)
        {
            base.CheckInValidator(server);
            
            //验证删除参数
            //server.InsertBefore(typeFromHandle, new ValidateDocConfig(base.IndexFields));
            //是否有删除配置
            //server.InsertBefore(typeFromHandle, new ValidateDocConfigDelete());
            //验证物料是否已经被使用
            //server.RegisterValidator(new ValidateCanUseMaterial());
            var empty = new ValidateEmpty();
            server.Replace(typeof(ValidateUpdateAddin), empty);
            server.Replace(typeof(ValidateStandardParts), empty);
            server.Replace(typeof(ValidateStandardPartsState), empty);
            server.Replace(typeof(ValidateStandardPartsMaterialRepeat), empty);
            server.Replace(typeof(ValidateStandardPartsDelete), empty);
        }

        protected override BOMStruct MergeDcounentByProperty(BOMStruct bom)
        {
            bool flag = bom.IsolatedNodes.Any<DocStruct>();

            TraceWriteLine("flag :" + flag);
            foreach (DocStruct current in bom.FindRootConfigs())
            {
                TraceWriteLine("进入循环");
                if (!current.IsConfigPart)
                {
                    break;
                }
                TraceWriteLine("进入循环 里面的处理");
                current.AddValue("__isrootconfig", true);
                current.AddValue("__hasolatedconfig", flag);
                if (current.ConfigName == current.GetString("activeconfig"))
                {
                    current.IsActive = true;
                    foreach (DocStruct current2 in current.GetDescendant())
                    {
                        current2.IsActive = true;
                    }
                }
            }
            foreach (DocView current3 in bom.BOMView)
            {
                current3.GetCurrentData().WriteValue("__defaultdoc", true);
            }
            return base.MergeDcounentByProperty(bom);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg"></param>
        [Conditional("DEBUG")]
        void TraceWriteLine(string msg)
        {
#if DEBUG
      Proway.Framework.Core.Log.Info(msg);
#endif
        }
    }
}
