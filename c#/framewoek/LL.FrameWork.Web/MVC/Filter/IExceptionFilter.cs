namespace LL.FrameWork.Web.MVC
{
    public interface IExceptionFilter
    {
        void OnException(ExceptionContext filterContext);
    }
}
