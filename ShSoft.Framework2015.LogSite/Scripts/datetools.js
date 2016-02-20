/// <reference path="easyui/jquery-1.8.0.min.js" />
/// <reference path="easyui/jquery.easyui.min.js" />
$(function () {
    tick();
    setInterval("tick()", 900);
});
function tick() {
    var today;
    today = new Date();
    $("#nowDate").html(format(today));
}
function format(date) {
    //年
    var yy = date.getYear() + 1900;
    //月
    var MM = date.getMonth() + 1;
    //日
    var dd = date.getDate();
    //时
    var hh = date.getHours()
    //分
    var mm = date.getMinutes();
    //秒
    var ss = date.getSeconds();
    //星期
    var ww = date.getDay();
    var ssStr = "";
    switch (ww) {
        case 0:
            ssStr = "星期日"
            break;
        case 1:
            ssStr = "星期一"
            break;
        case 2:
            ssStr = "星期二"
            break;
        case 3:
            ssStr = "星期三"
            break;
        case 4:
            ssStr = "星期四"
            break;
        case 5:
            ssStr = "星期五"
            break;
        case 6:
            ssStr = "星期六"
            break
    }
    return yy + "年" + MM + "月" + dd + "日  " + hh + "时" + mm + "分" + ss + "秒     " + ssStr;
}