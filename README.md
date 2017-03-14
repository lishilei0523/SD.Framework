##项目概述
###这是一个框架
---

	本框架致力于提升开发效率，减少领域驱动设计实现的复杂性。
	
	基于个人长期使用的经验，遵循面向对象的设计原则，不断从业务项目中提取，逐渐演变为该框架。

	框架中主要涉及Factory、Mediator、Provider、Facade、层超类型等模式。


###技术交流群：558010476
####示例项目：http://git.oschina.net/lishilei0523/ShSoft.UAC
####项目模板：http://git.oschina.net/lishilei0523/SD.Framework.Template
-----------------------------------

1、SD.Common
	
	公共基础类库，包含一些常用扩展工具。

	NuGet包地址：https://www.nuget.org/packages/SD.Common


2、SD.Infrastructure

	基础设施，包含系统常量、自定义异常、基接口与基类。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure


3、SD.Infrastructure.AOP

	AOP基础设施，包含为不同项目封装的不同处理异常的方式。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.AOP


4、SD.Infrastructure.CommandBase

	命令基础设施，包含命令基类、命令执行者工厂、命令中介者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.CommandBase


5、SD.Infrastructure.EventBase

	领域事件基础，包含领域事件基类、领域事件处理者工厂、领域事件中介者、Session存储提供者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.EventBase


6、SD.Infrastructure.EventStoreProvider.RabbitMQ

	RabbitMQ领域事件存储提供者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.EventStoreProvider.RabbitMQ


7、SD.Infrastructure.Global

	全局操作基础设施，包括全局初始化、释放资源、事务。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Global


8、SD.Infrastructure.Repository.EntityFramework

	仓储基础设施，包含EntityFramework DbContext的三次封装、数据库清理者、EntityFramework仓储提供者、EntityFramework UnitOfWork提供者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.EntityFramework


9、SD.Infrastructure.WCF.Server

	WCF服务端基础设施。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.WCF.Server