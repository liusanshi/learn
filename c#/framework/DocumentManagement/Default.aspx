<%@ Page Language="C#" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>首页</title>
    <meta charset="UTF-8">
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
            height:100%;
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
    </style>
</head>
<body>
    <div class="easyui-layout" style="width: 100%; height: 100%;">
        <div data-options="region:'north'" style="height: 50px">
            欢迎<%=User.Identity.Name %> 登录成功
        </div>
        <div data-options="region:'south',split:true" style="height: 50px;"></div>
        <%--<div data-options="region:'east',split:true" title="East" style="width: 180px;">
            <ul class="easyui-tree" data-options="url:'tree_data1.json',method:'get',animate:true,dnd:true"></ul>
        </div>--%>
        <div data-options="region:'west',split:true" title="West" style="width: 200px;">
            <div class="easyui-accordion" data-options="fit:true,border:false">
                <div title="Title1" style="padding: 10px;">
                    content1
                </div>
                <div title="Title2" data-options="selected:true" style="padding: 10px;">
                    content2
                </div>
                <div title="Title3" style="padding: 10px">
                    content3
                </div>
            </div>
        </div>
        <div data-options="region:'center',title:'Main Title',iconCls:'icon-ok'">
            <div class="easyui-tabs" data-options="fit:true,border:false,plain:true">
                <div title="About" data-options="href:'_content.html'" style="padding: 10px"></div>
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
    </div>
</body>
<script type="text/javascript" src="modules/sea.js"></script>
<script type="text/javascript" src="static/seajs-config.js"></script>

<script type="text/javascript">
    seajs.use('backbone', function (bb) {

    })
</script>
</html>
