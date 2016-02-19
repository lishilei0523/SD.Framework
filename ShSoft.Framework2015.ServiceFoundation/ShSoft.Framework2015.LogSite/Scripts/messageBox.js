/*{
    imghref:图片文件夹所在路径,
    waitImg:等待图片名,
    bgImg:背景图片名
   }
*/
function MessageBox(s) {
    var secondConst = 2000;//系统默认显示时间
    var secondWait = 2000;//显示时间
    var timer;//计时器
    var lf, tp;//左边距,顶边距
    var paras = {}; //json参数
    function readyMsgBox() {
        if (s != null){
            if (s.imghref != null) paras.imghref = s.imghref; else paras.imghref = "images/";
            if (s.waitImg != null) paras.waitImg = s.waitImg; else paras.waitImg = "loader.gif";
            if (s.bgImg != null) paras.bgImg = s.bgImg; else paras.bgImg = "qzonebg.gif";
        }
        else paras = { imghref: "./images/", waitImg: "loader.gif", bgImg: "qzonebg.gif" };
        paras.waitImgTag = "<img src='" + paras.imghref + paras.waitImg + "' style='margin-right:10px;' align='middle'/>    ";
        preloadImg(new Array(paras.imghref + paras.bgImg, paras.imghref + paras.waitImg));
        writeMsgBox();
        window.onresize = function(){setPosition();}
    }
    this.showMsgWait = function (msg) {
        this.showMsgAllT(paras.waitImgTag + msg, 0);
    }
    this.showMsgAllT = function (msg, type) {
        clearTimer();
        changeIco(type);
        gelContainer().innerHTML = msg;
        showBox();
    }
    this.hidBox = function () { hideBox(); };
    this.showMsgText = function (msg) {
        showMsgAllT(msg, 0);
    }
    this.showMsgInfo = function (msg) {
        if (arguments.length > 1) paras.callBack = arguments[1];
        showSysMsg(msg, 1);
    }
    this.showMsgInfoSide = function (eleId, msg, doHid) {//doHid 是否消失
        if (arguments.length > 3) paras.callBack = arguments[1];
        showSysMsgSideEle(eleId, msg, 1, doHid);
    }
    function analysisPara(args) {
        if (args.length > 1) paras.callBack = args[1];
    }
    this.showMsgOk = function (msg) {
        if (arguments.length > 1) paras.callBack = arguments[1];
        showSysMsg(msg, 2);
    }
    this.showMsgOkSide = function (eleId, msg, doHid) {
        if (arguments.length > 3) paras.callBack = arguments[1];
        showSysMsgSideEle(eleId, msg, 2, doHid);
    }
    this.showMsgErr = function (msg) {
        if (arguments.length > 1) paras.callBack = arguments[1];
        showSysMsg(msg, 3);
    }
    this.showMsgErrSide = function (eleId,msg,doHid) {
        if (arguments.length > 3) paras.callBack = arguments[1];
        showSysMsgSideEle(eleId, msg, 3, doHid);
    }
    this.showSysMsgWTime = function (msg, type, second) {
        if (arguments.length > 3) paras.callBack = arguments[3];
        changeIco(type);
        gelContainer().innerHTML = msg;
        showBox();
        secondWait = second;
        if (second >= 0)
            startTimer(emptyMsg);
    }
    function showSysMsg(msg, type) {
        changeIco(type);
        gelContainer().innerHTML = msg;
        showBox();
        secondWait = secondConst;
        startTimer(emptyMsg);
    }
    //---显示在元素右边
    function showSysMsgSideEle(eleId, msg, type, doHid) {
        changeIco(type);
        gelContainer().innerHTML = msg;
        setPosSideEle(eleId);
        if (doHid) {
            secondWait = secondConst;
            startTimer(emptyMsg);
        } else clearTimer();
    }
    function setPosSideEle(eleId) {
        var wid = document.getElementById(eleId).offsetWidth;
        var hig = document.getElementById(eleId).offsetHeight;
        var pos = getPos(eleId);
        gelBox().style.left = (wid+2 + pos.left) + "px";
        gelBox().style.top = (pos.top - (hig/2)) + "px";
        gelBox().style.display = "block";
    }
    //--------------
    this.showReqErr=function(){this.showMsgErr("请求错误 ToT!");}
    this.showReqOk=function(){this.showMsgOk("操作成功 ^o^!");}
    this.showReqVF = function () { this.showSysMsgWTime("会话过期,3秒后自动返回登录界面 -o-!",1,3000); }
    this.showWait = function () { this.showMsgWait("请稍后 l _ l ..."); }
    //-------------
    function startTimer(functionName) {
        clearTimer();
        timer=window.setTimeout(functionName, secondWait);
    }
    function clearTimer() {
        if (timer != null && timer != undefined) { clearTimeout(timer); }
    }
    function emptyMsg() {
        gelContainer().innerHTML = "";
        hideBox();
        if (paras.callBack != null) {paras.callBack(); paras.callBack = null; }
    }
    function writeMsgBox() {
        var msgBox = document.createElement("table");
        var msgTbody = document.createElement("tbody");
        var msgTr = document.createElement("tr");
        var msgBoxL = document.createElement("td");
        var msgBoxC = document.createElement("td");
        var msgBoxR = document.createElement("td");
        document.body.appendChild(msgBox);
        msgBox.appendChild(msgTbody);
        msgTbody.appendChild(msgTr);
        msgTr.appendChild(msgBoxL);
        msgTr.appendChild(msgBoxC);
        msgTr.appendChild(msgBoxR);
        msgBox.setAttribute("id", "msgBox");
        msgBox.setAttribute("cellpadding", "0");
        msgBox.setAttribute("cellspacing", "0");
        msgBox.style.cssText = "height:52px;width:auto;position:absolute;z-index:999999;display:none; background:url(" + paras.imghref + paras.bgImg+") 0px -161px;";
        msgBoxL.setAttribute("id", "msgBoxL");
        msgBoxL.style.cssText = "width:50px;background:url(" + paras.imghref + paras.bgImg+") -7px -108px no-repeat;";
        msgBoxC.setAttribute("id", "msgBoxC");
        msgBoxC.style.cssText = "width:auto;line-height:51px;color:#666666;font-weight:bold;font-size:14px;padding-right:10px;";
        msgBoxR.setAttribute("id", "msgBoxR");
        msgBoxR.style.cssText = "width:5px;background:url(" + paras.imghref + paras.bgImg+") 0px 0px no-repeat;";
    }
    function changeIco(ty) {
        if (ty == 0)//none
            document.getElementById("msgBoxL").style.width = "10px";
        else document.getElementById("msgBoxL").style.width = "50px";
        if (ty == 1)//info
            document.getElementById("msgBoxL").style.backgroundPosition = "-7px -54px";
        else if (ty == 2)//ok
            document.getElementById("msgBoxL").style.backgroundPosition = "-7px 0px";
        else if (ty == 3)//err
            document.getElementById("msgBoxL").style.backgroundPosition = "-7px -108px";
    }
    function gelBox() {
        return document.getElementById("msgBox");
    }
    function gelContainer() {
        return document.getElementById("msgBoxC");
    }
    function hideBox() {
        gelBox().style.display = "none";
    }
    function showBox() {
        setPosition();
        gelBox().style.display = "block";
    }
    function setPosition() {
        lf = document.body.clientWidth / 2 - (gelBox().innerHTML.replace(/<[^>].*?>/g, "").length) * 10;
        tp = window.screen.height / 2 - 200 + document.documentElement.scrollTop;
        gelBox().style.left = lf + "px";
        gelBox().style.top = tp + "px";
    }
    function preloadImg() {
        var Arrimg = new Array();
        if (typeof (arguments[0]) == "string") Arrimg[0] = arguments[0];
        if (typeof (arguments[0]) == "object") {
            for (var i = 0; i < arguments[0].length; i++) {
                Arrimg[i] = arguments[0][i];
            }
        }
        var img = new Array()
        for (var i = 0; i < Arrimg.length; i++) {
            img[i] = new Image();
            img[i].src = Arrimg[i];
        }
    }
    function getPos(eid) {var target = document.getElementById(eid);var left = 0, top = 0;
        do {left += target.offsetLeft || 0;top += target.offsetTop || 0;target = target.offsetParent;} while (target);
        return {left: left,top: top}
    }
    readyMsgBox();
}