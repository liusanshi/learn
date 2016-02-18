using System;
using System.Collections.Generic;
using System.Linq;

namespace FireFiddler.Component
{
    /// <summary>
    /// 将数据保存至内存
    /// </summary>
    public class HttpHeaderToMemory : IHttpHeaderPersistence
    {
        /// <summary>
        /// 包列表
        /// </summary>
        private FixedLengthDictionary<string, Packet> Packetes;
        
        public HttpHeaderToMemory()
        {
            Packetes = new FixedLengthDictionary<string, Packet>(50);
        }

        public void Save(Packet packet)
        {
            Packetes[packet.Identity] = packet;
            //Packetes.Add(packet.Identity, packet);
        }

        public Packet Load(string url)
        {
            string identity = Packet.CreateIdentity(url);
            Packet packet;
            Packetes.TryGetValue(identity, out packet);
            return packet;
        }

        public void Clear()
        {
            Packetes.Clear();
        }
    }
}
