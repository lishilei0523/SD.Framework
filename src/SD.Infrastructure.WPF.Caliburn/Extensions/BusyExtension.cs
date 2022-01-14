using SD.Infrastructure.WPF.Caliburn.Base;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.Linq;

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
            IList<IDisposable> disposables = ResolveMediator.GetServiceScopeDisposables();
            IEnumerable<OneActiveConductorBase> conductorBases = disposables.OfType<OneActiveConductorBase>();
            IEnumerable<ScreenBase> screenBases = disposables.OfType<ScreenBase>();
            foreach (OneActiveConductorBase conductorBase in conductorBases)
            {
                conductorBase.Dispose();
            }
            foreach (ScreenBase screenBase in screenBases)
            {
                screenBase.Dispose();
            }
        }
    }
}
