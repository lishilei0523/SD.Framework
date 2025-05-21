using Caliburn.Micro.Maui;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using SD.Infrastructure.Maui.Caliburn.Base;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace SD.Infrastructure.Maui.Caliburn.Extensions
{
    /// <summary>
    /// 导航扩展
    /// </summary>
    public static class NavigationExtension
    {
        #region # 重定向页面 —— static Task RedirectToViewModelAsync<TViewModel>(bool animated = true)
        /// <summary>
        /// 重定向页面
        /// </summary>
        public static async Task RedirectToViewModelAsync<TViewModel>(this INavigationService navigationService, bool animated = true)
        {
            //获取导航页
            NavigationPage navigationPage = ResolveMediator.Resolve<NavigationPage>();

            //获取导航前页面
            IList<Page> prevPages = new List<Page>(navigationPage.Navigation.NavigationStack);

            //导航至新页面
            await navigationService.NavigateToViewModelAsync<TViewModel>(animated);

            //移除导航前页面
            foreach (Page prevPage in prevPages)
            {
                navigationPage.Navigation.RemovePage(prevPage);
            }
        }
        #endregion

        #region # 重定向页面 —— static void Redirect<TViewModel>(bool animated = true)
        /// <summary>
        /// 重定向页面
        /// </summary>
        public static void Redirect<TViewModel>(this NavigateHelper<TViewModel> navigateHelper, bool animated = true)
        {
            //获取导航页
            NavigationPage navigationPage = ResolveMediator.Resolve<NavigationPage>();

            //获取导航前页面
            IList<Page> prevPages = new List<Page>(navigationPage.Navigation.NavigationStack);

            //导航至新页面
            navigateHelper.Navigate(animated);

            //移除导航前页面
            foreach (Page prevPage in prevPages)
            {
                navigationPage.Navigation.RemovePage(prevPage);
            }
        }
        #endregion

        #region # 展示对话框 —— static async Task<bool?> ShowDialogAsync<TViewModel>(this INavigationService...
        /// <summary>
        /// 展示对话框
        /// </summary>
        public static async Task<bool?> ShowDialogAsync<TViewModel>(this INavigationService navigationService, CancellationToken token = default) where TViewModel : INotifyPropertyChanged
        {
            Element view = ViewLocator.LocateForModelType(typeof(TViewModel), null, null);
            if (view is Popup popup)
            {
                NavigationPage navigationPage = ResolveMediator.Resolve<NavigationPage>();
                object result = await navigationPage.ShowPopupAsync(popup, token);

                return (bool?)result;
            }

            throw new InvalidCastException($"\"{view.GetType()}\"未继承\"{typeof(Popup)}\"！");
        }
        #endregion

        #region # 展示对话框 —— static async Task<bool?> ShowDialogAsync<TViewModel>(this INavigationService...
        /// <summary>
        /// 展示对话框
        /// </summary>
        public static async Task<bool?> ShowDialogAsync<TViewModel>(this INavigationService navigationService, TViewModel viewModel, CancellationToken token = default) where TViewModel : ScreenBase
        {
            Element view = ViewLocator.LocateForModel(viewModel, null, null);
            if (view is Popup popup)
            {
                ViewModelBinder.Bind(viewModel, view, null);
                NavigationPage navigationPage = ResolveMediator.Resolve<NavigationPage>();
                object result = await navigationPage.ShowPopupAsync(popup, token);

                return (bool?)result;
            }

            throw new InvalidCastException($"\"{view.GetType()}\"未继承\"{typeof(Popup)}\"！");
        }
        #endregion
    }
}
