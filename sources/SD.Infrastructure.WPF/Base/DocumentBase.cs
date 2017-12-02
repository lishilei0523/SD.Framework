using SD.Infrastructure.WPF.Toolkits;
using System.Windows.Input;

namespace SD.Infrastructure.WPF.Base
{
    /// <summary>
    /// 文档基类
    /// </summary>
    public abstract class DocumentBase : ElementBase
    {
        #region 属性

        #region 是否选中 —— bool Selected
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool _selected;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected
        {
            get { return this._selected; }
            protected set { this.Set(ref this._selected, value); }
        }
        #endregion

        #region 只读属性 - 关闭命令 —— ICommand CloseCommand
        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(x => this.Close());
            }
        }
        #endregion

        #endregion

        #region # 方法

        #region 打开 —— override void Open()
        /// <summary>
        /// 打开
        /// </summary>
        public override void Open()
        {
            if (ElementManager.Documents.Contains(this))
            {
                this.Active = true;
            }
            else
            {
                ElementManager.Documents.Add(this);
            }
        }
        #endregion

        #region 关闭 —— override void Close()
        /// <summary>
        /// 关闭
        /// </summary>
        public override void Close()
        {
            if (ElementManager.Documents.Contains(this))
            {
                ElementManager.Documents.Remove(this);
            }
        }
        #endregion

        #endregion
    }
}
