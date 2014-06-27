using System;
using System.Linq;
using System.Collections.Generic;

using Proway.Framework;
using Proway.PLM.Document;
using Proway.PLM.Material;

namespace Creo.Server
{
    /// <summary>
    /// 空的验证器，用于删除验证器
    /// </summary>
    public class ValidateEmpty : DefaultValidator
    {
        public override bool Validate(ValidateContext context)
        {
            return true;
        }
    }

    public class ValidateCreoConfig : DefaultValidator
    {
        private List<Associatefield> IndexFields;
        private DocConfigManager docconfigManager = new DocConfigManager();
        private MaterialBaseManager matbaseManager = new MaterialBaseManager();
        public ValidateCreoConfig(List<Associatefield> af)
        {
            this.IndexFields = af;
        }
        public override bool Validate(ValidateContext context)
        {
            DocStruct doc = context.ValidateObject as DocStruct;
            if (doc != null && doc.IsConfigPart)
            {
                string text = "借用";
                if (BOMHelp.Contains(doc.OperateType, EntityOperateType.NotCheckOut))
                {
                    text = "未检出";
                }
                DocumentVersion documentVersion = BOMHelp.GetOValueFromDictionary(context.CurScopeProperty, _.CURVALIDATEDOCUMENT) as DocumentVersion;
                DocumentCopy documentCopy = BOMHelp.GetOValueFromDictionary(context.CurScopeProperty, _.CURVALIDATEDOCCOPY) as DocumentCopy;
                if (documentVersion != null) //文档已经存在
                {
                    List<DocStruct> list = doc.DataView.DocList.ToList<DocStruct>();
                    IList<DocConfig> list2 = BOMHelp.GetOValueFromDictionary(context.ExtendProperty, documentVersion.VerId) as IList<DocConfig>;
                    if (list2 == null)
                    {
                        list2 = this.docconfigManager.GetStandardPartsConfigByDVerId(documentVersion.VerId);
                        BOMHelp.Write(context.ExtendProperty, documentVersion.VerId, list2);
                    }
                    if (BOMHelp.Contains(doc.OperateType, EntityOperateType.NotCheckOut))
                    {
                        DocConfig docConfig = list2.FirstOrDefault((DocConfig p) => p.ConfigName == doc.ConfigName);
                        if (docConfig == null) //查找索引相同的配置
                        {
                            if (list2.All((DocConfig p) => string.IsNullOrEmpty(p.ConfigName)))
                            {
                                docConfig = list2.FirstOrDefault((DocConfig p) => BOMHelp.IndexIsEqualsG<DocConfig>(p, doc, this.IndexFields));
                            }
                        }
                        if (docConfig == null) //未找到的配置
                        {
                            if (list2.Count < list.Count)
                            {
                                doc.SetDocStateL(false, "red", "{0}：文档配置不允许新增", text);
                            }
                            else
                            {
                                doc.SetDocStateL(false, "red", "{0}：文档配置名称被修改", text);
                            }
                            return false;
                        }
                        else
                        {
                            ValidatorHelp.ValiedateDocChanage<DocConfig>(doc, doc, docConfig, this.IndexFields, documentCopy.FileMD5, text, false);
                            if (string.IsNullOrEmpty(docConfig.ConfigName))
                            {
                                doc.CFG_OperateType = EntityOperateType.UpdateVer;
                            }
                        }
                    }
                    else
                    {
                        DocConfig docConfig2 = list2.FirstOrDefault((DocConfig p) => p.ConfigName == doc.ConfigName);
                        if (docConfig2 == null)
                        {
                            if (list2.All((DocConfig p) => string.IsNullOrEmpty(p.ConfigName)))
                            {
                                docConfig2 = list2.FirstOrDefault((DocConfig p) => BOMHelp.IndexIsEqualsG<DocConfig>(p, doc, this.IndexFields));
                            }
                        }
                        if (docConfig2 != null)
                        {
                            DocStruct currentData = doc.DataView.GetCurrentData();
                            if (BOMHelp.PropertyIsEqualsG<DocConfig>(docConfig2, doc, this.IndexFields, false))
                            {
                                doc.CFG_OperateType = (string.IsNullOrEmpty(docConfig2.ConfigName) ? EntityOperateType.UpdateVer : EntityOperateType.JustCheckIn);
                                doc.OperateType = EntityOperateType.JustCheckIn;
                                if (BOMHelp.Contains(currentData.OperateType, EntityOperateType.None))
                                {
                                    currentData.OperateType = EntityOperateType.JustCheckIn;
                                }
                            }
                            else
                            {
                                if (!BOMHelp.IndexIsEqualsG<DocConfig>(docConfig2, doc, this.IndexFields))
                                {
                                    doc.SetDocStateL(false, "red", "文档的索引已经修改");
                                    return false;
                                }
                                doc.OperateType = EntityOperateType.UpdateVer;
                                doc.CFG_OperateType = EntityOperateType.UpdateVer;
                                if (!BOMHelp.Contains(currentData.OperateType, EntityOperateType.UpdateVer))
                                {
                                    currentData.OperateType = EntityOperateType.UpdateVer;
                                }
                            }
                        }
                        else
                        {
                            doc.CFG_OperateType = EntityOperateType.CreateNewVer;
                            ValidatorHelp.SetSelfRelationType(doc, RelationOperateType.AddRelation);
                        }
                    }
                }
                else
                {
                    doc.CFG_OperateType = doc.OperateType;
                }
            }
            return true;
        }
    }
}
