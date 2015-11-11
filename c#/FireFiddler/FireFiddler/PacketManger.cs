using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Fiddler;

namespace FireFiddler
{
    /// <summary>
    /// 数据包管理
    /// 1. 拦截包
    /// 2. 管理包
    ///      a. 保存
    ///      `b. 过期
    /// 3. 加载
    /// </summary>
    public class PacketManger
    {
        /// <summary>
        /// 规则列表
        /// </summary>
        private RuleList RuleList;
        /// <summary>
        /// 包列表
        /// </summary>
        private List<Packet> Packetes;

        private static PacketManger packetManger;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Disabled { get; set; }

        private PacketManger()
        {
            RuleList = RuleList.LoadFile();
            Packetes = new List<Packet>();
        }

        static PacketManger()
        {
            packetManger = new PacketManger();
        }

        /// <summary>
        /// 数据包管理对象
        /// </summary>
        public static PacketManger PManger
        {
            get
            {
                return packetManger;
            }
        }

        /// <summary>
        /// 过滤session
        /// </summary>
        /// <param name="session"></param>
        public void FilterSession(Session session)
        {
            if (Disabled && RuleList.Match(session))
            {
                Packet p = new Packet(session.url, session.ResponseHeaders);
                var header = p.GetFilterHeader();
                if (header.Any()) //存在需要处理的头
                {
                    foreach (var item in header) //移除
                    {
                        session.ResponseHeaders.Remove(item);
                    }
                    //Packetes.Add(p); //先不存内存
                    p.Save(); //保存
                }
            }
        }

        /// <summary>
        /// 清空所有包
        /// </summary>
        public void ClearPacket()
        {
            Packetes.Clear();
            try
            {
                foreach (var item in Directory.GetFiles(Util.LogPath, "*.txt"))
                {
                    File.Delete(item);
                }
            }
            catch
            {
            }            
        }

        /// <summary>
        /// 加载包
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Packet LoadPacket(string url)
        {
            return Packet.LoadFile(url);
        }
    }
}
