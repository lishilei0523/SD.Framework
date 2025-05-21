namespace SD.Infrastructure.Avalonia.Interfaces
{
    /// <summary>
    /// 可繁忙接口
    /// </summary>
    public interface IBusy
    {
        /// <summary>
        /// 是否繁忙
        /// </summary>
        bool IsBusy { get; set; }
    }
}
