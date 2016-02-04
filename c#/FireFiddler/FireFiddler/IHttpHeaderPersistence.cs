using System;

namespace FireFiddler
{
    /// <summary>
    /// http头数据持久化的接口
    /// </summary>
    public interface IHttpHeaderPersistence
    {
        /// <summary>
        /// 保存数据
        /// </summary>
        void Save(Packet packet);

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        Packet Load(string url);

        /// <summary>
        /// 清除数据
        /// </summary>
        void Clear();
    }
}
