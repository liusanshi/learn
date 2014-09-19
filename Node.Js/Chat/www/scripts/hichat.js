/*
 *hichat v0.4.2
 *Wayou Mar 28,2014
 *MIT license
 *view on GitHub:https://github.com/wayou/HiChat
 *see it in action:http://hichat.herokuapp.com/
 */
var __sender = null;
var __receiver = "";
var __isfqr = true;
var __isAlone = false;
window.onload = function () {
    $.ajax({
        type: "post",
        url: "/interface/CreateChatRoom",
        dataType: "json",
        data: {
            chatroomname: "",
            ywtype: "",
            ywguid: "",
            creater:
            {
                id: "liuk",
                name: "刘可",
                copanyname: "明源软件",
                deptname: "产品ABU",
                iconsrc: "",
                isdeveloper: 1
            },
            participants:
            [{
                id: "kel",
                name: "柯灵",
                copanyname: "明源软件",
                iconsrc: "",
                isdeveloper: 1
            }, {
                id: "liul",
                name: "刘磊",
                copanyname: "明源软件1",
                iconsrc: "",
                isdeveloper: 0
            }]
        },
        async: false,
        success: function (data) {
            alert(data.chatroomid);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

    return;
    var objChatInfo = {};
    __sender = $.fnRequest("i");
    var nickname = $.fnRequest("n");

    var token = $.fnRequest("t");
    var fqr = $.fnRequest("fqr");

    if (fqr == "") {
        objChatInfo.token = token;
        objChatInfo.sender = __sender;
        objChatInfo.receivers = ["liul"];
    }
    else {
        __isfqr = false;
        __isAlone = true;
        __receiver = fqr;
        objChatInfo.token = token;
        objChatInfo.sender = fqr;
        objChatInfo.receivers = [__sender];
    }

    //var success = false;
    //$.ajax({
    //    type: "post",
    //    url: "/verifyToken",
    //    dataType: "jsonp",
    //    jsonp: 'callback',
    //    data: { "token": token, "kfsid": (__isfqr ? __sender : fqr), "gysid": (__isfqr ? "" : __sender) },
    //    async: false,
    //    success: function (data) {
    //        var data = $.parseJSON(data);
    //        if (data.message != "") {
    //            objChatInfo.receivers = data.message.split(";");
    //            success = true;
    //        }
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        alert(errorThrown);
    //    }
    //});

    //if (!success) {
    //    return;
    //}
    //alert(nickname); alert(objChatInfo);
    var hichat = new HiChat();
    if (nickname != "" && objChatInfo != {}) {

        hichat.init(nickname, objChatInfo);
    }
    else {
        alert("参数不全");
    }
};
var HiChat = function (config) {
    this.socket = null;
    this.config = config;
};
HiChat.prototype = {
    init: function (nickname, objChatInfo) {
        var lastDateTime = null;
        var that = this;
        this.socket = io.connect();

        this.socket.on('connect', function (err) {
        });
        this.socket.on('error', function (err) {
        });
        this.socket.on('finishChat', function () {
            $('#sendBtn').attr("disabled", "disabled");
            $('#sendImage').attr("disabled", "disabled");
        });
        this.socket.on('newMsg', function (objMsg) {
            lastDateTime = objMsg.servertime;
            that._displayNewMsg(objMsg.nickname, objMsg.msg, objMsg.color, objMsg.createtime);
        });
        this.socket.on('newImg', function (objMsg) {
            lastDateTime = objMsg.servertime;
            that._displayImage(objMsg.nickname, objMsg.msg, objMsg.color, objMsg.createtime);
        });

        function sendMessage() {
            var messageInput = $('#messageInput'),
                msg = messageInput.val(),
                color = $('#colorStyle').val();
            if ($.trim(msg).length != 0) {
                messageInput.val('');
                var createtime = (new Date()).format("yyyy/MM/dd hh:mm:ss");
                that.socket.emit('postMsg', { sender: __sender, receivers: (__isAlone ? __receiver : ""), msg: msg, color: color, createtime: createtime });
                that._displayNewMsg('me', msg, color, createtime);

                if (__isfqr) {
                    __isAlone = false;
                }
            };
        };

        $('#sendBtn').bind('click', function () {
            sendMessage();
        });
        $('#sendBtnA').bind('click', function () {
            if (__isfqr) {
                __receiver = "kl";
                __isAlone = true;
            }
            sendMessage();
        });
        $('#finishBtn').bind('click', function () {
            that.socket.emit('finishChat');
        });
        $('#changeBtn').bind('click', function () {
            $('#messageInput').val("");
            that.socket.emit('changeChat', { token: "1", sender: "lk", receivers: ["kl"] });
        });
        $('#addBtn').bind('click', function () {
            that.socket.emit('addReceivers', ["sb", "chqh"]);
        });
        $('#deleteBtn').bind('click', function () {
            that.socket.emit('deleteReceivers', ["sb"]);
        });
        $('#messageInput').bind('keyup', function (e) {
            if (e.keyCode == 13) {
                sendMessage();
            }
        });
        $('#clearBtn').bind('click', function () {
            $('#historyMsg').html('');
        });
        window.receiveMsg = function (msg) {
            $("#sendImage").val("");
            var color = $('#colorStyle').val();
            var createtime = (new Date()).format("yyyy/MM/dd hh:mm:ss");
            that._displayImage('me', msg, color, createtime);
            that.socket.emit('img', { sender: __sender, receivers: (__isAlone ? __receiver : ""), msg: msg, color: color, createtime: createtime });


        };
        $('#sendImage').bind('change', function () {
            $("#frmUpload").submit();
            return false;
        });
        this._initialEmoji();
        $('#emoji').bind('click', function (e) {
            var emojiwrapper = $('#emojiWrapper');
            emojiwrapper.show();
            e.stopPropagation();
        });
        $(document.body).bind('click', function (e) {
            var emojiwrapper = $('#emojiWrapper');
            if ($(e.target) != emojiwrapper) {
                emojiwrapper.hide();
            };
        });
        $('#emojiWrapper').bind('click', function (e) {
            var target = e.target;
            if (target.nodeName.toLowerCase() == 'img') {
                var messageInput = $('#messageInput');
                messageInput.focus();
                messageInput.val(messageInput.val() + '[emoji:' + target.title + ']');
            };
        });
        $(window).bind("unload", function () {
            if (lastDateTime) {
                that.socket.emit('updateState', { "sender": __sender, "lastDateTime": lastDateTime, "isfqr": __isfqr });
            }
        }
        );
        if (nickname && nickname != "" && objChatInfo && objChatInfo != {}) {
            that.socket.emit("joinChat", nickname, objChatInfo);
        }
    },
    _initialEmoji: function () {
        var emojiContainer = $('#emojiWrapper'),
            docFragment = document.createDocumentFragment();
        for (var i = 69; i > 0; i--) {
            var emojiItem = document.createElement('img');
            emojiItem.src = '../content/emoji/' + i + '.gif';
            emojiItem.title = i;
            docFragment.appendChild(emojiItem);
        };
        emojiContainer.append(docFragment);
    },
    _displayNewMsg: function (user, msg, color, date) {
        var container = $('#historyMsg'),
            msgToDisplay = document.createElement('p'),
            //determine whether the msg contains emoji
            msg = this._showEmoji(msg);
        msgToDisplay.style.color = color || '#000';
        msgToDisplay.innerHTML = user + '<span class="timespan">(' + date + '): </span>' + msg;
        container.append(msgToDisplay);
        container.attr("scrollTop", container.attr("scrollHeight"));
    },
    _displayImage: function (user, imgData, color, date) {
        var container = $('#historyMsg'),
            msgToDisplay = document.createElement('p');
        msgToDisplay.style.color = color || '#000';
        msgToDisplay.innerHTML = user + '<span class="timespan">(' + date + '): </span> <br/>' + '<a href="' + imgData + '" target="_blank"><img src="' + imgData + '"/></a>';
        container.append(msgToDisplay);
        container.attr("scrollTop", container.attr("scrollHeight"));
    },
    _showEmoji: function (msg) {
        var match, result = msg,
            reg = /\[emoji:\d+\]/g,
            emojiIndex,
            totalEmojiNum = $('#emojiWrapper').children.length;
        while (match = reg.exec(msg)) {
            emojiIndex = match[0].slice(7, -1);
            if (emojiIndex > totalEmojiNum) {
                result = result.replace(match[0], '[X]');
            } else {
                result = result.replace(match[0], '<img class="emoji" src="../content/emoji/' + emojiIndex + '.gif" />');//todo:fix this in chrome it will cause a new request for the image
            };
        };
        return result;
    }
};
