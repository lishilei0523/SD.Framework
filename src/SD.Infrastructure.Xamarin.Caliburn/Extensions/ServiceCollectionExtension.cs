using Caliburn.Micro.Xamarin.Forms;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace SD.Infrastructure.Xamarin.Caliburn.Extensions
{
    /// <summary>
    /// 容器建造者扩展
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 注册Caliburn导航服务
        /// </summary>
        /// <param name="serviceCollection">容器建造者</param>
        public static void RegisterNavigationService(this IServiceCollection serviceCollection)
        {
            NavigationPage navigationPage = new NavigationPage();
            INavigationService navigationService = new NavigationPageAdapter(navigationPage);
            serviceCollection.AddSingleton(navigationPage);
            serviceCollection.AddSingleton(navigationService);
        }
    }
}
