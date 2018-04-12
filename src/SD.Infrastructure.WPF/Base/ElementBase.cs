using Caliburn.Micro;

namespace SD.Infrastructure.WPF.Base
{
    /// <summary>
    /// 元素基类
    /// </summary>
    public abstract class ElementBase : PropertyChangedBase
    {
        /// <summary>
        /// 构造器
        /// </summary>
        protected ElementBase()
        {
            this.Active = false;
        }

        #region # 属性

        #region 标题 —— abstract string Title
        /// <summary>
        /// 标题
        /// </summary>
        public abstract string Title { get; }
        #endregion

        #region 是否活动 —— bool Active
        /// <summary>
        /// 是否活动
        /// </summary>
        private bool _active;

        /// <summary>
        /// 是否活动
        /// </summary>
        public bool Active
        {
            get { return this._active; }
            protected set { this.Set(ref this._active, value); }
        }
        #endregion

        #endregion

        #region # 方法

        #region 打开 —— abstract void Open()
        /// <summary>
        /// 打开
        /// </summary>
        public abstract void Open();
        #endregion

        #region 关闭 —— void Close()
        /// <summary>
        /// 关闭
        /// </summary>
        public abstract void Close();
        #endregion

        #endregion
    }
}
