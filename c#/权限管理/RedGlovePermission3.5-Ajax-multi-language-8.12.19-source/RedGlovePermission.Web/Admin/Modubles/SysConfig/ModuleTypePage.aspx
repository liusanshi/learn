<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleTypePage.aspx.cs"
    Inherits="RedGlovePermission.Web.Admin.Modubles.SysConfig.ModuleTypePage" %>

<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=ResourceManager.GetString("ModuleTypePage_Title_name")%></title>
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexControl.css" rel="stylesheet" type="text/css" />
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/GridView.css" rel="stylesheet" type="text/css" />

    <script src="../../../Inc/Script/pub.js" type="text/javascript"></script>

    <script type="text/javascript">

        //新增数据合法性检测
        function CheckAdd() {
            var ret = true;

            if ($id("txt_name").value == "") {
                alert('<%=ResourceManager.GetString("Pub_Msg_V_1")%>');
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
            <%=ResourceManager.GetString("ModuleTypePage_Title_name")%></div>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <div id="title" class="childtoolbar">
                <div class="rowdiv" style="margin-left: 3px;">
                    <%=ResourceManager.GetString("ModuleTypePage_Lab_1")%>:</div>
                <div class="rowdiv">
                    <asp:TextBox ID="txt_Name" Width="120" MaxLength="30" CssClass="inputbox" runat="server"></asp:TextBox></div>
                <div class="rowdiv" style="margin-left: 3px;">
                    <%=ResourceManager.GetString("ModuleTypePage_Lab_3")%>:</div>
                <div class="rowdiv" style="margin-left: 3px;">
                    <asp:TextBox ID="txt_Description" CssClass="inputbox" runat="server"></asp:TextBox>
                </div>
                <div class="rowdiv" style="margin-left: 3px;">
                    <%=ResourceManager.GetString("ModuleTypePage_Lab_4")%>:</div>
                <div class="rowdiv" style="margin-left: 3px;">
                    <asp:TextBox ID="txt_order" CssClass="inputbox" Width="30px" runat="server" onkeypress="return event.keyCode>=48&&event.keyCode<=57"
                        onpaste="return !clipboardData.getData('text').match(/\D/)"></asp:TextBox>
                </div>
                <div class="rowdiv" style="margin-left: 5px;line-height: 30px;">
                    <asp:Button ID="btn_add" class="button" runat="server" Text=" 新 增 " OnClick="btn_add_Click" /></div>
            </div>
            <div class="gv">
                <asp:GridView ID="ModuleTypeList" runat="server" DataKeyNames="ModuleTypeID" CssClass="Grid"
                    AllowSorting="True" OnRowCommand="ModuleTypeList_RowCommand" OnRowDataBound="ModuleTypeList_RowDataBound"
                    AllowPaging="True" OnPageIndexChanging="ModuleTypeList_PageIndexChanging" PageSize="15"
                    AutoGenerateColumns="False" OnRowCancelingEdit="ModuleTypeList_RowCancelingEdit"
                    OnRowEditing="ModuleTypeList_RowEditing" 
                    OnRowUpdating="ModuleTypeList_RowUpdating" 
                    onrowcreated="ModuleTypeList_RowCreated">
                    <FooterStyle CssClass="GridFooter" />
                    <RowStyle CssClass="Row" />
                    <Columns>
                        <asp:BoundField DataField="ModuleTypeID" HeaderText="编号" ReadOnly="True">
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="名称">
                            <ItemTemplate>
                                <asp:HyperLink ID="link_name" runat="server" NavigateUrl='<%# Eval("ModuleTypeID", "ModulesPage.aspx?pid={0}") %>'
                                    Text='<%# ResourceManager.GetString(Eval("ModuleTypeName").ToString()) %>'></asp:HyperLink>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_name" Width="120" MaxLength="30" CssClass="inputbox" runat="server"
                                    Text='<%# Eval("ModuleTypeName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="说明">
                            <ItemTemplate>
                                <asp:Label ID="Lab_Description" runat="server" Text='<%# Eval("ModuleTypeDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_Description" Width="120" MaxLength="30" CssClass="inputbox"
                                    runat="server" Text='<%# Eval("ModuleTypeDescription") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="排序">
                            <ItemTemplate>
                                <asp:Label ID="lab_order" runat="server" Text='<%# Eval("ModuleTypeOrder") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_order" Width="30" MaxLength="3" CssClass="inputbox" onkeypress="return event.keyCode>=48&&event.keyCode<=57"
                                    onpaste="return !clipboardData.getData('text').match(/\D/)" runat="server" Text='<%# Eval("ModuleTypeOrder") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="编辑" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btn_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_edit")%>' CommandArgument='<%# Eval("ModuleTypeID")%>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btn_update" runat="server" CausesValidation="True" CommandName="Update"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_update")%>'></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="btn_cancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_cancel")%>'></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="删除" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btn_del" runat="server" CausesValidation="False" CommandName="Del"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_delete")%>' CommandArgument='<%# Eval("ModuleTypeID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="HeadingCell" />
                    <AlternatingRowStyle BorderStyle="None" CssClass="AlternatingRow" />
                </asp:GridView>
                <div id="GridViewMsg" style="padding: 5px;" runat="server">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
