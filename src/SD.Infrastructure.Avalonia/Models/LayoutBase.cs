using Avalonia.Collections;
using Avalonia.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SD.Infrastructure.Avalonia.Models
{
    /// <summary>
    /// 布局
    /// </summary>
    public abstract class LayoutBase : INotifyPropertyChanged
    {
        #region # 字段及构造器

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 默认构造器
        /// </summary>
        protected LayoutBase()
        {
            this.RowDefinitions = [];
            this.ColumnDefinitions = [];
        }

        /// <summary>
        /// 创建布局构造器
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        protected LayoutBase(int rows, int columns)
            : this()
        {
            this.Rows = rows;
            this.Columns = columns;
        }

        #endregion

        #region # 属性

        #region 行数 —— int Rows
        /// <summary>
        /// 行数
        /// </summary>
        public int Rows
        {
            get;
            set
            {
                field = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.RowDefinitions));
            }
        }
        #endregion

        #region 列数 —— int Columns
        /// <summary>
        /// 列数
        /// </summary>
        public int Columns
        {
            get;
            set
            {
                field = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.ColumnDefinitions));
            }
        }
        #endregion

        #region 行定义列表 —— AvaloniaList<RowDefinition> RowDefinitions
        /// <summary>
        /// 行定义列表
        /// </summary>
        public AvaloniaList<RowDefinition> RowDefinitions { get; set; }
        #endregion

        #region 列定义列表 —— AvaloniaList<ColumnDefinition> ColumnDefinitions
        /// <summary>
        /// 列定义列表
        /// </summary>
        public AvaloniaList<ColumnDefinition> ColumnDefinitions { get; set; }
        #endregion

        #region 只读属性 - 单元格列表 —— abstract GridCell[] Cells
        /// <summary>
        /// 只读属性 - 单元格列表
        /// </summary>
        public abstract GridCell[] Cells { get; }
        #endregion

        #endregion

        #region # 方法

        #region 构建行列定义 —— virtual void BuildDefinitions()
        /// <summary>
        /// 构建行列定义
        /// </summary>
        public virtual void BuildDefinitions()
        {
            this.RowDefinitions.Clear();
            this.ColumnDefinitions.Clear();
            for (int row = 0; row < this.Rows; row++)
            {
                RowDefinition rowDefinition = new RowDefinition(GridLength.Star);
                this.RowDefinitions.Add(rowDefinition);
            }
            for (int col = 0; col < this.Columns; col++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition(GridLength.Star);
                this.ColumnDefinitions.Add(columnDefinition);
            }
        }
        #endregion

        #region 触发属性变更通知 —— virtual void OnPropertyChanged(string propertyName)
        /// <summary>
        /// 触发属性变更通知
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #endregion
    }
}
