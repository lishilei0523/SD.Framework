//提交表单完成后执行方法
function submited(jsonData) {
    if (jsonData.Status === '1') {
        window.top.window.$.messager.alert('OK', jsonData.Message); //主窗口弹出消息提示
        window.parent.$('#treeMenu').tree('reload'); //刷新父窗体的栏目树
    } else {
        window.top.window.$.messager.alert('Error', jsonData.Message); //主窗口弹出消息提示
    }
}