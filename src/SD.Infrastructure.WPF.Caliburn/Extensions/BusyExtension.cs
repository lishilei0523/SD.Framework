using Caliburn.Micro;
using SD.Infrastructure.WPF.Caliburn.Base;
using System.Windows;

namespace SD.Infrastructure.WPF.Caliburn.Extensions
{
    /// <summary>
    /// 繁忙扩展
    /// </summary>
    public static class BusyExtension
    {
        /// <summary>
        /// 全局释放繁忙状态
        /// </summary>
        public static void GlobalIdle()
        {
            #region # 验证

            if (Application.Current == null)
            {
                return;
            }

            #endregion

            foreach (Window window in Application.Current.Windows)
            {
                object viewModel = ViewModelLocator.LocateForView.Invoke(window);
                if (viewModel is ScreenBase screenBase)
                {
                    screenBase.Idle();
                }
                if (viewModel is OneActiveConductorBase conductorBase)
                {
                    conductorBase.Idle();
                }
            }
        }
    }
}
