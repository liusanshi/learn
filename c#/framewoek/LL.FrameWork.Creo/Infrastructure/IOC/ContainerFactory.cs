using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Core.Infrastructure.IOC
{
    /// <summary>
    /// Container Factory
    /// </summary>
    public static class ContainerFactory
    {
        #region Members

        static IContainerFactory _currentContainerFactory = null;
        static IContainer _currentContainer = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the container factory to use
        /// </summary>
        /// <param name="logFactory">container factory to use</param>
        public static void SetCurrent(IContainerFactory logFactory)
        {
            _currentContainerFactory = logFactory;
        }

        /// <summary>
        /// Createt a new <paramref name="LL.FrameWork.Core.Infrastructure.IOC.IContainer"/>
        /// </summary>
        /// <returns>Created IContainer</returns>
        public static IContainer CreateContainer()
        {
            if (_currentContainer == null)
            {
                lock (_currentContainerFactory)
                {
                    if (_currentContainer == null)
                    {
                        _currentContainer = (_currentContainerFactory != null) ? _currentContainerFactory.Create() : null;
                    }
                }
            }
            return _currentContainer;
        }

        #endregion
    }
}
