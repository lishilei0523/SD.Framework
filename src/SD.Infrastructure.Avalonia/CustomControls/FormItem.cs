using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using System;

namespace SD.Infrastructure.Avalonia.CustomControls
{
    /// <summary>
    /// 表单项
    /// </summary>
    public class FormItem : ContentControl
    {
        /// <summary>
        /// 标识 IsRequired 依赖属性。
        /// </summary>
        public static readonly StyledProperty<bool> IsRequiredProperty;

        /// <summary>
        /// 标识 Description 依赖属性。
        /// </summary>
        public static readonly StyledProperty<object> DescriptionProperty;

        /// <summary>
        /// 标识 Label 依赖属性。
        /// </summary>
        public static readonly StyledProperty<object> LabelProperty;

        /// <summary>
        /// 标识 LabelTemplate 依赖属性。
        /// </summary>
        public static readonly StyledProperty<DataTemplate> LabelTemplateProperty;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static FormItem()
        {
            IsRequiredProperty = AvaloniaProperty.Register<FormItem, bool>(nameof(IsRequired));
            DescriptionProperty = AvaloniaProperty.Register<FormItem, object>(nameof(Description));
            LabelProperty = AvaloniaProperty.Register<FormItem, object>(nameof(Label));
            LabelTemplateProperty = AvaloniaProperty.Register<FormItem, DataTemplate>(nameof(LabelTemplate));
            LabelProperty.Changed.AddClassHandler<FormItem>(OnLabelChanged);
            LabelTemplateProperty.Changed.AddClassHandler<FormItem>(OnLabelTemplateChanged);
        }

        /// <summary>
        /// 获取或设置Label的值
        /// </summary>
        public object Label
        {
            get => this.GetValue(LabelProperty);
            set => this.SetValue(LabelProperty, value);
        }

        /// <summary>
        /// 获取或设置LabelTemplate的值
        /// </summary>
        public DataTemplate LabelTemplate
        {
            get => this.GetValue(FormItem.LabelTemplateProperty);
            set => this.SetValue(LabelTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置Description的值
        /// </summary>
        public object Description
        {
            get => this.GetValue(DescriptionProperty);
            set => this.SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// 获取或设置IsRequired的值
        /// </summary>
        public bool IsRequired
        {
            get => this.GetValue(FormItem.IsRequiredProperty);
            set => this.SetValue(IsRequiredProperty, value);
        }

        /// <summary>
        /// Gets the type by which the element is styled.
        /// </summary>
        protected override Type StyleKeyOverride
        {
            get => typeof(FormItem);
        }

        /// <summary>
        /// Label 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Label 属性的旧值。</param>
        /// <param name="newValue">Label 属性的新值。</param>
        protected virtual void OnLabelChanged(object oldValue, object newValue)
        {

        }

        /// <summary>
        /// LabelTemplate 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">LabelTemplate 属性的旧值。</param>
        /// <param name="newValue">LabelTemplate 属性的新值。</param>
        protected virtual void OnLabelTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {

        }

        private static void OnLabelChanged(FormItem sender, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            object oldValue = eventArgs.OldValue;
            object newValue = eventArgs.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            sender?.OnLabelChanged(oldValue, newValue);
        }

        private static void OnLabelTemplateChanged(FormItem sender, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            DataTemplate oldValue = (DataTemplate)eventArgs.OldValue;
            DataTemplate newValue = (DataTemplate)eventArgs.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            sender?.OnLabelTemplateChanged(oldValue, newValue);
        }
    }
}
