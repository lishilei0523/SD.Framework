﻿namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 形状
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
            this.Fill = new ColorL(255, 255, 255, 0);
            this.Stroke = new ColorL(255, 0, 0);
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

        #region 填充颜色 —— ColorL Fill
        /// <summary>
        /// 填充颜色
        /// </summary>
        public ColorL Fill { get; set; }
        #endregion

        #region 边框颜色 —— ColorL Stroke
        /// <summary>
        /// 边框颜色
        /// </summary>
        public ColorL Stroke { get; set; }
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

        #region 只读属性 - 文本 —— string Text
        /// <summary>
        /// 只读属性 - 文本
        /// </summary>
        public string Text
        {
            get => this.ToString();
        }
        #endregion

        #endregion
    }
}
