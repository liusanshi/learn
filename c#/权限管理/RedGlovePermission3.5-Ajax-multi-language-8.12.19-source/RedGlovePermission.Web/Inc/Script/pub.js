function $id(s) {
    return document.getElementById(s);
}

//判断小数 
function IsFloat(s) {
    if (!/^[+\-]?\d+(.\d+)?$/.test(s))
        return false;
    else
        return true;
}
//判断正小数
function IsPlusFloat(s) {
    if (!/^[+]?\d+(.\d+)?$/.test(s))
        return false;
    else
        return true;
}

//判断正整数
function IsPlusInt(s) {
    if (!/^\d*$/.test(s))
        return false;
    else
        return true;
}

//判断是否为字母和数字
function IsName(s) {
    if (!/^[A-Za-z0-9]+$/.test(s))
        return false;
    else
        return true;
}

//取RadioList的值
function GetRadioList(s) {
    var radListItems = document.all(s);
    var radListItesCount = radListItems.length - 1;
    var radListCheckedValue = "";
    
    //遍歷Item的Text和Value
    for (var i = 1; i <= radListItesCount; i++) {
        if (radListItems[i].checked)
            radListCheckedValue = radListItems[i].value;
    }
    
    return radListCheckedValue;
}

function Setfocus() { document.body.focus(); }