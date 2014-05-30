<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModulesPage.aspx.cs" Inherits="RedGlovePermission.Web.Admin.Modubles.SysConfig.ModulesPage" %>

<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ResourceManager.GetString("ModulesPage_Title_name")%></title>
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexControl.css"
        rel="stylesheet" type="text/css" />
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/GridView.css"
        rel="stylesheet" type="text/css" />

    <script src="../../../Inc/Script/pub.js" type="text/javascript"></script>

    <script type="text/javascript">

        //新增数据合法性检测
        function CheckAdd() {
            var ret = true;

            if ($id("txt_name").value == "") {
                alert('<%=ResourceManager.GetString("Pub_Msg_V_1")%>');
                ret = false;
            }
            else if ($id("txt_Tag").value == "") {
                alert('<%=ResourceManager.GetString("Pub_Msg_V_2") %>');
                ret = false;
            }
            else if ($id("txt_order").value == "") {
                alert('<%=ResourceManager.GetString("Pub_Msg_V_3") %>');
                ret = false;
            }
            else if (!IsPlusInt($id("txt_order").value)) {
                alert('<%=ResourceManager.GetString("Pub_Msg_V_4") %>');
                ret = false;
            }
            return ret;
        }
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divToolBar" class="topBar">
        <div class="toolBar">
        </div>
        <div class="moduleName">
            <%=ResourceManager.GetString("ModulesPage_Title_name")%></div>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <div id="title" class="childtoolbar">
                <div class="rowdiv" style="margin-left: 3px;">
                    <%=ResourceManager.GetString("Pub_Lab_selecttype")%>:</div>
                <div class="rowdiv">
                    <asp:DropDownList ID="ModuleType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ModuleType_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="rowdiv" style="margin-left: 3px;">
                    <asp:Button ID="btn_add" class="button" runat="server" Text=" 新 增 " OnClick="btn_add_Click" /></div>
            </div>
            <div id="Glist" runat="server" style="margin: 10px; width: 370px; float: left; border: 0px solid #a9bfd3;">
                <asp:GridView ID="ModuleList" Width="370px" runat="server" DataKeyNames="ModuleID"
                    CssClass="Grid" AllowSorting="True" OnRowCommand="ModuleList_RowCommand" OnRowDataBound="ModuleList_RowDataBound"
                    AllowPaging="True" PageSize="15" AutoGenerateColumns="False" OnRowCreated="ModuleList_RowCreated">
                    <FooterStyle CssClass="GridFooter" />
                    <RowStyle CssClass="Row" />
                    <Columns>
                        <asp:BoundField DataField="ModuleID" HeaderText="编号" Visible="false">
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="名称">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtn_name" CommandArgument='<%# Eval("ModuleID")%>' CommandName="EditView"
                                    runat="server" Text='<%# ResourceManager.GetString(Eval("ModuleName").ToString()) %>'
                                    ToolTip='<%# ResourceManager.GetString(Eval("ModuleDescription").ToString()) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Lab_state" runat="server" Text='<%# (Convert.ToBoolean(Eval("ModuleDisabled"))==false)?ResourceManager.GetString("Pub_State_close"):ResourceManager.GetString("Pub_State_open") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="显示到菜单">
                            <ItemTemplate>
                                <asp:Label ID="Lab_menu" runat="server" Text='<%# (Convert.ToBoolean(Eval("IsMenu"))==false)?ResourceManager.GetString("Pub_State_invisible"):ResourceManager.GetString("Pub_State_visible") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="排序">
                            <ItemTemplate>
                                <asp:Label ID="lab_order" runat="server" Text='<%# Eval("ModuleOrder") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="编辑" ShowHeader="False">
                            <ItemTemplate>
                            <asp:LinkButton ID="lbtn_edit" CommandArgument='<%# Eval("ModuleID")%>' CommandName="EditView"
                                    runat="server" Text='<%# ResourceManager.GetString("Pub_Lbtn_edit") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="删除" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btn_del" runat="server" CausesValidation="False" CommandName="Del"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_delete")%>' CommandArgument='<%# Eval("ModuleID")%>'></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="HeadingCell" />
                    <AlternatingRowStyle BorderStyle="None" CssClass="AlternatingRow" />
                </asp:GridView>
                <div id="GridViewMsg" style="padding: 5px;" runat="server">
                </div>
            </div>
            <asp:Panel ID="AddPanel" Visible="false" runat="server" Style="margin: 10px 10px 10px 0px;
                float: left; border: 1px solid #d0d0d0; padding: 10px;">
                <table width="310" border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td style="text-align: right;">
                            <%=ResourceManager.GetString("Modules_Lab_1")%>：
                        </td>
                        <td width="260">
                            <asp:TextBox ID="txt_Name" Width="250" MaxLength="30" CssClass="inputbox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <%=ResourceManager.GetString("Modules_Lab_2")%>：
                        </td>
                        <td>
                            <asp:TextBox ID="txt_tag" Width="250" MaxLength="30" CssClass="inputbox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <%=ResourceManager.GetString("Modules_Lab_8")%>：
                        </td>
                        <td>
                            <asp:TextBox ID="txt_url" Width="250" CssClass="inputbox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <%=ResourceManager.GetString("Modules_Lab_6")%>：
                        </td>
                        <td>
                            <asp:RadioButtonList ID="txt_state" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="1">启用</asp:ListItem>
                                <asp:ListItem Value="0">关闭</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <%=ResourceManager.GetString("Modules_Lab_4")%>：
                        </td>
                        <td>
                            <asp:TextBox ID="txt_order" Width="250" MaxLength="4" CssClass="inputbox" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <%=ResourceManager.GetString("Modules_Lab_3")%>：
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Description" Width="250px" MaxLength="120" CssClass="inputbox"
                                runat="server" Height="40px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;white-space:nowrap;">
                            <%=ResourceManager.GetString("Modules_Lab_5")%>：
                        </td>
                        <td>
                            <asp:RadioButtonList ID="IsMenu" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="1">显示</asp:ListItem>
                                <asp:ListItem Value="0">不显示</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%=ResourceManager.GetString("Modules_Lab_7")%>
                            <asp:Label ID="AuthorityNum" runat="server" Text="0" Style="display: none"></asp:Label>
                            <asp:Label ID="M_ID" runat="server" Text="" Style="display: none"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="background-color: #fff; margin-top: 10px; margin-bottom: 10px; border: 1px solid #ccc;">
                                <div style="border-top: 1px solid #666; border-left: 1px solid #666; border-right: 1px solid #fff;
                                    border-bottom: 1px solid #fff;">
                                    <div style="border-top: 1px solid #666; border-left: 1px solid #666; border-right: 1px solid #c1c1c1;
                                        border-bottom: 1px solid #c1c1c1;">
                                        <table border="0" cellpadding="0" cellspacing="2">
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td width="100">
                                                    <div id="divstate" runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btn_save" class="button" runat="server" Text=" 保 存 " OnClick="btn_save_Click" />
                            <asp:Button ID="btn_update" class="button" runat="server" Text=" 更 新 " OnClick="btn_update_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btn_cancel" class="button" runat="server" Text=" 取 消 " OnClick="btn_cancel_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
