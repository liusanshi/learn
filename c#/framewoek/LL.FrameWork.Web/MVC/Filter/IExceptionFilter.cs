namespace LL.Framework.Web.MVC
{
    public interface IExceptionFilter
    {
        void OnException(ExceptionContext filterContext);
    }
}
