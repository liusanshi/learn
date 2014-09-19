var uuid = require('node-uuid');//v1();是生成一个时间戳uuid。 v4();是生成一个纯随机数uuid。
var mysql = require('mysql');
var db_config = require('./config.js').db_config;

var sqlpool = mysql.createPool(db_config);

function execSQL(sql, data, callback) {
    sqlpool.getConnection(function (err, connection) {
        if (err) {
            if (callback) {
                callback(err);
            }
        }
        else {
            connection.query(sql, data, function (err1, results) {
                connection.release();
                if (callback) {
                    if (err1) {
                        callback(err1);
                    }
                    else {
                        callback(err1, results);
                    }
                }
            });
        }
    });
}

//1、	创建聊天室（CreateChatRoom）
exports.CreateChatRoom = function (data, callback) {
    var isSuccess = true;
    var token = "";
    var dataParams = [];
    var sql = "";
    var now = new Date();
    var ChatRoomName = "";

    token = uuid.v1();
    ChatRoomName = data.chatroomname;
    if (ChatRoomName == "") {
        ChatRoomName = "讨论组（" + data.participants.length + "人）";
    }
    //聊天室主记录   
    if (data.ywguid != "") {
        sql = "insert into chatroominfo(ChatRoomID, ChatRoomName, CreatorID, CreatedDate, MembersCount, BusinessID, BusinessType, LastRecordDate, IsEnd) select ?,?,?,?,?,?,?,?,0 from dual where not exists(select ChatRoomID,CreatorID,ChatRoomName from chatroominfo where BusinessID=? and BusinessType=?);";
        dataParams.push(token, ChatRoomName, data.creater.id, now, data.participants.length, data.ywguid, data.ywtype, now.getTime(), data.ywguid, data.ywtype);

        sql += "select @ChatRoomID:=ChatRoomID,@CreatorID:=CreatorID,@ChatRoomName:=ChatRoomName,@IsAdmin:=case when CreatorID=? then 1 else 0 end from chatroominfo where BusinessID=? and BusinessType=?;";
        dataParams.push(data.creater.id, data.ywguid, data.ywtype);

        sql += "insert into chatparticipantinfo(ChatRoomID, ParticipantID, IsAdmin, IsDeveloper, JoinDate, LastReveiveDate, IsDelete, DeleteDate, IsShow) select @ChatRoomID,?,@IsAdmin,?,?,?,0,null,1 from dual where not exists(select ChatRoomID from chatparticipantinfo a where a.ChatRoomID=@ChatRoomID and a.ParticipantID=? and a.id<>-1);"
        dataParams.push(data.creater.id, data.creater.isdeveloper, now.getTime(), now.getTime(), data.creater.id);

        sql += "update chatcontactersinfo set ContacterIsDeveloper=? where OwnerID=@CreatorID and ContacterID=? and id<>-1 and @IsAdmin=0;";
        dataParams.push(data.creater.isdeveloper, data.creater.id);

        sql += "insert into chatcontactersinfo(OwnerID, ContacterID, ContacterIsDeveloper) ";
        sql += " select @CreatorID,?,? from dual ";
        sql += "where not exists(select ContacterID from chatcontactersinfo a where a.OwnerID=@CreatorID and a.ContacterID=? and a.id<>-1) and @IsAdmin=0;";
        dataParams.push(data.creater.id, data.creater.isdeveloper, data.creater.id);

        sql += "REPLACE into chatcontacterdetails(ContacterID, ContacterName, ContacterCompanyName, ContacterDeptName, IconSrc)values(?,?,?,?,?);";
        dataParams.push(data.creater.id, data.creater.name, data.creater.companyname, data.creater.deptname, data.creater.iconsrc);

        //成员信息处理
        for (var i = 0; i < data.participants.length; i++) {
            sql += "insert into chatparticipantinfo(ChatRoomID, ParticipantID, IsAdmin, IsDeveloper, JoinDate, LastReveiveDate, IsDelete, DeleteDate, IsShow) select @ChatRoomID,?,0,?,?,?,0,null,1 from dual where not exists(select ChatRoomID from chatparticipantinfo a where a.ChatRoomID=@ChatRoomID and a.ParticipantID=? and a.id<>-1) and  @CreatorID!= ?;"
            dataParams.push(data.participants[i].id, data.participants[i].isdeveloper, now.getTime(), now.getTime(), data.participants[i].id, data.participants[i].id);

            sql += "update chatcontactersinfo set ContacterIsDeveloper=? where OwnerID=@CreatorID and ContacterID=? and id<>-1 and  @CreatorID!= ?;";
            dataParams.push(data.participants[i].isdeveloper, data.participants[i].id, data.participants[i].id);

            sql += "insert into chatcontactersinfo(OwnerID, ContacterID, ContacterIsDeveloper) ";
            sql += " select @CreatorID,?,? from dual ";
            sql += "where not exists(select ContacterID from chatcontactersinfo a where a.OwnerID=@CreatorID and a.ContacterID=? and a.id<>-1)  and  @CreatorID!= ?;";

            dataParams.push(data.participants[i].id, data.participants[i].isdeveloper, data.participants[i].id, data.participants[i].id);

            if (data.participants[i].isdeveloper == 1 || data.participants[i].isdeveloper == "1") {
                sql += "REPLACE into chatcontacterdetails(ContacterID, ContacterName, ContacterCompanyName, ContacterDeptName, IconSrc)values(?,?,?,?,?);";
                dataParams.push(data.participants[i].id, data.participants[i].name, data.participants[i].companyname, data.participants[i].deptname, data.participants[i].iconsrc);
            }
        }

        sql += "select ChatRoomID,CreatorID,ChatRoomName from chatroominfo where BusinessID=? and BusinessType=?";
        dataParams.push(data.ywguid, data.ywtype);

        execSQL(sql, dataParams, function (err, results) {
            if (err instanceof Error) { callback(null); }
            if (results.length > 0 && results[results.length - 1] && results[results.length - 1].length && results[results.length - 1].length == 1 && results[results.length - 1][0].ChatRoomID) {
                callback({ chatroomid: results[results.length - 1][0].ChatRoomID, chatroomname: results[results.length - 1][0].ChatRoomName });
            }
            else {
                callback(null);
            }
        });
    }
    else {
        sql = "insert into chatroominfo(ChatRoomID, ChatRoomName, CreatorID, CreatedDate, MembersCount, BusinessID, BusinessType, LastRecordDate, IsEnd)values(?,?,?,?,?,?,?,?,0);";
        dataParams.push(token, ChatRoomName, data.creater.id, now, data.participants.length, data.ywguid, data.ywtype, now.getTime());

        //创建人信息处理
        sql += "insert into chatparticipantinfo(ChatRoomID, ParticipantID, IsAdmin, IsDeveloper, JoinDate, LastReveiveDate, IsDelete, DeleteDate, IsShow)values(?,?,1,?,?,?,0,null,1);"
        dataParams.push(token, data.creater.id, data.creater.isdeveloper, now.getTime(), now.getTime());
        sql += "REPLACE into chatcontacterdetails(ContacterID, ContacterName, ContacterCompanyName, ContacterDeptName, IconSrc)values(?,?,?,?,?);";
        dataParams.push(data.creater.id, data.creater.name, data.creater.companyname, data.creater.deptname, data.creater.iconsrc);

        //成员信息处理
        for (var i = 0; i < data.participants.length; i++) {
            sql += "insert into chatparticipantinfo(ChatRoomID, ParticipantID, IsAdmin, IsDeveloper, JoinDate, LastReveiveDate, IsDelete, DeleteDate, IsShow)values(?,?,0,?,?,?,0,null,1);"
            dataParams.push(token, data.participants[i].id, data.participants[i].isdeveloper, now.getTime(), now.getTime());

            sql += "update chatcontactersinfo set ContacterIsDeveloper=? where OwnerID=? and ContacterID=? and id<>-1;";
            dataParams.push(data.participants[i].isdeveloper, data.creater.id, data.participants[i].id);

            sql += "insert into chatcontactersinfo(OwnerID, ContacterID, ContacterIsDeveloper) ";
            sql += " select ?,?,? from dual ";
            sql += "where not exists(select ContacterID from chatcontactersinfo a where a.OwnerID=? and a.ContacterID=? and a.id<>-1);";

            dataParams.push(data.creater.id, data.participants[i].id, data.participants[i].isdeveloper, data.creater.id, data.participants[i].id);

            if (data.participants[i].isdeveloper == 1 || data.participants[i].isdeveloper == "1") {

                sql += "REPLACE into chatcontacterdetails(ContacterID, ContacterName, ContacterCompanyName, ContacterDeptName, IconSrc)values(?,?,?,?,?);";
                dataParams.push(data.participants[i].id, data.participants[i].name, data.participants[i].companyname, data.participants[i].deptname, data.participants[i].iconsrc);
            }
        }

        execSQL(sql, dataParams, function (err, results) {
            if (err instanceof Error) callback(null);
            callback({ chatroomid: token, chatroomname: ChatRoomName });
        });
    }
}

//2、	是否存在聊天室, 是否聊天室创建人,聊天人信息（GetChatRoom）
exports.GetChatRoom = function (data, callback) {
    var dataParams = [];

    var sql = "SELECT a.IsAdmin iscreator,case when a.IsDelete=1 then 1 else b.IsEnd end isend, b.chatroomname chatroomname,b.MembersCount memberscount," +
       "d.ContacterName creator,DATE_FORMAT(b.CreatedDate,'%Y/%m/%d %k:%i')  createddate, a.ParticipantID id,c.ContacterName name," +
       "IFNULL(c.ContacterCompanyName,'') companyname, IFNULL(c.ContacterDeptName,'') deptname,  " +
       "c.iconsrc iconsrc,a.IsDeveloper isdeveloper " +
       "FROM chatparticipantinfo a inner join chatroominfo b " +
       "on a.ChatRoomID=b.ChatRoomID and a.ChatRoomID=? and a.ParticipantID=? " +
       "inner join chatcontacterdetails c on a.ParticipantID=c.ContacterID " +
       "left join chatcontacterdetails d on b.creatorid=d.ContacterID;";

    dataParams.push(data.chatroomid, data.participantid);

    execSQL(sql, dataParams, function (err, results) {
        if (err instanceof Error) callback({ isexist: 0 });
        else {
            result = {};
            if (results.length > 0) {
                result = {
                    isexist: 1, iscreator: results[0].iscreator, isend: results[0].isend, chatroom: { chatroomname: results[0].chatroomname, memberscount: results[0].memberscount, creator: results[0].creator, createddate: results[0].createddate },
                    me: {
                        id: results[0].id,
                        name: results[0].name,
                        companyname: results[0].companyname,
                        deptname: results[0].deptname,
                        iconsrc: results[0].iconsrc,
                        isdeveloper: results[0].isdeveloper
                    }
                };
            }
            else {
                result = { isexist: 0 };
            }
            callback(result);
        }
    });
}

//3、	当前聊天室当前用户联系人列表（GetChatRoomParticipants）
exports.GetChatRoomParticipants = function (data, callback) {
    var dataParams = [];

    var sql = "SELECT a.ParticipantID senderid,c.ContacterName sendername," +
        "IFNULL(c.ContacterCompanyName,'') companyname,IFNULL(c.ContacterDeptName,'') deptname" +
        ",case when c.iconsrc is null or c.iconsrc='' then (case when a.IsDeveloper=1 then '/images/global/chat_icon0.png' else 'images/pic/normal.png' end) else c.iconsrc end iconsrc,a.IsDeveloper isdeveloper " +
        "FROM chatparticipantinfo  a  inner join " +
        "chatcontacterdetails c on a.ParticipantID=c.ContacterID " +
        "and a.ChatRoomID=? " +
        "where exists(select 1 from chatparticipantinfo  a where a.ChatRoomID=? " +
        "and a.ParticipantID=? and a.isdelete=0) and a.isdelete=0 and a.isshow=1 and a.ParticipantID<>?  " +
        "and (exists(select 1 from chatparticipantinfo where ChatRoomID=? " +
        "and ParticipantID=? and IsDeveloper=1) or a.IsDeveloper=1);";

    dataParams.push(data.chatroomid, data.chatroomid, data.participantid, data.participantid, data.chatroomid, data.participantid);

    execSQL(sql, dataParams, function (err, results) {
        if (err instanceof Error) { callback([]); }
        else
        {
            callback(results);
        }
    });
}

//4、	保存会话记录（SaveChatRecord）
exports.SaveChatRecord = function (data, callback) {
    var dataParams = [];

    var sql = "insert into chatrecord(ChatRoomID, SenderID, SenderName, SenderCompanyName, ReceiverName, ReceiverID, MessageContent, ClientDate, ServerDate, ReceiverCompanyName)values(?,?,?,?,?,?,?,?,?,?);"
    dataParams.push(data.chatroomid, data.senderid, data.sendername, data.sendercompanyname, data.receivername, data.receiverid, data.messagecontent, new Date(data.clientdate), data.serverdate, data.receivercompanyname);

    sql += "update chatparticipantinfo set LastReveiveDate=? where ChatRoomID=? and ParticipantID=? and LastReveiveDate<?;"
    dataParams.push(data.serverdate, data.chatroomid, data.senderid, data.serverdate);

    sql += "update chatroominfo set LastRecordDate=? where ChatRoomID=? and LastRecordDate<?;"
    dataParams.push(data.serverdate, data.chatroomid, data.serverdate);

    execSQL(sql, dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback(err); }
            else
            {
                callback({});
            }
        }
    });
}

//5、	更新当前用户的状态(UpdateParticipantState)
exports.UpdateParticipantState = function (data, callback) {
    var dataParams = [];

    var sql = "update chatparticipantinfo set LastReveiveDate=? where ChatRoomID=? and ParticipantID=? and LastReveiveDate<?;"
    dataParams.push(data.serverdate, data.chatroomid, data.senderid, data.serverdate);

    execSQL(sql, dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback({}); }
            else
            {
                callback({});
            }
        }
    });
}

//6、	添加联系人(UpdateParticipants)
exports.UpdateParticipants = function (data, callback) {
    var now = new Date();
    var dataParams = [];
    var sql = "";

    //成员信息处理
    for (var i = 0; i < data.deleteparticipants.length; i++) {
        sql += "update chatparticipantinfo set isdelete=1,deletedate=? where ChatRoomID=? and ParticipantID=?;"
        dataParams.push(now.getTime(), data.chatroomid, data.deleteparticipants[i].senderid);
    }

    //成员信息处理
    for (var i = 0; i < data.addparticipants.length; i++) {
        sql += "update chatparticipantinfo set isdelete=0,deletedate=null where ChatRoomID=? and ParticipantID=? and id<>-1;";
        dataParams.push(data.chatroomid, data.addparticipants[i].senderid);

        sql += "insert into chatparticipantinfo(ChatRoomID, ParticipantID, IsAdmin, IsDeveloper, JoinDate, LastReveiveDate, IsDelete, DeleteDate, IsShow)select ?,?,0,?,?,?,0,null,1 from dual where not exists(select ChatRoomID from  chatparticipantinfo where ChatRoomID=? and ParticipantID=? and id<>-1);"
        dataParams.push(data.chatroomid, data.addparticipants[i].senderid, data.addparticipants[i].isdeveloper, now.getTime(), now.getTime(), data.chatroomid, data.addparticipants[i].senderid);

        sql += "update chatcontactersinfo set ContacterIsDeveloper=? where OwnerID=? and ContacterID=? and id<>-1;";
        dataParams.push(data.addparticipants[i].isdeveloper, data.senderid, data.addparticipants[i].senderid);

        sql += "insert into chatcontactersinfo(OwnerID, ContacterID, ContacterIsDeveloper) ";
        sql += " select ?,?,? from dual ";
        sql += "where not exists(select ContacterID from chatcontactersinfo a where a.OwnerID=? and a.ContacterID=? and a.id<>-1);";
        dataParams.push(data.senderid, data.addparticipants[i].senderid, data.addparticipants[i].isdeveloper, data.senderid, data.addparticipants[i].senderid);

        if (data.addparticipants[i].isdeveloper == 1 || data.addparticipants[i].isdeveloper == "1") {
            sql += "REPLACE into chatcontacterdetails(ContacterID, ContacterName, ContacterCompanyName, ContacterDeptName, IconSrc)values( ?,?,?,?,?);";
            dataParams.push(data.addparticipants[i].senderid, data.addparticipants[i].sendername, data.addparticipants[i].companyname, data.addparticipants[i].deptname, data.addparticipants[i].iconsrc);
        }
    }

    sql += "update chatroominfo set MembersCount=? where ChatRoomID=? and CreatorID=?;";
    dataParams.push(data.selCnt, data.chatroomid, data.senderid);

    execSQL(sql, dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback({}); }
            else
            {
                callback({});
            }
        }
    });
}

//7、	终止会话(EndChatRoom)
exports.EndChatRoom = function (data, callback) {
    var dataParams = [];
    var sql = "update chatroominfo set isend=1 where ChatRoomID=? and CreatorID=?;";
    dataParams.push(data.chatroomid, data.senderid);

    execSQL(sql, dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback({ issuccess: 0 }); }
            else
            {
                callback({ issuccess: 1 });
            }
        }
    });
}

//8、	获取沟通记录概要信息（GetChatRoomDetails）
exports.GetChatRoomDetails = function (data, callback) {
    var dataParams = [];
    var sql = [];
    sql.push("SELECT a1.chatroomid, a1.chatroomname, CASE WHEN MembersCount > 1 THEN 1 ELSE 0 END ChatType,");
    sql.push("case when a1.MembersCount = 1 then (case when a4.isdeveloper=1 then concat(trim('", data.CgZtbWebSite, "'),(case when  a5.iconsrc is null or a5.iconsrc ='' then '/images/global/chat_icon0.png' else a5.iconsrc end)) else concat(trim('", data.mywebsite, "'),(case when  a5.iconsrc is null or a5.iconsrc ='' then 'images/pic/normal.png' else a5.iconsrc end)) end) else '' end  as iconsrc,");
    sql.push("case when  a2.IsDelete=0 and a1.LastRecordDate>a2.LastReveiveDate  ");
    sql.push("then (SELECT count(1) from chatrecord r where r.chatroomid = a1.ChatRoomID AND r.ServerDate > a2.LastReveiveDate  and r.ServerDate>=a2.joindate ");
    sql.push(" and (r.senderid=a2.ParticipantID or r.ReceiverID=a2.ParticipantID Or (r.ReceiverID is null and (a2.isdeveloper=1  or ");
    sql.push(" (a2.isdeveloper=0 and exists(select * from chatparticipantinfo aa where a1.ChatRoomID = aa.ChatRoomID and r.senderid=aa.ParticipantID and aa.isdeveloper=1)))))) ");
    sql.push("when a2.IsDelete=1 and a1.LastRecordDate>a2.LastReveiveDate ");
    sql.push("then (SELECT count(1) from chatrecord r where r.chatroomid = a1.ChatRoomID AND r.ServerDate > a2.LastReveiveDate and r.ServerDate>=a2.joindate  ");
    sql.push("and r.ServerDate < a2.deleteDate  and (r.senderid=a2.ParticipantID or r.ReceiverID=a2.ParticipantID Or (r.ReceiverID is null and (a2.isdeveloper=1 ");
    sql.push("   or (a2.isdeveloper=0 and exists(select * from chatparticipantinfo aa where a1.ChatRoomID = aa.ChatRoomID and r.senderid=aa.ParticipantID and aa.isdeveloper=1)))))) ");
    sql.push("else 0 end as chatnotreadcount ");
    sql.push("FROM chatroominfo a1 ");
    sql.push("inner JOIN chatparticipantinfo a2 ");
    sql.push("ON a1.ChatRoomID = a2.ChatRoomID ");
    sql.push("left JOIN chatparticipantinfo a4 ");
    sql.push("ON a1.MembersCount = 1 and a1.ChatRoomID = a4.ChatRoomID and a4.ParticipantID!=?  ");
    sql.push("left JOIN chatcontacterdetails a5 ");
    sql.push("ON a4.ParticipantID= a5.ContacterID ");
    sql.push("WHERE a2.IsShow = 1 AND a2.ParticipantID = ? ");
    sql.push(" ORDER BY a1.LastRecordDate DESC ");
    sql.push(" LIMIT 18;");

    dataParams.push(data.participantid, data.participantid);

    execSQL(sql.join(" "), dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback([]); }
            else
            {
                callback(results);
            }
        }
    });
}

//9、	获取沟通记录概要信息2（GetChatRoomDetails2）
exports.GetChatRoomDetails2 = function (data, callback) {
    var dataParams = [];
    var sql = [];
    sql.push("SELECT a1.chatroomid,a1.chatroomname, CASE WHEN MembersCount > 1 THEN 1 ELSE 0 END ChatType,");
    sql.push("case when a1.MembersCount = 1 then (case when a4.isdeveloper=1 then concat(trim('", data.CgZtbWebSite, "'),(case when  a5.iconsrc is null or a5.iconsrc ='' then '/images/global/chat_icon0.png' else a5.iconsrc end)) else concat(trim('", data.mywebsite, "'),(case when  a5.iconsrc is null or a5.iconsrc ='' then 'images/pic/normal.png' else a5.iconsrc end)) end) else '' end  as iconsrc,");
    sql.push("case when  a2.IsDelete=0 and a1.LastRecordDate>a2.LastReveiveDate  ");
    sql.push("then (SELECT count(1) from chatrecord r where r.chatroomid = a1.ChatRoomID AND r.ServerDate > a2.LastReveiveDate  and r.ServerDate>=a2.joindate  ");
    sql.push(" and (r.senderid=a2.ParticipantID or r.ReceiverID=a2.ParticipantID Or (r.ReceiverID is null and (a2.isdeveloper=1  or ");
    sql.push(" (a2.isdeveloper=0 and exists(select * from chatparticipantinfo aa where a1.ChatRoomID = aa.ChatRoomID and r.senderid=aa.ParticipantID and aa.isdeveloper=1)))))) ");
    sql.push("when a2.IsDelete=1 and a1.LastRecordDate>a2.LastReveiveDate ");
    sql.push("then (SELECT count(1) from chatrecord r where r.chatroomid = a1.ChatRoomID AND r.ServerDate > a2.LastReveiveDate and r.ServerDate>=a2.joindate  ");
    sql.push("and r.ServerDate < a2.deleteDate  and (r.senderid=a2.ParticipantID or r.ReceiverID=a2.ParticipantID Or (r.ReceiverID is null and (a2.isdeveloper=1 ");
    sql.push("   or (a2.isdeveloper=0 and exists(select * from chatparticipantinfo aa where a1.ChatRoomID = aa.ChatRoomID and r.senderid=aa.ParticipantID and aa.isdeveloper=1)))))) ");
    sql.push("else 0 end as chatnotreadcount,");
    sql.push("ifnull(case when a3.ID is not null then  concat('{\"recorddate\":\"', ");
    sql.push("case when DATE_FORMAT(a3.clientdate,'%Y%m%d') =DATE_FORMAT(now(),'%Y%m%d') then DATE_FORMAT(a3.clientdate,'%k:%i')");
    sql.push(" else  DATE_FORMAT(a3.clientdate,'%c-%e') end, '\", \"recordsender\":\"',");
    sql.push("case when a3.SenderID=a2.ParticipantID then '我' else CONCAT(a3.SenderName,case when a3.SenderCompanyName is null or a3.SenderCompanyName='' then '' else CONCAT('(',a3.SenderCompanyName,')') end) end,");
    sql.push("'\",\"recordmessage\":\"', replace(replace(a3.MessageContent,'\"','\\\\\"'),'\\n','<br/>'),'\"}') ");
    sql.push("else(SELECT concat('{\"recorddate\":\"', ");
    sql.push("case when DATE_FORMAT(r.clientdate,'%Y%m%d') =DATE_FORMAT(now(),'%Y%m%d') then DATE_FORMAT(r.clientdate,'%k:%i')");
    sql.push(" else  DATE_FORMAT(r.clientdate,'%c-%e') end, '\", \"recordsender\":\"',");
    sql.push("case when r.SenderID=a2.ParticipantID then '我' else CONCAT(r.SenderName,case when r.SenderCompanyName is null or r.SenderCompanyName='' then '' else CONCAT('(',r.SenderCompanyName,')') end) end,");
    sql.push("'\",\"recordmessage\":\"', replace(replace(r.MessageContent,'\"','\\\\\"'),'\\n','<br/>'),'\"}')  from chatrecord r where r.chatroomid = a1.ChatRoomID  and r.ServerDate>=a2.joindate ");
    sql.push("and (a2.IsDelete=0 or (a2.IsDelete=1 and r.ServerDate<a2.deleteDate))  and (r.senderid=a2.ParticipantID or r.ReceiverID=a2.ParticipantID Or (r.ReceiverID is null and (a2.isdeveloper=1 ");
    sql.push("   or (a2.isdeveloper=0 and exists(select * from chatparticipantinfo aa where a1.ChatRoomID = aa.ChatRoomID and r.senderid=aa.ParticipantID and aa.isdeveloper=1)))))  order by r.serverdate desc  limit 1) end,'{}') recordinfo ");
    sql.push("FROM chatroominfo a1 ");
    sql.push("inner JOIN chatparticipantinfo a2 ");
    sql.push("ON a1.ChatRoomID = a2.ChatRoomID ");
    sql.push("left join chatrecord a3 on a1.ChatRoomID = a3.ChatRoomID  and a3.ServerDate>=a2.joindate and ");
    sql.push("a1.LastRecordDate = a3.ServerDate  and (a3.senderid=a2.ParticipantID or a3.ReceiverID=a2.ParticipantID Or (a3.ReceiverID is null and (a2.isdeveloper=1 ");
    sql.push(" or (a2.isdeveloper=0 and exists(select * from chatparticipantinfo aa where a1.ChatRoomID = aa.ChatRoomID and a3.senderid=aa.ParticipantID and aa.isdeveloper=1)))))  and (a2.IsDelete=0 or (a2.IsDelete=1 and a1.LastRecordDate<a2.deleteDate)) ");
    sql.push("left JOIN chatparticipantinfo a4 ");
    sql.push("ON a1.MembersCount = 1 and a1.ChatRoomID = a4.ChatRoomID and a4.ParticipantID!=?");
    sql.push("left JOIN chatcontacterdetails a5 ");
    sql.push("ON a4.ParticipantID= a5.ContacterID ");
    sql.push("WHERE a2.IsShow = 1 AND a2.ParticipantID = ? ");
    sql.push("ORDER BY a1.LastRecordDate DESC ");
    sql.push("LIMIT " + (parseInt(data.page) - 1) * 18 + ",19;");

    dataParams.push(data.participantid, data.participantid);

    execSQL(sql.join(" "), dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback({ isshowmore: 0, data: [] }); }
            else
            {
                if (results.length == 19) {
                    results.pop();
                    callback({ isshowmore: 1, data: results });
                }
                else {
                    callback({ isshowmore: 0, data: results });
                }
            }
        }
    });
}

//10、	获取聊天记录（GetRecords）
exports.GetRecords = function (data, callback) {
    var dataParams = [];
    var sql = [];
    var now = new Date();

    sql.push("update chatparticipantinfo set LastReveiveDate=? where ChatRoomID=? and ParticipantID=? and LastReveiveDate<?;");
    dataParams.push(now.getTime(), data.chatroomid, data.participantid, now.getTime());
    sql.push("SELECT a3.SenderID senderid,a5.isdeveloper,case when a4.iconsrc is null or a4.iconsrc='' then (case when a5.isdeveloper=1 then '/images/global/chat_icon0.png' else 'images/pic/normal.png' end) else a4.iconsrc end iconsrc,");
    sql.push("a3.SenderName as sendername,a3.receivername as receivername,a3.SenderCompanyName as sendercompanyname,a3.receiverCompanyName as receivercompanyname,");
    sql.push("a3.ReceiverId as receiverid, DATE_FORMAT(a3.clientdate,'%c-%e %k:%i:%s') as senddate,a3.MessageContent as message,a3.ServerDate serverdate ");
    sql.push("FROM chatroominfo a1 ");
    sql.push("inner JOIN	chatparticipantinfo a2 ");
    sql.push("ON a1.ChatRoomID = a2.ChatRoomID ");
    sql.push("inner join chatrecord a3 on a1.ChatRoomID = a3.ChatRoomID and a3.ServerDate>=a2.joindate ");
    sql.push(" and (a2.IsDelete=0 or (a2.IsDelete=1 and a3.ServerDate<a2.deleteDate)) ");
    sql.push("inner JOIN chatparticipantinfo a5 ");
    sql.push("ON a1.ChatRoomID = a5.ChatRoomID and a3.senderid=a5.ParticipantID ");
    sql.push("and (a3.senderid=a2.ParticipantID or a3.ReceiverID=a2.ParticipantID Or (a3.ReceiverID is null and (a2.isdeveloper=1  or (a2.isdeveloper=0 and a5.isdeveloper=1)))) ");
    sql.push("inner join chatcontacterdetails a4 ");
    sql.push("on a3.senderid=a4.ContacterID ");
    sql.push("WHERE a1.ChatRoomID = ? and a2.ParticipantID = ? ");
    sql.push("ORDER BY a3.ServerDate;");

    dataParams.push(data.chatroomid, data.participantid);

    execSQL(sql.join(" "), dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback([]); }
            else
            {
                callback(results[1]);
            }
        }
    });
}

//11、	更新聊天室名称(UpdateChatRoomName)
exports.UpdateChatRoomName = function (data, callback) {
    var sql = "update chatroominfo set ChatRoomName=? where ChatRoomID=? ;", dataParams = [];
    dataParams.push(data.chatroomname, data.chatroomid);

    execSQL(sql, dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback({ issuccess: 0 }); }
            else
            {
                callback({ issuccess: 1 });
            }
        }
    });
}

//12、获取当前用户联系人列表（GetParticipants）
exports.GetParticipants = function (data, callback) {
    var dataParams = [];

    var sql = "SELECT a.ContacterID senderid,c.ContacterName sendername, " +
        "IFNULL(c.ContacterCompanyName,'') companyname, IFNULL(c.ContacterDeptName,'') deptname, " +
        "case when c.iconsrc is null or c.iconsrc='' then (case when a.ContacterIsDeveloper=1 then '/images/global/chat_icon0.png' else 'images/pic/normal.png' end) else c.iconsrc end iconsrc,a.ContacterIsDeveloper isdeveloper   " +
        "FROM chatcontactersinfo  a  inner join  " +
        "chatcontacterdetails c on a.ContacterID=c.ContacterID   " +
        "and a.ownerid=?  and a.ContacterID <>?  inner join (select ParticipantID,max(LastReveiveDate) maxdate from chatparticipantinfo group by ParticipantID ) cc on a.ContacterID=cc.ParticipantID order by cc.maxdate desc;"

    dataParams.push(data.participantid, data.participantid);

    execSQL(sql, dataParams, function (err, results) {
        if (err instanceof Error) { callback([]); }
        else
        {
            callback(results);
        }
    });
}

//13、	当前用户退出聊天室(DeleteChatRooms)
exports.DeleteChatRooms = function (data, callback) {
    var dataParams = [];

    var sql = "update chatparticipantinfo set isShow=0 where ChatRoomID in ('" + data.chatroomids.replace(/,/ig, "','") + "') and ParticipantID=?;"
    dataParams.push(data.senderid);

    execSQL(sql, dataParams, function (err, results) {
        if (callback) {
            if (err instanceof Error) { callback({}); }
            else
            {
                callback({});
            }
        }
    });
}

//14、获取当前用户开发商信息（QueryUserInfo）
exports.QueryUserInfo = function (data, callback) {
    var dataParams = [];

    var sql = "SELECT ContacterID senderid, ContacterName sendername, IFNULL(ContacterCompanyName,'') companyname, IFNULL(ContacterDeptName,'') deptname, IconSrc iconsrc,1 isdeveloper " +
        "FROM chatcontacterdetails " +
        " where ContacterID=? ;"

    dataParams.push(data.participantid);

    execSQL(sql, dataParams, function (err, results) {
        if (err instanceof Error) { callback({}); }
        else
        {
            if (results.length == 0) {
                callback({});
            }
            else {
                callback(results[0]);
            }
        }
    });
}

//15、获取供应商个人信息（GetProviderContact）
exports.GetProviderContact = function (data, callback) {
    var sql = "SELECT ContacterID identifier, ContacterName contactname, JobPosition jobposition, Mobile mobile, Email email, Tel tel, IconSrc imgsrc, EnterpriseCode enterprisecode, CompanyName companyname " +
        "FROM providercontacts " +
        " where ContacterID=? ;"

    execSQL(sql, data, function (err, results) {
        if (err instanceof Error) { callback({}); }
        else
        {
            if (results.length == 0) {
                callback({});
            }
            else {
                callback(results[0]);
            }
        }
    });
}

//16、保存供应商个人信息（SaveProviderContact）
exports.SaveProviderContact = function (data, callback) {
    var dataParams = [];

    var sql = "REPLACE into providercontacts(ContacterID, ContacterName, JobPosition, Mobile, Email, Tel, IconSrc, EnterpriseCode, CompanyName)values(  ?,?,?,?,?,?,?,?,?);";
    dataParams.push(data.identifier, data.contactName, data.jobPosition, data.mobile, data.email, data.tel, data.imgsrc, data.enterprisecode, data.companyname);

    sql += "REPLACE into chatcontacterdetails(ContacterID, ContacterName, ContacterCompanyName, ContacterDeptName, IconSrc)values( ?,?,?,?,?);";
    dataParams.push(data.identifier, data.contactName, data.companyname, "", data.imgsrc);

    execSQL(sql, dataParams, function (err, results) {
        if (err instanceof Error) { callback({ Success: false }); }
        else
        {
            if (results.length == 0) {
                callback({ Success: false });
            }
            else {
                callback({ Success: true });
            }
        }
    });
}

//17、获取供应商个人信息列表（GetProviderContacts）
exports.GetProviderContacts = function (data, callback) {
    var sql = "SELECT ContacterID identifier, ContacterName contactname, JobPosition jobposition, Mobile mobile, Email email, Tel tel, IconSrc imgsrc, EnterpriseCode enterprisecode, CompanyName companyname " +
        "FROM providercontacts " +
        " where ContacterID in ('" + data.contacterids.replace(/,/ig, "','") + "') ;"

    execSQL(sql, data, function (err, results) {
        if (err instanceof Error) { callback([]); }
        else
        {
            if (results.length == 0) {
                callback([]);
            }
            else {
                callback(results);
            }
        }
    });
}
