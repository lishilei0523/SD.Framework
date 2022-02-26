using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using SD.Infrastructure.Xamarin.Caliburn.Base;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace SD.Infrastructure.Xamarin.Caliburn.Extensions
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

        #region # 展示对话框 —— static Task<bool?> ShowDialogAsync(Screen viewModel)
        /// <summary>
        /// 展示对话框
        /// </summary>
        public static async Task<bool?> ShowDialogAsync(this INavigationService navigationService, Screen viewModel)
        {
            Element view = ViewLocator.LocateForModelType(viewModel.GetType(), null, null);
            Dialog dialog = view as Dialog;

            #region # 验证

            if (dialog == null)
            {
                throw new NotSupportedException($"\"{view.GetType()}\"未继承\"{typeof(Dialog)}\"！");
            }

            #endregion

            ViewModelBinder.Bind(viewModel, view, null);

            NavigationPage navigationPage = ResolveMediator.Resolve<NavigationPage>();
            bool? result = await navigationPage.Navigation.ShowPopupAsync(dialog);

            return result;
        }
        #endregion

        #region # 展示对话框 —— static Task<bool?> ShowDialogAsync<TViewModel>()
        /// <summary>
        /// 展示对话框
        /// </summary>
        public static async Task<bool?> ShowDialogAsync<TViewModel>(this INavigationService navigationService) where TViewModel : Screen
        {
            Screen viewModel = ResolveMediator.Resolve<TViewModel>();
            Element view = ViewLocator.LocateForModelType(typeof(TViewModel), null, null);
            Dialog dialog = view as Dialog;

            #region # 验证

            if (dialog == null)
            {
                throw new NotSupportedException($"\"{view.GetType()}\"未继承\"{typeof(Dialog)}\"！");
            }

            #endregion

            ViewModelBinder.Bind(viewModel, view, null);

            NavigationPage navigationPage = ResolveMediator.Resolve<NavigationPage>();
            bool? result = await navigationPage.Navigation.ShowPopupAsync(dialog);

            return result;
        }
        #endregion
    }
}
