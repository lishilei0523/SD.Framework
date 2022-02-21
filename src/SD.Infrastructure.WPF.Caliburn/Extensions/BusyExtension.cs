using Caliburn.Micro;
using SD.Infrastructure.WPF.Caliburn.Base;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

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

                ICollection<DependencyObject> elements = new HashSet<DependencyObject>();
                window.GetDeepSubElements(ref elements);
                foreach (DependencyObject element in elements)
                {
                    object subViewModel = ViewModelLocator.LocateForView.Invoke(element);
                    if (subViewModel is ScreenBase subScreenBase)
                    {
                        subScreenBase.Idle();
                    }
                    if (subViewModel is OneActiveConductorBase subConductorBase)
                    {
                        subConductorBase.Idle();
                    }
                }
            }
        }

        /// <summary>
        /// 深度获取子元素列表
        /// </summary>
        private static void GetDeepSubElements(this DependencyObject reference, ref ICollection<DependencyObject> elements)
        {
            for (int index = 0; index < VisualTreeHelper.GetChildrenCount(reference); index++)
            {
                DependencyObject subReference = VisualTreeHelper.GetChild(reference, index);
                elements.Add(subReference);

                GetDeepSubElements(subReference, ref elements);
            }
        }
    }
}
