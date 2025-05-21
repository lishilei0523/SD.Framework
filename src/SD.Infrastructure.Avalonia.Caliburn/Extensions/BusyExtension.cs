using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;
using Caliburn.Micro;
using SD.Infrastructure.Avalonia.Caliburn.Base;
using System.Collections.Generic;

namespace SD.Infrastructure.Avalonia.Caliburn.Extensions
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

            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                foreach (Window window in lifetime.Windows)
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

                    ICollection<Visual> elements = new HashSet<Visual>();
                    window.GetDeepSubElements(ref elements);
                    foreach (Visual element in elements)
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
        }

        /// <summary>
        /// 深度获取子元素列表
        /// </summary>
        private static void GetDeepSubElements(this Visual reference, ref ICollection<Visual> elements)
        {
            IEnumerable<Visual> children = reference.GetVisualChildren();
            foreach (Visual child in children)
            {
                elements.Add(child);
                GetDeepSubElements(child, ref elements);
            }
        }
    }
}
