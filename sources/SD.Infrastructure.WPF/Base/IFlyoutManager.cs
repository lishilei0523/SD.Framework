using Caliburn.Micro;

namespace SD.Infrastructure.WPF.Base
{
    /// <summary>
    /// 飞窗管理器
    /// </summary>
    public interface IFlyoutManager
    {
        BindableCollection<FlyoutBase> Flyouts { get; set; }
    }
}
