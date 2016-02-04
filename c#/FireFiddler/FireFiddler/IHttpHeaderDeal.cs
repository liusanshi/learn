using Fiddler;

namespace FireFiddler
{
    /// <summary>
    /// http处理的接口
    /// </summary>
    public interface IHttpHeaderProcess
    {
        void Do(Session oSession);
    }
}
