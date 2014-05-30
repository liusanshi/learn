<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RedGlovePermission.Web.Admin.Frameset.Default" %>

<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=ResourceManager.GetString("sitename") %></title>
    <link href="../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexFramework.css"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="topdiv">
        <div class="header">
            <div class="sitename">
                <%=ResourceManager.GetString("sitename") %>-<%=ResourceManager.GetString("ver")%></div>
            <div class="mainbutton">
                <div class="ico">
                    <img alt="" src="../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/ico/home.gif" /></div>
                <div class="text">
                    <a href="/" class="mainlink"><%=ResourceManager.GetString("Default_Link_1") %></a></div>
                <div class="ico">
                    <img alt="" src="../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/ico/key.png" /></div>
                <div class="text">
                    <a href="../ChangePassword.aspx" class="mainlink" target="modulePanel"><%=ResourceManager.GetString("Default_Link_2") %></a></div>
                <div class="ico">
                    <img alt="" src="../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/ico/user.png" /></div>
                <div class="text">
                    <asp:LinkButton ID="lnkbtnLogout" CssClass="mainlink" runat="server" OnClick="lnkbtnLogout_Click"><%=ResourceManager.GetString("Default_Link_3") %></asp:LinkButton></div>
                <div class="toplist">
                    <asp:DropDownList ID="StyleList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="StyleList_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="x-toolbar">
            <div class="welcome">
                <%=ResourceManager.GetString("Default_Lab_2") %>,<asp:Label ID="loginname" runat="server" Text=""></asp:Label></div>
            <div style="float: left; display: none">
            </div>
        </div>
    </div>
    <div id="leftdiv" style="float: left;">
        <div class="Splitterheader">
            <%=ResourceManager.GetString("Default_Lab_1") %></div>
        <div class="SplitterPane">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:TreeView ID="MenuTreeView" runat="server" ImageSet="Msdn" NodeIndent="10">
                        <ParentNodeStyle Font-Bold="False" />
                        <HoverNodeStyle BorderColor="#ff0000" />
                        <SelectedNodeStyle BorderColor="#888888" BorderStyle="Solid" BorderWidth="1px" Font-Underline="False"
                            HorizontalPadding="3px" VerticalPadding="1px" />
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                            NodeSpacing="1px" VerticalPadding="2px" />
                    </asp:TreeView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="splitdiv" style="float: left;" onclick="on_tool();">
        <p>
            <img id="frameshow" src="../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/ui/splitter_horCol.gif"></p>
    </div>
    <div id="rightdiv" style="float: left;">
        <iframe id="modulePanel" name="modulePanel" frameborder="0" scrolling="auto" src="Welcome.aspx?icke=<%=Request.Cookies["UIStyle"].Value%>"
            height="100%" width="100%"></iframe>
    </div>
    </form>
</body>
</html>

<script type="text/javascript">

    //2. 只打开一个节点,关闭其他兄弟节点(Just one expanded node, all other will collaps)
    function TreeView_ToggleNode(data, index, node, lineType, children) {
        var img = node.childNodes[0];
        var newExpandState;
        try {
            //***折叠兄弟节点(Collapse Brothers)-----
            CollapseBrothers(data, children);
            //---------------------------------------

            if (children.style.display == "none") {
                children.style.display = "block";
                newExpandState = "e";
                if ((typeof (img) != "undefined") && (img != null)) {
                    if (lineType == "l") {
                        img.src = data.images[15];
                    }
                    else if (lineType == "t") {
                        img.src = data.images[12];
                    }
                    else if (lineType == "-") {
                        img.src = data.images[18];
                    }
                    else {
                        img.src = data.images[5];
                    }
                    img.alt = data.collapseToolTip.replace(/\{0\}/, TreeView_GetNodeText(node));
                }
            }
            else {
                children.style.display = "none";
                newExpandState = "c";
                if ((typeof (img) != "undefined") && (img != null)) {
                    if (lineType == "l") {
                        img.src = data.images[14];
                    }
                    else if (lineType == "t") {
                        img.src = data.images[11];
                    }
                    else if (lineType == "-") {
                        img.src = data.images[17];
                    }
                    else {
                        img.src = data.images[4];
                    }
                    img.alt = data.expandToolTip.replace(/\{0\}/, TreeView_GetNodeText(node));
                }
            }
        }
        catch (e) { }
        data.expandState.value = data.expandState.value.substring(0, index) + newExpandState + data.expandState.value.slice(index + 1);
    }

    //折叠兄弟节点(Collapse Brothers)
    function CollapseBrothers(data, childContainer) {
        var parent = childContainer.parentNode;
        for (i = 0; i < parent.childNodes.length; i++) {
            if (parent.childNodes[i].tagName.toLowerCase() == "div") {
                if (parent.childNodes[i].id != childContainer.id) {
                    parent.childNodes[i].style.display = "none"
                }
            }
            else if (parent.childNodes[i].tagName.toLowerCase() == "table") {
                var treeLinks = parent.childNodes[i].getElementsByTagName("a");
                if (treeLinks.length > 2) {
                    var j = 0;
                    if (treeLinks[j].firstChild.tagName) {
                        if (treeLinks[j].firstChild.tagName.toLowerCase() == "img") {
                            var img = treeLinks[j].firstChild;
                            if (i == 0)
                                img.src = data.images[8];
                            else if (i == parent.childNodes.length - 2)
                                img.src = data.images[14];
                            else
                                img.src = data.images[11];
                        }
                    }
                }
            }
        }
    }
    //-----------------------------------------------------------------------------


    var navigationHight = 60; //顶部导航条高度
    var menuWidth = 180; //菜单宽度

    var marginValue, m2;

    if (window.XMLHttpRequest) {
        if (!window.ActiveXObject) {
            marginValue = 11; // Mozilla, Safari,...
            m2 = 7;
        } else {
            marginValue = 10; //IE7
            m2 = 5;
        }
    } else {
        marginValue = 10; //IE6
        m2 = 8;
    }

    var leftdiv = document.getElementById("leftdiv");
    var splitdiv = document.getElementById("splitdiv");
    var rightdiv = document.getElementById("rightdiv");

    var frameshow = document.getElementById("frameshow");

    var FrameResize = function() {
        leftdiv.style.height = document.body.clientHeight - navigationHight - marginValue;
        rightdiv.style.height = document.body.clientHeight - navigationHight - marginValue;
        splitdiv.style.height = document.body.clientHeight - navigationHight - marginValue;
        rightdiv.style.width = document.body.clientWidth - menuWidth - marginValue - m2;
    }

    function on_tool() {
        //判断菜单是否隐藏
        if (leftdiv.style.display == "none") {
            leftdiv.style.height = document.body.clientHeight - navigationHight - marginValue;
            rightdiv.style.height = document.body.clientHeight - navigationHight - marginValue;
            splitdiv.style.height = document.body.clientHeight - navigationHight - marginValue;
            rightdiv.style.width = document.body.clientWidth - menuWidth - marginValue - m2;
            leftdiv.style.display = "block";
            //splitdiv.style.marginLeft="0px";
            frameshow.src = "../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/ui/splitter_horCol.gif";
        }
        else {
            rightdiv.style.height = document.body.clientHeight - navigationHight - marginValue;
            splitdiv.style.height = document.body.clientHeight - navigationHight - marginValue;
            rightdiv.style.width = document.body.clientWidth - marginValue;
            leftdiv.style.display = "none";
            //splitdiv.style.marginLeft="0px";
            frameshow.src = "../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/ui/splitter_horExp.gif";
        }
    }

    //设定Cookie值
    function SetCookie(name, value) {
        var expdate = new Date();
        var argv = SetCookie.arguments;
        var argc = SetCookie.arguments.length;
        var expires = (argc > 2) ? argv[2] : null;
        var path = (argc > 3) ? argv[3] : null;
        var domain = (argc > 4) ? argv[4] : null;
        var secure = (argc > 5) ? argv[5] : false;
        if (expires != null) expdate.setTime(expdate.getTime() + (expires * 1000));
        document.cookie = name + "=" + escape(value) + ((expires == null) ? "" : ("; expires=" + expdate.toGMTString()))
    + ((path == null) ? "" : ("; path=" + path)) + ((domain == null) ? "" : ("; domain=" + domain))
    + ((secure == true) ? "; secure" : "");
    }

    //刷新页面     
    function settyle() {
        SetCookie("InterfaceStyle", document.getElementById("ddlInterfaceStyle").value)
        location.reload();
    }

    FrameResize();

    if (window.XMLHttpRequest) {
        if (!window.ActiveXObject) {
            window.onload = FrameResize; // Mozilla, Safari...          
        } else {
            window.onresize = FrameResize; //IE7
        }
    } else {
        window.onresize = FrameResize; //IE6
    }
</script>

