using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace SD.Infrastructure.WPF.Base
{
    /// <summary>
    /// 飞窗基类
    /// </summary>
    public abstract class FlyoutBase : ElementBase
    {
        #region # 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        protected FlyoutBase()
        {
            this.ExecuteOk = false;
        }
        #endregion

        #region # 事件

        #region 飞窗关闭事件 —— event Action<FlyoutBase> FlyoutCloseEvent
        /// <summary>
        /// 飞窗关闭事件
        /// </summary>
        public event Action<FlyoutBase> FlyoutCloseEvent;
        #endregion

        #endregion

        #region # 属性

        #region 位置 —— Position Position
        /// <summary>
        /// 位置
        /// </summary>
        private Position _position;

        /// <summary>
        /// 位置
        /// </summary>
        public Position Position
        {
            get { return this._position; }
            protected set { this.Set(ref this._position, value); }
        }
        #endregion

        #region 外边距 —— Thickness Margin
        /// <summary>
        /// 外边距
        /// </summary>
        private Thickness _margin;

        /// <summary>
        /// 外边距
        /// </summary>
        public Thickness Margin
        {
            get { return this._margin; }
            protected set { this.Set(ref this._margin, value); }
        }
        #endregion

        #region 是否执行成功 —— bool ExecuteOk
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool ExecuteOk { get; protected set; }
        #endregion

        #endregion

        #region # 方法

        #region 打开 —— override void Open()
        /// <summary>
        /// 打开
        /// </summary>
        public override void Open()
        {
            this.Active = true;
        }
        #endregion

        #region 关闭 —— override void Close()
        /// <summary>
        /// 关闭
        /// </summary>
        public override void Close()
        {
            this.Active = false;

            if (this.ExecuteOk && this.FlyoutCloseEvent != null)
            {
                this.FlyoutCloseEvent.Invoke(this);
            }
        }
        #endregion

        #endregion
    }
}
