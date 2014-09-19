//模板里面注册帮助函数
template.helper('$name', function (sendername2, receivename, companyname) {
    //return sendername2 + '@' + receivename + '&nbsp;-&nbsp;' + companyname;
    return sendername2 + '&nbsp;-&nbsp;' + companyname;
});
$(document).on("mobileinit", function () {
    jQuery.mobile.autoInitializePage = false;
});

$(function () {
    var socket, roomid = $.fnRequest("roomid"), senderid = $.fnRequest("senderid")
        , im_chat_list_block = template.compile($('#im_chat_list_block').html()), im_chat_item_block = template.compile($('#im_chat_item_block').html())
        , im_chat_window = $('#im_chat_window'), CurUserInfo, txt_msg = $('#txt_msg')
        , completedWebsite //开发商icon的服务器地址
        , imgType = ['.jpg', '.png']
        , start = 0, size = 50, pages = 2
        , printTime = function (time) {//输出时间
            time = time || new Date().toString();
            im_chat_window.append('<p class="history_time">' + time + '</p>');
        };

    //呈现消息
    //printTime('15:54');

    connect(); //连接服务器
    getInfo(); //获取所有数据
    
    initialEmoji(start, size);
    initPageControls();

    //按enter键发送消息
    $('#btn_send').on('tap', function (e) {
        var data = {
            message: txt_msg.val(), isme: 'curr'
            , iconsrc: CurUserInfo.iconsrc, sendername: CurUserInfo.sendername
            , companyname: CurUserInfo.companyname
        };
        if (data.message) {
            sendMsg(data);
            txt_msg.val('');
        } else {
            txt_msg.focus();
        }
    });

    //发送表情
    $('.ico_face').on('tap', function () {
        $('#emojiWrapper').show();
        return false;
    });
    txt_msg.keyup(function (e) {
        if (e.keyCode === 8) {
            txt_msg.val(removeEmoji(txt_msg.val(), getCursorPosition(this)));
        }
    });
    //发送图片
    $('#filesInput').on('change', function (e) {
        var file = e.target.files[0], reader, imgurl, formdata, reg = /receiveMsg\(.*\)/;
        if (file) {
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function (e) {
                if (xhr.readyState == 4) {
                    if (xhr.status == 200) {
                        var matchs = reg.exec(xhr.responseText);
                        if (matchs.length > 0)
                            Function(matchs[0])();
                    }
                }
            };
            formdata = new FormData();
            formdata.append("sendImage", file);
            xhr.open("POST", "/file-upload", true);
            xhr.send(formdata);
        }
    });

    $(document).on('tap', function (e) {
        if (e.target.id !== "emojiWrapper")
            $('#emojiWrapper').hide();
    });
    
    $('#emojiWrapper').on('swiperight', function () {
        up();
    }).on('swipeleft', function () {
        down();
    }).on('tap', 'img', function () {
        txt_msg.focus();
        txt_msg.val(txt_msg.val() + '[emoji:' + this.title + ']');
        $('#emojiWrapper').hide();
        return false;
    }).on('tap', 'span', function () {
        var $this = $(this);
        initialEmoji(+($this.attr('data-start')), size);
        $('#emojiWrapper span').removeClass('page-item');
        $this.addClass('page-item');
        return false;
    });
    //向上翻页
    function up() {
        if (start < size * (pages - 1)) {
            start += size;
            initialEmoji(start, size);
            togglePageControl(start);
        }
    }
    //向下翻页
    function down() {
        if (start >= size) {
            start -= size;
            initialEmoji(start, size);
            togglePageControl(start);
        }
    }
    //切换页签
    function togglePageControl(start) {
        $('#emojiWrapper span').removeClass('page-item');
        $('#emojiWrapper span[data-start="' + start + '"]').addClass('page-item');
    }

    //连接服务器
    function connect() {
        if (!(socket && socket.connected)) {
            socket = io.connect();
            //显示页面数据信息
            //1.当前用户的信息、2.历史聊天信息
            socket.on('get_info', function (data) {
                //0.判断聊天室是否存在
                if (!roomIsExist(data)) return;
                
                //1.当前用户的信息
                completedWebsite = data.completedWebsite;
                CurUserInfo = data.me;
                dealIconSrc(CurUserInfo);

                //2.历史聊天信息.history
                showHistory(data.history, CurUserInfo.senderid);

                //房间是否已经终止聊天
                if (!roomIsEnd(data)) return;

                //初始化发送文件的方法
                initPage(data);
            });

            //接受聊天消息
            socket.on('say', function (data) {
                var serverdate = data.serverdate;
                showMsg([data]);

                socket.emit('UpdateParticipantState', serverdate);
            });

            //离开群的通知
            socket.on('leaveroom', function () {
                btn_disabled('您已不在该群！');

            });

            //终止聊天
            socket.on('end_chat', function (data) {
                if (data.success) {
                    btn_disabled('当前对话已终止，不能发送消息');
                }
            });
        }
    }

    //按钮置灰
    function btn_disabled(msg) {
        $('.ico_face').hide();
        $('.ico_pic').hide();
        $('#btn_send').hide();
        $('.im_chat_end').addClass('in').html(msg);
    }

    //获取所有数据信息
    function getInfo() {
        socket.emit('get_info', { roomid: roomid, senderid: senderid });
    }

    //发送消息
    function sendMsg(data) {
        var msg = { "message": data.message };
        data.serverdate = new Date();
        msg.clientdate = data.serverdate.format('yyyy/MM/dd hh:mm:ss');
        socket.emit('say', msg);
        showMsg([data]);
    }
    //处理iconsrc
    function dealIconSrc(data) {
        if (data && data.isdeveloper) {
            data.iconsrc = completedWebsite + data.iconsrc;
            if ('deptname' in data) {
                data.companyname = data.deptname; //开发商使用部门名称
            } else if ('sendercompanyname' in data) {
                data.companyname = data.sendercompanyname; //开发商使用部门名称 sendercompanyname
            }
        }
    };

    //显示消息
    function showMsg(data) {
        for (var i = 0, item; item = data[i]; i++) {
            item.Msg = delMessage(item.message);
            item.serverdate = new Date(item.serverdate).format('MM/dd hh:mm:ss').replace(/-/g, '/').replace('T', ' ').substring(0, 19);
            if (item.isme === 'curr') { //我发送的
                item.sendername2 = '我';
                item.receivename = "所有人";
                item.iconsrc = CurUserInfo.iconsrc;
                item.companyname = CurUserInfo.companyname;
            } else { //接受
                item.sendername2 = item.sendername;
                if (item.receiverid) { //单独回复
                    item.receivename = "你";
                } else {
                    item.receivename = "所有人";
                }
                dealIconSrc(item);
            }
        }
        im_chat_window.append(im_chat_list_block({ data: data }));
        $(document.body).scrollTop($('.container').height());
    }
    //处理消息
    function delMessage(msg) {
        msg = showEmoji(msg); //处理表情
        return msg.replace(/\n/g, '<br/>');
    }
    //显示表情
    function showEmoji(msg) {
        var match, result = msg,
            reg = /\[emoji:\d+\]/g,
            emojiIndex,
            totalEmojiNum = size * pages;
        while (match = reg.exec(msg)) {
            emojiIndex = match[0].slice(7, -1);
            if (emojiIndex > totalEmojiNum) {
                result = result.replace(match[0], '[X]');
            } else {
                result = result.replace(match[0], '<img class="emoji" src="../content/emoji/' + emojiIndex + '.gif" />');//todo:fix this in chrome it will cause a new request for the image
            };
        };
        return result;
    };

    //删除表情
    function removeEmoji(str, posi) {
        var regs = /\[[^\[]*$/, rege = /^[^\[]*]/
      , match1 = regs.exec(str.substring(0, posi))
      , match2 = rege.exec(str.substring(posi)), lens, lene;
        if (match1 && match2) {
            lens = match1[0].length;
            lene = match2[0].length;
            if (lens + lene <= 10)
                return str.substring(0, posi - lens) + str.substring(posi + lene);
        } else {
            var reg = /\[emoji:\d*$/, txt1 = str.substring(0, posi), match = reg.exec(txt1);
            if (match && match.length) {
                return str.substring(0, posi - match[0].length) + str.substring(posi);
            } else {
                reg = /^emoji:\d+]/, txt1 = str.substring(posi), match = reg.exec(txt1);
                if (match && match.length) {
                    return str.substring(0, posi) + str.substring(posi + match[0].length);
                }
            }
        }
        return str;
    }
    //获取光标的位置
    function getCursorPosition(el) {
        var pos = 0;
        if ('selectionStart' in el) {
            pos = el.selectionStart;
        } else if ('selection' in document) {
            el.focus();
            var Sel = document.selection.createRange();
            var SelLength = document.selection.createRange().text.length;
            Sel.moveStart('character', -el.value.length);
            pos = Sel.text.length - SelLength;
        }
        return pos;
    }

    //初始化表情
    function initialEmoji(start, size) {
        var emojiContainer = $('#emojis'), html = [], index = 0;
        for (var i = 1; i <= size; i++) {
            index = i + start;
            html.push('<img src="../content/emoji/' + index + '.gif" title="' + index + '" />');
        };
        emojiContainer.html(html.join(''));
    }
    //初始化分页控件
    function initPageControls() {
        var html = [];
        for (var i = 0; i < pages; i++) {
            html.push('<span ' + (i == 0 ? 'class="page-item"' : '') + 'data-start="' + (size * i) + '"></span>');
        }
        $('#pageControls').html(html.join(''));
    }

    //显示历史消息
    function showHistory(history, senderid) {
        for (var i = 0, item; item = history[i]; i++) {
            if (item.senderid === senderid) {
                item.isme = 'curr';
            }
        }
        showMsg(history);
    }

    //房间不存在时
    function roomIsExist(data) {
        if (data.isexist) { return true; }
        btn_disabled('不存在聊天群！');
        return false;
    }
    //房间是否终止聊天
    function roomIsEnd(data) {
        if (!data.isend) return true;
        btn_disabled('您已不在该群！');
        return false;
    }

    //初始化初始化页面
    function initPage(data) {
        var CurUserInfo = data.me;
        window.receiveMsg = function (msg) {
            msg = '<a target="_blank" href="' + msg + '">' + (checkImg(msg) ? '<img class="send_img" src="' + msg + '">' : msg) + '</a>';
            var data = {
                message: msg, isme: 'curr'
            , iconsrc: CurUserInfo.iconsrc, sendername: CurUserInfo.sendername, companyname: CurUserInfo.companyname
            };
            sendMsg(data);
        };
        
        if (!data.iscreator) {
            $('.im_add_friend').hide();
            $('#btn_end').hide();
        }
        $('#lbl_group_name').html(data.chatroom.chatroomname);
        $('#txt_group_name').val(data.chatroom.chatroomname);
        $('#creator').html(data.chatroom.creator);
        $('#createdate').html(data.chatroom.createddate);
        $('#group_img').attr('src', '');
        data = null;
    };
    //检查是否图片
    function checkImg(fn) {
        var item;
        for (var i = 0, len = imgType.length; i < len; i++) {
            item = imgType[i];
            if (fn.toLowerCase().indexOf(item) === fn.length - item.length)
                return true;
        }
        return false;
    };
});