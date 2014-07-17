namespace LL.Framework.Impl.Infrastructure.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LL.Framework.Core.Infrastructure.Validator;

    /// <summary>
    /// Data Annotations based entity validator factory
    /// </summary>
    public class DataAnnotationsEntityValidatorFactory
        : IEntityValidatorFactory
    {
        /// <summary>
        /// Create a entity validator
        /// </summary>
        /// <returns></returns>
        public IEntityValidator Create()
        {
            return new DataAnnotationsEntityValidator();
        }
    }
}
