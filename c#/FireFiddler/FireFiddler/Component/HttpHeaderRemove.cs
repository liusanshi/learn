using System;
using System.Linq;

namespace FireFiddler.Component
{
    /// <summary>
    /// 移除http头
    /// </summary>
    public class HttpHeaderRemove : IHttpHeaderProcess
    {
        public void Do(Fiddler.Session oSession)
        {
            var header = Packet.GetFirePhpHeader(oSession.ResponseHeaders).ToArray();
            if (header.Any()) //存在需要处理的头
            {
                foreach (var item in header) //移除
                {
                    oSession.ResponseHeaders.Remove(item);
                }
            }
        }
    }
}
