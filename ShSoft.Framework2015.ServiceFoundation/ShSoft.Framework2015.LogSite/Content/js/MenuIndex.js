//DOM初始化
$(function () {
    //初始化菜单树
    $('#treeMenu').tree({
        url: "/Menu/List",
        animate: true,
        checkbox: true,
        onlyLeafCheck: true,
        lines: true,
        onClick: chooseMenu
    });
});

//选中菜单时显示修改页面
function chooseMenu() {
    //1.获取选中的菜单
    var selectedNode = $('#treeMenu').tree('getSelected');
    //2.让iframe链接指向修改视图
    $('#ifAddOrEdit').attr('src', '/Menu/Edit/' + selectedNode.id);
}

//增加下一级栏目
function addSub() {
    //1.获取选中的菜单
    var selectedNode = $('#treeMenu').tree('getSelected');
    //判断用户是否有选中
    if (selectedNode != null) {
        //2.让iframe链接指向添加视图
        $('#ifAddOrEdit').attr('src', '/Menu/Add/' + selectedNode.id);
    } else {
        $.messager.alert('Warning', '请选择一个上级栏目！');
    }
}

//删除单选选中的菜单
function deleteSingle() {
    //1.获取用户单选选中的节点
    var selectedNode = $('#treeMenu').tree('getSelected');
    if (selectedNode != null) {
        $.messager.confirm('Warning', '确定要删除吗？', function (result) {
            if (result) {
                $.post('/Menu/DeleteSingle/' + selectedNode.id, null, function (jsonData) {
                    if (jsonData.Status == '1') {
                        $('#treeMenu').tree('reload');
                        $.messager.alert('OK', jsonData.Message);
                    } else {
                        $.messager.alert('Error', jsonData.Message);
                    }
                }, 'json');
            }
        });
    } else {
        $.messager.alert('Warning', '请选择要删除的菜单！');
    }
}

//删除勾选选中的栏目
function deleteSelected() {
    //获取所有的选中节点
    var selectedNodes = $('#treeMenu').tree('getChecked');
    //判断用户有没有选中
    if (selectedNodes.length > 0) {
        $.messager.confirm('确认', '确定要删除吗？', function (result) {
            if (result) {
                //定义一个数组来装选中节点id
                var selectedIds = [];
                for (var i = 0; i < selectedNodes.length; i++) {
                    selectedIds.push(selectedNodes[i].id);
                }
                //发送至后台
                $.post('/Menu/DeleteSelected', { 'selectedIds': selectedIds.toString() }, function (jsonData) {
                    if (jsonData.Status == '1') {
                        $('#treeMenu').tree('reload');
                        $.messager.alert('OK', jsonData.Message);
                    } else {
                        $.messager.alert('Error', jsonData.Message);
                    }
                }, 'json');
            }
        });
    } else {
        $.messager.alert('Warning', '请选择要删除的菜单！');
    }
}