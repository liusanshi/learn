<%@ Page Language="C#" %>
<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        this.Response.Redirect(string.Format("{0}?redirect_uri={1}&client_id={2}&response_type=code&state=1&view=web",
            XDHelper.Common.AppConfig.authorize_url, XDHelper.Common.AppConfig.redirect_uri, XDHelper.Common.AppConfig.Appkey));
    }
</script>
