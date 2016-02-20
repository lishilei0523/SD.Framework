using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using ShSoft.Framework2015.Infrastructure.Constants;
using ShSoft.Framework2015.Infrastructure.IOC.Configuration;
using ShSoft.Framework2015.Infrastructure.WCF;

namespace ShSoft.Framework2015.Infrastructure.IOC.UnityProvider
{
    /// <summary>
    /// Unity依赖注入容器
    /// </summary>
    internal static class UnityContainer
    {
        #region # 访问器 —— static IUnityContainer Current
        /// <summary>
        /// 访问器
        /// </summary>
        public static IUnityContainer Current
        {
            get { return _Container; }
        }
        #endregion

        #region # Unity IOC容器实例 —— static readonly UnityContainer _Container
        /// <summary>
        /// Unity IOC容器实例
        /// </summary>
        private static readonly IUnityContainer _Container;
        #endregion

        #region # 静态构造器
        /// <summary>
        /// 静态构造器
        /// </summary>
        static UnityContainer()
        {
            //实例化容器实例
            _Container = new Microsoft.Practices.Unity.UnityContainer();

            RegisterInterfaceAssemblies(_Container);
            RegisterBaseAssemblies(_Container);
            RegisterSelfAssemblies(_Container);
            RegisterInterfaceTypes(_Container);
            RegisterBaseTypes(_Container);
            RegisterSelfTypes(_Container);
            RegisterWcfInterfaces(_Container);
        }
        #endregion

        #region # 注册接口形式程序集 —— static void RegisterInterfaceAssemblies(IUnityContainer container)
        /// <summary>
        /// 注册接口形式程序集
        /// </summary>
        /// <param name="container">容器实例</param>
        private static void RegisterInterfaceAssemblies(IUnityContainer container)
        {
            foreach (AssemblyElement element in InjectionConfiguration.Setting.AsInterfaceAssemblies)
            {
                Assembly currentAssembly = Assembly.Load(element.Name.Trim());
                IEnumerable<Type> types = currentAssembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract);

                container.RegisterTypes(types, type => type.GetInterfaces(), type => type.Name);
            }
        }
        #endregion

        #region # 注册基类形式程序集 —— static void RegisterBaseAssemblies(IUnityContainer container)
        /// <summary>
        /// 注册基类形式程序集
        /// </summary>
        /// <param name="container">容器实例</param>
        private static void RegisterBaseAssemblies(IUnityContainer container)
        {
            foreach (AssemblyElement element in InjectionConfiguration.Setting.AsBaseAssemblies)
            {
                Assembly currentAssembly = Assembly.Load(element.Name.Trim());
                IEnumerable<Type> types = currentAssembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface);

                foreach (Type type in types)
                {
                    container.RegisterType(type, type.BaseType);
                }
            }
        }
        #endregion

        #region # 注册自身形式程序集 —— static void RegisterSelfAssemblies(IUnityContainer container)
        /// <summary>
        /// 注册自身形式程序集
        /// </summary>
        /// <param name="container">容器实例</param>
        private static void RegisterSelfAssemblies(IUnityContainer container)
        {
            foreach (AssemblyElement element in InjectionConfiguration.Setting.AsSelfAssemblies)
            {
                Assembly currentAssembly = Assembly.Load(element.Name.Trim());
                IEnumerable<Type> types = currentAssembly.GetTypes().Where(type => !type.IsInterface && !type.IsAbstract);

                container.RegisterTypes(types, null, type => type.Name);
            }
        }
        #endregion

        #region # 注册接口形式类型 —— static void RegisterInterfaceTypes(IUnityContainer container)
        /// <summary>
        /// 注册接口形式类型
        /// </summary>
        /// <param name="container">容器实例</param>
        private static void RegisterInterfaceTypes(IUnityContainer container)
        {
            foreach (TypeElement element in InjectionConfiguration.Setting.AsInterfaceTypes)
            {
                Assembly currentAssembly = Assembly.Load(element.Assembly.Trim());
                Type type = currentAssembly.GetType(element.Name.Trim());

                container.RegisterTypes(new[] { type }, typ => typ.GetInterfaces());
            }
        }
        #endregion

        #region # 注册基类形式类型 —— static void RegisterBaseTypes(IUnityContainer container)
        /// <summary>
        /// 注册基类形式类型
        /// </summary>
        /// <param name="container">容器实例</param>
        private static void RegisterBaseTypes(IUnityContainer container)
        {
            foreach (TypeElement element in InjectionConfiguration.Setting.AsBaseTypes)
            {
                Assembly currentAssembly = Assembly.Load(element.Assembly.Trim());
                Type type = currentAssembly.GetType(element.Name.Trim());

                container.RegisterType(type, type.BaseType);
            }
        }
        #endregion

        #region # 注册自身形式类型 —— static void RegisterSelfTypes(IUnityContainer container)
        /// <summary>
        /// 注册自身形式类型
        /// </summary>
        /// <param name="container">容器实例</param>
        private static void RegisterSelfTypes(IUnityContainer container)
        {
            foreach (TypeElement element in InjectionConfiguration.Setting.AsSelfTypes)
            {
                Assembly currentAssembly = Assembly.Load(element.Assembly.Trim());
                Type type = currentAssembly.GetType(element.Name.Trim());

                container.RegisterType(type);
            }
        }
        #endregion

        #region # 注册WCF接口列表 —— static void RegisterWcfInterfaces(IUnityContainer container)
        /// <summary>
        /// 注册WCF接口列表
        /// </summary>
        /// <param name="container">容器实例</param>
        private static void RegisterWcfInterfaces(IUnityContainer container)
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

                    container.RegisterType(proxyType);
                    container.RegisterInstance(type, propChannel.GetValue(container.Resolve(proxyType)), new TransientLifetimeManager());
                }
            }
        }
        #endregion
    }
}
