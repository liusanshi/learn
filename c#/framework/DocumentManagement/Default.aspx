<%@ Page Language="C#" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>我的文档管理</title>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge;ie=9;ie=8;" />
    <link rel="stylesheet" type="text/css" href="modules/jquery-easyui/themes/default/easyui.css">
    <link rel="stylesheet" type="text/css" href="modules/jquery-easyui/themes/icon.css">

    <style>
        * {
            font-size: 12px;
        }

        html, body {
            font-family: verdana,helvetica,arial,sans-serif;
            font-size: 12px;
            margin: 0;
            height: 100%;
        }

        h2 {
            font-size: 18px;
            font-weight: bold;
            margin: 0;
            margin-bottom: 15px;
        }

        .demo-info {
            padding: 0 0 12px 0;
        }

        .demo-tip {
            display: none;
        }

        /* 开始 主菜单主要的css*/
        .main-menu {
            list-style-type: none;
            padding: 0;
        }

            .main-menu > li:hover {
                background-color: #ffe88c;
                border: solid 1px #d69c00;
                padding: 0;
            }

            .main-menu > li {
                padding: 1px;
                cursor: pointer;
            }

                .main-menu > li > div {
                    text-align: center;
                }

                .main-menu > li > .menuitem-icon {
                    height: 50px;
                    line-height: 50px;
                }
        /* 结束 主菜单主要的css*/
    </style>
</head>
<body class="easyui-layout" style="width: 100%; height: 100%;">
    <div data-options="region:'north'" style="height: 50px">
        欢迎<%=User.Identity.Name %> 登录成功
    </div>
    <div data-options="region:'south',split:true" style="height: 50px;"></div>
    <%--<div data-options="region:'east',split:true,show:false" title="East" style="width: 180px;">
            <ul class="easyui-tree" data-options="url:'tree_data1.json',method:'get',animate:true,dnd:true"></ul>
        </div>--%>
    <div data-options="region:'west',split:true" title="导航菜单" style="width: 200px;">
        <div class="easyui-accordion" data-options="fit:true,border:false">
            <div title="我的桌面" style="padding: 10px;">
                content1
            </div>
            <div title="文档管理" data-options="selected:true" style="padding: 10px;">
                content2
            </div>
            <div title="系统设置" style="padding: 10px">
                <ul class="main-menu">
                    <!--菜单-->
                </ul>
            </div>
        </div>
    </div>
    <div data-options="region:'center',title:'Main Title',iconCls:'icon-ok'">
        <div class="easyui-tabs" id="main_content" data-options="fit:true,border:false,plain:true">
            <div title="主页" data-options="href:'_content.html'" style="padding: 10px"></div>
            <div title="DataGrid" style="padding: 5px">
                <table class="easyui-datagrid"
                    data-options="url:'datagrid_data1.json',method:'get',singleSelect:true,fit:true,fitColumns:true">
                    <thead>
                        <tr>
                            <th data-options="field:'itemid'" width="80">Item ID</th>
                            <th data-options="field:'productid'" width="100">Product ID</th>
                            <th data-options="field:'listprice',align:'right'" width="80">List Price</th>
                            <th data-options="field:'unitcost',align:'right'" width="80">Unit Cost</th>
                            <th data-options="field:'attr1'" width="150">Attribute</th>
                            <th data-options="field:'status',align:'center'" width="50">Status</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
</body>
<script type="text/javascript" src="modules/sea-debug.js"></script>
<script type="text/javascript" src="static/seajs-config.js"></script>
<script src="modules/jquery-1.11.0.min.js"></script>
<script src="modules/jquery-easyui/jquery.easyui.min.js"></script>
<script src="modules/jquery-easyui/easyloader.js"></script>

<script type="text/javascript">
    seajs.use('/static/index', function (obj) {
        var Menu = obj.Menu, Data = obj.Data,
        menu = new Menu($('.main-menu'), [new Data(), new Data()]);//添加菜单
        menu.render();
    });
    //TabManager
    seajs.use('/static/tabManager', function (tabobj) {
        using('tabs', function () {
            var TabManager = tabobj.TabManager.tabManager = new tabobj.TabManager($('#main_content'));
            TabManager.open({ title: "nihao", url: 'nihao' });
        });
    });
</script>
</html>
