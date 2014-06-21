using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Core.Infrastructure.IOC
{
    public interface IContainerFactory
    {
        /// <summary>
        /// Create a type Container
        /// </summary>
        /// <returns>The created IContainer</returns>
        IContainer Create();
    }
}
