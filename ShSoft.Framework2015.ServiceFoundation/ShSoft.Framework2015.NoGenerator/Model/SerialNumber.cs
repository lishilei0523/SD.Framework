using System;

namespace ShSoft.Framework2015.NoGenerator.Model
{
    /// <summary>
    /// 序列号字典
    /// </summary>
    internal class SerialNumber
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        internal SerialNumber()
        {
            //默认值
            this.Id = Guid.NewGuid();
            this.TodayCount = 1;
        }
        #endregion

        #region 02.创建序列号字典构造器
        /// <summary>
        /// 创建序列号字典构造器
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="formatDate">数据格式</param>
        /// <param name="className">类名</param>
        /// <param name="length">长度</param>
        /// <param name="description">描述</param>
        public SerialNumber(string prefix, string formatDate, string className, int length, string description)
            : this()
        {
            this.Prefix = prefix;
            this.FormatDate = formatDate;
            this.ClassName = className;
            this.Length = length;
            this.Description = string.IsNullOrWhiteSpace(description) ? className : description;
        }
        #endregion

        #endregion

        #region # 属性

        #region 标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        public Guid Id { get; internal set; }
        #endregion

        #region 前缀 —— string Prefix
        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; internal set; }
        #endregion

        #region 格式化日期 —— string FormatDate
        /// <summary>
        /// 格式化日期
        /// </summary>
        public string FormatDate { get; internal set; }
        #endregion

        #region 类名 —— string ClassName
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; internal set; }
        #endregion

        #region 流水号长度 —— int Length
        /// <summary>
        /// 流水号长度
        /// </summary>
        public int Length { get; internal set; }
        #endregion

        #region 当天流水号数 —— int TodayCount
        /// <summary>
        /// 当天流水号数
        /// </summary>
        public int TodayCount { get; internal set; }
        #endregion

        #region 描述 —— string Description
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; internal set; }
        #endregion

        #endregion

        #region # 方法

        #region 更新信息 —— void UpdateInfo(int todayCount, string description)
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="todayCount">当天流水号数</param>
        /// <param name="description">描述</param>
        public void UpdateInfo(int todayCount, string description)
        {
            this.TodayCount = todayCount;
            this.Description = string.IsNullOrWhiteSpace(description) ? this.ClassName : description;
        }
        #endregion

        #endregion
    }
}