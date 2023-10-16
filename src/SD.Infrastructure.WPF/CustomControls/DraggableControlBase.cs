using SD.Infrastructure.WPF.Constants;
using SD.Infrastructure.WPF.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 可拖拽控件基类
    /// </summary>
    [TemplatePart(Name = MainGrid, Type = typeof(Grid))]
    public abstract class DraggableControlBase : ContentControl
    {
        #region # 字段及构造器

        /// <summary>
        /// 主窗格名称
        /// </summary>
        private const string MainGrid = "PART_MainGrid";

        /// <summary>
        /// 最小尺寸
        /// </summary>
        private const double MinSize = 10;

        /// <summary>
        /// 主窗格
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private Grid _mainGrid;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DraggableControlBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DraggableControlBase), new FrameworkPropertyMetadata(typeof(DraggableControlBase)));
            CornerWidthProperty = DependencyProperty.Register(nameof(CornerWidth), typeof(int), typeof(DraggableControlBase), new PropertyMetadata(31));
            DragCompletedEvent = EventManager.RegisterRoutedEvent(nameof(DragCompleted), RoutingStrategy.Bubble, typeof(DraggedEventHandler), typeof(DraggableControlBase));
            DragChangingEvent = EventManager.RegisterRoutedEvent(nameof(DragChanging), RoutingStrategy.Bubble, typeof(DraggedEventHandler), typeof(DraggableControlBase));
        }

        /// <summary>
        /// 默认构造器
        /// </summary>
        protected DraggableControlBase()
        {
            base.SetResourceReference(StyleProperty, typeof(DraggableControlBase));
        }

        #endregion

        #region # 依赖属性

        #region 角标宽度 —— int CornerWidth

        /// <summary>
        /// 角标宽度依赖属性
        /// </summary>
        public static DependencyProperty CornerWidthProperty;

        /// <summary>
        /// 角标宽度
        /// </summary>
        public int CornerWidth
        {
            get => (int)base.GetValue(CornerWidthProperty);
            set => base.SetValue(CornerWidthProperty, value);
        }

        #endregion

        #endregion

        #region # 路由事件

        #region 拖拽改变事件 —— RoutedEvent DragChanging

        /// <summary>
        /// 拖拽改变事件
        /// </summary>
        protected static readonly RoutedEvent DragChangingEvent;

        /// <summary>
        /// 拖拽改变事件处理程序
        /// </summary>
        public event DraggedEventHandler DragChanging
        {
            add => this.AddHandler(DragChangingEvent, value);
            remove => this.RemoveHandler(DragChangingEvent, value);
        }

        #endregion

        #region 拖拽完成事件 —— DraggedEventHandler DragCompleted

        /// <summary>
        /// 拖拽完成事件
        /// </summary>
        protected static readonly RoutedEvent DragCompletedEvent;

        /// <summary>
        /// 拖拽完成事件处理程序
        /// </summary>
        public event DraggedEventHandler DragCompleted
        {
            add => this.AddHandler(DragCompletedEvent, value);
            remove => this.RemoveHandler(DragCompletedEvent, value);
        }

        #endregion

        #endregion

        #region # 属性

        #region 只读属性 - 上级元素 —— FrameworkElement ParentElement
        /// <summary>
        /// 上级元素
        /// </summary>
        protected FrameworkElement ParentElement
        {
            get => this.Parent as FrameworkElement;
        }
        #endregion

        #endregion

        #region # 方法

        #region 适用元素模板 —— override void OnApplyTemplate()
        /// <summary>
        /// 适用元素模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._mainGrid = (Grid)this.Template.FindName(MainGrid, this);
            this.AddLogicalChild(this._mainGrid);

            this.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
            this.AddHandler(Thumb.DragCompletedEvent, new RoutedEventHandler(this.OnDragCompleted));

            this.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region 拖拽移动回调方法 —— void OnDragDelta(object sender, DragDeltaEventArgs eventArgs)
        /// <summary>
        /// 拖拽移动回调方法
        /// </summary>
        private void OnDragDelta(object sender, DragDeltaEventArgs eventArgs)
        {
            #region # 验证

            if (!this.GetTargetIsEditable())
            {
                eventArgs.Handled = true;
                return;
            }
            if (!(eventArgs.OriginalSource is StretchableThumb stretchableThumb))
            {
                return;
            }

            #endregion

            double verticalChange = eventArgs.VerticalChange;
            double horizontalChange = eventArgs.HorizontalChange;

            Rect newBound = stretchableThumb.DragDirection == DragDirection.MiddleCenter
                ? this.DragElement(horizontalChange, verticalChange)
                : this.ResizeElement(stretchableThumb, horizontalChange, verticalChange);

            this.RaiseDragChangingEvent(newBound);
            this.SetTargetActualBound(newBound);

            eventArgs.Handled = true;
        }
        #endregion

        #region 拖拽完成回调方法 —— void OnDragCompleted(object sender, RoutedEventArgs eventArgs)
        /// <summary>
        /// 拖拽完成回调方法
        /// </summary>
        private void OnDragCompleted(object sender, RoutedEventArgs eventArgs)
        {
            Rect newBound = new Rect
            {
                Y = Canvas.GetTop(this),
                X = Canvas.GetLeft(this),
                Width = this.ActualWidth,
                Height = this.ActualHeight
            };

            this.RaiseDragCompletedEvent(newBound);

            eventArgs.Handled = true;
        }
        #endregion 

        #region 初始化视觉元素属性 —— void InitVisualProperties(double thickness, bool editable)
        /// <summary>
        /// 初始化视觉元素属性
        /// </summary>
        /// <param name="thickness">粗细</param>
        /// <param name="editable">是否可编辑</param>
        protected void InitVisualProperties(double thickness, bool editable)
        {
            Visibility cornerVisibe = editable ? Visibility.Visible : Visibility.Collapsed;
            double actualMargin = (this.CornerWidth - thickness) / 2.0;
            this._mainGrid.Margin = new Thickness(0 - actualMargin);
            foreach (StretchableThumb stretchableThumb in this._mainGrid.Children)
            {
                if (stretchableThumb != null)
                {
                    stretchableThumb.BorderThickness = new Thickness(thickness);
                    if (stretchableThumb.DragDirection == DragDirection.MiddleCenter)
                    {
                        stretchableThumb.Margin = new Thickness(actualMargin);
                    }
                    else
                    {
                        stretchableThumb.Visibility = cornerVisibe;
                    }
                }
            }
        }
        #endregion


        #region 获取目标是否可编辑 —— abstract bool GetTargetIsEditable()
        /// <summary>
        /// 获取目标是否可编辑
        /// </summary>
        /// <returns>是否可编辑</returns>
        protected abstract bool GetTargetIsEditable();
        #endregion

        #region 获取目标实际边界 —— abstract Rect GetTargetActualBound()
        /// <summary>
        /// 获取目标实际边界
        /// </summary>
        /// <returns>实际边界</returns>
        protected abstract Rect GetTargetActualBound();
        #endregion

        #region 设置目标实际边界 —— abstract void SetTargetActualBound(Rect newBound)
        /// <summary>
        /// 设置目标实际边界
        /// </summary>
        /// <param name="newBound">新边界</param>
        protected abstract void SetTargetActualBound(Rect newBound);
        #endregion

        #region 挂起拖拽改变事件 —— abstract void RaiseDragChangingEvent(Rect newBound)
        /// <summary>
        /// 挂起拖拽改变事件
        /// </summary>
        /// <param name="newBound">新边界</param>
        protected abstract void RaiseDragChangingEvent(Rect newBound);
        #endregion

        #region 挂起拖拽完成事件 —— abstract void RaiseDragCompletedEvent(Rect newBound)
        /// <summary>
        /// 挂起拖拽完成事件
        /// </summary>
        /// <param name="newBound">新边界</param>
        protected abstract void RaiseDragCompletedEvent(Rect newBound);
        #endregion


        #region 拖拽元素 —— Rect DragElement(double horizontalChange, double verticalChange)
        /// <summary>
        /// 拖拽元素
        /// </summary>
        /// <param name="horizontalChange">水平变化</param>
        /// <param name="verticalChange">垂直变化</param>
        /// <returns>新边界</returns>
        private Rect DragElement(double horizontalChange, double verticalChange)
        {
            Rect targetActualBound = this.GetTargetActualBound();

            double oldTop = CorrectDoubleValue(targetActualBound.Y);
            double oldLeft = CorrectDoubleValue(targetActualBound.X);
            double newTop = CorrectDoubleValue(oldTop + verticalChange);
            double newLeft = CorrectDoubleValue(oldLeft + horizontalChange);

            newTop = CorrectTop(this.ParentElement, newTop, targetActualBound.Height);
            newLeft = CorrectLeft(this.ParentElement, newLeft, targetActualBound.Width);

            Canvas.SetTop(this, newTop);
            Canvas.SetLeft(this, newLeft);

            Rect newBound = new Rect(newLeft, newTop, targetActualBound.Width, targetActualBound.Height);

            return newBound;
        }
        #endregion

        #region 调整元素大小 —— Rect ResizeElement(StretchableThumb hitedThumb, double horizontalChange...
        /// <summary>
        /// 调整元素大小
        /// </summary>
        /// <param name="hitedThumb">选中可伸缩拖动控件</param>
        /// <param name="horizontalChange">水平变化</param>
        /// <param name="verticalChange">垂直变化</param>
        /// <returns>新边界</returns>
        private Rect ResizeElement(StretchableThumb hitedThumb, double horizontalChange, double verticalChange)
        {
            #region # 验证

            if (hitedThumb == null)
            {
                return Rect.Empty;
            }

            #endregion

            Rect targetActualBound = this.GetTargetActualBound();

            double oldTop = CorrectDoubleValue(targetActualBound.Y);
            double oldLeft = CorrectDoubleValue(targetActualBound.X);
            double oldWidth = CorrectDoubleValue(targetActualBound.Width);
            double oldHeight = CorrectDoubleValue(targetActualBound.Height);

            double newTop = oldTop;
            double newLeft = oldLeft;
            double newWidth = oldWidth;
            double newHeight = oldHeight;

            if (hitedThumb.DragDirection == DragDirection.TopLeft ||
                hitedThumb.DragDirection == DragDirection.TopCenter ||
                hitedThumb.DragDirection == DragDirection.TopRight)
            {
                ResizeFromTop(this.ParentElement, oldTop, oldHeight, verticalChange, out newTop, out newHeight);
            }
            if (hitedThumb.DragDirection == DragDirection.TopRight ||
                hitedThumb.DragDirection == DragDirection.MiddleRight ||
                hitedThumb.DragDirection == DragDirection.BottomRight)
            {
                ResizeFromRight(this.ParentElement, oldLeft, oldWidth, horizontalChange, out newWidth);
            }
            if (hitedThumb.DragDirection == DragDirection.BottomLeft ||
                hitedThumb.DragDirection == DragDirection.BottomCenter ||
                hitedThumb.DragDirection == DragDirection.BottomRight)
            {
                ResizeFromBottom(this.ParentElement, oldTop, oldHeight, verticalChange, out newHeight);
            }
            if (hitedThumb.DragDirection == DragDirection.TopLeft ||
                hitedThumb.DragDirection == DragDirection.MiddleLeft ||
                hitedThumb.DragDirection == DragDirection.BottomLeft)
            {
                ResizeFromLeft(this.ParentElement, oldLeft, oldWidth, horizontalChange, out newLeft, out newWidth);
            }

            this.Width = newWidth;
            this.Height = newHeight;
            Canvas.SetTop(this, newTop);
            Canvas.SetLeft(this, newLeft);

            Rect newBound = new Rect(newLeft, newTop, newWidth, newHeight);

            return newBound;
        }
        #endregion


        #region 从顶端调整大小 —— static void ResizeFromTop(FrameworkElement parentElement, double oldTop...
        /// <summary>
        /// 从顶端调整大小
        /// </summary>
        /// <param name="parentElement">上级元素</param>
        /// <param name="oldTop">原顶端位置</param>
        /// <param name="oldHeight">原高度</param>
        /// <param name="verticalChange">垂直变化</param>
        /// <param name="newTop">新顶端位置</param>
        /// <param name="newHeight">新高度</param>
        private static void ResizeFromTop(FrameworkElement parentElement, double oldTop, double oldHeight, double verticalChange, out double newTop, out double newHeight)
        {
            double top = oldTop + verticalChange;
            newTop = top + MinSize > oldHeight + oldTop
                ? oldHeight + oldTop - MinSize
                : top;
            newTop = newTop < 0 ? 0 : newTop;
            newHeight = oldHeight + oldTop - newTop;
            newHeight = CorrectHeight(parentElement, newTop, newHeight);
        }
        #endregion

        #region 从左端调整大小 —— static void ResizeFromLeft(FrameworkElement parentElement, double oldLeft...
        /// <summary>
        /// 从左端调整大小
        /// </summary>
        /// <param name="parentElement">上级元素</param>
        /// <param name="oldLeft">原左端位置</param>
        /// <param name="oldWidth">原宽度</param>
        /// <param name="horizontalChange">水平变化</param>
        /// <param name="newLeft">新左端位置</param>
        /// <param name="newWidth">新宽度</param>
        private static void ResizeFromLeft(FrameworkElement parentElement, double oldLeft, double oldWidth, double horizontalChange, out double newLeft, out double newWidth)
        {
            double left = oldLeft + horizontalChange;
            newLeft = left + MinSize > oldWidth + oldLeft
                ? oldWidth + oldLeft - MinSize
                : left;
            newLeft = newLeft < 0 ? 0 : newLeft;
            newWidth = oldWidth + oldLeft - newLeft;
            newWidth = CorrectWidth(parentElement, newLeft, newWidth);
        }
        #endregion

        #region 从右端调整大小 —— static void ResizeFromRight(FrameworkElement parentElement, double oldLeft...
        /// <summary>
        /// 从右端调整大小
        /// </summary>
        /// <param name="parentElement">上级元素</param>
        /// <param name="oldLeft">原左端位置</param>
        /// <param name="oldWidth">原宽度</param>
        /// <param name="horizontalChange">水平变化</param>
        /// <param name="newWidth">新宽度</param>
        private static void ResizeFromRight(FrameworkElement parentElement, double oldLeft, double oldWidth, double horizontalChange, out double newWidth)
        {
            if (oldLeft + oldWidth + horizontalChange < parentElement.ActualWidth)
            {
                newWidth = oldWidth + horizontalChange;
            }
            else
            {
                newWidth = parentElement.ActualWidth - oldLeft;
            }

            newWidth = newWidth < 0 ? 0 : newWidth;
        }
        #endregion

        #region 从底端调整大小 —— static void ResizeFromBottom(FrameworkElement parentElement, double oldTop...
        /// <summary>
        /// 从底端调整大小
        /// </summary>
        /// <param name="parent">上级元素</param>
        /// <param name="oldTop">原顶端位置</param>
        /// <param name="oldHeight">原高度</param>
        /// <param name="verticalChange">垂直变化</param>
        /// <param name="newHeight">新高度</param>
        private static void ResizeFromBottom(FrameworkElement parent, double oldTop, double oldHeight, double verticalChange, out double newHeight)
        {
            if (oldTop + oldHeight + verticalChange < parent.ActualWidth)
            {
                newHeight = oldHeight + verticalChange;
            }
            else
            {
                newHeight = parent.ActualWidth - oldTop;
            }

            newHeight = newHeight < 0 ? 0 : newHeight;
        }
        #endregion

        #region 修正顶端位置 —— static double CorrectTop(FrameworkElement parentElement, double top...
        /// <summary>
        /// 修正顶端位置
        /// </summary>
        /// <param name="parent">上级元素</param>
        /// <param name="top">顶端位置</param>
        /// <param name="height">高度</param>
        /// <returns>顶端位置</returns>
        private static double CorrectTop(FrameworkElement parent, double top, double height)
        {
            double newHeight = top + height > parent.ActualHeight
                ? parent.ActualHeight - height
                : top;

            return newHeight < 0 ? 0 : newHeight;
        }
        #endregion

        #region 修正左端位置 —— static double CorrectLeft(FrameworkElement parentElement, double left...
        /// <summary>
        /// 修正左端位置
        /// </summary>
        /// <param name="parent">上级元素</param>
        /// <param name="left">左端位置</param>
        /// <param name="width">宽度</param>
        /// <returns>左端位置</returns>
        private static double CorrectLeft(FrameworkElement parent, double left, double width)
        {
            double newLeft = left + width > parent.ActualWidth
                ? parent.ActualWidth - width
                : left;

            return newLeft < 0 ? 0 : newLeft;
        }
        #endregion

        #region 修正宽度 —— static double CorrectWidth(FrameworkElement parentElement, double left...
        /// <summary>
        /// 修正宽度
        /// </summary>
        /// <param name="parent">上级元素</param>
        /// <param name="left">左端位置</param>
        /// <param name="width">宽度</param>
        /// <returns>宽度</returns>
        private static double CorrectWidth(FrameworkElement parent, double left, double width)
        {
            double newWidth = left + width > parent.ActualWidth
                ? parent.ActualWidth - left
                : width;

            return newWidth < 0 ? 0 : newWidth;
        }
        #endregion

        #region 修正高度 —— static double CorrectHeight(FrameworkElement parentElement, double top...
        /// <summary>
        /// 修正高度
        /// </summary>
        /// <param name="parent">上级元素</param>
        /// <param name="top">顶端位置</param>
        /// <param name="height">高度</param>
        /// <returns>高度</returns>
        private static double CorrectHeight(FrameworkElement parent, double top, double height)
        {
            double newHeight = top + height > parent.ActualHeight
                ? parent.ActualHeight - top
                : height;

            return newHeight < 0 ? 0 : newHeight;
        }
        #endregion

        #region 修正double数值 —— static double CorrectDoubleValue(double value)
        /// <summary>
        /// 修正double数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>修正值</returns>
        protected static double CorrectDoubleValue(double value)
        {
            return (double.IsNaN(value) || value < 0.0) ? 0 : value;
        }
        #endregion

        #endregion
    }
}
