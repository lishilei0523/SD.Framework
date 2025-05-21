using System;
using System.ComponentModel;

namespace SD.Infrastructure.Avalonia.Models
{
    /// <summary>
    /// 数据项
    /// </summary>
    public class Item : INotifyPropertyChanged
    {
        #region # 字段及构造器

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 无参构造器
        /// </summary>
        public Item()
        {
            //默认值
            this.IsSelected = false;
            this.IsChecked = false;
        }

        /// <summary>
        /// 创建数据项构造器
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <param name="number">编号</param>
        /// <param name="name">名称</param>
        /// <param name="isSelected">是否选中</param>
        /// <param name="isChecked">是否勾选</param>
        /// <param name="groupKey">分组键</param>
        public Item(Guid id, string number, string name, bool? isSelected, bool? isChecked, string groupKey = null)
            : this()
        {
            this.Id = id;
            this.Number = number;
            this.Name = name;
            this.IsSelected = isSelected;
            this.IsChecked = isChecked;
            this.GroupKey = groupKey;
        }

        #endregion

        #region # 属性

        #region 标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        public Guid Id { get; set; }
        #endregion

        #region 编号 —— string Number
        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }
        #endregion

        #region 名称 —— string Name
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region 是否选中 —— bool? IsSelected

        /// <summary>
        /// 是否选中
        /// </summary>
        private bool? _isSelected;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool? IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this._isSelected = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsSelected)));
            }
        }
        #endregion

        #region 是否勾选 —— bool? IsChecked

        /// <summary>
        /// 是否勾选
        /// </summary>
        private bool? _isChecked;

        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool? IsChecked
        {
            get
            {
                return this._isChecked;
            }
            set
            {
                this._isChecked = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsChecked)));
            }
        }
        #endregion

        #region 图标 —— string Icon
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        #endregion

        #region 分组键 —— string GroupKey
        /// <summary>
        /// 分组键
        /// </summary>
        public string GroupKey { get; set; }
        #endregion

        #endregion
    }
}
