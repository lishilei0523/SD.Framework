using Avalonia.Media;
using System;

namespace SD.Infrastructure.Avalonia.Extensions
{
    /// <summary>
    /// 颜色扩展
    /// </summary>
    public static class ColorExtension
    {
        #region # 随机颜色 —— static Color RandomColor()
        /// <summary>
        /// 随机颜色
        /// </summary>
        /// <returns>颜色</returns>
        public static Color RandomColor()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            byte r = (byte)random.Next(0, 255);
            byte g = (byte)random.Next(0, 255);
            byte b = (byte)random.Next(0, 255);

            return Color.FromRgb(r, g, b);
        }
        #endregion

        #region # 反转颜色 —— static Color Invert(this Color color)
        /// <summary>
        /// 反转颜色
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>相反颜色</returns>
        public static Color Invert(this Color color)
        {
            byte nr = (byte)(255 - color.R);
            byte ng = (byte)(255 - color.G);
            byte nb = (byte)(255 - color.B);

            return Color.FromRgb(nr, ng, nb);
        }
        #endregion
    }
}
