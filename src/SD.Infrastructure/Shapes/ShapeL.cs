using System.Drawing;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 形状基类
    /// </summary>
    public abstract class ShapeL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected ShapeL()
        {
            //默认值
            this.Fill = Color.Transparent;
            this.Stroke = Color.Red;
            this.StrokeThickness = 2;
            this.Tag = null;
        }
        #endregion

        #endregion

        #region # 属性

        #region 名称 —— string Name
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region 层次索引 —— int ZIndex
        /// <summary>
        /// 层次索引
        /// </summary>
        public int ZIndex { get; set; }
        #endregion

        #region 填充颜色 —— Color Fill
        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color Fill { get; set; }
        #endregion

        #region 边框颜色 —— Color Stroke
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color Stroke { get; set; }
        #endregion

        #region 边框宽度 —— int StrokeThickness
        /// <summary>
        /// 边框宽度
        /// </summary>
        public int StrokeThickness { get; set; }
        #endregion

        #region 自定义标签 —— object Tag
        /// <summary>
        /// 自定义标签
        /// </summary>
        public object Tag { get; set; }
        #endregion 

        #endregion
    }
}
