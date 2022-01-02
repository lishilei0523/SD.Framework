using Caliburn.Micro.Xamarin.Forms;
using System;

namespace SD.Infrastructure.Xamarin.Caliburn.Mediators
{
    /// <summary>
    /// 导航
    /// </summary>
    public static class Navigator
    {
        /// <summary>
        /// 导航服务
        /// </summary>
        private static INavigationService _NavigationService;

        /// <summary>
        /// 初始化导航服务
        /// </summary>
        /// <param name="navigationService">导航服务</param>
        public static void Initialize(INavigationService navigationService)
        {
            _NavigationService = navigationService;
        }

        /// <summary>
        /// 导航服务
        /// </summary>
        public static INavigationService NavigationService
        {
            get
            {
                if (_NavigationService == null)
                {
                    throw new InvalidOperationException("导航服务未初始化！");
                }

                return _NavigationService;
            }
        }
    }
}
