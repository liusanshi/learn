using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.FrameWork.Core.Infrastructure.IOC
{
    /// <summary>
    /// 解耦对象的获取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResolver<T>
    {
        T Current
        {
            get;
        }
    }
}
