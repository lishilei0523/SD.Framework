using Caliburn.Micro;
using SD.IOC.Core.Mediator;
using System;
using System.Linq;

namespace SD.Infrastructure.WPF.Base
{
    /// <summary>
    /// 元素管理器基类
    /// </summary>
    public abstract class ElementManagerBase : Screen, IElementManager
    {
        #region # 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        protected ElementManagerBase()
        {
            this.Documents = new BindableCollection<DocumentBase>();
        }
        #endregion

        #region # 属性

        #region 文档列表 —— BindableCollection<DocumentBase> Documents
        /// <summary>
        /// 文档列表
        /// </summary>
        private BindableCollection<DocumentBase> _documents;

        /// <summary>
        /// 文档列表
        /// </summary>
        public BindableCollection<DocumentBase> Documents
        {
            get { return this._documents; }
            protected set { this.Set(ref this._documents, value); }
        }
        #endregion

        #region 飞窗 —— FlyoutBase Flyout
        /// <summary>
        /// 飞窗
        /// </summary>
        private FlyoutBase _flyout;

        /// <summary>
        /// 飞窗
        /// </summary>
        public FlyoutBase Flyout
        {
            get { return this._flyout; }
            protected set { this.Set(ref this._flyout, value); }
        }
        #endregion

        #endregion

        #region # 方法

        #region 打开文档 —— DocumentBase OpenDocument(Type type)
        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="type">文档类型</param>
        /// <returns>文档</returns>
        public DocumentBase OpenDocument(Type type)
        {
            //验证
            if (!type.IsSubclassOf(typeof(DocumentBase)))
            {
                throw new InvalidCastException("给定类型不是文档！");
            }

            DocumentBase document = this.Documents.SingleOrDefault(x => x.GetType().FullName == type.FullName);

            if (document == null)
            {
                document = (DocumentBase)ResolveMediator.Resolve(type);
                this.Documents.Add(document);
            }

            document.Open();

            return document;
        }
        #endregion

        #region 打开文档 —— T OpenDocument<T>()
        /// <summary>
        /// 打开文档
        /// </summary>
        /// <typeparam name="T">文档类型</typeparam>
        /// <returns>文档</returns>
        public T OpenDocument<T>() where T : DocumentBase
        {
            DocumentBase document = this.Documents.SingleOrDefault(x => x.GetType().FullName == typeof(T).FullName);

            if (document == null)
            {
                document = ResolveMediator.Resolve<T>();
                this.Documents.Add(document);
            }

            document.Open();

            return (T)document;
        }
        #endregion

        #region 打开飞窗 —— FlyoutBase OpenFlyout(Type type)
        /// <summary>
        /// 打开飞窗
        /// </summary>
        /// <param name="type">飞窗类型</param>
        /// <returns>飞窗</returns>
        public FlyoutBase OpenFlyout(Type type)
        {
            //验证
            if (!type.IsSubclassOf(typeof(FlyoutBase)))
            {
                throw new InvalidCastException("给定类型不是飞窗！");
            }

            this.Flyout = (FlyoutBase)ResolveMediator.Resolve(type);
            this.Flyout.Open();

            return this.Flyout;
        }
        #endregion

        #region 打开飞窗 —— T OpenFlyout<T>()
        /// <summary>
        /// 打开飞窗
        /// </summary>
        /// <typeparam name="T">飞窗类型</typeparam>
        /// <returns>飞窗</returns>
        public T OpenFlyout<T>() where T : FlyoutBase
        {
            this.Flyout = ResolveMediator.Resolve<T>();
            this.Flyout.Open();

            return (T)this.Flyout;
        }
        #endregion

        #endregion
    }
}
