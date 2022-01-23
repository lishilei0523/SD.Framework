using ToastNotifications.Position;

namespace SD.Infrastructure.WPF.Caliburn.Models
{
    /// <summary>
    /// 通知设置
    /// </summary>
    public class NotifierOptions
    {
        #region # 构造器

        #region 01.创建通知设置构造器
        /// <summary>
        /// 创建通知设置构造器
        /// </summary>
        /// <param name="location">位置</param>
        /// <param name="marginX">水平边距</param>
        /// <param name="marginY">垂直边距</param>
        /// <param name="lifetime">生命周期（秒）</param>
        /// <param name="maxCount">最大数量</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="showCloseButton">显示关闭按钮</param>
        /// <param name="freezeOnMouseEnter">鼠标进入冻结</param>
        /// <param name="unfreezeOnMouseLeave">鼠标离开取消冻结</param>
        public NotifierOptions(Corner location = Corner.BottomCenter, double marginX = 25, double marginY = 25, int lifetime = 5, int maxCount = 10, double fontSize = 14, bool showCloseButton = true, bool freezeOnMouseEnter = true, bool unfreezeOnMouseLeave = true)
        {
            this.Location = location;
            this.MarginX = marginX;
            this.MarginY = marginY;
            this.Lifetime = lifetime;
            this.MaxCount = maxCount;
            this.FontSize = fontSize;
            this.ShowCloseButton = showCloseButton;
            this.FreezeOnMouseEnter = freezeOnMouseEnter;
            this.UnfreezeOnMouseLeave = unfreezeOnMouseLeave;
        }
        #endregion 

        #endregion

        #region # 属性

        #region 位置 —— Corner Location
        /// <summary>
        /// 位置
        /// </summary>
        public Corner Location { get; set; }
        #endregion

        #region 水平边距 —— double MarginX
        /// <summary>
        /// 水平边距
        /// </summary>
        public double MarginX { get; set; }
        #endregion

        #region 垂直边距 —— double MarginY
        /// <summary>
        /// 垂直边距
        /// </summary>
        public double MarginY { get; set; }
        #endregion

        #region 生命周期（秒） —— int Lifetime
        /// <summary>
        /// 生命周期（秒）
        /// </summary>
        public int Lifetime { get; set; }
        #endregion

        #region 最大数量 —— int MaxCount
        /// <summary>
        /// 最大数量
        /// </summary>
        public int MaxCount { get; set; }
        #endregion

        #region 字体大小 —— double FontSize
        /// <summary>
        /// 字体大小
        /// </summary>
        public double FontSize { get; set; }
        #endregion

        #region 显示关闭按钮 —— bool ShowCloseButton
        /// <summary>
        /// 显示关闭按钮
        /// </summary>
        public bool ShowCloseButton { get; set; }
        #endregion

        #region 鼠标进入冻结 —— bool FreezeOnMouseEnter
        /// <summary>
        /// 鼠标进入冻结
        /// </summary>
        public bool FreezeOnMouseEnter { get; set; }
        #endregion

        #region 鼠标离开取消冻结 —— bool UnfreezeOnMouseLeave
        /// <summary>
        /// 鼠标离开取消冻结
        /// </summary>
        public bool UnfreezeOnMouseLeave { get; set; }
        #endregion

        #endregion
    }
}
