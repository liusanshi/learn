namespace LL.Framework.Web.MVC
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
