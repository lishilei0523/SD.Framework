using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using SD.IOC.Core.Mediator;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SD.Infrastructure.WPF.Base
{
    /// <summary>
    /// 元素管理器静态工具类
    /// </summary>
    public static class ElementManager
    {
        #region # 字段

        /// <summary>
        /// 元素管理器实例
        /// </summary>
        private static IElementManager _Current;

        #endregion

        #region # 初始化 —— static void Init()
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            _Current = ResolveMediator.Resolve<IElementManager>();
        }
        #endregion

        #region # 文档列表 —— static BindableCollection<DocumentBase> Documents
        /// <summary>
        /// 文档列表
        /// </summary>
        public static BindableCollection<DocumentBase> Documents
        {
            get
            {
                #region # 验证

                if (_Current == null)
                {
                    throw new ApplicationException("元素管理器未初始化！");
                }

                #endregion

                return _Current.Documents;
            }
        }
        #endregion

        #region # 打开文档 —— static DocumentBase OpenDocument(Type type)
        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="type">文档类型</param>
        /// <returns>文档</returns>
        public static DocumentBase OpenDocument(Type type)
        {
            #region # 验证

            if (_Current == null)
            {
                throw new ApplicationException("元素管理器未初始化！");
            }
            if (type == null)
            {
                return null;
            }

            #endregion

            return _Current.OpenDocument(type);
        }
        #endregion

        #region # 打开文档 —— static DocumentBase OpenDocument<T>()
        /// <summary>
        /// 打开文档
        /// </summary>
        /// <typeparam name="T">文档类型</typeparam>
        /// <returns>文档</returns>
        public static DocumentBase OpenDocument<T>() where T : DocumentBase
        {
            #region # 验证

            if (_Current == null)
            {
                throw new ApplicationException("元素管理器未初始化！");
            }

            #endregion

            return _Current.OpenDocument<T>();
        }
        #endregion

        #region # 打开飞窗 —— static FlyoutBase OpenFlyout(Type type)
        /// <summary>
        /// 打开飞窗
        /// </summary>
        /// <param name="type">飞窗类型</param>
        /// <returns>飞窗</returns>
        public static FlyoutBase OpenFlyout(Type type)
        {
            #region # 验证

            if (_Current == null)
            {
                throw new ApplicationException("元素管理器未初始化！");
            }
            if (type == null)
            {
                return null;
            }

            #endregion

            return _Current.OpenFlyout(type);
        }
        #endregion

        #region # 打开飞窗 —— static T OpenFlyout<T>()
        /// <summary>
        /// 打开飞窗
        /// </summary>
        /// <typeparam name="T">飞窗类型</typeparam>
        /// <returns>飞窗</returns>
        public static T OpenFlyout<T>() where T : FlyoutBase
        {
            #region # 验证

            if (_Current == null)
            {
                throw new ApplicationException("元素管理器未初始化！");
            }
            #endregion

            return _Current.OpenFlyout<T>();
        }
        #endregion

        #region # 打开消息框 —— static async Task<MessageDialogResult> ShowMessage(...
        /// <summary>
        /// 打开消息框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        /// <param name="style">样式</param>
        /// <param name="config">配置</param>
        public static async Task<MessageDialogResult> ShowMessage(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings config = null)
        {
            MetroWindow currentView = Application.Current.MainWindow as MetroWindow;

            if (currentView == null && _Current != null)
            {
                ViewAware viewAware = (ViewAware)_Current;
                currentView = (MetroWindow)viewAware.GetView();
            }
            else if (currentView == null)
            {
                return MessageDialogResult.Negative;
            }
            else
            {
                return MessageDialogResult.Negative;
            }

            return await currentView.ShowMessageAsync(title, message, style, config);
        }
        #endregion

        #region # 打开遮罩 —— static void ShowOverlay()
        /// <summary>
        /// 打开遮罩
        /// </summary>
        public static void ShowOverlay()
        {
            MetroWindow currentView = Application.Current.MainWindow as MetroWindow;

            if (currentView == null && _Current != null)
            {
                ViewAware viewAware = (ViewAware)_Current;
                currentView = (MetroWindow)viewAware.GetView();
            }
            else if (currentView == null)
            {
                return;
            }
            else
            {
                return;
            }

            currentView.ShowOverlay();
        }
        #endregion

        #region # 打开遮罩 —— static async Task ShowOverlayAsync()
        /// <summary>
        /// 打开遮罩
        /// </summary>
        public static async Task ShowOverlayAsync()
        {
            MetroWindow currentView = Application.Current.MainWindow as MetroWindow;

            if (currentView == null && _Current != null)
            {
                ViewAware viewAware = (ViewAware)_Current;
                currentView = (MetroWindow)viewAware.GetView();
            }
            else if (currentView == null)
            {
                return;
            }
            else
            {
                return;
            }

            await currentView.ShowOverlayAsync();
        }
        #endregion

        #region # 隐藏遮罩 —— static void HideOverlay()
        /// <summary>
        /// 隐藏遮罩
        /// </summary>
        public static void HideOverlay()
        {
            MetroWindow currentView = Application.Current.MainWindow as MetroWindow;

            if (currentView == null && _Current != null)
            {
                ViewAware viewAware = (ViewAware)_Current;
                currentView = (MetroWindow)viewAware.GetView();
            }
            else if (currentView == null)
            {
                return;
            }
            else
            {
                return;
            }

            currentView.HideOverlay();
        }
        #endregion

        #region # 隐藏遮罩 —— static async Task HideOverlayAsync()
        /// <summary>
        /// 隐藏遮罩
        /// </summary>
        public static async Task HideOverlayAsync()
        {
            MetroWindow currentView = Application.Current.MainWindow as MetroWindow;

            if (currentView == null && _Current != null)
            {
                ViewAware viewAware = (ViewAware)_Current;
                currentView = (MetroWindow)viewAware.GetView();
            }
            else if (currentView == null)
            {
                return;
            }
            else
            {
                return;
            }

            await currentView.HideOverlayAsync();
        }
        #endregion
    }
}
