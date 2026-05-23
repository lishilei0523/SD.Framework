using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SD.Infrastructure.Avalonia.Models
{
    /// <summary>
    /// Grid单元格
    /// </summary>
    public class GridCell : INotifyPropertyChanged
    {
        #region # 字段及构造器

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 默认构造器
        /// </summary>
        public GridCell()
        {
            this.RowSpan = 1;
            this.ColumnSpan = 1;
            this.IsVisible = true;
        }

        /// <summary>
        /// 创建Grid单元格构造器
        /// </summary>
        /// <param name="row">行索引</param>
        /// <param name="column">列索引</param>
        public GridCell(int row, int column)
            : this()
        {
            this.Row = row;
            this.Column = column;
        }

        /// <summary>
        /// 创建Grid单元格构造器
        /// </summary>
        /// <param name="row">行索引</param>
        /// <param name="column">列索引</param>
        /// <param name="rowSpan">跨行数</param>
        /// <param name="columnSpan">跨列数</param>
        public GridCell(int row, int column, int rowSpan, int columnSpan)
            : this()
        {
            this.Row = row;
            this.Column = column;
            this.RowSpan = rowSpan;
            this.ColumnSpan = columnSpan;
        }

        #endregion

        #region # 属性

        #region 行索引 —— int Row
        /// <summary>
        /// 行索引
        /// </summary>
        public int Row
        {
            get;
            set
            {
                field = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        #region 列索引 —— int Column
        /// <summary>
        /// 列索引
        /// </summary>
        public int Column
        {
            get;
            set
            {
                field = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        #region 跨行数 —— int RowSpan
        /// <summary>
        /// 跨行数
        /// </summary>
        public int RowSpan
        {
            get;
            set
            {
                field = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        #region 跨列数 —— int ColumnSpan
        /// <summary>
        /// 跨列数
        /// </summary>
        public int ColumnSpan
        {
            get;
            set
            {
                field = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        #region 是否可见 —— bool IsVisible
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible
        {
            get;
            set
            {
                field = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region # 方法

        #region 触发属性变更通知 —— virtual void OnPropertyChanged(string propertyName)
        /// <summary>
        /// 触发属性变更通知
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #endregion
    }
}
