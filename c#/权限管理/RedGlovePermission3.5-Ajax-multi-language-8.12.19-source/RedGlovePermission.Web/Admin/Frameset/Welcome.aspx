<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="RedGlovePermission.Web.Admin.Frameset.Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>欢迎光临</title>
    <link href="../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexControl.css"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 700px; font-size: 14px; line-height: 22px; margin: 10px">
        <b>作 者 : Nick.Yan
            <br />
            QQ : 13458753
            <br />
            E-mail : <a href="mailto:nick.yanchen@gmail.com" target="_blank">nick.yanchen@gmail.com</a>
            <br />
            开源网址:<a href="http://www.codeplex.com/RedGlovePermission" target="_blank">http://www.codeplex.com/RedGlovePermission</a><br />
            技术服务:<a href="http://www.cnblogs.com/nick4" target="_blank">http://www.cnblogs.com/nick4</a><br />
            <br />
        </b><b>版本更新(Ver 8.12.20)</b><br />
        <li>改换成工厂模式</li>
        <li>支持MySQL数据</li>
        <li>多语言</li>
        <li>多皮肤</li>
        <li>单点登录</li>
        <li>将模块权限列表加宽分成两列,方便操作更多的权限 </li>
        <li>默认权限加到10种</li><br />
        <br /><b>系统操作流程</b><br />
        &nbsp; (1)建立权限列表，加添需要权限，初始权限(浏览/新增/编辑/删除/搜索/审核/移动/打印/下载/备份)<br />
        &nbsp; (2)建立用户组管理,为了方便将用户分类<br />
        &nbsp; (3)建立模块分类,将功能模块分类<br />
        &nbsp; (4)建立模块管理,添加子模块，详细功能权限设置<br />
        &nbsp; (5)建立角色管理<br />
        &nbsp; (6)建立角色授权，将模块权限权限给角色<br />
        <br />
        &nbsp; 测试用户<br />
        &nbsp; 用户名 密码<br />
        &nbsp; Admin admin<br />
        &nbsp; test1 test<br />
        &nbsp; test2 test<br />
        &nbsp; test3 test<br />
        &nbsp;<b>版权申明</b><br />
        &nbsp; 本系统完全开源，免费使用，如果你要使用，希望您能保留版权信息，本系统会不断完善更新有什么问题给发送邮件，如果您有好见意或意见，但说无访，希望这个系统真能为您帮上点忙，那就是我最开心的事了，也希望更多的朋友加入进来，先申明，没薪水的啊，呵呵，目的在于分享自己成功
    </div>
    </form>
</body>
</html>
