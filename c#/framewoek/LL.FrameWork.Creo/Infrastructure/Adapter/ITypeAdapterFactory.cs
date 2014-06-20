using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Core.Infrastructure.Crosscutting.Adapter
{
    /// <summary>
    /// Base contract for adapter factory
    /// </summary>
    public interface  ITypeAdapterFactory
    {
        /// <summary>
        /// Create a type adater
        /// </summary>
        /// <returns>The created ITypeAdapter</returns>
        ITypeAdapter Create();
    }
}
