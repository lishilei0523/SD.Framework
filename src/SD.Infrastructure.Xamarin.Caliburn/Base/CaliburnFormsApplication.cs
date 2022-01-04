using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SD.Infrastructure.Xamarin.Caliburn.Base
{
    /// <summary>
    /// Caliburn Xamarin.Forms应用程序基类
    /// </summary>
    public abstract class CaliburnFormsApplication : Application
    {
        /// <summary>
        /// 是否已初始化
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// 初始化Caliburn框架
        /// </summary>
        protected void Initialize()
        {
            #region # 验证

            if (this._isInitialized)
            {
                return;
            }

            #endregion

            this._isInitialized = true;
            PlatformProvider.Current = new FormsPlatformProvider(PlatformProvider.Current);
            Func<Assembly, IEnumerable<Type>> baseExtractTypes = AssemblySourceCache.ExtractTypes;
            AssemblySourceCache.ExtractTypes = assembly =>
            {
                IEnumerable<Type> baseTypes = baseExtractTypes(assembly);
                IEnumerable<Type> elementTypes =
                    assembly.ExportedTypes
                        .Where(type => typeof(Element)
                        .GetTypeInfo()
                        .IsAssignableFrom(type.GetTypeInfo()));

                return baseTypes.Union(elementTypes);
            };

            AssemblySource.Instance.Refresh();
        }

        /// <summary>
        /// 展示根视图模型
        /// </summary>
        /// <typeparam name="T">视图模型类型</typeparam>
        protected async Task DisplayRootViewForAsync<T>() where T : Screen
        {
            INavigationService navigationService = ResolveMediator.Resolve<INavigationService>();
            await navigationService.NavigateToViewModelAsync<T>();
        }
    }
}
