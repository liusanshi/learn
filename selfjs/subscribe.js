/*
页面间消息通信
msgManage 接口：
postMsg 	//发送消息
getMsg		//获取消息
destroyMsg  //销毁消息
listenMsg   //监听消息
*/
(function  (msgManage, win) {
	var fns = {}
	, subscribe = {
		on : function (msgName, callback) {
			(fns[msgName] || (fns[msgName] = [])).push(callback);
			return this;
		}
		, off: function (msgName, callback) {
			var fn = fns[msgName] || [], i = 0, fun;
			if(!fn.length) return;
			if(typeof callback === 'undefined'){
				fn.length = 0;
				delete fns[msgName];
			} else {
				while(fun = fn[i++]){
					if(fun === callback){
						fn.splice(--i, 1);
					}
				}
			}
			return this;
		}
		, trigger : function (msgName, value) {
			msgManage.postMsg(msgName, value);
			setTimeout(function(){
				msgManage.destroyMsg(msgName);
			}, msgManage.deleteDelay);
			return this;
		}
	}
	//监听消息
	msgManage.listenMsg(function (e) {
        var funs = fns[e.key] || [], i = 0, f, newval = e.newValue;
        while(f = funs[i++]){
        	f(newval);
        }
	});

	win.$subscribe = subscribe;
}((function (win) {
	var msgManage, ls, isNative = true
	, changeKey = '__changekey__', changeValue = '__changeValue__'
	, _trigger, eventPrefix = 'evprefix-';
	if(win.localStorage){
		ls = win.localStorage;
	} else {
		//使用userData 模拟localStorage
		isNative = false;
		ls = win.localStorage = (function () {
			var box = win.document.body || win.document.getElementsByTagName("head")[0] || win.document.documentElement
			, db = win.document.createElement('input'), dbName = '__subscribe', isopen = false;
			db.type='hidden';
			db.addBehavior ("#default#userData");
			db.style.display = 'none';
			box.appendChild(db);
			return {
				setItem : function (key, val) {
					if(!key || !val) return;
					db.setAttribute(key, val);
					db.save(dbName);
					isopen = false;
				}
				, getItem : function (key) {
					if(!key) return null;
					isopen || (isopen = true ,db.load(dbName));
					return db.getAttribute(key)
				}
				, removeItem : function (key) {
					if(!key) return;
					db.removeAttribute(key);
					db.save(dbName);
					isopen = false;
				}
			};
		}());
	}

	var changeInfo = !!win.ActiveXObject ? function(key) {
		if(arguments.length == 0){
			return {
				key : ls.getItem(changeKey),
				oldvalue : ls.getItem(changeValue)
			};
		} else {
			ls.setItem(changeKey, key);
			ls.setItem(changeValue, ls.getItem(key));
		}
	} : function () {
		return void 0;
	};
	msgManage = {
		postMsg: function (msgName, msg) {
			msgName = eventPrefix + msgName;
			changeInfo(msgName);
			ls.setItem(msgName, msg);
			_trigger && _trigger();
		}
		, getMsg: function (msgName) {
			msgName = eventPrefix + msgName;
			return ls.getItem(msgName);
		}
		, destroyMsg : function (msgName) {
			msgName = eventPrefix + msgName;
			changeInfo(msgName);
			ls.removeItem(msgName);
		}
	};
	msgManage.deleteDelay = !!win.ActiveXObject ? 50 : 0; //延迟删除的时间
	msgManage.listenMsg = isNative ? function (func) {
			if (win.addEventListener) {
				win.addEventListener("storage", handle_storage(func), false);
			} else {
				win.document.attachEvent("onstorage", handle_storage(func));
			}
		} : function (func) {
			_trigger = handle_storage(func);
			function timer () {
				_trigger();
				setTimeout(timer, 1000);//定时1S 检查一遍
			}
			timer();
		}
	return msgManage;

	function handle_storage (func) {
		return function (e) {
			e = e || win.event || {};
			var key = e.key, newval = e.newValue, oldValue = e.oldValue, c;
			if(e.url === win.location.href) return; //ie。本页面也触发
			if(!key){//ie8的实现
				c = changeInfo();
				key = e.key = c.key;
				oldValue = e.oldValue = c.oldvalue;
				newval = e.newValue = ls.getItem(key);
			}
			if(key && key.indexOf(eventPrefix) !== 0) return; //只处理定义好的事件
			if(newval && oldValue !== newval){
				func({
					key: key.replace(eventPrefix, '')
					, oldValue: oldValue
					, newValue: newval
					, url: e.url
				});
			}
		}
	}

}(window)), window));