namespace SD.Infrastructure.WPF.Interfaces
{
    /// <summary>
    /// 需要初始化接口
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public interface IInitialize<in T>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="model">模型</param>
        void Initialize(T model);
    }
}
