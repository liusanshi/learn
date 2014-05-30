function document.oncontextmenu() {
    var s = event.srcElement.tagName;

    if (s && s != "INPUT" && s != "TEXTAREA" || event.srcElement.disabled || document.selection.createRange().text.length == 0) {
        event.returnValue = false;
    }
}