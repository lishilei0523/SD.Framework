using SD.Infrastructure.WPF.Models;

namespace SD.Infrastructure.WPF.Extensions
{
    /// <summary>
    /// 包裹模型扩展
    /// </summary>
    public static class WrapExtension
    {
        /// <summary>
        /// 包裹模型
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns>包裹模型</returns>
        public static Wrap<T> Wrap<T>(this T model)
        {
            Wrap<T> wrapModel = new Wrap<T>(model);

            return wrapModel;
        }

        /// <summary>
        /// 包裹模型
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="isChecked">是否勾选</param>
        /// <returns>包裹模型</returns>
        public static Wrap<T> Wrap<T>(this T model, bool? isChecked)
        {
            Wrap<T> wrapModel = new Wrap<T>(false, isChecked, model);

            return wrapModel;
        }

        /// <summary>
        /// 包裹模型
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="isSelected">是否选中</param>
        /// <param name="isChecked">是否勾选</param>
        /// <returns>包裹模型</returns>
        public static Wrap<T> Wrap<T>(this T model, bool? isSelected, bool? isChecked)
        {
            Wrap<T> wrapModel = new Wrap<T>(isSelected, isChecked, model);

            return wrapModel;
        }
    }
}
