## 鸣谢 [JetBrains](https://www.jetbrains.com) 为 SD.Framework 提供的 [Resharper](https://www.jetbrains.com/resharper/) [开源许可证](https://www.jetbrains.com/support/community/#section=open-source)。 

<div>
  <a href="https://www.jetbrains.com/resharper/">
    <img alt="R#" width="72" heigth="72" vspace="20" hspace="20" src="./docs/icon_ReSharper.png">
  </a>
</div>

---

## 项目概述
### 这是一个框架（支持.NET Core）
---

        本框架致力于提升开发效率，减少领域驱动设计实现的复杂性；
	
        基于个人长期使用的经验，遵循面向对象的设计原则，不断从业务项目中提取，逐渐演变为该框架；

        框架中主要涉及泛型、反射、委托、事件、特性、Lambda表达式、EntityFramework、WCF行为扩展、WF书签、ASP.NET HttpModule、OWIN Middleware等.NET技术；

        涉及Newtonsoft.Json、MrAdvice、Redis等第三方技术；

        涉及Factory、Mediator、Adapter、Provider、Facade、规约、层超类型等模式；


### 技术交流群：[558010476](//shang.qq.com/wpa/qunwpa?idkey=22cd396d1b7d25fb7632c45c4e40c95ffe2bfa6e48b47a18b7b31c5d4c8d1065)
#### 示例项目：https://gitee.com/lishilei0523/SD.IdentitySystem
#### ASP.NET Core练习项目： https://gitee.com/lishilei0523/AspNetCore.Practice

-----------------------------------
##### 2019.06.27 - 框架近期调整说明

1、修改DbContext访问级别为protected；

2、增加构造实体过滤表达式方法，实体仓储无需override FindAllInner方法过滤关联的聚合根为null的或Deleted的实体对象；

-----------------------------------
##### 2019.03.20 - 框架近期调整说明

增加定时任务功能组件，包含定时执行任务、Redis存储持久化等；

-----------------------------------
##### 2018.04.26 - 框架近期调整说明

增加ASP.NET Core表现层基础，包含常用扩展方法及异常过滤器、授权过滤器等等；

-----------------------------------
##### 2018.04.25 - 框架近期调整说明

1、PlainEntity中AddedTime属性访问级别变更为protected，为了兼容EF Core；

2、修复EF/EF Core仓储实现中实例已Dispose的bug；

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
##### 2017.05.21 - 框架近期调整说明

1、Common中干掉PanGu分词依赖；

2、领域服务基接口干掉GetKeywords；

3、PageModel实现IEnumerable接口；

4、Keywords从PlainEntity转移至AggregateRootEntity中；

5、调整相关仓储接口与实现；

-----------------------------------
##### 2017.04.28 - 框架近期调整说明

1、聚合根中增加操作人账户（OperatorAccount）属性；

2、普通实体基类中干掉排序（Sort）属性；

3、调整SavedTime、Deleted、DeletedTime属性的访问级别为protected internal；

4、EF仓储提供者中增加获取操作人信息事件，用于统一记录对聚合根执行增删改操作时的操作人信息；

5、将原普通的实体的部分属性（Number、Name、SavedTime、Deleted、DeletedTime）移动至聚合根；

6、拆分仓储接口，分为聚合仓储基接口与普通实体仓储基接口；

7、将相应Entity Framework与RavenDB的仓储提供者拆分实现；

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

- 定时任务部分

SD.Infrastructure.CrontabBase（.NET Standard 2.0）

	定时任务基础，包含定时任务基类、定时任务执行者工厂、调度中介者

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.CrontabBase

SD.Infrastructure.CrontabStoreProvider.Redis（.NET Standard 2.0）

	Redis定时任务存储提供者。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.CrontabStoreProvider.Redis

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

SD.Infrastructure.AspNetCore（.NET Core 2.0）

	ASP.NET Core表现层基础

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.AspNetCore

SD.Infrastructure.MVC（.NET Framework 4.6.1）

	ASP.NET MVC 5表现层基础。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.MVC

SD.Infrastructure.WPF（.NET Framework 4.6.1）
	
	WPF表现层基础。

	NuGet包地址：https://www.nuget.org/packages/SD.Infrastructure.WPF