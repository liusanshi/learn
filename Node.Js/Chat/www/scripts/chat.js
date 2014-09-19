//模板里面注册帮助函数
template.helper('$name', function (sendername, receivername, companyname, ishowcomp) {
    return '<span class="im_user_name">' + sendername + '&nbsp;@&nbsp;' +
        receivername + '</span>' + (ishowcomp ? '-<span class="im_company_name" title="' + companyname + '">' + companyname + '</span>' : '');
});

$(function () {
    var socket, roomid = $.fnRequest("roomid"), senderid = $.fnRequest("senderid")
        , friend_list_tpl = template.compile($('#friend_list_tpl').html())
        , friend_item_tpl = template.compile($('#friend_item_tpl').html())
        , msg_tpl = template.compile($('#msg_tpl').html())
        , im_friend_list = $('.im_friend_list')
        , txt_msg = $('#txt_msg')
        , PrivateChatTo = '' //单独发送给谁
        , CurUserInfo
        , completedWebsite //开发商icon的服务器地址
        , imgType = ['.jpg', '.png']
        , start = 0, size = 50, pages = 2
        , UserList;

    connect(); //连接服务器

    getInfo(); //获取所有数据
    
    initialEmoji(start, size);
    initPageControls();

    //发送消息
    $('#btn_send').click(function () {
        var data = {
            message: txt_msg.val(), ShowPosi: 'im_r_show'
            , iconsrc: CurUserInfo.iconsrc, sendername: CurUserInfo.sendername
            , companyname: CurUserInfo.companyname
        };
        if (data.message) {
            sendMsg(data);
            txt_msg.val('');
        } else {
            txt_msg.focus();
            //$('#input_tips').html('----全体发送----');
            //privateChat(false);
        }
    });

    //终止对话
    $('#btn_end').click(function () {
        $('.im_dialog_container').addClass('in');
        $('.top_layer').addClass('in');
    });

    //专门的二次对话框
    $('.im_dialog_container').on('click', '.close', function () {
        dialog_close();
    }).on('click', '#btn_cancel', function () {
        dialog_close();
    }).on('click', '#btn_ok', function () {
        socket.emit('end_chat', { "senderid": senderid, "roomid": roomid });
    });
    //对话框关闭
    function dialog_close() {
        $('.im_dialog_container').removeClass('in');
        $('.top_layer').removeClass('in');
    }

    //关闭
    $('#btn_close').click(function () {
        socket.emit('disconnect', { "senderid": senderid, "roomid": roomid });
        //调用父页面的关闭方法
        parentClose(roomid);
    });

    //选择表情
    $('.im_face').click(function () {
        var $ew = $('#emojiWrapper'), $this = $(this);
        $ew.css('top', $this.position().top - $ew.height()).show();
        return false;
    });
    //选择人员
    $('.im_add_friend').click(function () {
        var win = $('#im_select_user');
        win.find('iframe')
            .attr('src', '/selectParticipants.html?chatroomid=' + roomid + '&developerid=' + senderid
            + '&callback=modifyUserList&T=' + new Date().getTime());
        win.show();
    });

    $('#emojiWrapper').on('click', 'img', function () {
        txt_msg.focus();
        txt_msg.val(txt_msg.val() + '[emoji:' + this.title + ']');
        $('#emojiWrapper').hide();
        return false;
    }).on('click', 'span', function () {
        var $this = $(this);
        initialEmoji(+($this.attr('data-start')), size);
        $('#emojiWrapper span').removeClass('page-item');
        $this.addClass('page-item');
        return false;
    }).on('click', '#pageControls', function () {
        return false;
    });

    $('#sendImage').on('change', function () {
        //$('#filename').val(this.files[0].name);
        $("#frmUpload").submit();
        return false;
    });

    //单独回复
    $(document).on('click', '.im_reply_btn', function () {
        PrivateChatTo = this.getAttribute('data-senderid');
        if (PrivateChatTo === '_all_') {
            PrivateChatTo = '';
            $('#input_tips').html('----全体发送----');
            privateChat(false);
        } else {
            var Sender = findSenderInfo(PrivateChatTo);
            if (Sender) {
                $('#input_tips').html('----单独回复 @' + Sender.sendername + ' ' + Sender.companyname + '----');
                privateChat(true);
            }
        }
        txt_msg.focus();
    }).on('click', function (e) {
        if (e.target.id !== "emojiWrapper")
            $('#emojiWrapper').hide();
    });
    txt_msg.on('keyup', function (e) {
        if ((e.keyCode === 13 && e.ctrlKey)
         || (e.keyCode === 83 && e.altKey)) {
            $('#btn_send').click();
        }
    });

    txt_msg.keyup(function (e) {
        if (txt_msg.val().split('\n').length >= 4) {
            txt_msg.removeClass('txt-noscroll').addClass('txt-scroll');
        } else if (txt_msg.hasClass('txt-scroll')) {
            txt_msg.removeClass('txt-scroll').addClass('txt-noscroll');
        }
        if (e.keyCode === 8) {
            txt_msg.val(removeEmoji(txt_msg.val(), getCursorPosition(this)));
        }
    });

    $('#lbl_group_name').click(function () {
        $(this).hide();
        $('#txt_group_name').show().focus();
    });
    $('#txt_group_name').blur(function () {
        socket.emit('updategroupname', { "groupname": $(this).val(), "roomid": CurUserInfo.roomid }); //修改群名称
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

    //私聊时的效果
    function privateChat(isp) {
        txt_msg[isp ? 'addClass' : 'removeClass']('txt-pc');
    }

    //连接服务器
    function connect() {
        if (!(socket && socket.connected)) {
            socket = io.connect();
            //显示页面数据信息
            //1.联系人信息、2.当前用户的信息、3.历史聊天信息
            socket.on('get_info', function (data) {
                //0.判断聊天室是否存在
                if (!roomIsExist(data)) return;
                completedWebsite = data.completedWebsite;
                
                //1.当前用户的信息
                CurUserInfo = data.me;
                if (!CurUserInfo) {
                    CurUserInfo = { roomid: roomid, senderid: senderid };
                }
                dealIconSrc(CurUserInfo);
                UserList = data.UserList || [];
                for (var j = 0, len = UserList.length; j < len; j++) {
                    dealIconSrc(UserList[j]);
                }

                //2.联系人信息
                im_friend_list.html(friend_list_tpl({ data: data.UserList }));                

                //3.历史聊天信息.history
                showHistory(data.history, CurUserInfo.senderid);
                
                //房间是否已经终止聊天
                if (!roomIsEnd(data)) return;

                //初始化发送文件的方法
                initPage(data);
            });

            //接受聊天消息
            socket.on('say', function (data) {
                var serverdate = data.serverdate;
                showMsg(data);

                socket.emit('UpdateParticipantState', serverdate);
            });

            //终止聊天
            socket.on('end_chat', function (data) {
                if (data.success) {
                    end_chat();
                    dialog_close();
                }
            });

            //修改群名称
            socket.on('updategroupname', function (name) {
                var self = $('#txt_group_name').hide();
                $('#lbl_group_name').html(name).show();
                modifyRoomName(CurUserInfo.roomid, name);
            });

            //离开群的通知
            socket.on('leaveroom', function () {
                end_chat('您已不在该群！');
            });
        }
    }

    //获取所有数据信息
    function getInfo() {
        socket.emit('get_info', { roomid: roomid, senderid: senderid });
    }

    //显示历史消息
    function showHistory(history, senderid) {
        for (var i = 0, item; item = history[i]; i++) {
            if (item.senderid === senderid) {
                item.ShowPosi = 'im_r_show';
            }
            showMsg(item);
        }
    }

    //查找参与人信息
    function findSenderInfo(senderid) {
        for (var i = 0, len = UserList.length; i < len; i++) {
            if (UserList[i].senderid === senderid)
                return UserList[i];
        }
    }
    //处理iconsrc
    function dealIconSrc(data) {
        if (data && data.isdeveloper) {
            if (data.iconsrc && data.iconsrc.indexOf(completedWebsite) === -1)
                data.iconsrc = completedWebsite + data.iconsrc;
            data.companyname = data.deptname; //开发商使用部门名称
        }
    };

    //显示消息
    function showMsg(data) {
        data.Msg = delMessage(data.message);
        data.serverdate = new Date(data.serverdate).format('MM/dd hh:mm:ss').replace(/-/g, '/').substring(0, 19);
        if (data.ShowPosi === 'im_r_show') { //我发送的
            data.sendername2 = '我';
            if (PrivateChatTo) { //单独回复
                data.ishowcomp = true;
                var u = findSenderInfo(PrivateChatTo);
                data.receivername2 = u ? u.sendername : '所有人';
                data.companyname = u ? u.companyname : '';
                data.ShowPosi = 'im_r_show_pc';
            } else if (data.receiverid) {
                data.ishowcomp = true;
                data.receivername2 = data.receivername || '所有人';
                data.companyname = data.receivercompanyname || '';
                data.ShowPosi = 'im_r_show_pc';
            } else {
                data.receivername2 = "所有人";
            }
            data.iconsrc = CurUserInfo.iconsrc;
        } else { //接受
            data.sendername2 = data.sendername;
            if (data.receiverid) { //单独回复
                data.receivername2 = "你";
                data.ShowPosi = 'pc';
            } else {
                data.receivername2 = "所有人";
            }
            dealIconSrc(data);
        }
        var msgContent = $('.im_record_list > ul');
        msgContent.append(msg_tpl(data)).parent().scrollTop(msgContent.height());
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

    //初始化初始化页面
    function initPage(data) {
        var CurUserInfo = data.me, chatroom = data.chatroom;
        window.receiveMsg = function (msg) {
            msg = '<a target="_blank" href="' + msg + '">' + (checkImg(msg) ? '<img class="send_img" src="' + msg + '">' : msg) + '</a>';
            var data = {
                message: msg, ShowPosi: 'im_r_show'
            , iconsrc: CurUserInfo.iconsrc, sendername: CurUserInfo.sendername, companyname: CurUserInfo.companyname
            };
            sendMsg(data);
        };
        //选择窗体关闭
        window.hideUserSelectWin = function () {
            $('#im_select_user').hide();
        }

        //选择人员回调
        window.modifyUserList = function (data) {
            //{ sel: selectedCys, del: delCys, add: addCys }*/
            if (data.del.length != 0 || data.add.length != 0) {
                for (var j = 0, len = data.sel.length; j < len; j++) {
                    dealIconSrc(data.sel[j]);
                }
                im_friend_list.html(friend_list_tpl({ data: data.sel }));
                delAndaddUserlist(data.del, data.add);
                socket.emit("UpdateParticipants", { chatroomid: roomid, senderid: senderid, selCnt: data.sel.length, deleteparticipants: data.del, addparticipants: data.add });
            }

            //更新数据  
            window.hideUserSelectWin();
        }

        if (!data.iscreator) {
            $('.im_add_friend').hide();
            $('#btn_end').hide();
        }
        $('#lbl_group_name').html(chatroom.chatroomname);
        $('#txt_group_name').val(chatroom.chatroomname);
        $('#creator').html(chatroom.creator);
        $('#createdate').html(chatroom.createddate);
        $('#group_img').attr('src', (chatroom.memberscount === 1 ? '/images/pic/single.png' : '/images/pic/more.png'));
        data = null;
    };

    //发送消息
    function sendMsg(data) {
        var msg = { "message": data.message }, to = PrivateChatTo;
        data.serverdate = new Date();
        to && (msg.to = to);
        msg.clientdate = data.serverdate.format('yyyy/MM/dd hh:mm:ss');
        socket.emit('say', msg);
        showMsg(data);
        PrivateChatTo = '';
        $('#input_tips').html('----全体发送----');
        privateChat(false);
    }
    //房间不存在或者不在该房间时
    function roomIsExist(data) {
        if (data.isexist) {
            return true;
        }
        end_chat('不存在聊天群！');
        return false;
    }
    //房间是否终止聊天
    function roomIsEnd(data) {
        if (!data.isend) return true;
        end_chat();
        return false;
    }
    //终止聊天
    function end_chat(msg) {
        btn_hide();
        txt_msg.prop('disabled', true).css('text-align', 'center');
        txt_msg.val(msg || '\n当前对话已终止，不能发送消息');
    }
    //按钮隐藏
    function btn_hide() {
        $('.im_face').off();
        $('.im_add_friend').hide();
        $('#btn_send').hide();
        $('#sendImage').hide();
        $('#btn_end').hide();
    }
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
    //添加或者删除UserList
    function delAndaddUserlist(dels, adds) {
        hebin(dels, function (i, item) {
            (i > -1) && UserList.splice(i, 1);
        });
        hebin(adds, function (i, item) {
            (i == -1) && UserList.push(item);
        });
    };
    //合并
    function hebin(arr, operate) {
        var index = -1;
        if (arr && arr.length) {
            for (var i = 0, item; item = arr[i]; i++) {
                index = indexOf(UserList, function (j) {
                    return j.senderid === item.senderid;
                });
                operate(index, item);
            }
        }
    };

    //过滤
    function indexOf(array, filter) {
        if (array && array.length) {
            for (var i = 0, item; item = array[i]; i++) {
                if (filter(item)) return i;
            }
        }
        return -1;
    };

    //修改房间名称
    function modifyRoomName(id, name) {
        parent && parent.$('#tabContainer').find('li[con="' + id + '"]').html(name + '<i class="im_ico"></i>');
    };

    //父页面的关闭
    function parentClose(id) {
        if (parent) {
            var $$ = parent.$;
            $$('#tabContainer').find('li[con="' + id + '"] > .im_ico').click();
            $$(parent.document).off('tabchange' + roomid, dealScroll); //移出事件
        }
    };

    (function () {
        if (parent) {
            var $$ = parent.$;
            $$(parent.document).on('tabchange' + roomid, dealScroll);
        }
    }());

    //处理滚动条
    function dealScroll(e, id) {
        var msgContent = $('.im_record_list > ul');
        msgContent.parent().scrollTop(msgContent.height());
    };
});