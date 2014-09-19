var __user = "";
var $container = $("#recordListContainer");
var $userFrame = $("#userIframe");
var $mainFrame = $("#main_iframeContainer");
var page = 1;

//获取数据
function getRecords(user) {
    if (!user || user == "") { alert("该会话不存在"); return false; }
    $.ajax({
        type: "get",
        url: "/GetChatRoomDetails2",
        dataType: "json",
        data: { "participantid": user, "page": page,"r":Math.random() },
        success: function (result) {
            createRecordHtml(result.data);
            if (result.isshowmore == 1) {
                $("#record_more").show();
            }
            else {
                $("#record_more").text("没有更多了");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}
//创建数据列表
function createRecordHtml(data) {
        var html = [];
        $.each(data, function (i, n) {
            var recordinfo = { recordsender: "&nbsp;", recorddate: "&nbsp;", recordmessage: "&nbsp;" };

            if (n.recordinfo != "{}")
            {
                recordinfo = JSON.parse(n.recordinfo);
            }

            html.push('<li id='+n.chatroomid+'>');
            html.push('<div class="im_item_inner">');
            html.push('<div class="im_records_msg clearfix">');
            html.push('<img class="im_img" src="' + (n.ChatType == "1" ? "/images/pic/more.png" : n.iconsrc) + '" alt=""/>');
            html.push('<div class="im_info">');
            html.push('<h3><span id="title">' + n.chatroomname + '</span>');
            html.push('' + (n.chatnotreadcount == "0" ? "" : "<i class=\"im_ico im_num\"><span>" + (n.chatnotreadcount > 99 ? "99+" : n.chatnotreadcount) + "</span></i>") + '</h3>');
            html.push('<p>');
            html.push('<span class="im_user_name">' + recordinfo.recordsender + '</span>');
            html.push('<span class="im_time">' + recordinfo.recorddate + '</span>');
            html.push('</p>');
            html.push('</div>');
            html.push('</div>');
            html.push('<div class="im_tip_msg"><p>' + recordinfo.recordmessage + '</p></div>');
            html.push('</div>');
            html.push('<i class="im_ico im_ico_close"></i>');
            html.push('</li>');
        });
        $container.append(html.join(""));
        control.sel();
        control.open();
        control.close();
}

//编辑、删除操作
var control = {
    get:function(){
        getRecords(__user);
    },
    edit: function () {
        $("#record_edit").click(function () {
            if ($(this).val() == "管理") {
                $container.find(".im_ico_close").show();
                $("#record_start").hide();
                $(this).val("确定");
            }
            else {
                $("#record_start").show();
                $container.find(".im_ico_close").hide();
                $(this).val("管理");
            }
        })
    },
    sel: function () {
        $container.find("li").click(function () {
            $container.find("li").removeClass("sel");
            $(this).addClass("sel");
        })
    },
    open:function(){
        $container.find("li").dblclick(function () {
            var title = $(this).find("#title").text();
            var id = $(this).attr("id");
            //var url = "/window.html?roomid=" + id + "&senderid=" + __user + "";
            window.parent.addTab(title, id);
        })
    },
    user: function () {
        $("#record_start").click(function () {
            window.parent.userStart();
        })
    },
    close: function () {
        $container.find(".im_ico_close").click(function () {
            if (confirm("该操作将删除此对话的消息记录，并不再接收该群组/联系人的消息，是否继续？")) {
                control.del($(this).parent().attr("id"))
            }
        });
    },
    del: function (id) {
        if (window.parent.isExistChat(id)) {
            $.ajax({
                type: "get",
                url: "/DeleteChatRooms",
                dataType: "json",
                data: { "chatroomids": id, "senderid": __user },
                async: false,
                success: function (data) {
                    $("#" + id).fadeOut(1000, function () {
                        $(this).remove();
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
        else {
            alert("该群组正在沟通！");
            return false;
        }
    },
    more:function(){
        $("#record_more").click(function () {
            page++;
            control.get();
        })
    },
    init: function () {
        __user = $.getUrlParam("senderid");
        control.get();
        control.user();
        control.edit();
        control.more();
    }
}

//recordsIndex
//切换聊天口
function changeTarget(frameId) {
    if (frameId == "record_iframe") {
        var _src = $("#record_iframe").attr("defalut"); $("#record_iframe").attr("src", _src + "&r=" + Math.random());
        changeRecordTitle(1);
    }
    $("#main_iframeContainer iframe").hide();
    $("#tabContainer").find("li").removeClass("curr");
    $("#tabContainer").find("li[con=" + frameId + "]").addClass("curr");
    $("#" + frameId).show();
    $(document).trigger('tabchange' + frameId); //触发事件
}
//新增聊天窗口
function addTab(title, id) {
    if ($("#" + id).length > 0) {
        changeTarget(id);
    }
    else {
        $("#tabContainer").find("li").removeClass("curr");
        $("#tabContainer").append('<li class="curr" con="' + id + '">' + title + '<i class="im_ico"></i></li>');
        $("#main_iframeContainer iframe").hide();
        $("#main_iframeContainer").append('<iframe id="' + id + '" class="im_chat_window" frameboder="0" src="/window.html?roomid=' + id + '&senderid=' + __user + '"></iframe>');
        changeTab();
        delTarget();
        //更改标题
        changeRecordTitle();
    }
}
//切换操作
function changeTab() {
    $("#tabContainer").find("li").click(function () {
        changeTarget($(this).attr("con"));
        changeRecordTitle();
    })
}
//关闭会话
function delTarget() {
    $("#tabContainer").find(".im_ico").click(function () {
        var sframeId = $(this).parent().attr("con");
        $("#tabContainer").find("li[con=" + sframeId + "]").remove();
        $("#" + sframeId).remove();
        $(document).off('tabchange' + sframeId); //注销事件
        //切回最近的会话
        changeTarget($("#main_iframeContainer iframe:last").attr("id"));
        //更改标题
        changeRecordTitle();
    })
}
//检测当前会话是否开启
function isExistChat(roomId) {
    if ($("#main_iframeContainer").find("#" + roomId + "").length > 0) { return false; }
    else { return true; }
}

//更改沟通记录标题
function changeRecordTitle(type) {
    if (type == 1) {
        $("#chatRecordTitle").text("沟通大厅");
    }
    else if ($("#main_iframeContainer").find("iframe").length > 1) {
        $("#chatRecordTitle").text("返回沟通大厅");
    }
    else {
        $("#chatRecordTitle").text("沟通大厅");
    }
}

//发起会话选择用户
function userStart() {
    $mainFrame.hide();
    $userFrame.show();
    $userFrame.find("iframe").attr("src", "/selectParticipants.html?developerid=" + __user+"&r="+Math.random());
}

var sessionID = "";

function Login() {
    $.ajax({
        url: "/selectuser/Login?rdm=" + Math.random(),
        type: "get",
        contentType: "application/json",
        dataType: "text",
        data: { userGUID: __user },
        async: false,
        success: function (d) {
            sessionID = d;
        },
        error: function (e) {
        }
    });
}

//创建会话
function createChat(data) {
    if (sessionID == "") {
        Login();
    }

    var userInfo = [];
    $.ajax({
        type: "get",
        url: "/selectuser/QueryERPUserInfo",
        dataType: "json",
        data: { sessionID: sessionID, "userGUID": __user },
        async: false,
        success: function (data) {
            userInfo = data;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

    var chatInfo = [];
    var chatInfo =
        {
            chatroomname: "",
            ywtype: "",
            ywguid: "",
            creater: {
                id: __user,
                name: userInfo.name,
                companyname: userInfo.companyname,
                deptname: userInfo.deptname,
                iconsrc: userInfo.iconsrc,
                isdeveloper: userInfo.isdeveloper
            },
            participants: data
        }

    $.ajax({
        type: "post",
        url: "/CreateChatRoom?isinner=1",
        dataType: "json",
        data: chatInfo,
        async: false,
        success: function (data) {
            addTab(data.chatroomname, data.chatroomid);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

//选择用户回调函数
function startUserChat(users) {
    createChat(users)
    $mainFrame.show();
    $userFrame.hide();
}

//取消选择用户回调
function closeUserChat() {
    $mainFrame.show();
    $userFrame.hide();
    return true;
}

//初始化带参调用函数
function init() {
    __user = $.getUrlParam("senderid");
    $("#record_iframe").attr("defalut", "recordsIframe.html?senderid=" + __user);
    $("#record_iframe").attr("src", "/recordsIframe.html?senderid=" + __user +"&r="+Math.random());
    if ($.getUrlParam("id") != null) {
        var rommId = $.getUrlParam("id");
        var userId = $.getUrlParam("senderid");
        var roomName = "";
        $.ajax({
            type: "get",
            url: "/GetChatRoom",
            dataType: "json",
            data: { "chatroomid": rommId, "participantid": __user, "r": Math.random() },
            async: false,
            success: function (data) {
                if (data.isexist == 1) {
                    roomName = data.chatroom.chatroomname;
                    __user = userId;
                    addTab(roomName, rommId);
                }
                else {
                    alert("该会话不存在");
                    return false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
}

//获取url参数
(function($){
    $.getUrlParam = function(name)
    {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r!=null) return unescape(r[2]); return null;
    }
})(jQuery);