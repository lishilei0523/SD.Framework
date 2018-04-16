## 项目概述
### 这是一个框架
---

        本框架致力于提升开发效率，减少领域驱动设计实现的复杂性。
	
        基于个人长期使用的经验，遵循面向对象的设计原则，不断从业务项目中提取，逐渐演变为该框架。

        框架中主要涉及泛型、反射、委托、事件、特性、Lambda表达式、EntityFramework、WCF行为扩展、ASP.NET HttpModule等.NET技术。

        涉及Autofac、Postsharp、Redis等第三方技术。

        涉及Factory、Mediator、Adapter、Provider、Facade、、规约、层超类型等模式。


### 技术交流群：[558010476](//shang.qq.com/wpa/qunwpa?idkey=22cd396d1b7d25fb7632c45c4e40c95ffe2bfa6e48b47a18b7b31c5d4c8d1065)
#### 示例项目：https://gitee.com/lishilei0523/ShSoft.UAC
#### 项目模板：https://gitee.com/lishilei0523/SD.Framework.Template
-----------------------------------
##### 2018.04.16 - 框架近期调整说明

1、聚合根实体增加创建人姓名、操作人姓名；

2、增加可审核接口与可停用接口；

3、使用CallContext.LogicalSetData()/LogicalGetData()，为异步编程做准备；

4、增加WebApi服务端基础；

5、.NET Framework版本升级至4.6.1；

6、调整事务隔离级别为快照，增加异步事务流转支持；

7、UnitOfWork/UnitOfWorkExtension增加异步操作接口；

8、领域事件增加异步操作理接口；

9、精简仓储API；

-----------------------------------
SD.Infrastructure

	基础设施，包含系统常量、自定义异常、基接口与基类。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure


SD.Infrastructure.AOP

	AOP基础设施，包含为不同项目封装的不同处理异常的方式。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.AOP


SD.Infrastructure.EventBase

	领域事件基础，包含领域事件基类、领域事件处理者工厂、领域事件中介者、Session存储提供者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.EventBase

SD.Infrastructure.EventStoreProvider.RabbitMQ

	RabbitMQ领域事件存储提供者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.EventStoreProvider.RabbitMQ

SD.Infrastructure.EventStoreProvider.Redis

	Redis领域事件存储提供者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.EventStoreProvider.Redis

SD.Infrastructure.Global

	全局操作基础设施，包括全局初始化、释放资源、事务。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Global

SD.Infrastructure.MVC

	MVC基础。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.MVC

SD.Infrastructure.Repository.EntityFramework

	仓储基础设施，包含EntityFramework DbContext的三次封装、数据库清理者、EntityFramework仓储提供者、EntityFramework UnitOfWork提供者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.EntityFramework

SD.Infrastructure.Repository.MongoDB

	仓储基础 - MongoDB实现。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.MongoDB

SD.Infrastructure.Repository.RavenDB

	仓储基础 - RavenDB实现。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.RavenDB

SD.Infrastructure.Repository.Redis

	仓储基础 - Redis实现。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.Redis

SD.Infrastructure.MVC.Server
	
	MVC服务端基础。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.MVC.Server

SD.Infrastructure.WebApi.Server
	
	WebApi服务端基础。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.WebApi.Server

SD.Infrastructure.WCF.Server

	WCF服务端基础设施。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.WCF.Server

SD.Infrastructure.WPF
	
	WPF客户端基础设施。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.WPF