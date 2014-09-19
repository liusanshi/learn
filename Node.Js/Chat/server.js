var websiteConfig = require('./config.js').websiteConfig;

var express = require('express');
var url = require('url');
var http = require('http');
var app = express();
//// 移动文件需要使用fs模块
var fs = require('fs');
var server = require('http').createServer(app);

var io = require('socket.io').listen(server);
var db_helper = require("./db_helper.js");
//var uuid = require('node-uuid');//v1();是生成一个时间戳uuid。 v4();是生成一个纯随机数uuid。

var clients = {};//存放所有的客户端
//var lostClients = {}; //存放断掉的客户端

var bodyParser = require('body-parser');
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

app.use(require('connect-multiparty')({ uploadDir: './www/upload' }));

//var log = require('./log');
//log.use(app);
//var logger = require('../log').logger;
//logger.info("this is log");
//specify the html we will use
app.use('/', express.static(__dirname + '/www'));

app.use('/file-upload', function (req, res) {
    // 获得文件的临时路径
    var tmp_path = req.files.sendImage.path;
    res.send('<script>parent.receiveMsg(\'' + tmp_path.substr(tmp_path.indexOf("\\") + 1).replace(/\\/g, "/") + '\')</script>');

    if (req.body.imgsrc != undefined && req.body.imgsrc != "") {
        fs.unlinkSync(tmp_path.substr(0, tmp_path.lastIndexOf("\\")).replace(/\\/g, "/") + "\\" + req.body.imgsrc.substr(req.body.imgsrc.lastIndexOf("/") + 1), function () { });
    }
    return;

    // 指定文件上传后的目录 - 示例为"images"目录。
    //var target_path = './www/upload/';
    //var fileName = uuid.v1() + tmp_path.substr(tmp_path.lastIndexOf("."));

    //// 移动文件
    //fs.rename(tmp_path, target_path + fileName, function (err) {
    //    if (err) throw err;
    //    // 删除临时文件夹文件
    //    fs.unlink(tmp_path, function () {
    //        if (err) res.send('<script>alert("图片上传失败！")</script>');
    //        res.send('<script>parent.receiveMsg("' + 'upload/' + fileName + '")</script>');
    //    });
    //});
});
function getResponse(res1, webpath, datatype, key) {
    http.get(webpath, function (res) {
        var xml = '';
        res.setEncoding('utf8');
        res.on('data', function (chunk) {
            xml += chunk;
        }).on('end', function () {
            var jsonResult = JSON.parse(xml);

            if (jsonResult.Success) {
                if (datatype == "json") {
                    if (key instanceof Function) {
                        res1.end(JSON.stringify(key(jsonResult.Data)));
                    }
                    else {
                        res1.end(JSON.stringify(jsonResult.Data));
                    }
                }
                else {
                    if (key == "") {
                        res1.end(jsonResult.Data);
                    }
                    else {
                        res1.end(jsonResult.Data[key]);
                    }
                }
            }
            else {
                if (datatype == "json") {
                    res1.end("[]");
                }
                else {
                    res1.end("");
                }
            }
        });
    }).on('error', function (err) {
        if (datatype == "json") {
            res1.end("[]");
        }
        else {
            res1.end("");
        }
    });
}

app.get('/selectuser/:action', function (req, res1) {
    var queryObj = url.parse(req.url, true).query;

    switch (req.params.action) {
        case "GetParticipants":
            var param = { participantid: queryObj.participantid };
            db_helper.GetParticipants(param, function (UserList) {
                res1.end(JSON.stringify(UserList));
            });
            break;
        case "GetChatRoomParticipants":
            var param = { chatroomid: queryObj.chatroomid, participantid: queryObj.participantid };
            db_helper.GetChatRoomParticipants(param, function (UserList) {
                res1.end(JSON.stringify(UserList));
            });
            break;
        case "Login":
            getResponse(res1, websiteConfig.CgZtbWeb.path + "WorkGroup/WorkGroupService.svc/Login?appKey=140AC56F-0A48-448B-9BAD-75296022FEEF", "text", "SessionID");
            break;
        case "QueryProviderType":
            getResponse(res1, websiteConfig.CgZtbWeb.path + "WorkGroup/WorkGroupService.svc/QueryProviderType?session=" + escape(queryObj.sessionID) + "&userGUID=" + escape(queryObj.userGUID), "json");
            break;
        case "QueryProviderContact":
            getResponse(res1, websiteConfig.CgZtbWeb.path + "WorkGroup/WorkGroupService.svc/QueryProviderContact?session=" + escape(queryObj.sessionID) + "&userGUID=" + escape(queryObj.userGUID) + "&providerTypeCode=" + escape(queryObj.providerTypeCode) + "&conditionText=" + escape(queryObj.conditionText), "json");
            break;
        case "QueryBusinessUnit":
            getResponse(res1, websiteConfig.CgZtbWeb.path + "WorkGroup/WorkGroupService.svc/QueryBusinessUnit?session=" + escape(queryObj.sessionID) + "&userGUID=" + escape(queryObj.userGUID), "json");
            break;
        case "QueryERPUser":
            getResponse(res1, websiteConfig.CgZtbWeb.path + "WorkGroup/WorkGroupService.svc/QueryERPUser?session=" + escape(queryObj.sessionID) + "&userGUID=" + escape(queryObj.userGUID) + "&businessUnitID=" + escape(queryObj.businessUnitID) + "&conditionText=" + escape(queryObj.conditionText), "json");
            break;
        case "QueryERPUserInfo":
            getResponse(res1, websiteConfig.CgZtbWeb.path + "WorkGroup/WorkGroupService.svc/QueryERPUserInfo?session=" + escape(queryObj.sessionID) + "&userGUID=" + escape(queryObj.userGUID), "json",
            function (data) {
                var me = {
                    id: data.ContactIdentifier,
                    name: data.ContactName,
                    companyname: "金隅集团",
                    deptname: data.DepartName,
                    iconsrc: data.UserImage,
                    isdeveloper: 1
                }
                return me;
            }
           );
            break;
        case "GetProviderName":
            getResponse(res1, websiteConfig.CgZtbWeb.path + "WorkGroup/WorkGroupService.svc/GetProviderName?session=" + escape(queryObj.sessionID) + "&enterpriseCode=" + escape(queryObj.enterpriseCode), "text", "");
            break;
        case "QueryUserInfo":
            db_helper.QueryUserInfo({ participantid: queryObj.participantid }, function (val) {
                res1.end(JSON.stringify(val));
            });
            break;
        default:
            break;
    }
});

app.post('/selectuser/:action', function (req, res1) {
    var queryObj = url.parse(req.url, true).query;

    switch (req.params.action) {
        case "GetProviderContacts":
            var param = { contacterids: req.body.contacterids };
            db_helper.GetProviderContacts(param, function (UserList) {
                res1.end(JSON.stringify(UserList));
            });
            break;
        case "QueryERPUserDetail":
            var opt = {
                method: 'POST',
                host: websiteConfig.CgZtbWeb.host,
                port: websiteConfig.CgZtbWeb.port,
                path: "/WorkGroup/WorkGroupService.svc/QueryERPUserDetail?session=" + escape(queryObj.session) + "&userGUID=" + escape(queryObj.userGUID),
                headers: {
                    "Content-Type": 'application/json'
                }
            }

            var body = '';
            var req2 = http.request(opt, function (res2) {
                res2.setEncoding('utf8');
                res2.on('data', function (d) {
                    body += d;
                }).on('end', function () {
                    res1.end(body);
                });
            }).on('error', function (e) {
                res1.end('{Success:false}');
            });

            req2.write(JSON.stringify({ erpContactIdentifiers: req.body.erpContactIdentifiers }));
            req2.end();
            break;
        default:
            break;
    }
});

app.post('/card/:action', function (req, res1) {
    switch (req.params.action) {
        case "get":
            db_helper.GetProviderContact([req.body.openid], function (result) {
                res1.end(JSON.stringify(result));
            });
            break;
        case "save":
            var ProviderContactData = {
                identifier: req.body.identifier,
                contactName: req.body.contactName,
                jobPosition: req.body.jobPosition,
                email: req.body.email,
                mobile: req.body.mobile,
                tel: req.body.tel,
                imgsrc: escape(req.body.imgsrc),
                enterprisecode: req.body.enterpriseCode,
                companyname: req.body.companyname
            };

            var data = {
                identifier: req.body.identifier,
                contactName: req.body.contactName,
                jobPosition: req.body.jobPosition,
                email: req.body.email,
                mobile: req.body.mobile,
                tel: req.body.tel,
                fax: req.body.fax,
                gender: req.body.gender
            };

            data = JSON.stringify(data);

            var sAction = "Add";
            if (req.body.isSaved == "true") {
                sAction = "Update";
            }

            var opt = {
                method: 'POST',
                host: websiteConfig.CgZtbWeb.host,
                port: websiteConfig.CgZtbWeb.port,
                path: "/WorkGroup/WorkGroupService.svc/" + sAction + "ProviderContact?session=" + escape(req.body.sessionID) + "&enterpriseCode=" + escape(req.body.enterpriseCode),
                headers: {
                    "Content-Type": 'application/json'
                }
            }

            var body = '';
            var req = http.request(opt, function (res) {
                res.setEncoding('utf8');
                res.on('data', function (d) {
                    body += d;
                }).on('end', function () {
                    var jsonResult = JSON.parse(body);

                    if (jsonResult.Success) {
                        db_helper.SaveProviderContact(ProviderContactData, function (result) {
                            if (result.Success) {
                                res1.end('保存成功！');
                            }
                            else {
                                res1.end('保存失败！');
                            }
                        });
                    }
                    else {
                        res1.end('保存失败！');
                    }
                });
            }).on('error', function (e) {
                res1.end('保存失败！');
            });

            req.write(data);
            req.end();
            break;
        default:
            break;
    }
});

app.use('/GetChatRoom', function (req, res) {
    var queryObj = url.parse(req.url, true).query;
    db_helper.GetChatRoom({ chatroomid: queryObj.chatroomid, participantid: queryObj.participantid }, function (val) {
        res.end(JSON.stringify(val));
    });
});

app.use('/CreateChatRoom', function (req, res) {
    var tmpdata = req.body;
    var isSuccess = true;
    var queryObj = url.parse(req.url, true).query;
    if (queryObj.isinner != "1") {
        var erpusers = [];
        var otherusers = [];
        for (i = 0; i < tmpdata.participants.length; i++) {
            if (tmpdata.participants[i].isdeveloper == "1") {
                erpusers.push(tmpdata.participants[i].id);
            }
            else {
                otherusers.push(tmpdata.participants[i]);
            }
        }

        if (erpusers.length > 0) {
            http.get(websiteConfig.CgZtbWeb.path + "WorkGroup/WorkGroupService.svc/Login?appKey=140AC56F-0A48-448B-9BAD-75296022FEEF", function (res1) {
                var xml = '';
                res1.setEncoding('utf8');
                res1.on('data', function (chunk) {
                    xml += chunk;
                }).on('end', function () {
                    var jsonResult = JSON.parse(xml);

                    if (jsonResult.Success) {
                        var SessionID = jsonResult.Data.SessionID;
                        var opt = {
                            method: 'POST',
                            host: websiteConfig.CgZtbWeb.host,
                            port: websiteConfig.CgZtbWeb.port,
                            path: "/WorkGroup/WorkGroupService.svc/QueryERPUserDetail?session=" + escape(SessionID) + "&userGUID=" + escape(tmpdata.creater.id),
                            headers: {
                                "Content-Type": 'application/json'
                            }
                        }

                        var body = '';
                        var req = http.request(opt, function (res2) {
                            res2.setEncoding('utf8');
                            res2.on('data', function (d) {
                                body += d;
                            }).on('end', function () {
                                 var jsonResult = JSON.parse(body);

                                if (jsonResult.Success) {
                                    var objdata = jsonResult.Data;
                                    for (i = 0; i < objdata.length; i++) {
                                        var userInfo = objdata[i];
                                        var obj = {
                                            id: userInfo.ContactIdentifier,
                                            name: userInfo.ContactName,
                                            companyname: "金隅集团",
                                            deptname: userInfo.DepartName,
                                            iconsrc: userInfo.UserImage,
                                            isdeveloper: 1
                                        }

                                        otherusers.push(obj);
                                    }
                                    
                                    var data1 = tmpdata;

                                    data1.participants = otherusers;

                                    db_helper.CreateChatRoom(data1, function (val) {
                                        if (val) {
                                            res.end(JSON.stringify(val));
                                        }
                                        else {
                                            res.end('{"chatroomid":""}');
                                        }
                                    });
                                }
                                else {
                                    res.end('{"chatroomid":""}');
                                }
                            });
                        }).on('error', function (e) {
                            res.end('{"chatroomid":""}');
                        });

                        req.write(JSON.stringify({ erpContactIdentifiers: erpusers }));
                        req.end();
                    }
                    else {
                        res.end('{"chatroomid":""}');
                    }
                });
            }).on('error', function (err) {
                res.end('{"chatroomid":""}');
            });
        }
        else {
            db_helper.CreateChatRoom(tmpdata, function (val) {
                if (val) {
                    res.end(JSON.stringify(val));
                }
                else {
                    res.end('{"chatroomid":""}');
                }
            });
        }
    }
    else {
        db_helper.CreateChatRoom(tmpdata, function (val) {
            if (val) {
                res.end(JSON.stringify(val));
            }
            else {
                res.end('{"chatroomid":""}');
            }
        });
    }
});

app.use('/GetChatRoomDetails', function (req, res) {
    var queryObj = url.parse(req.url, true).query;

    db_helper.GetChatRoomDetails({ participantid: queryObj.participantid, CgZtbWebSite: websiteConfig.CgZtbWeb.path, mywebsite: websiteConfig.mysite }, function (val) {
        res.end(queryObj.callback + '(' + JSON.stringify(val) + ')');
    });

});

app.use('/GetChatRoomDetails2', function (req, res) {
    var queryObj = url.parse(req.url, true).query;

    db_helper.GetChatRoomDetails2({ participantid: queryObj.participantid, page: queryObj.page, CgZtbWebSite: websiteConfig.CgZtbWeb.path, mywebsite: websiteConfig.mysite }, function (val) {
        res.end(JSON.stringify(val));
    });

});

app.use('/DeleteChatRooms', function (req, res) {
    var queryObj = url.parse(req.url, true).query;

    db_helper.DeleteChatRooms({ senderid: queryObj.senderid, chatroomids: queryObj.chatroomids }, function (val) {
        res.end(JSON.stringify(val));
    });

});

//获取房间与用户信息
function getRoomInfo(roomid, senderid, cb) {
    var param = { chatroomid: roomid, participantid: senderid };
    db_helper.GetChatRoomParticipants(param, function (UserList) {
        db_helper.GetChatRoom(param, function (senderinfo) {
            cb({
                UserList: UserList, isexist: senderinfo.isexist, iscreator: senderinfo.iscreator, isend: senderinfo.isend
        , chatroom: senderinfo.chatroom, me: senderinfo.me
            });
        });
    });
};

//获取历史聊天记录
function getHistory(roomid, senderid, cb) {
    var param = { chatroomid: roomid, participantid: senderid };
    db_helper.GetRecords(param, function (h) {
        cb(h);
    });
}

server.listen(process.env.PORT || 5858);//publish to heroku

//handle the socket
io.sockets.on('connection', function (socket) {
    var curInfo = {};

    //获取房间信息
    socket.on('get_info', function (data) {
        var roomid = data.roomid, senderid = data.senderid;
        getRoomInfo(roomid, senderid, function (info) {
            var curuser = info.me;
            curInfo.roomid = roomid;
            curInfo.senderid = senderid;
            curInfo.UserList = info.UserList;
            if (curuser) {
                curuser.senderid = senderid;
                curuser.sendername = curuser.name;
                curuser.roomid = roomid;

                curInfo.iconsrc = curuser.iconsrc || '';
                curInfo.sendername = curuser.sendername || '';
                curInfo.companyname = curuser.companyname || '';
                curInfo.deptname = curuser.deptname || '';
                curInfo.isdeveloper = curuser.isdeveloper;
            }

            if (!info.isend) {//没有结束，就将该人放入房间
                ready_chat(data);
            }

            info.completedWebsite = websiteConfig.CgZtbWeb.path;//开发商的服务器地址

            //历史记录
            if (info.isexist) {
                getHistory(roomid, senderid, function (h) {
                    info.history = h;
                    socket.emit('get_info', info);
                });
            } else {
                info.history = [];
                socket.emit('get_info', info);
            }
        });
    });

    //发送给一部分人
    socket.on('say', function (data) {
        var userList, client, roomid = curInfo.roomid, to = data.to, msg = data.message, issendmsg = false
            , message = {
                message: msg, serverdate: new Date().getTime(), clientdate: data.clientdate, senderid: curInfo.senderid, iconsrc: curInfo.iconsrc
                , sendername: curInfo.sendername, companyname: curInfo.companyname, isdeveloper: curInfo.isdeveloper
            };
        if (curInfo.isdeveloper) { //发送消息方的部门信息
            message.deptname = curInfo.deptname;
        }
        if (to) {//私聊
            message.isprivate = true;
            message.receiverid = to;
            sayToOne(to + '@' + roomid, message);
            issendmsg = true;
        } else if (curInfo.isdeveloper) {//开发商
            message.isprivate = false;
            socket.broadcast.to(roomid).emit('say', message);
            issendmsg = true;
        } else if (!curInfo.isdeveloper && curInfo.UserList) {//供应商
            userList = curInfo.UserList;
            message.isprivate = false;
            for (var i = 0, len = userList.length; i < len; i++) {
                if (userList[i].isdeveloper) {
                    sayToOne(userList[i].senderid + '@' + roomid, message);
                    issendmsg = true;
                }
            }
        }

        //保存聊天记录
        issendmsg && setTimeout(function () {
            //return;
            SaveChatRecord(curInfo, to, message);
        }, 0);
    });

    //更新参与者记录
    socket.on('UpdateParticipantState', function (serverdata) {
        //return;
        db_helper.UpdateParticipantState({ chatroomid: curInfo.roomid, senderid: curInfo.senderid, serverdate: serverdata });
    });

    //更新参与者
    socket.on('UpdateParticipants', function (data) {
        data.chatroomid = curInfo.roomid;
        data.senderid = curInfo.senderid;
        noticeleave(curInfo.roomid, data.deleteparticipants);
        //return;
        db_helper.UpdateParticipants(data);
    });

    //终止聊天
    socket.on('end_chat', function () {
        //io.sockets.in(curInfo.roomid).emit('end_chat', { success: true });
        //return;
        //修改数据库标识为终止
        db_helper.EndChatRoom({ chatroomid: curInfo.roomid, senderid: curInfo.senderid }, function () {
            io.sockets.in(curInfo.roomid).emit('end_chat', { success: true });
        });
    });

    socket.on('updategroupname', function (data) {
        //io.sockets.in(curInfo.roomid).emit('updategroupname', data.groupname);
        //return;
        //修改数据库中的群名称
        db_helper.UpdateChatRoomName({ chatroomname: data.groupname, chatroomid: data.roomid }, function () {
            io.sockets.in(curInfo.roomid).emit('updategroupname', data.groupname);
        });
    });

    //断掉链接的时候删除数据
    socket.on('disconnect', function () {
        var key = getCurClientKey(), client = clients[key];
        if (client) {
            for (var i = 0, item; item = client[i]; i++) {
                if (socket.id === item.socket.id) {
                    client.splice(i--, 1);
                }
            }
        }
        socket.leave(curInfo.roomid);//离开房间

        //userlist\更新在线状态
        //updateUserList(curInfo.roomid);
    });

    //聊天前的准备
    function ready_chat(data) {
        var roomid = data.roomid, senderid = data.senderid, key = senderid + '@' + roomid;
        //if (!clients[senderid + '@' + roomid]) { } //不在发送列表中

        //在获取的时候，将他加入到制定的房间
        socket.join(roomid);
        (clients[key] || (clients[key] = [])).push({
            senderid: senderid,
            socket: socket,
            roomid: roomid
        });

        //delete lostClients[getCurClientKey()]; //将该客户端标识为上线
        //userlist\更新在线状态
        //updateUserList(roomid);
    };
    //给某个人发送消息
    function sayToOne(key, msg) {
        var client = clients[key];
        if (client) {
            for (var i = 0, item; item = client[i]; i++) {
                if (item.socket) {
                    item.socket.emit('say', msg);
                }
            }
        }
    };

    //更新在线用户状态
    function updateUserList(room) {
        var userList = [];
        for (i in clients) {
            if (clients[i].length && clients[i][0].room === room) {
                userList.push(clients[i][0].senderid);
            }
        }
        io.sockets.in(room).emit('userlist', userList);
    };

    //踢出群通知
    function noticeleave(rommid, leaves) {
        var currid, curuser;
        for (var i = 0, item; item = leaves[i]; i++) {
            currid = item.senderid + '@' + rommid;
            curuser = clients[currid];
            if (curuser) {
                for (var j = 0, u; u = curuser[j]; j++) {
                    u.socket.emit('leaveroom', '');
                }
            }
            delete clients[currid];
        }
    }

    //查找当前的用户
    function findCurUser(userlist, senderid) {
        if (userlist) {
            for (var i = 0, len = userlist.length; i < len; i++) {
                if (userlist[i].senderid === senderid)
                    return userlist[i];
            }
        }
    }
    //获取当前用户的key
    function getCurClientKey() {
        return curInfo.senderid + '@' + curInfo.roomid;
    }
    //保存聊天记录
    function SaveChatRecord(from, to, message) {
        if (from.roomid) {
            var user = findCurUser(from.UserList, from.senderid), chat = {
                chatroomid: from.roomid,
                senderid: from.senderid,
                sendername: from.sendername,
                sendercompanyname: getUserPartInfo(from),
                messagecontent: message.message,
                clientdate: message.clientdate,
                serverdate: message.serverdate
            };
            user && (chat.sendername = user.sendername);
            if (to) {
                chat.receiverid = to;
                user = findCurUser(from.UserList, to);
                if (user) {//存储接受方的信息
                    chat.receivercompanyname = getUserPartInfo(user);
                    chat.receivername = user.sendername;
                }
            }

            db_helper.SaveChatRecord(chat);
        }
    }

    //获取用户的附加描述
    function getUserPartInfo(user) {
        return user.isdeveloper ? (user.deptname) : (user.companyname);
    }
});