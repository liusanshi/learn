<%@ Control Language="C#" ClassName="CommentList" Inherits="ViewUserControlBase<List<Comment>>" %>
<%--<%@ OutputCache Duration="6000" VaryByParam="*" %>--%>

<p><b>评论列表</b></p>
<% foreach( var comment in Model ) { %>
<p>
	<b><%= comment.Title %></b><br />
	<%= comment.Text %>
</p>
<hr />
<% } %>
