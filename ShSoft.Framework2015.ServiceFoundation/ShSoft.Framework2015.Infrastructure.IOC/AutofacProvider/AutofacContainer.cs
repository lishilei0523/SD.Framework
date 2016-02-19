using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using ShSoft.Framework2015.Infrastructure.Constants;
using ShSoft.Framework2015.Infrastructure.IOC.Configuration;
using ShSoft.Framework2015.Infrastructure.WCF;

namespace ShSoft.Framework2015.Infrastructure.IOC.AutofacProvider
{
    /// <summary>
    /// Autofac依赖注入容器
    /// </summary>
    internal static class AutofacContainer
    {
        #region # 访问器 —— static IContainer Current
        /// <summary>
        /// 访问器
        /// </summary>
        public static IContainer Current
        {
            get { return _Container; }
        }
        #endregion

        #region # Autofac IOC容器实例 —— static readonly IContainer _Container
        /// <summary>
        /// Autofac IOC容器实例
        /// </summary>
        private static readonly IContainer _Container;
        #endregion

        #region # 静态构造器
        /// <summary>
        /// 静态构造器
        /// </summary>
        static AutofacContainer()
        {
            //实例化容器建造者
            ContainerBuilder builder = new ContainerBuilder();

            RegisterInterfaceAssemblies(builder);
            RegisterBaseAssemblies(builder);
            RegisterSelfAssemblies(builder);
            RegisterInterfaceTypes(builder);
            RegisterBaseTypes(builder);
            RegisterSelfTypes(builder);
            RegisterWcfInterfaces(builder);

            //得到容器对象
            _Container = builder.Build();
        }
        #endregion

        #region # 注册接口形式程序集 —— static void RegisterInterfaceAssemblies(ContainerBuilder builder)
        /// <summary>
        /// 注册接口形式程序集
        /// </summary>
        /// <param name="builder">容器建造者</param>
        private static void RegisterInterfaceAssemblies(ContainerBuilder builder)
        {
            foreach (AssemblyElement element in InjectionConfiguration.Setting.AsInterfaceAssemblies)
            {
                builder.RegisterAssemblyTypes(Assembly.Load(element.Name.Trim())).AsImplementedInterfaces();
            }
        }
        #endregion

        #region # 注册基类形式程序集 —— static void RegisterBaseAssemblies(ContainerBuilder builder)
        /// <summary>
        /// 注册基类形式程序集
        /// </summary>
        /// <param name="builder">容器建造者</param>
        private static void RegisterBaseAssemblies(ContainerBuilder builder)
        {
            foreach (AssemblyElement element in InjectionConfiguration.Setting.AsBaseAssemblies)
            {
                Assembly currentAssembly = Assembly.Load(element.Name.Trim());
                IEnumerable<Type> types = currentAssembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface);

                foreach (Type type in types)
                {
                    builder.RegisterType(type).As(type.BaseType);
                }
            }
        }
        #endregion

        #region # 注册自身形式程序集 —— static void RegisterSelfAssemblies(ContainerBuilder builder)
        /// <summary>
        /// 注册自身形式程序集
        /// </summary>
        /// <param name="builder">容器建造者</param>
        private static void RegisterSelfAssemblies(ContainerBuilder builder)
        {
            foreach (AssemblyElement element in InjectionConfiguration.Setting.AsSelfAssemblies)
            {
                builder.RegisterAssemblyTypes(Assembly.Load(element.Name.Trim()));
            }
        }
        #endregion

        #region # 注册接口形式类型 —— static void RegisterInterfaceTypes(ContainerBuilder builder)
        /// <summary>
        /// 注册接口形式类型
        /// </summary>
        /// <param name="builder">容器建造者</param>
        private static void RegisterInterfaceTypes(ContainerBuilder builder)
        {
            foreach (TypeElement element in InjectionConfiguration.Setting.AsInterfaceTypes)
            {
                Assembly currentAssembly = Assembly.Load(element.Assembly.Trim());
                Type type = currentAssembly.GetType(element.Name.Trim());

                builder.RegisterType(type).AsImplementedInterfaces();
            }
        }
        #endregion

        #region # 注册基类形式类型 —— static void RegisterBaseTypes(ContainerBuilder builder)
        /// <summary>
        /// 注册基类形式类型
        /// </summary>
        /// <param name="builder">容器建造者</param>
        private static void RegisterBaseTypes(ContainerBuilder builder)
        {
            foreach (TypeElement element in InjectionConfiguration.Setting.AsBaseTypes)
            {
                Assembly currentAssembly = Assembly.Load(element.Assembly.Trim());
                Type type = currentAssembly.GetType(element.Name.Trim());

                builder.RegisterType(type).As(type.BaseType);
            }
        }
        #endregion

        #region # 注册自身形式类型 —— static void RegisterSelfTypes(ContainerBuilder builder)
        /// <summary>
        /// 注册自身形式类型
        /// </summary>
        /// <param name="builder">容器建造者</param>
        private static void RegisterSelfTypes(ContainerBuilder builder)
        {
            foreach (TypeElement element in InjectionConfiguration.Setting.AsSelfTypes)
            {
                Assembly currentAssembly = Assembly.Load(element.Assembly.Trim());
                Type type = currentAssembly.GetType(element.Name.Trim());

                builder.RegisterType(type);
            }
        }
        #endregion

        #region # 注册WCF接口列表 —— static void RegisterWcfInterfaces(ContainerBuilder builder)
        /// <summary>
        /// 注册WCF接口列表
        /// </summary>
        /// <param name="builder">容器建造者</param>
        private static void RegisterWcfInterfaces(ContainerBuilder builder)
        {
            foreach (AssemblyElement element in InjectionConfiguration.Setting.WcfInterfaces)
            {
                //加载程序集
                Assembly wcfInterfaceAssembly = Assembly.Load(element.Name);

                //获取WCF接口类型集
                IEnumerable<Type> types = wcfInterfaceAssembly.GetTypes().Where(type => type.IsInterface);

                //获取服务代理泛型类型
                Type proxyGenericType = typeof(ServiceProxy<>);

                //注册WCF接口
                foreach (Type type in types)
                {
                    Type proxyType = proxyGenericType.MakeGenericType(type);
                    PropertyInfo propChannel = proxyType.GetProperty(CommonConstants.ChannelPropertyName, type);

                    builder.RegisterType(proxyType);
                    builder.Register(container => propChannel.GetValue(container.Resolve(proxyType))).
                        As(type).
                        OnRelease(channel => channel.CloseChannel());
                }
            }
        }
        #endregion
    }
}
