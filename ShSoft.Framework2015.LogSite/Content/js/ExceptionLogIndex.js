/***********定义全局变量$tbGrid，用于刷新表格**********/
var $tbGrid = null;

//DOM初始化
$(function () {
    //执行加载列表方法
    loadList();
});

//加载列表方法
function loadList(queryParams) {
    //1.指定url
    $.globalParas.datagrid.url = '/ExceptionLog/List';
    //2.指定列
    $.globalParas.datagrid.columns = [
        [
            { field: 'ck', checkbox: true, halign: 'center' },
            { field: 'Id', title: 'Id', halign: 'center', hidden: true },
            { field: 'ClassName', title: '类名', halign: 'center' },
            { field: 'MethodName', title: '方法名', halign: 'center' },
            { field: 'ExceptionType', title: '异常类型', halign: 'center' },
            { field: 'ExceptionMessage', title: '异常消息', halign: 'center', width: 400 },
            { field: 'OccurredTimeString', title: '发生时间', align: 'center', halign: 'center' },
            { field: 'IPAddress', title: 'IP地址', align: 'center', halign: 'center' }
        ]
    ];
    //3.清空按钮
    $.globalParas.datagrid.removeBtn('查看');
    $.globalParas.datagrid.removeBtn('删除');
    //4.添加按钮
    $.globalParas.datagrid.addBtn('查看', watch, 'icon-tip');
    $.globalParas.datagrid.addBtn('删除', deleteSeleted, 'icon-remove');
    //5.添加参数
    $.globalParas.datagrid.queryParams = queryParams;
    //6.绑定并返回$tbGrid
    $tbGrid = $("#tblList").datagrid($.globalParas.datagrid);
}

//查看日志明细
function watch() {
    //获取所有的选中行
    var selectedRows = $('#tblList').datagrid('getSelections');
    //判断用户有没有选中
    if (selectedRows.length !== 1) {
        $.messager.alert('Warning', "请选择一条查看！");
        return false;
    }
    $.globalParas.showComWindow('查看日志明细', '/ExceptionLog/Detail/' + selectedRows[0].Id, 1024, 600);
}

//删除选中行
function deleteSeleted() {
    //获取所有的选中行
    var selectedRows = $('#tblList').datagrid('getSelections');
    //判断用户有没有选中
    if (selectedRows.length > 0) {
        $.messager.confirm('Warning', '确定要删除吗？', function (result) {
            if (result) {
                //定义一个数组来装选中行Id
                var selectedIds = [];
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedIds.push(selectedRows[i].Id);
                }
                //发送至后台
                $.post('/ExceptionLog/DeleteSelected', { 'selectedIds': selectedIds.toString() }, function (jsonData) {
                    if (jsonData.Status == '1') {
                        $('#tblList').datagrid('clearSelections'); //清除选中
                        $('#tblList').datagrid('reload'); //刷新表格
                        $.messager.alert('OK', jsonData.Message);
                    } else {
                        $('#tblList').datagrid('clearSelections'); //清除选中
                        $('#tblList').datagrid('reload'); //刷新表格
                        $.messager.alert('Error', jsonData.Message);
                    }
                }, 'json');
            }
        });
    } else {
        $.messager.alert('警告', '请选择要删除的日志！');
    }
}

//搜索
function find() {
    var beginDate = $('#srchStartTime').val();
    var endDate = $('#srchEndTime').val();
    var d1 = new Date(beginDate.replace(/\-/g, "\/"));
    var d2 = new Date(endDate.replace(/\-/g, "\/"));

    if (beginDate !== '' && endDate !== '' && d1 > d2) {
        $.messager.alert('Error', '开始时间不能大于结束时间！');
        return false;
    }
    //搜索后台的数据
    var queryData = {
        startTime: $("#srchStartTime").val(),
        endTime: $("#srchEndTime").val(),
        logId: $("#logId").val()
    };
    //重新加载列表
    loadList(queryData);
}

//计算相对宽度
function fixWidth(percent) {
    return $('#dvList').width() * percent;
}