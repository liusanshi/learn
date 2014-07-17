namespace LL.Framework.Impl.Infrastructure.Adapter.EmitMapperImpl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EmitMapper;

    using LL.Framework.Core.Infrastructure.Adapter;
    
    public class EmitMapperTypeAdapterFactory : ITypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        public EmitMapperTypeAdapterFactory() { EmitMapHelper.Initialize(); }
        #endregion

        #region ITypeAdapterFactory Members

        public ITypeAdapter Create()
        {
            return new EmitMapperTypeAdapter();
        }
        #endregion
    }
}
