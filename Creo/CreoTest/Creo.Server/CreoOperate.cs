using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            server.RegisterValidator(new ValidateDeleteDoc(this.dicDeleteDoc, (DocumentVersion docver, DocStruct doc) => BOMHelp.IsEquals(BOMHelp.GetFileNameNoVersion(docver.FileName), BOMHelp.GetFileNameNoVersion(doc.FileName))));
            //验证删除参数
            server.RegisterValidator(new ValidateDocConfig(base.IndexFields));
            //验证物料是否已经被使用
            server.RegisterValidator(new ValidateCanUseMaterial());
        }

        /// <summary>
        /// 检入时的验证
        /// </summary>
        /// <param name="server"></param>
        protected override void CheckInValidator(ValidateServer server)
        {
            base.CheckInValidator(server);
            Type typeFromHandle = typeof(ValidateIsNewMaterial);
            //验证删除参数
            server.InsertBefore(typeFromHandle, new ValidateDocConfig(base.IndexFields));
            //是否有删除配置
            server.InsertBefore(typeFromHandle, new ValidateDocConfigDelete());
            //验证物料是否已经被使用
            server.RegisterValidator(new ValidateCanUseMaterial());
        }
    }
}
