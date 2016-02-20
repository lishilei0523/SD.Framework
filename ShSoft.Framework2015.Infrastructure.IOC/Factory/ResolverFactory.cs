using System;
using System.Configuration;
using ShSoft.Framework2015.Infrastructure.IIOC;
using ShSoft.Framework2015.Infrastructure.IOC.AutofacProvider;
using ShSoft.Framework2015.Infrastructure.IOC.UnityProvider;

namespace ShSoft.Framework2015.Infrastructure.IOC.Factory
{
    /// <summary>
    /// 解析者工厂
    /// </summary>
    internal static class ResolverFactory
    {
        #region # 字段及构造器

        /// <summary>
        /// 解析者类型
        /// </summary>
        private static readonly ProviderType? _ProviderType;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ResolverFactory()
        {
            string provider = ConfigurationManager.AppSettings["IocProvider"];
            if (!string.IsNullOrWhiteSpace(provider))
            {
                _ProviderType = (ProviderType)Enum.Parse(typeof(ProviderType), provider);
            }
        }

        #endregion

        #region # 获取默认解析者 —— static IInstanceResolver GetDefaultInstanceResolver()
        /// <summary>
        /// 获取默认解析者
        /// </summary>
        /// <returns>解析者</returns>
        public static IInstanceResolver GetDefaultInstanceResolver()
        {
            return new AutofacInstanceResolver(AutofacContainer.Current);
        }
        #endregion

        #region # 根据配置文件获取解析者 —— static IInstanceResolver GetInstanceResolver()
        /// <summary>
        /// 根据配置文件获取解析者
        /// </summary>
        /// <returns>解析者</returns>
        public static IInstanceResolver GetInstanceResolver()
        {
            switch (_ProviderType)
            {
                case ProviderType.Autofac:
                    return new AutofacInstanceResolver(AutofacContainer.Current);
                case ProviderType.Unity:
                    return new UnityInstanceResolver(UnityContainer.Current);
                default:
                    return GetDefaultInstanceResolver();
            }
        }
        #endregion
    }
}
