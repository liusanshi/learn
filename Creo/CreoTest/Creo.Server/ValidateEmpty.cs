﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Proway.Framework;
using Proway.PLM.Document;
using Proway.PLM.Material;
using Proway.PLM.Settings;

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
        /// <summary>
        /// 标记是否族表的Key
        /// </summary>
        private const string ZUBIAOKey = "_zubiao";

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
                    if (doc.IsBorrow || BOMHelp.Contains(doc.OperateType, EntityOperateType.NotCheckOut))
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
                            if (IsZuBiao(doc))
                            {
                                doc.CFG_OperateType = EntityOperateType.CreateNewVer;
                                ValidatorHelp.SetSelfRelationType(doc, RelationOperateType.AddRelation);
                            }
                            else
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

        /// <summary>
        /// 是否族表零件
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        internal static bool IsZuBiao(DocStruct doc)
        {
            return doc.GetString(ZUBIAOKey) == "1";
        }
    }

    /// <summary>
    /// Creo 导入时的验证
    /// 主要原因是：新增族实例的时候可以导入
    /// </summary>
    public class ValidateImportCreoExists : DefaultValidator
    {
        private ObjectRelationDataAccess objrelatda = new ObjectRelationDataAccess();
		private List<Associatefield> IndexFields;
		private IntegrationFileImportManager FileImportManager = new IntegrationFileImportManager();
		private DocumentVersionManager docversionManager = new DocumentVersionManager();
		private DocumentCopyManager doccopyManager = new DocumentCopyManager();
		private RelationObjectManager ormanager = new RelationObjectManager();
		private DocConfigManager STDManager = new DocConfigManager();
        /// <summary>
        /// Creo 导入时的验证
        /// 主要原因是：新增族实例的时候可以导入
        /// </summary>
        /// <param name="af"></param>
        public ValidateImportCreoExists(List<Associatefield> af)
		{
			this.IndexFields = af;
		}
		public override bool Validate(ValidateContext context)
		{
			DocStruct docStruct = context.ValidateObject as DocStruct;
			if (docStruct != null && !BOMHelp.IsNullOrEmptyG<Associatefield>(this.IndexFields))
			{
				string valueFromDictionary = BOMHelp.GetValueFromDictionary(context.ExtendProperty, "__DocCategoryId");
				string valueFromDictionary2 = BOMHelp.GetValueFromDictionary(context.ExtendProperty, "__FileTypeId");
				DocumentVersion docversion = this.docversionManager.GetDocumentVersionByFileName(BOMHelp.GetFileNameNoVersion(docStruct.FileName));
				if (docversion == null)
				{
					if (docStruct.STD_IsStandardParts)
					{
						docStruct.OperateType = EntityOperateType.CreateNewVer;
						ValidatorHelp.SetSelfRelationType(docStruct, RelationOperateType.AddRelation);
					}
					else
					{
						if (docStruct.DrawingType != IntegrationDrawingType.Engineering)
						{
							if (this.FileImportManager.GetDocVerByIndex(this.IndexFields, docStruct, valueFromDictionary, valueFromDictionary2) != null)
							{
								docStruct.SetDocStateL(false, "red", "文档库中存在相同索引的文档");
								return false;
							}
							if (!this.ValidateGeneralWithoutDB(docStruct))
							{
								return false;
							}
						}
						else
						{
							if (!this.ValidateEngineeringWithoutDB(docStruct, docStruct.OperateDocStrcut))
							{
								return false;
							}
						}
					}
				}
				else
				{
					BOMHelp.Add(context.CurScopeProperty, "__CurValidateDocument", docversion);
					if (docversion.StateId == 3)
					{
						string @string = docStruct.GetString("phydocvercode");
						if (docversion.PhyDocVerCode == null)
						{
							docversion.PhyDocVerCode = string.Empty;
						}
						bool flag = this.ComparePhyDocVerCode(docversion.PhyDocVerCode, @string);
						if (flag)
						{
							docStruct.OperateType = EntityOperateType.CreateNewVer;
							ValidatorHelp.SetSelfRelationType(docStruct, RelationOperateType.AddRelation);
							return true;
						}
					}
					docStruct.SetVerId(docversion.VerId);
					docStruct.DocBaseId = docversion.DocId;
					docStruct.OperateType = EntityOperateType.None;
					docStruct.IsBorrow = true;
					DocumentCopy currentCopyByVerId = this.doccopyManager.GetCurrentCopyByVerId(docversion.VerId);
					if (currentCopyByVerId != null)
					{
						BOMHelp.Add(context.CurScopeProperty, "__CurValidateDocumentCopy", currentCopyByVerId);
						if (docStruct.STD_IsStandardParts || docStruct.IsConfigPart)
						{
							return ValidateConfigParts(docStruct, context, ValidatorHelp.CheckDocConfig(this.STDManager.GetStandardPartsConfigByDVerIdAndName(docStruct.RealityVerId, docStruct.ConfigName, true), docStruct, this.IndexFields), (ChildStruct p) => this.objrelatda.GetObjectRelations(p.ParentNode.RealityVerId, p.ChildNode.RealityVerId));
						}
						if (docStruct.DrawingType == IntegrationDrawingType.Engineering)
						{
							docStruct.SetDocStateL(false, "借用");
							if (!this.ValidateEngineering(docStruct))
							{
								return false;
							}
						}
						else
						{
							if (!this.ValidateGeneral(docStruct))
							{
								return false;
							}
							ValidatorHelp.ValiedateDocChanage<DocumentVersion>(docStruct, docStruct, docversion, this.IndexFields, currentCopyByVerId.FileMD5, "借用", false);
						}
					}
				}
			}
			return true;
		}
		private bool ValidateEngineering(DocStruct doc)
		{
            foreach (RelationObjectStruct current in doc.ScourceRelation.Where(p => p.ScourceNode != null))
            {
                DocStruct scourceNode = current.ScourceNode;
                IList docSpecialRelationObject = this.ormanager.GetDocSpecialRelationObject(current.ScourceNode.RealityVerId);
                if (!docSpecialRelationObject.Cast<RelationObject>().Any((RelationObject p) => p.DesObjectId == doc.RealityVerId))
                {
                    if (scourceNode.IsBorrow || BOMHelp.Contains(scourceNode.OperateType, EntityOperateType.NotCheckOut))
                    {
                        scourceNode.SetDocStateL(false, "blue", "借用：该图纸添加了新的工程图！");
                        doc.OperateType = EntityOperateType.None;
                    }
                    else
                    {
                        current.RelationOperateType = RelationOperateType.AddRelation;
                    }
                }
                break;
            }
			return true;
		}
		private bool ValidateEngineeringWithoutDB(DocStruct sourcedoc, DocStruct operatedoc)
		{
			if (operatedoc.IsBorrow)
			{
				sourcedoc.OperateType = EntityOperateType.None;
				sourcedoc.IsBorrow = true;
				sourcedoc.SetDocStateL(false, "不处理文档");
				operatedoc.SetDocStateL(false, "blue", "借用：该图纸添加了新的工程图！");
			}
			else
			{
				sourcedoc.OperateType = EntityOperateType.CreateNewVer;
				ValidatorHelp.SetEngineeringRelationType(sourcedoc, RelationOperateType.AddRelation);
			}
			return true;
		}
		private bool ValidateGeneralWithoutDB(DocStruct doc)
		{
			if (BOMHelp.IsNullOrEmptyG<ChildStruct>(doc.ParentRelation))
			{
				doc.OperateType = EntityOperateType.CreateNewVer;
			}
			else
			{
				IEnumerable<ChildStruct> source = 
					from p in doc.ParentRelation
					where p.ParentNode != null && !p.ParentNode.IsBorrow
					select p;
				if (!source.Any<ChildStruct>())
				{
					doc.OperateType = EntityOperateType.None;
					doc.IsBorrow = true;
					doc.SetDocStateL(false, "不处理文档");
					foreach (DocStruct current in 
						from p in doc.ParentRelation
						where p.ParentNode != null && p.ParentNode.IsBorrow
						select p.ParentNode)
					{
						current.SetDocStateL(false, "red", "{0}：不允许修改文档结构", new object[]
						{
							current.IsBorrow ? "借用" : "未检出"
						});
					}
					return false;
				}
				doc.OperateType = EntityOperateType.CreateNewVer;
				ValidatorHelp.SetSelfRelationType(doc, RelationOperateType.AddRelation);
			}
			return true;
		}
		private bool ValidateGeneral(DocStruct doc)
		{
			return this.ValidateGeneral(doc, (ChildStruct p) => this.objrelatda.GetObjectRelation(p.ParentNode.RealityVerId, p.ChildNode.RealityVerId));
		}
		private bool ValidateGeneral(DocStruct doc, Func<ChildStruct, ObjectRelation> getrelation)
		{
			foreach (ChildStruct current in 
				from p in doc.ParentRelation
				where p.ParentNode != null
				select p)
			{
				DocStruct parentNode = current.ParentNode;
				ObjectRelation objectRelation = this.objrelatda.GetObjectRelation(parentNode.RealityVerId, doc.RealityVerId);
				if (objectRelation == null)
				{
					if (!ValidatorHelp.CheckPCRelation(current))
					{
						return false;
					}
				}
				else
				{
					current.RelationId = objectRelation.RelationId;
					current.RelationOperateType = RelationOperateType.None;
				}
			}
			return true;
		}
		private bool ComparePhyDocVerCode(string VerCode1, string VerCode2)
		{
			int num;
			bool flag = int.TryParse(VerCode1, out num);
			int num2;
			bool flag2 = int.TryParse(VerCode2, out num2);
			if (!flag)
			{
				if (flag2)
				{
					return true;
				}
			}
			else
			{
				if (flag2)
				{
					return num2 > num;
				}
			}
			return false;
		}

        /// <summary>
        /// 验证族实例
        /// </summary>
        /// <param name="sourcedoc"></param>
        /// <param name="context"></param>
        /// <param name="std"></param>
        /// <param name="GetRelations"></param>
        /// <returns></returns>
        static bool ValidateConfigParts(DocStruct sourcedoc, ValidateContext context, DocConfig std, Func<ChildStruct, IList> GetRelations)
        {
            if (std != null)
            {
                sourcedoc.ConfigId = std.ConfigID;
                if (!string.IsNullOrEmpty(std.MatVerId)) 
                    sourcedoc.MaterialVerId = std.MatVerId;
                BOMHelp.Write(context.CurScopeProperty, "__STD_OLD_MaterialVerId", std.MatVerId);
                foreach (var current in sourcedoc.ParentRelation.Where(p => p.ParentNode != null))
                {
                    DocStruct parent = current.ParentNode;
                    IEnumerable<ObjectRelation> source = GetRelations(current).Cast<ObjectRelation>();
                    if (!source.Any<ObjectRelation>())
                    {
                        if (!ValidatorHelp.CheckPCRelation(current))
                        {
                            bool result = false;
                            return result;
                        }
                        sourcedoc.IsBorrow = true;
                        current.RelationOperateType = RelationOperateType.AddRelation;
                    }
                    else
                    {
                        if (parent.IsBorrow)
                        {
                            sourcedoc.IsBorrow = true;
                        }
                        else
                        {
                            ObjectRelation objectRelation = source.FirstOrDefault((ObjectRelation p) => (string.IsNullOrEmpty(p.ParentConfigID) || p.ParentConfigID == parent.ConfigId) && BOMHelp.IsEquals(p.ConfigID, std.ConfigID));
                            if (objectRelation != null)
                            {
                                current.RelationId = objectRelation.RelationId;
                                bool flag = current.Count == current.HiddenCount;
                                if (objectRelation.IsHidden != flag || !BOMHelp.Contains(parent.OperateType, EntityOperateType.NotCheckOut))
                                {
                                    current.RelationOperateType = RelationOperateType.ModifyRelation;
                                }
                            }
                            else
                            {
                                current.RelationOperateType = RelationOperateType.AddRelation;
                            }
                        }
                    }
                }
            }
            else if (!ValidateCreoConfig.IsZuBiao(sourcedoc)) //没有到如果该配置 同时不是族表实例
            {
                foreach (var item in sourcedoc.ParentRelation.Where(p => p.ParentNode != null))
                {
                    if (!ValidatorHelp.CheckPCRelation(item))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 验证族实例的删除
    /// </summary>
    public class ValidateZuInstanceDelete : DefaultValidator
    {
        private Dictionary<string, ArrayList> dicDeleteSTDRelation;
		private DocumentVersionManager dvManager = new DocumentVersionManager();
		private bool SupportMultiConfig;
		private int count = -1;
        public ValidateZuInstanceDelete(Dictionary<string, ArrayList> dic)
		{
			this.dicDeleteSTDRelation = dic;
		}
		public override bool Validate(ValidateContext context)
		{
			BOMStruct bOMStruct = context.ValidateList as BOMStruct;
			if (bOMStruct != null)
			{
                this.SupportMultiConfig = context.ExtendProperty.GetValue(_.MultiConfiguration, false);

                foreach (DocStruct doc in bOMStruct.BOMView.Cast<DocView>().Where(p => !((DocStruct)p).STD_IsStandardParts))
                {
                    IList dbRelation = this.dvManager.SearchObjectRelationByParent(doc.RealityVerId);
                    ArrayList arrayList = new ArrayList(5);
                    IEnumerable<ChildStruct> children =
                        from p in doc.DataView.ChildRelation
                        where p.ChildNode != null && p.ParentNode != null
                        select p;
                    if (!this.ValidateRelation(dbRelation, (ObjectRelation DBR) => this.ObjectRelationExists(DBR, doc, children), doc, arrayList))
                    {
                        return false;
                    }
                    if (!BOMHelp.IsNullOrEmpty(arrayList))
                    {
                        BOMHelp.Add<string, ArrayList>(this.dicDeleteSTDRelation, doc.RealityVerId, arrayList, (ArrayList il) => true);
                    }
                }
			}
			return true;
		}
		private bool ObjectRelationExists(ObjectRelation or, DocStruct parent, IEnumerable<ChildStruct> children)
		{
			if (this.SupportMultiConfig)
			{
                if (parent.Configs.Any(p => p.ConfigId == or.ParentConfigID))//父ID存在时才判断
                {
                    return children.Any((ChildStruct p) => BOMHelp.IsEquals(or.ConfigID, p.ChildNode.ConfigId)
                                       && BOMHelp.IsEquals(or.ParentConfigID, p.ParentNode.ConfigId));
                }
                return true;
			}
			return children.Any(delegate(ChildStruct p)
			{
				if (p.ChildNode.STD_IsStandardParts)
				{
					return BOMHelp.IsEquals(or.ConfigID, p.ChildNode.ConfigId);
				}
				return BOMHelp.IsEquals(or.ChildObject, p.ChildNode.RealityVerId);
			});
		}
		private bool ValidateRelation(IList dbRelation, Func<ObjectRelation, bool> exists, DocStruct doc, ArrayList list)
		{
			foreach (ObjectRelation objectRelation in dbRelation)
			{
				if (!string.IsNullOrEmpty(objectRelation.ConfigID) && !exists(objectRelation))
				{
					if (doc.IsBorrow)
					{
						doc.SetDocStateL(false, "red", "借用：不允许修改文档结构");
						bool result = false;
						return result;
					}
					if (BOMHelp.Contains(doc.OperateType, EntityOperateType.NotCheckOut))
					{
						doc.SetDocStateL(false, "red", "未检出：不允许修改文档结构");
						bool result = false;
						return result;
					}
					list.Add(objectRelation.RelationId);
				}
			}
			return true;
		}
		public override bool OnPreValidate(object sender, int index)
		{
			if (this.count == -1)
			{
				DocStruct docStruct = sender as DocStruct;
				this.count = docStruct.Owner.Count;
			}
			return this.count == index;
		}
    }

    /// <summary>
    /// 验证程序是否过去
    /// </summary>
    public class ValidateTime : DefaultValidator
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        private readonly DateTime GUOQI = new DateTime(2014, 10, 01);

        public override bool Validate(ValidateContext context)
        {
            var doc = context.ValidateObject as DocStruct;
            if (doc != null)
            {
                if (DateTime.Now >= GUOQI)
                {
                    doc.SetDocStateL(false, "red", "程序已经过期，请联系实施人员！");
                    return false;
                }
            }
            return true;
        }
    }
}
