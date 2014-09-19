$.extend({
    fnRequest: function (strName) {
        ///	<summary>
        ///		获取Url后跟的参数值，相当于服务端的Request.QueryString()方法
        ///	</summary>
        ///	<returns type="String" />
        var strHref = window.document.location.href;
        var intPos = strHref.indexOf("?");
        var strRight = strHref.substr(intPos + 1);
        var arrTmp = strRight.split("&");
        for (var i = 0; i < arrTmp.length; i++) {
            var arrTemp = arrTmp[i].split("=");
            if (arrTemp[0].toUpperCase() == strName.toUpperCase()) return arrTemp[1];
        }
        return "";
    },
    fnGetLenB: function (str) {
        var b = 0, l = str.length;
        if (l) {
            for (var i = 0; i < l; i++) {
                if (this.charCodeAt(i) > 255) {
                    b += 2;
                } else {
                    b++;
                }
            }
            return b;
        } else {
            return 0;
        }
    },
    isIE: function () { //ie?  
        if (!!window.ActiveXObject || "ActiveXObject" in window)
            return true;
        else
            return false;
    },
    getFormatDate:function(str)
    {
        var dd = new Date(str.replace(/-/g, "/"));

           return dd.getFullYear().toString() + "/" + dd.getMonth().toString() + "/" + dd.getDate().toString() + " " + dd.getHours().toString() + ":" + dd.getMinutes().toString() + ":" + dd.getSeconds().toString();
      }
   
}
);

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(), //day 
        "h+": this.getHours(), //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
        "S": this.getMilliseconds() //millisecond 
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }

    return format;
}