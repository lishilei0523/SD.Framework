var messageBox = null;      //消息框
var topHelper = {};             //提供给 iframe里子页面 操作当前页面 的一些便捷 方法
$(function () {
    //初始化用户菜单
    $('#menuTree').tree({
        url: '/Home/GetMenuList',
        animate: true,
        lines: true,
        onClick: function (e) {
            addTab(e.text, e.attributes.href, e.attributes.isLink);
        }
    });

    //初始化消息框
    messageBox = new MessageBox({ imghref: "/Content/images/" });

    //注销用户按钮
    $('#btn_exit').linkbutton({
        iconCls: 'icon-cancel',
        plain: true,
        text: '注销用户'
    });

    //注销按钮事件
    $('#btn_exit').click(function () {
        $.messager.confirm('警告', '确定注销当前用户吗？', function (result) {
            if (result) {
                $.post('/Admin/User/Logout', null, function (jsonData) {
                    if (jsonData.Status == '1') {
                        window.location.href = jsonData.ReturnUrl;
                    }
                }, 'json');
            }
        });
    });

    //修改密码按钮
    $('#btn_padlock').linkbutton({
        iconCls: 'icon-lock',
        plain: true,
        text: '修改密码'
    });

    //修改密码按钮事件
    $('#btn_padlock').click(function () {
        //修改密码窗口
        $('#dvUpdatePwd').window('open');
        $('#dvUpdatePwd').window('setTitle', '修改密码');
        $('#fmUpdatePwd').form('clear');
    });

    //创建弹修改密码弹出窗口
    $('#dvUpdatePwd').window({
        width: 350,
        height: 200,
        maximizable: false,
        resizable: false,
        draggable: false,
        modal: true,
        minimizable: false,
        collapsible: false,
        closed: true
    });

    //初始化公共窗体
    topHelper.comWin = $('#commonWindow').window({
        width: 800,
        height: 500,
        collapsible: false,
        minimizable: false,
        maximizable: true
    }).window('close');

    //添加一个打开公共窗体的方法
    topHelper.showComWindow = function (title, url, width, height) {
        var trueTitle = "公共窗体";
        var trueWidth = 1200;
        var trueHeight = 500;
        if (title) trueTitle = title;
        if (width && parseInt(width) > 10) {
            trueWidth = width;
        }
        if (height && parseInt(height) > 10) {
            trueHeight = height;
        }
        //判断是否置顶url，如果有，则设置公共窗体里的iframe的src
        if (url && url.length && url.length > 10) {
            $("#commonWindow iframe").attr("src", url);
        }
        //重新设置窗体的大小，并自动居中，然后才显示
        topHelper.comWin.window({
            title: trueTitle,
            width: trueWidth,
            height: trueHeight
        }).window("center").window("open");
    };

    //添加一个关闭公共窗体方法
    topHelper.closeComWindow = function () {
        topHelper.comWin.window("close");
    };
});

//修改密码
function changePwd() {
    //var va = $("#oldPwd").val();
    //var val = $("#newPwd").val();
    //var val1 = $("#newPwd2").val();
    //if (va.length == 0 || val.length == 0 || val1.length == 0) {
    //    messageBox.showMsgErr('密码不可为空，请重新输入！');
    //} else if (val != val1) {
    //    messageBox.showMsgErr('两次密码不一致，请重试！');
    //} else {
    //    //动态将用户Id赋值给隐藏域，否则接收不到
    //    $('#txtUserId').val(@Model.Id);                 //最恶心的就是这块
    //    $('#fmUpdatePwd').submit();
    //}
}

//修改密码时执行
function updatingPwd() {
    messageBox.showMsgWait("修改中，请稍后...");
}

//修改密码后执行
function updatedPwd(jsonData) {
    if (jsonData.Status == 1) {
        $.messager.alert('OK', jsonData.Message);
        $('#dvUpdatePwd').window('close');
        window.location.href = jsonData.ReturnUrl;
    } else {
        messageBox.showMsgErr(jsonData.Message);
        $('#dvUpdatePwd').window('close');
    }
}

//添加tab方法
function addTab(title, url, isLink) {
    //判断是否是链接
    if (isLink == '1') {          //是链接
        if ($('#tabs').tabs('exists', title)) {
            var currTab = $('#tabs').tabs('getSelected');
            $('#tabs').tabs('select', title);
            var url = $(currTab.panel('options').content).attr('src');
            if (url != undefined) {
                $('#tabs').tabs('update', {
                    tab: currTab,
                    options: {
                        content: createFrame(url)
                    }
                });
            }
        } else {
            var content = createFrame(url);
            $('#tabs').tabs('add', {
                title: title,
                content: content,
                closable: true
            });
        }
    }
}

//创建iframe方法
function createFrame(url) {
    var tabHeight = $("#tabs").height() - 35;
    var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:' + tabHeight + 'px;"></iframe>';
    return s;
}

//新增或修改成功后，可通过此方法更新tab里的DataGrid组件
function updateDataGridInTab() {
    //1.获取后台首页的tab容器
    var $tabBox = $("#tabs");
    //2.获取选中的tab
    var $curTab = $tabBox.tabs('getSelected');
    //3.从选中的tab中获取iframe，并以jq对象返回
    var $ifram = $("iframe", $curTab);
    //4.从jq对象中获取iframe，并通过伟大的contentWindow对象操作iframe里的window的全局变量$tbGrid
    $ifram[0].contentWindow.$tbGrid.datagrid("clearSelections");         //清除选中
    $ifram[0].contentWindow.$tbGrid.datagrid("reload");                      //刷新表格
}