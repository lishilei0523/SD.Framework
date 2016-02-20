(function (jqObj) {
    jqObj.extend(jqObj, {
        globalParas: {
            updateDataGridInTab: function () {
                window.top.updateDataGridInTab();       //清除选中，刷新表格 datagrid
            },
            //代理方法：显示父窗体的EasyUI的进度加载条
            progressShow: function () {
                window.top.$.messager.progress();
            },
            //代理方法：关闭父窗体的EasyUI的进度加载条
            progressClose: function () {
                window.top.$.messager.progress("close");
            },
            //通用EasyUI的window组件配置
            window: {
                title: "欢迎登录SlamDunk内容管理系统 - beta 0.1",
                collapsible: false,
                minimizable: false,
                maximizable: false,
                closable: true,
                nowrap: false,
                width: 340,
                height: 200,
                modal: true,
                //初始化窗口方法
                init: function (title, width, height) {
                    this.title = title;
                    this.width = width;
                    this.height = height
                }
            },
            //代理方法：调用后台主页面的topHelper.showComWindow(title, url, width, height)方法
            showComWindow: function (title, url, width, height) {
                window.top.topHelper.showComWindow(title, url, width, height);
            },
            //代理方法：调用后台主页面的topHelper.showComWindow(title, url, width, height)方法
            closeComWindow: function () {
                window.top.topHelper.closeComWindow();
            },
            //将序列化成json格式后日期(毫秒数)转成日期格式
            changeDateFormat: function (cellval) {
                var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
                var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                return date.getFullYear() + "-" + month + "-" + currentDate;
            },
            //EasyUI表格组件属性设置
            datagrid: {
                url: null,
                title: null,
                columns: null,
                fitColumns: false,
                fit: true,
                idField: 'Id',
                loadMsg: '正在加载...',
                singleSelect: false,                                //多选
                rownumbers: true,
                pagination: true,                                  //启用分页
                pageNumber: 1,                                   //第一次请求默认请求的页码
                pageList: [5, 10, 15, 20, 30, 50],             //页容量数组
                pageSize: 15,                                       //页容量（必须和 pageList 里某一个值一致）
                queryParams: null,
                onLoadSuccess: function (jsonObj) {
                    //如果Datagrid获取的服务器数据 包含 Statu的话，那就可以证明在过滤器验证权限时失败，那就将过滤器返回的权限消息交给msgAlert统一处理！
                    //if (jsonObj.Statu) {
                    //    $.msgAlert(jsonObj);
                    //}
                },
                toolbar: [],                                         //按钮数组
                init: function (url, columns, idFiled) {
                    this.url = url;
                    this.columns = columns;
                    this.idField = idFiled;
                },
                //绑定指定名字的按钮方法
                bindBtnEvent: function (text, func) {
                    //1.根据按钮名字
                    for (var i = 0; i < this.toolbar.length; i++) {
                        if (this.toolbar[i].text == text) {
                            this.toolbar[i].handler = func;
                            break;
                        }
                    }
                },
                //新增按钮
                addBtn: function (text, func, iconCls) {
                    if (this.toolbar.length > 0) {
                        this.toolbar[this.toolbar.length] = "-";
                    }
                    this.toolbar[this.toolbar.length] = {
                        text: text,
                        iconCls: iconCls ? iconCls : 'icon-ok',
                        handler: func
                    };
                },
                //根据按钮名字，删除按钮
                removeBtn: function (text) {
                    //1.根据按钮名字，找到按钮所在下标
                    var btnIndex = -1;
                    for (var i = 0; i < this.toolbar.length; i++) {
                        if (this.toolbar[i].text == text) {
                            btnIndex = i;
                            break;
                        }
                    }
                    //1.1根据找到的按钮下标，移除按钮
                    this.removeBtnByIndex(btnIndex);

                    //2.找到相邻的两个【-】，如果有，则删除一个
                    var splitIndex = -1;
                    for (var i = 0; i < this.toolbar.length; i++) {
                        if (this.toolbar[i] == "-" && this.toolbar[i] == this.toolbar[i + 1]) {
                            splitIndex = i;
                            break;
                        }
                    }
                    //2.1根据找到的 分隔符 下标，移除 分隔符
                    this.removeBtnByIndex(splitIndex);

                    if (this.toolbar.length <= 0) return;
                    //3.如果数组的第一个是分隔符，则删除掉此分隔符
                    if (this.toolbar[0] == "-") {
                        this.removeBtnByIndex(0);
                    }
                    //4.如果数组的最后一个是分隔符，则删除掉此分隔符
                    if (this.toolbar[this.toolbar.length - 1] == "-") {
                        this.removeBtnByIndex(this.toolbar.length - 1);
                    }
                },
                //根据按钮下标，删除按钮
                removeBtnByIndex: function (itemIndex) {
                    //如果没找到要删除的按钮，则停止执行
                    if (itemIndex <= -1) return;
                    for (var i = itemIndex; i < this.toolbar.length - 1; i++) {
                        this.toolbar[i] = this.toolbar[i + 1];
                    }
                    this.toolbar.length = this.toolbar.length - 1;
                }
            }
        }
    });
})($);
