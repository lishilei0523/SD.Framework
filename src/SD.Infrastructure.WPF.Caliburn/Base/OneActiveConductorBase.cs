using Caliburn.Micro;
using SD.Infrastructure.WPF.Caliburn.Aspects;
using SD.Infrastructure.WPF.Interfaces;

namespace SD.Infrastructure.WPF.Caliburn.Base
{
    /// <summary>
    /// 单活动Conductor基类
    /// </summary>
    public class OneActiveConductorBase : Conductor<IScreen>.Collection.OneActive, IBusy
    {
        /// <summary>
        /// 是否繁忙
        /// </summary>
        [DependencyProperty]
        public bool IsBusy { get; set; }

        /// <summary>
        /// 挂起繁忙状态
        /// </summary>
        public void Busy()
        {
            this.IsBusy = true;
        }

        /// <summary>
        /// 释放繁忙状态
        /// </summary>
        public void Idle()
        {
            this.IsBusy = false;
        }
    }
}
