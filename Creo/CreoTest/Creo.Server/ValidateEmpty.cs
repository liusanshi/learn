using System;

using Proway.Framework;

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
}
