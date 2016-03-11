#AOP组件
	功能主治：统一异常处理，记录程序运行日志，简化事务处理代码。

使用方式：
	1、新建个空库或使用现有数据库
	2、在App.config文件中配置key为"LogConnection"的连接字符串name
	3、运行测试
	4、查看数据库的RunningLogs表与ExceptionLogs表

Ps：
	使用前需安装PostSharp-tool-4.1.13.exe，在解决方案根目录下
	依赖项目：ShSoft.Framework2015.Common
	依赖NuGet包：PostSharp
	当编译时出现PostSharp提示对话框时，请选择Install
	异常过滤特性标签可以打在程序集、类、构造器、方法上
	运行日志特性标签可以打在构造器、方法上