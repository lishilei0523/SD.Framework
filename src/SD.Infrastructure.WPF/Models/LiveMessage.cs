using System.Windows.Media;

namespace SD.Infrastructure.WPF.Models
{
    /// <summary>
    /// 实时消息
    /// </summary>
    public class LiveMessage
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public LiveMessage() { }
        #endregion

        #region 01.创建实时消息构造器
        /// <summary>
        /// 创建实时消息构造器
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="color">颜色</param>
        public LiveMessage(string content, SolidColorBrush color)
            : this()
        {
            this.Content = content;
            this.Color = color;
        }
        #endregion

        #endregion

        #region # 属性

        #region 内容 —— string Content
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        #endregion

        #region 颜色 —— SolidColorBrush Color
        /// <summary>
        /// 颜色
        /// </summary>
        public SolidColorBrush Color { get; set; }
        #endregion 

        #endregion
    }
}
