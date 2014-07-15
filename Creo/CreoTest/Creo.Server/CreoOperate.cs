using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Proway.PLM.Document;
using Proway.Framework;
using Proway.PLM.Document.Validate;
using System.Collections;

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
            server.Context.ExtendProperty[_.MultiConfiguration] = SupportMultiConfiguration;

            server.RegisterValidator(new ValidateOption());
            server.RegisterValidator(new ValidateDocName(true));
            server.RegisterValidator(new ValidateDrawingType(base.IntegType));
            server.RegisterValidator(new ValidateData(base.IndexFields));
            server.RegisterValidator(new ValidateNullIndex(base.IndexFields, base.IntegType));
            server.RegisterValidator(new ValidateRequired(base.IndexFields));
            server.RegisterValidator(new ValidateCheckinDocPropertyLength(base.IndexFields));
            server.RegisterValidator(new ValidateIndexExists(base.IndexFields, base.IntegType));
            server.RegisterValidator(new ValidateImportCreoExists(base.IndexFields));
            server.RegisterValidator(new ValidateDocumentCopy(base.IsCreateCopy, CreateCopyType.Import));
            //base.ImportValidator(server);
            //验证删除
            server.RegisterValidator(new ValidateDeleteDoc(this.dicDeleteDoc, (DocumentVersion docver, DocStruct doc) => BOMHelp.IsEquals(BOMHelp.GetFileNameNoVersion(docver.FileName), BOMHelp.GetFileNameNoVersion(doc.FileName))));
            //验证删除参数
            server.RegisterValidator(new ValidateCreoConfig(base.IndexFields));
            //验证物料是否已经被使用
            server.RegisterValidator(new ValidateCanUseMaterial());

            //var empty = new ValidateEmpty();
            //server.Replace(typeof(ValidateUpdateAddin), empty);
            //server.Replace(typeof(ValidateStandardParts), empty);
            //server.Replace(typeof(ValidateStandardPartsState), empty);
            //server.Replace(typeof(ValidateStandardPartsMaterialRepeat), empty);
        }

        /// <summary>
        /// 检入时的验证
        /// </summary>
        /// <param name="server"></param>
        protected override void CheckInValidator(ValidateServer server)
        {
            server.Context.ExtendProperty[_.MultiConfiguration] = SupportMultiConfiguration;

            server.RegisterValidator(new ValidateRootExists(base.IndexFields));
            server.RegisterValidator(new ValidateDocName(true));
            server.RegisterValidator(new ValidateDrawingType(base.IntegType));
            server.RegisterValidator(new ValidateData(base.IndexFields));
            server.RegisterValidator(new ValidateNullIndex(base.IndexFields, base.IntegType));
            server.RegisterValidator(new ValidateIndexChange(base.IndexFields));
            server.RegisterValidator(new ValidateRequired(base.IndexFields));
            server.RegisterValidator(new ValidateCheckinDocPropertyLength(base.IndexFields));
            server.RegisterValidator(new ValidateIndexExists(base.IndexFields, base.IntegType));
            server.RegisterValidator(new ValidateDocumentIsCheckOut(base.UserId, false));
            server.RegisterValidator(new ValidateRecodeCheckOutState(base.UserId));
            server.RegisterValidator(new ValidateCheckinDoc3DExists(base.IndexFields));
            server.RegisterValidator(new ValidateDocumentCopy(base.IsCreateCopy, CreateCopyType.Import));
            server.RegisterValidator(new ValidatePDFOriginId());
            server.RegisterValidator(new ValidateDeleteDoc(this.dicDeleteDoc, (DocumentVersion docver, DocStruct doc) => BOMHelp.IsEquals(BOMHelp.GetFileNameNoVersion(docver.FileName), BOMHelp.GetFileNameNoVersion(doc.FileName))));
            server.RegisterValidator(new ValidateEffectedObject(base.IndexFields, base.IntegType, base.CategoryId, this.listEffectedObject));
            server.RegisterValidator(new ValidateIsNewMaterial(base.IndexFields));
            server.RegisterValidator(new ValidateDocumentMaterialState(this.dicDeleteDoc));
            server.RegisterValidator(new ValidateCheckMaterialProperty(base.IndexFields));
            server.RegisterValidator(new ValidateCheckNeedAddReation());
            
            //base.CheckInValidator(server);            
            Type typeFromHandle = typeof(ValidateIsNewMaterial);
            //验证删除参数
            server.InsertBefore(typeFromHandle, new ValidateCreoConfig(base.IndexFields));
            //是否有删除配置
            //server.InsertBefore(typeFromHandle, new ValidateDocConfigDelete());
            //验证物料是否已经被使用
            server.RegisterValidator(new ValidateCanUseMaterial());
            //var empty = new ValidateEmpty();
            //server.Replace(typeof(ValidateUpdateAddin), empty);
            //server.Replace(typeof(ValidateStandardParts), empty);
            //server.Replace(typeof(ValidateStandardPartsState), empty);
            //server.Replace(typeof(ValidateStandardPartsMaterialRepeat), empty);
            // 验证删除的关系
            server.RegisterValidator(new ValidateZuInstanceDelete(dicDeleteSTDRelation));
        }
    }
}
