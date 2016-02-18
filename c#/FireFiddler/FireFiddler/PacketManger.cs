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
        private bool mDisabled = true;
        private IHttpHeaderPersistence mPHPersistence = null;
        private IHttpHeaderProcess mHttpHeaderProcess = null;

        private static PacketManger packetManger;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Disabled
        {
            get { return mDisabled; }
            set
            {
                mDisabled = value;
                RuleList.Disabled = value;
            }
        }

        private PacketManger()
        {
            RuleList = RuleList.LoadFile();
            mDisabled = RuleList.Disabled;
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
        /// http保存的接口
        /// </summary>
        public IHttpHeaderPersistence PHPersistence
        {
            set { mPHPersistence = value; }
            private get { return mPHPersistence; }
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="deal"></param>
        public IHttpHeaderProcess HttpHeaderProcess
        {
            set { mHttpHeaderProcess = value; }
            private get { return mHttpHeaderProcess; }
        }

        /// <summary>
        /// 过滤session
        /// </summary>
        /// <param name="session"></param>
        public void FilterSession(Session session)
        {
            if (Disabled && RuleList.Match(session))
            {
                if (PHPersistence != null) //保存请求头
                {
                    Packet p = new Packet(session.url, session.ResponseHeaders);
                    if (p.HasData())
                    {
                        PHPersistence.Save(p);
                    }
                }

                if (HttpHeaderProcess != null)
                {
                    HttpHeaderProcess.Do(session);
                }
            }
        }

        /// <summary>
        /// 获取session对应的包
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public Packet GetPacket(Session session)
        {
            Packet packet = null;
            if (PHPersistence != null)
            {
                packet = PHPersistence.Load(session.url);
            }
            if (packet == null)
            {
                packet = new Packet(session.url, session.ResponseHeaders);
            }
            return packet;
        }

        /// <summary>
        /// 添加rule
        /// </summary>
        /// <param name="rule"></param>
        public void AddRule(IRule rule)
        {
            RuleList.Add(rule);
        }

        /// <summary>
        /// 获取所有的条件
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IRule> GetRules()
        {
            return RuleList;
        }

        /// <summary>
        /// 保存rule
        /// </summary>
        public void SaveRule()
        {
            RuleList.Save();
        }
    }
}
