namespace LL.FrameWork.Web.MVC
{
    public interface IMvcFilter
    {
        bool AllowMultiple
        {
            get;
        }
        int Order
        {
            get;
        }
    }
}
