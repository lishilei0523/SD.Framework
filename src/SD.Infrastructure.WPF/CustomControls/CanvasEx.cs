using SD.Infrastructure.WPF.Constants;
using SD.Infrastructure.WPF.Visual2Ds;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 增强Canvas
    /// </summary>
    public class CanvasEx : Canvas
    {
        #region # 字段及构造器

        /// <summary>
        /// 操作模式依赖属性
        /// </summary>
        public static readonly DependencyProperty ModeProperty;

        /// <summary>
        /// 缩放系数依赖属性
        /// </summary>
        public static readonly DependencyProperty ScaledFactorProperty;

        /// <summary>
        /// 背景图像依赖属性
        /// </summary>
        public static readonly DependencyProperty BackgroundImageProperty;

        /// <summary>
        /// 显示网格线依赖属性
        /// </summary>
        public static readonly DependencyProperty ShowGridLinesProperty;

        /// <summary>
        /// 元素可拖拽依赖属性
        /// </summary>
        public static readonly DependencyProperty DraggableProperty;

        /// <summary>
        /// 元素可改变尺寸依赖属性
        /// </summary>
        public static readonly DependencyProperty ResizableProperty;

        /// <summary>
        /// 元素改变尺寸路由事件
        /// </summary>
        public static readonly RoutedEvent ElementResizeEvent;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CanvasEx()
        {
            ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(CanvasMode), typeof(CanvasEx), new PropertyMetadata(CanvasMode.Scale));
            ScaledFactorProperty = DependencyProperty.Register(nameof(ScaledFactor), typeof(float), typeof(CanvasEx), new PropertyMetadata(1.1f));
            BackgroundImageProperty = DependencyProperty.Register(nameof(BackgroundImage), typeof(Image), typeof(CanvasEx), new PropertyMetadata(null, OnBackgroundImageChanged));
            ShowGridLinesProperty = DependencyProperty.Register(nameof(ShowGridLines), typeof(bool), typeof(CanvasEx), new PropertyMetadata(false, OnShowGridLinesChanged));
            DraggableProperty = DependencyProperty.Register(nameof(Draggable), typeof(bool), typeof(CanvasEx), new PropertyMetadata(true));
            ResizableProperty = DependencyProperty.Register(nameof(Resizable), typeof(bool), typeof(CanvasEx), new PropertyMetadata(true));
            ElementResizeEvent = EventManager.RegisterRoutedEvent(nameof(ElementResize), RoutingStrategy.Direct, typeof(ElementResizeEventHandler), typeof(CanvasEx));
        }

        /// <summary>
        /// 变换矩阵
        /// </summary>
        private readonly MatrixTransform _matrixTransform;

        /// <summary>
        /// 矫正起始位置
        /// </summary>
        private Point? _rectifiedStartPosition;

        /// <summary>
        /// 拖拽位置
        /// </summary>
        private Vector _draggingPosition;

        /// <summary>
        /// 选中元素
        /// </summary>
        private UIElement _selectedVisual;

        /// <summary>
        /// 默认构造器
        /// </summary>
        public CanvasEx()
        {
            this._matrixTransform = new MatrixTransform();
            base.MouseDown += this.OnMouseDown;
            base.MouseMove += this.OnMouseMove;
            base.MouseWheel += this.OnMouseWheel;
            base.MouseUp += this.OnMouseUp;
            base.ClipToBounds = true;
            base.HorizontalAlignment = HorizontalAlignment.Stretch;
            base.VerticalAlignment = VerticalAlignment.Stretch;
            base.Background = new SolidColorBrush(Colors.Transparent);
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 操作模式 —— CanvasMode Mode
        /// <summary>
        /// 依赖属性 - 操作模式
        /// </summary>
        public CanvasMode Mode
        {
            get => (CanvasMode)this.GetValue(ModeProperty);
            set => this.SetValue(ModeProperty, value);
        }
        #endregion

        #region 依赖属性 - 缩放系数 —— float ScaledFactor
        /// <summary>
        /// 依赖属性 - 缩放系数
        /// </summary>
        public float ScaledFactor
        {
            get => (float)this.GetValue(ScaledFactorProperty);
            set => this.SetValue(ScaledFactorProperty, value);
        }
        #endregion

        #region 依赖属性 - 背景图像 —— Image BackgroundImage
        /// <summary>
        /// 依赖属性 - 背景图像
        /// </summary>
        public Image BackgroundImage
        {
            get => (Image)this.GetValue(BackgroundImageProperty);
            set => this.SetValue(BackgroundImageProperty, value);
        }
        #endregion

        #region 依赖属性 - 显示网格线 —— bool ShowGridLines
        /// <summary>
        /// 依赖属性 - 显示网格线
        /// </summary>
        public bool ShowGridLines
        {
            get => (bool)this.GetValue(ShowGridLinesProperty);
            set => this.SetValue(ShowGridLinesProperty, value);
        }
        #endregion

        #region 依赖属性 - 元素可拖拽 —— bool Draggable
        /// <summary>
        /// 依赖属性 - 元素可拖拽
        /// </summary>
        public bool Draggable
        {
            get => (bool)this.GetValue(DraggableProperty);
            set => this.SetValue(DraggableProperty, value);
        }
        #endregion

        #region 依赖属性 - 元素可改变尺寸 —— bool Resizable
        /// <summary>
        /// 依赖属性 - 元素可改变尺寸
        /// </summary>
        public bool Resizable
        {
            get => (bool)this.GetValue(ResizableProperty);
            set => this.SetValue(ResizableProperty, value);
        }
        #endregion

        #region 附加属性 - 元素可拖拽 —— bool Draggable

        /// <summary>
        /// 获取元素可拖拽
        /// </summary>
        public static bool GetDraggable(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(DraggableProperty);
        }

        /// <summary>
        /// 设置元素可拖拽
        /// </summary>
        public static void SetDraggable(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(DraggableProperty, value);
        }

        #endregion

        #region 附加属性 - 元素可改变尺寸 —— bool Resizable

        /// <summary>
        /// 获取元素可改变尺寸
        /// </summary>
        public static bool GetResizable(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(ResizableProperty);
        }

        /// <summary>
        /// 设置元素可改变尺寸
        /// </summary>
        public static void SetResizable(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(ResizableProperty, value);
        }

        #endregion

        #region 只读属性 - 缩放率 —— double ScaledRatio
        /// <summary>
        /// 只读属性 - 缩放率
        /// </summary>
        public double ScaledRatio
        {
            get => this._matrixTransform.Matrix.M11;
        }
        #endregion

        #region 只读属性 - 变换矩阵 —— MatrixTransform MatrixTransform
        /// <summary>
        /// 只读属性 - 变换矩阵
        /// </summary>
        public MatrixTransform MatrixTransform
        {
            get => this._matrixTransform;
        }
        #endregion

        #region 只读属性 - 矫正起始位置 —— Point? RectifiedStartPosition
        /// <summary>
        /// 只读属性 - 矫正起始位置
        /// </summary>
        public Point? RectifiedStartPosition
        {
            get => this._rectifiedStartPosition;
        }
        #endregion

        #region 只读属性 - 拖拽位置 —— Vector DraggingPosition
        /// <summary>
        /// 只读属性 - 拖拽位置
        /// </summary>
        public Vector DraggingPosition
        {
            get => this._draggingPosition;
        }
        #endregion

        #region 只读属性 - 选中元素 —— UIElement SelectedVisual
        /// <summary>
        /// 只读属性 - 选中元素
        /// </summary>
        public UIElement SelectedVisual
        {
            get => this._selectedVisual;
        }
        #endregion

        #region 路由事件 - 元素改变尺寸 —— event ElementResizeEventHandler ElementResize
        /// <summary>
        /// 路由事件 - 元素改变尺寸
        /// </summary>
        public event ElementResizeEventHandler ElementResize
        {
            add => base.AddHandler(ElementResizeEvent, value);
            remove => base.RemoveHandler(ElementResizeEvent, value);
        }
        #endregion

        #endregion

        #region # 方法

        #region 背景图像改变事件 —— static void OnBackgroundImageChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 背景图像改变事件
        /// </summary>
        private static void OnBackgroundImageChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            CanvasEx canvas = (CanvasEx)dependencyObject;
            Image oldImage = eventArgs.OldValue as Image;
            Image newImage = eventArgs.NewValue as Image;
            if (oldImage != null && canvas.Children.Contains(oldImage))
            {
                canvas.Children.Remove(oldImage);
            }
            if (newImage != null)
            {
                newImage.Stretch = Stretch.Uniform;
                SetZIndex(newImage, int.MinValue);
                SetDraggable(newImage, false);
                SetResizable(newImage, false);
                canvas.Children.Add(newImage);
            }
        }
        #endregion

        #region 显示网格线改变事件 —— static void OnShowGridLinesChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 显示网格线改变事件
        /// </summary>
        private static void OnShowGridLinesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            CanvasEx canvas = (CanvasEx)dependencyObject;
            bool oldShow = (bool)eventArgs.OldValue;
            bool newShow = (bool)eventArgs.NewValue;
            if (oldShow && newShow)
            {
                return;
            }
            if (oldShow && !newShow)
            {
                foreach (UIElement element in canvas.Children)
                {
                    if (element is GridLinesVisual2D)
                    {
                        canvas.Children.Remove(element);
                    }
                }
            }
            if (!oldShow && newShow)
            {
                GridLinesVisual2D gridLines = new GridLinesVisual2D();
                SetZIndex(gridLines, int.MaxValue);
                SetDraggable(gridLines, false);
                SetResizable(gridLines, false);
                canvas.Children.Add(gridLines);
            }
        }
        #endregion

        #region 获取矫正左边距 —— double GetRectifiedLeft(UIElement element)
        /// <summary>
        /// 获取矫正左边距
        /// </summary>
        /// <param name="element">UI元素</param>
        /// <returns>矫正左边距</returns>
        public double GetRectifiedLeft(UIElement element)
        {
            double left = GetLeft(element);
            left = double.IsNaN(left) ? 0 : left;
            double retifiedLeft = left / this.ScaledRatio;

            return retifiedLeft;
        }
        #endregion

        #region 获取矫正上边距 —— double GetRectifiedTop(UIElement element)
        /// <summary>
        /// 获取矫正上边距
        /// </summary>
        /// <param name="element">UI元素</param>
        /// <returns>矫正上边距</returns>
        public double GetRectifiedTop(UIElement element)
        {
            double top = GetTop(element);
            top = double.IsNaN(top) ? 0 : top;
            double retifiedTop = top / this.ScaledRatio;

            return retifiedTop;
        }
        #endregion

        #region 鼠标按下事件 —— void OnMouseDown(object sender, MouseButtonEventArgs eventArgs)
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        private void OnMouseDown(object sender, MouseButtonEventArgs eventArgs)
        {
            Point mousePosition = eventArgs.GetPosition(this);
            if (eventArgs.ChangedButton == MouseButton.Middle)
            {
                this._rectifiedStartPosition = this._matrixTransform.Inverse!.Transform(mousePosition);
            }
            if ((this.Mode == CanvasMode.Drag || this.Mode == CanvasMode.Resize) &&
                (eventArgs.ChangedButton == MouseButton.Left))
            {
                UIElement element = (UIElement)eventArgs.Source;
                if (this.Children.Contains(element))
                {
                    double elementX = double.IsNaN(GetLeft(element)) ? 0 : GetLeft(element);
                    double elementY = double.IsNaN(GetTop(element)) ? 0 : GetTop(element);
                    Point elementPosition = new Point(elementX, elementY);
                    this._selectedVisual = element;
                    this._draggingPosition = elementPosition - mousePosition;
                }
            }
        }
        #endregion

        #region 鼠标移动事件 —— void OnMouseMove(object sender, MouseEventArgs eventArgs)
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs eventArgs)
        {
            Point mousePosition = eventArgs.GetPosition(this);
            Point rectifiedMousePosition = this._matrixTransform.Inverse!.Transform(mousePosition);
            if (eventArgs.MiddleButton == MouseButtonState.Pressed && this._rectifiedStartPosition.HasValue)
            {
                //设置光标
                Mouse.OverrideCursor = Cursors.Hand;

                Vector delta = Point.Subtract(rectifiedMousePosition, this._rectifiedStartPosition.Value);
                TranslateTransform transform = new TranslateTransform(delta.X, delta.Y);
                this._matrixTransform.Matrix = transform.Value * this._matrixTransform.Matrix;
                foreach (UIElement element in this.Children)
                {
                    element.RenderTransform = this._matrixTransform;
                }
            }
            if (this.Mode == CanvasMode.Drag && eventArgs.LeftButton == MouseButtonState.Pressed && this._selectedVisual != null)
            {
                bool elementIsDraggable = GetDraggable(this._selectedVisual);
                if (elementIsDraggable)
                {
                    //设置光标
                    Mouse.OverrideCursor = Cursors.Hand;

                    SetLeft(this._selectedVisual, mousePosition.X + this._draggingPosition.X);
                    SetTop(this._selectedVisual, mousePosition.Y + this._draggingPosition.Y);
                }
            }
            if (this.Mode == CanvasMode.Resize && eventArgs.LeftButton == MouseButtonState.Pressed && this._selectedVisual != null)
            {
                bool elementIsResizable = GetResizable(this._selectedVisual);
                if (elementIsResizable)
                {
                    //设置光标
                    Mouse.OverrideCursor = Cursors.SizeNWSE;

                    //挂起路由事件
                    this.RaiseEvent(new ElementResizeEventArgs(ElementResizeEvent, this, mousePosition, rectifiedMousePosition));
                }
            }
        }
        #endregion

        #region 鼠标滚轮事件 —— void OnMouseWheel(object sender, MouseWheelEventArgs eventArgs)
        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        private void OnMouseWheel(object sender, MouseWheelEventArgs eventArgs)
        {
            float scaledFactor = this.ScaledFactor;
            if (eventArgs.Delta < 0)
            {
                scaledFactor = 1f / scaledFactor;
            }

            Point mousePostion = eventArgs.GetPosition(this);
            Matrix scaledMatrix = this._matrixTransform.Matrix;
            scaledMatrix.ScaleAt(scaledFactor, scaledFactor, mousePostion.X, mousePostion.Y);
            this._matrixTransform.Matrix = scaledMatrix;
            foreach (UIElement element in this.Children)
            {
                double x = GetLeft(element);
                double y = GetTop(element);
                double scaledX = x * scaledFactor;
                double scaledY = y * scaledFactor;
                SetLeft(element, scaledX);
                SetTop(element, scaledY);
                element.RenderTransform = this._matrixTransform;
            }
        }
        #endregion

        #region 鼠标松开事件 —— void OnMouseUp(object sender, MouseButtonEventArgs eventArgs)
        /// <summary>
        /// 鼠标松开事件
        /// </summary>
        private void OnMouseUp(object sender, MouseButtonEventArgs eventArgs)
        {
            //设置光标
            Mouse.OverrideCursor = Cursors.Arrow;

            this._rectifiedStartPosition = null;
            this._selectedVisual = null;
        }
        #endregion 

        #endregion
    }


    /// <summary>
    /// 元素改变尺寸事件处理程序
    /// </summary>
    public delegate void ElementResizeEventHandler(CanvasEx canvas, ElementResizeEventArgs eventArgs);


    /// <summary>
    /// 元素改变尺寸事件参数
    /// </summary>
    public class ElementResizeEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 创建元素改变尺寸事件参数构造器
        /// </summary>
        public ElementResizeEventArgs(RoutedEvent routedEvent, object source, Point mousePosition, Point rectifiedMousePosition)
            : base(routedEvent, source)
        {
            this.MousePosition = mousePosition;
            this.RectifiedMousePosition = rectifiedMousePosition;
        }

        /// <summary>
        /// 鼠标位置
        /// </summary>
        public Point MousePosition { get; set; }

        /// <summary>
        /// 矫正鼠标位置
        /// </summary>
        public Point RectifiedMousePosition { get; set; }
    }
}
