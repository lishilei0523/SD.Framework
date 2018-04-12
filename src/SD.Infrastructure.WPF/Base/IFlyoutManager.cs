using Caliburn.Micro;

namespace SD.Infrastructure.WPF.Base
{
    /// <summary>
    /// 飞窗管理器
    /// </summary>
    public interface IFlyoutManager
    {
        /// <summary>
        /// 飞窗列表
        /// </summary>
        BindableCollection<FlyoutBase> Flyouts { get; set; }
    }
}
