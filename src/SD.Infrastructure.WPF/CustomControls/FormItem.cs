using System.Windows;
using System.Windows.Controls;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 表单项
    /// </summary>
    public class FormItem : ContentControl
    {
        #region # 字段及构造器

        /// <summary>
        /// 标签依赖属性
        /// </summary>
        public static readonly DependencyProperty LabelProperty;

        /// <summary>
        /// 标签模板依赖属性
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty;

        /// <summary>
        /// 是否必填依赖属性
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty;

        /// <summary>
        /// 描述依赖属性
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static FormItem()
        {
            LabelProperty = DependencyProperty.Register(nameof(Label), typeof(object), typeof(FormItem), new PropertyMetadata(default, OnLabelChanged));
            LabelTemplateProperty = DependencyProperty.Register(nameof(LabelTemplate), typeof(DataTemplate), typeof(FormItem), new PropertyMetadata(default(DataTemplate), OnLabelTemplateChanged));
            IsRequiredProperty = DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(FormItem), new PropertyMetadata(default(bool)));
            DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(object), typeof(FormItem), new PropertyMetadata(default(object)));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public FormItem()
        {
            this.DefaultStyleKey = typeof(FormItem);
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 标签 —— object Label
        /// <summary>
        /// 依赖属性 - 标签
        /// </summary>
        public object Label
        {
            get => this.GetValue(LabelProperty);
            set => this.SetValue(LabelProperty, value);
        }
        #endregion

        #region 依赖属性 - 标签模板 —— DataTemplate LabelTemplate
        /// <summary>
        /// 依赖属性 - 标签模板
        /// </summary>
        public DataTemplate LabelTemplate
        {
            get => (DataTemplate)this.GetValue(LabelTemplateProperty);
            set => this.SetValue(LabelTemplateProperty, value);
        }
        #endregion

        #region 依赖属性 - 是否必填 —— bool IsRequired
        /// <summary>
        /// 依赖属性 - 是否必填
        /// </summary>
        public bool IsRequired
        {
            get => (bool)this.GetValue(IsRequiredProperty);
            set => this.SetValue(IsRequiredProperty, value);
        }
        #endregion

        #region 依赖属性 - 描述 —— object Description
        /// <summary>
        /// 依赖属性 - 描述
        /// </summary>
        public object Description
        {
            get => this.GetValue(DescriptionProperty);
            set => this.SetValue(DescriptionProperty, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 标签属性值更改事件 —— static void OnLabelChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 标签属性值更改事件
        /// </summary>
        private static void OnLabelChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            object oldValue = eventArgs.OldValue;
            object newValue = eventArgs.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            FormItem target = (FormItem)dependencyObject;
            target.OnLabelChanged(oldValue, newValue);
        }
        #endregion

        #region 标签模板属性值更改事件 —— static void OnLabelTemplateChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 标签模板属性值更改事件
        /// </summary>
        private static void OnLabelTemplateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DataTemplate oldValue = (DataTemplate)eventArgs.OldValue;
            DataTemplate newValue = (DataTemplate)eventArgs.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            FormItem target = (FormItem)dependencyObject;
            target.OnLabelTemplateChanged(oldValue, newValue);
        }
        #endregion

        #region 标签属性值更改事件 —— virtual void OnLabelChanged(object oldValue, object newValue)
        /// <summary>
        /// 标签属性值更改事件
        /// </summary>
        /// <param name="oldValue">标签旧值</param>
        /// <param name="newValue">标签新值</param>
        protected virtual void OnLabelChanged(object oldValue, object newValue)
        {

        }
        #endregion

        #region 标签模板属性值更改事件 —— virtual void OnLabelTemplateChanged(DataTemplate oldValue...
        /// <summary>
        /// 标签模板属性值更改事件
        /// </summary>
        /// <param name="oldValue">标签模板旧值</param>
        /// <param name="newValue">标签模板新值</param>
        protected virtual void OnLabelTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {

        }
        #endregion 

        #endregion
    }
}
