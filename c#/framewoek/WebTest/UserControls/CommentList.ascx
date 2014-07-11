<%@ Control Language="C#" ClassName="CommentList" Inherits="MyUserControlView<List<Comment>>" %>

<p><b>评论列表</b></p>
<% foreach( var comment in Model ) { %>
<p>
	<b><%= comment.Title %></b><br />
	<%= comment.Text %>
</p>
<hr />
<% } %>
