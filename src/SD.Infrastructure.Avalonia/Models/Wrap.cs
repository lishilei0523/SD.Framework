using System.ComponentModel;

namespace SD.Infrastructure.Avalonia.Models
{
    /// <summary>
    /// 包裹模型
    /// </summary>
    public class Wrap<T> : INotifyPropertyChanged
    {
        #region # 构造器

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 无参构造器
        /// </summary>
        public Wrap()
        {
            //默认值
            this.IsSelected = false;
            this.IsChecked = false;
        }

        /// <summary>
        /// 创建包裹模型构造器
        /// </summary>
        /// <param name="model">数据模型</param>
        public Wrap(T model)
            : this()
        {
            this.Model = model;
        }

        /// <summary>
        /// 创建包裹模型构造器
        /// </summary>
        /// <param name="isSelected">是否选中</param>
        /// <param name="isChecked">是否勾选</param>
        /// <param name="model">数据模型</param>
        public Wrap(bool? isSelected, bool? isChecked, T model)
        {
            this.IsSelected = isSelected;
            this.IsChecked = isChecked;
            this.Model = model;
        }

        #endregion

        #region # 属性

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

        #region 数据模型 —— T Model
        /// <summary>
        /// 数据模型
        /// </summary>
        public T Model { get; set; }
        #endregion 

        #endregion
    }
}
