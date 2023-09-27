using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Draggable.Controls
{
    /// <summary>
    /// 可拖拽控件
    /// </summary>
    public class DraggableControl : DraggableControlBase
    {
        #region # 字段及构造器

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DraggableControl()
        {
            TargetElementProperty = DependencyProperty.Register(nameof(TargetElement), typeof(FrameworkElement), typeof(DraggableControl), new FrameworkPropertyMetadata(default(FrameworkElement), DraggableControl.OnTargetElementChanged));
            IsSelectableProperty = DependencyProperty.RegisterAttached(nameof(IsSelectable), typeof(bool), typeof(DraggableControlBase), new PropertyMetadata(false));
            IsEditableProperty = DependencyProperty.RegisterAttached(nameof(IsEditable), typeof(bool), typeof(DraggableControlBase), new PropertyMetadata(false));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public DraggableControl()
        {
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        #endregion

        #region # 依赖属性

        #region 目标元素 —— FrameworkElement TargetElement

        /// <summary>
        /// 目标元素
        /// </summary>
        public static DependencyProperty TargetElementProperty;

        /// <summary>
        /// 目标元素
        /// </summary>
        public FrameworkElement TargetElement
        {
            get => (FrameworkElement)base.GetValue(TargetElementProperty);
            set => base.SetValue(TargetElementProperty, value);
        }

        #endregion 

        #region 是否可编辑 —— DependencyProperty IsEditable

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public static DependencyProperty IsEditableProperty;

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public static DependencyProperty IsEditable
        {
            get => IsEditableProperty;
            set => IsEditableProperty = value;
        }

        /// <summary>
        /// 获取是否可编辑
        /// </summary>
        public static bool GetIsEditable(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsEditableProperty);
        }

        /// <summary>
        /// 设置是否可编辑
        /// </summary>
        public static void SetIsEditable(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsEditableProperty, value);
        }

        #endregion

        #region 是否可选择 —— DependencyProperty IsSelectable

        /// <summary>
        /// 是否可选择
        /// </summary>
        public static DependencyProperty IsSelectableProperty;

        /// <summary>
        /// 是否可选择
        /// </summary>
        public static DependencyProperty IsSelectable
        {
            get => IsSelectableProperty;
            set => IsSelectableProperty = value;
        }

        /// <summary>
        /// 获取是否可选择
        /// </summary>
        public static bool GetIsSelectable(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsSelectableProperty);
        }

        /// <summary>
        /// 设置是否可选择
        /// </summary>
        public static void SetIsSelectable(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsSelectableProperty, value);
        }

        #endregion

        #endregion

        #region # 回调方法

        #region 控件已加载回调方法 —— void OnLoaded(object sender, RoutedEventArgs eventArgs)
        /// <summary>
        /// 控件已加载回调方法
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs eventArgs)
        {
            this.AttachParentEvents();
            this.Loaded -= this.OnLoaded;
        }
        #endregion

        #region 控件已卸载回调方法 —— void OnUnloaded(object sender, RoutedEventArgs eventArgs)
        /// <summary>
        /// 控件已卸载回调方法
        /// </summary>
        private void OnUnloaded(object sender, RoutedEventArgs eventArgs)
        {
            this.DetachParentEvents();
            this.Unloaded -= this.OnUnloaded;
        }
        #endregion

        #region 目标元素改变回调方法 —— static void OnTargetElementChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 目标元素改变回调方法
        /// </summary>
        private static void OnTargetElementChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            #region # 验证

            if (!(dependencyObject is DraggableControl))
            {
                return;
            }

            #endregion

            FrameworkElement oldElement = eventArgs.OldValue as FrameworkElement;
            DraggableControl draggableControl = (DraggableControl)dependencyObject;
            draggableControl.DetachTatgetEvents(oldElement);
            FrameworkElement newElement = eventArgs.NewValue as FrameworkElement;

            #region # 验证

            if (newElement == null ||
                newElement is DraggableControl ||
                newElement.RenderSize.IsEmpty ||
                double.IsNaN(newElement.RenderSize.Width) ||
                double.IsNaN(newElement.RenderSize.Height) ||
                !GetIsSelectable(newElement))
            {
                draggableControl.Visibility = Visibility.Collapsed;
                return;
            }

            #endregion

            draggableControl.AttachTatgetEvents(newElement);

            int zIndex = Panel.GetZIndex(newElement);
            if (zIndex >= Panel.GetZIndex(draggableControl))
            {
                Panel.SetZIndex(draggableControl, zIndex + 1);
            }

            double x = CorrectDoubleValue(Canvas.GetLeft(newElement));
            double y = CorrectDoubleValue(Canvas.GetTop(newElement));

            Canvas.SetLeft(draggableControl, x);
            Canvas.SetTop(draggableControl, y);

            draggableControl.Width = newElement.ActualWidth;
            draggableControl.Height = newElement.ActualHeight;
        }
        #endregion

        #region 上级元素鼠标左键按下回调方法 —— void OnParentMouseLeftButtonDown(object sender...
        /// <summary>
        /// 上级元素鼠标左键按下回调方法
        /// </summary>
        private void OnParentMouseLeftButtonDown(object sender, MouseButtonEventArgs eventArgs)
        {
            FrameworkElement selectedElement = eventArgs.OriginalSource as FrameworkElement;
            this.TargetElement = this.CheckTargetIsSelectable(selectedElement) ? selectedElement : null;

            selectedElement?.Focus();
        }
        #endregion

        #region 目标元素获得焦点事件回调方法 —— void OnTargetElementGotFocus(object sender...
        /// <summary>
        /// 目标元素获得焦点事件回调方法
        /// </summary>
        private void OnTargetElementGotFocus(object sender, RoutedEventArgs eventArgs)
        {
            this.Visibility = Visibility.Visible;
        }
        #endregion

        #region 目标元素失去焦点事件回调方法 —— void OnTargetElementLostFocus(object sender...
        /// <summary>
        /// 目标元素失去焦点事件回调方法
        /// </summary>
        private void OnTargetElementLostFocus(object sender, RoutedEventArgs eventArgs)
        {
            this.TargetElement = null;
            this.Visibility = Visibility.Collapsed;
        }
        #endregion

        #endregion

        #region # 方法

        #region 获取目标是否可编辑 —— override bool GetTargetIsEditable()
        /// <summary>
        /// 获取目标是否可编辑
        /// </summary>
        /// <returns>是否可编辑</returns>
        protected override bool GetTargetIsEditable()
        {
            return GetIsEditable(this.TargetElement);
        }
        #endregion

        #region 获取目标实际边界 —— override Rect GetTargetActualBound()
        /// <summary>
        /// 获取目标实际边界
        /// </summary>
        /// <returns>实际边界</returns>
        protected override Rect GetTargetActualBound()
        {
            #region # 验证

            if (this.TargetElement == null)
            {
                return Rect.Empty;
            }

            #endregion

            double left = CorrectDoubleValue(Canvas.GetLeft(this.TargetElement));
            double top = CorrectDoubleValue(Canvas.GetTop(this.TargetElement));

            return new Rect(left, top, this.TargetElement.ActualWidth, this.TargetElement.ActualHeight);
        }
        #endregion

        #region 设置目标实际边界 —— override void SetTargetActualBound(Rect newBound)
        /// <summary>
        /// 设置目标实际边界
        /// </summary>
        /// <param name="newBound">新边界</param>
        protected override void SetTargetActualBound(Rect newBound)
        {
            if (this.TargetElement != null)
            {
                this.TargetElement.Width = newBound.Width;
                this.TargetElement.Height = newBound.Height;
                Canvas.SetLeft(this.TargetElement, newBound.X);
                Canvas.SetTop(this.TargetElement, newBound.Y);
            }
        }
        #endregion

        #region 挂起拖拽改变事件 —— override void RaiseDragChangingEvent(Rect newBound)
        /// <summary>
        /// 挂起拖拽改变事件
        /// </summary>
        /// <param name="newBound">新边界</param>
        protected override void RaiseDragChangingEvent(Rect newBound)
        {
            DraggedEventArgs eventArgs = new DraggedEventArgs(DragChangingEvent, newBound, this.TargetElement);
            base.RaiseEvent(eventArgs);
        }
        #endregion

        #region 挂起拖拽完成事件 —— override void RaiseDragCompletedEvent(Rect newBound)
        /// <summary>
        /// 挂起拖拽完成事件
        /// </summary>
        /// <param name="newBound">新边界</param>
        protected override void RaiseDragCompletedEvent(Rect newBound)
        {
            DraggedEventArgs eventArgs = new DraggedEventArgs(DragCompletedEvent, newBound, this.TargetElement);
            base.RaiseEvent(eventArgs);
        }
        #endregion

        #region 附加上级元素事件 —— void AttachParentEvents()
        /// <summary>
        /// 附加上级元素事件
        /// </summary>
        private void AttachParentEvents()
        {
            if (!(this.Parent is Canvas canvas))
            {
                throw new InvalidOperationException("DraggableControl must place into Canvas!");
            }

            canvas.MouseLeftButtonDown += this.OnParentMouseLeftButtonDown;
        }
        #endregion

        #region 脱离上级元素事件 —— void DetachParentEvents()
        /// <summary>
        /// 脱离上级元素事件
        /// </summary>
        private void DetachParentEvents()
        {
            if (!(this.Parent is Canvas canvas))
            {
                throw new InvalidOperationException("DraggableControl must place into Canvas!");
            }

            canvas.MouseLeftButtonDown -= this.OnParentMouseLeftButtonDown;
        }
        #endregion

        #region 附加目标元素事件 —— void AttachTatgetEvents(FrameworkElement targetElement)
        /// <summary>
        /// 附加目标元素事件
        /// </summary>
        /// <param name="targetElement">目标元素</param>
        private void AttachTatgetEvents(FrameworkElement targetElement)
        {
            #region # 验证

            if (targetElement == null)
            {
                throw new ArgumentNullException(nameof(targetElement));
            }

            #endregion

            targetElement.Focusable = true;
            targetElement.GotFocus += this.OnTargetElementGotFocus;
            targetElement.LostFocus += this.OnTargetElementLostFocus;

            double thickness = 1.0;
            if (targetElement is Shape shape)
            {
                thickness = shape.StrokeThickness;
            }

            bool editable = GetIsEditable(targetElement);
            this.InitVisualProperties(thickness, editable);
        }
        #endregion

        #region 脱离目标元素事件 —— void DetachTatgetEvents(FrameworkElement targetElement)
        /// <summary>
        /// 脱离目标元素事件
        /// </summary>
        /// <param name="targetElement">目标元素</param>
        private void DetachTatgetEvents(FrameworkElement targetElement)
        {
            if (targetElement != null)
            {
                targetElement.GotFocus -= this.OnTargetElementGotFocus;
                targetElement.LostFocus -= this.OnTargetElementLostFocus;
            }
        }
        #endregion

        #region 检查目标是否可选择 —— bool CheckTargetIsSelectable(FrameworkElement targetElement)
        /// <summary>
        /// 检查目标是否可选择
        /// </summary>
        /// <param name="targetElement">目标元素</param>
        /// <returns>是否可选择</returns>
        private bool CheckTargetIsSelectable(FrameworkElement targetElement)
        {
            return (targetElement != null) && !targetElement.Equals(this.Parent) && !targetElement.Equals(this) && GetIsSelectable(targetElement);
        }
        #endregion 

        #endregion
    }
}
