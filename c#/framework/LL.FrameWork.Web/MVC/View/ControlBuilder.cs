using System;
using System.CodeDom;
using System.Web.UI;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 控件生成接口
    /// </summary>
    internal interface IMvcControlBuilder
    {
        /// <summary>
        /// 基类类型
        /// </summary>
        string Inherits
        {
            set;
        }
    }

    internal sealed class ViewUserControlControlBuilder : FileLevelUserControlBuilder, IMvcControlBuilder
    {
        public string Inherits
        {
            get;
            set;
        }

        public override void ProcessGeneratedCode(
            CodeCompileUnit codeCompileUnit,
            CodeTypeDeclaration baseType,
            CodeTypeDeclaration derivedType,
            CodeMemberMethod buildMethod,
            CodeMemberMethod dataBindingMethod)
        {
            // 如果分析器找到一个有效的类型，就使用它。
            if (!string.IsNullOrEmpty(Inherits))
            {
                derivedType.BaseTypes[0] = new CodeTypeReference(Inherits);
            }
        }
    }

    internal sealed class ViewPageControlBuilder : FileLevelPageControlBuilder, IMvcControlBuilder
    {
        public string Inherits
        {
            get;
            set;
        }

        public override void ProcessGeneratedCode(
            CodeCompileUnit codeCompileUnit,
            CodeTypeDeclaration baseType,
            CodeTypeDeclaration derivedType,
            CodeMemberMethod buildMethod,
            CodeMemberMethod dataBindingMethod)
        {
            // 如果分析器找到一个有效的类型，就使用它。
            if (!string.IsNullOrEmpty(Inherits))
            {
                derivedType.BaseTypes[0] = new CodeTypeReference(Inherits);
            }
        }
    }

    internal sealed class ViewMasterPageControlBuilder : FileLevelMasterPageControlBuilder, IMvcControlBuilder
    {
        public string Inherits
        {
            get;
            set;
        }
        public override void ProcessGeneratedCode(CodeCompileUnit codeCompileUnit, CodeTypeDeclaration baseType, CodeTypeDeclaration derivedType, CodeMemberMethod buildMethod, CodeMemberMethod dataBindingMethod)
        {
            if (!string.IsNullOrEmpty(this.Inherits))
            {
                derivedType.BaseTypes[0] = new CodeTypeReference(this.Inherits);
            }
        }
    }
}
