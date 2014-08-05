<%@ Page Language="C#" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;ie=9;ie=8;" />
    <title>登录</title>

    <link href="modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        .loginBox {
            width: 420px;
            height: 230px;
            padding: 0 20px;
            border: 1px solid #fff;
            color: #000;
            /*margin-top: 40px;*/
            border-radius: 8px;
            background: white;
            box-shadow: 0 0 15px #222;
            background: -moz-linear-gradient(top, #fff, #efefef 8%);
            background: -webkit-gradient(linear, 0 0, 0 100%, from(#f6f6f6), to(#f4f4f4));
            font: 11px/1.5em 'Microsoft YaHei';
            position: absolute;
            left: 50%;
            top: 50%;
            margin-left: -210px;
            margin-top: -115px;
        }
    </style>
</head>
<body>
    <form id="form1" action="login.aspx" method="post">
        <%= AntiForgery.GetHtml() %>
        <div class="loginBox row-fluid">
            <div class="span7 left">
                <h2>用户登录</h2>
                <p>
                    <input type="text" name="username" />
                </p>
                <p>
                    <input type="text" name="password" />
                </p>
                <div class="row-fluid">
                    <div class="span8 lh30">
                        <label>
                            <input type="checkbox" name="rememberme" />下次自动登录</label>
                    </div>
                    <div class="span1">
                        <input class="btn btn-primary" type="submit" value=" 登录 " id="btnlogin"/>
                    </div>
                </div>
            </div>
            <div class="span5 right">
                <h2>写什么呢？</h2>
                <div>
                    <p>这里有一段文字啊，很多的文字啊，太多太多的文字了，可以放一些图片之类的东西。。</p>
                </div>
            </div>
        </div>
    </form>
    <%--<script src="static/jquery-easyui/jquery.min.js"></script>
    <script src="static/jquery-easyui/plugins/jquery.form.js"></script>
    <script type="text/javascript">
        $('#btnlogin').click(function () {
            $("#form1").form({ url: "loginActive.aspx" }).submit();
        });
    </script>--%>
</body>
</html>
