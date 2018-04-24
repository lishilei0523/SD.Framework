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
##### 2018.04.24 - 框架近期调整说明

1、全面开启v2.6.1；

2、框架核心类库迁移到.NET Standard 2.0，详见项目列表；

3、增加EF Core仓储实现；

4、增加ASP.NET Core服务端基础中间件；

-----------------------------------
##### 2018.04.22 - 框架近期调整说明

1、恢复UnitedCommit默认事务隔离级别，因为快照隔离级别不支持分布式事务；

2、干掉Redis仓储实现，没什么用；

3、Framework版本全部升级至4.6.1；

4、WebConfigSetting重命名为GlobalSetting；

5、涉及TransactionScope地方全部启用异步支持；

6、CallContext.LogicalSet/GetData整体替换为AsyncLocal<T>；

7、使用StackExchange.Redis替换ServiceStack.Redis；

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

#### 项目列表


- 基础设施部分

SD.Infrastructure（.NET Standard 2.0）

	基础设施，包含系统常量、自定义异常、基接口与基类。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure

SD.Infrastructure.AOP（.NET Standard 2.0）

	AOP基础设施，包含为不同项目封装的不同处理异常的方式。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.AOP

SD.Infrastructure.Global（.NET Standard 2.0）

	全局操作基础设施，包括全局初始化、释放资源、事务。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Global

- 领域事件部分

SD.Infrastructure.EventBase（.NET Standard 2.0）

	领域事件基础，包含领域事件基类、领域事件处理者工厂、领域事件中介者、Session存储提供者。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.EventBase

SD.Infrastructure.EventStoreProvider.RabbitMQ（.NET Standard 2.0）

	RabbitMQ领域事件存储提供者。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.EventStoreProvider.RabbitMQ

SD.Infrastructure.EventStoreProvider.Redis（.NET Standard 2.0）

	Redis领域事件存储提供者。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.EventStoreProvider.Redis

- 服务端部分

SD.Infrastructure.AspNetCore.Server（.NET Core 2.0）

	ASP.NET Core服务端基础。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.AspNetCore.Server

SD.Infrastructure.MVC.Server（.NET Framework 4.6.1）
	
	ASP.NET MVC 5服务端基础。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.MVC.Server

SD.Infrastructure.WebApi.Server（.NET Framework 4.6.1）
	
	ASP.NET WebApi 2.0服务端基础。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.WebApi.Server

SD.Infrastructure.WCF.Server（.NET Framework 4.6.1）

	WCF服务端基础设施。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.WCF.Server

- 仓储部分

SD.Infrastructure.Repository.EntityFramework（.NET Framework 4.6.1）

	仓储基础设施，包含EntityFramework DbContext的三次封装、数据库清理者、EntityFramework仓储提供者、EntityFramework UnitOfWork提供者。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.EntityFramework

SD.Infrastructure.Repository.EntityFrameworkCore（.NET Core 2.0）

	仓储基础 - EF Core实现。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.EntityFrameworkCore

SD.Infrastructure.Repository.MongoDB（.NET Standard 2.0）

	仓储基础 - MongoDB实现。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.MongoDB

SD.Infrastructure.Repository.RavenDB（.NET Standard 2.0）

	仓储基础 - RavenDB实现。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.Repository.RavenDB

- 表现层部分

SD.Infrastructure.MVC（.NET Framework 4.6.1）

	ASP.NET MVC 5表现层基础。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.MVC

SD.Infrastructure.WPF（.NET Framework 4.6.1）
	
	WPF表现层基础。
	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.WPF