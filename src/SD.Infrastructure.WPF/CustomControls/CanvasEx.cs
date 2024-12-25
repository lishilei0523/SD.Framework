using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Enums;
using SD.Infrastructure.WPF.Models;
using SD.Infrastructure.WPF.Visual2Ds;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 增强Canvas
    /// </summary>
    public class CanvasEx : Canvas
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认缩放系数
        /// </summary>
        public const float DefaultScaledFactor = 1.1f;

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultBorderThickness = 2.0d;

        /// <summary>
        /// 操作模式依赖属性
        /// </summary>
        public static readonly DependencyProperty ModeProperty;

        /// <summary>
        /// 缩放系数依赖属性
        /// </summary>
        public static readonly DependencyProperty ScaledFactorProperty;

        /// <summary>
        /// 边框厚度依赖属性
        /// </summary>
        public static readonly DependencyProperty BorderThicknessProperty;

        /// <summary>
        /// 边框画刷依赖属性
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty;

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
        /// 形状列表依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty;

        /// <summary>
        /// 形状数据列表依赖属性
        /// </summary>
        public static readonly DependencyProperty DatasSourceProperty;

        /// <summary>
        /// 元素拖拽路由事件
        /// </summary>
        public static readonly RoutedEvent ElementDragEvent;

        /// <summary>
        /// 元素改变尺寸路由事件
        /// </summary>
        public static readonly RoutedEvent ElementResizeEvent;

        /// <summary>
        /// 绘制开始路由事件
        /// </summary>
        public static readonly RoutedEvent DrawEvent;

        /// <summary>
        /// 绘制中路由事件
        /// </summary>
        public static readonly RoutedEvent DrawingEvent;

        /// <summary>
        /// 绘制完成路由事件
        /// </summary>
        public static readonly RoutedEvent DrawnEvent;

        /// <summary>
        /// 形状点击路由事件
        /// </summary>
        public static readonly RoutedEvent ShapeClickEvent;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static CanvasEx()
        {
            ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(CanvasMode), typeof(CanvasEx), new PropertyMetadata(CanvasMode.Scale));
            ScaledFactorProperty = DependencyProperty.Register(nameof(ScaledFactor), typeof(float), typeof(CanvasEx), new PropertyMetadata(DefaultScaledFactor));
            BorderThicknessProperty = DependencyProperty.Register(nameof(BorderThickness), typeof(double), typeof(CanvasEx), new PropertyMetadata(DefaultBorderThickness, OnBorderThicknessChanged));
            BorderBrushProperty = DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(CanvasEx), new PropertyMetadata(new SolidColorBrush(Colors.Red), OnBorderBrushChanged));
            BackgroundImageProperty = DependencyProperty.Register(nameof(BackgroundImage), typeof(Image), typeof(CanvasEx), new PropertyMetadata(null, OnBackgroundImageChanged));
            ShowGridLinesProperty = DependencyProperty.Register(nameof(ShowGridLines), typeof(bool), typeof(CanvasEx), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, OnShowGridLinesChanged));
            DraggableProperty = DependencyProperty.Register(nameof(Draggable), typeof(bool), typeof(CanvasEx), new PropertyMetadata(true));
            ResizableProperty = DependencyProperty.Register(nameof(Resizable), typeof(bool), typeof(CanvasEx), new PropertyMetadata(true));
            ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(ObservableCollection<Shape>), typeof(CanvasEx), new PropertyMetadata(new ObservableCollection<Shape>(), OnItemsSourceChanged));
            DatasSourceProperty = DependencyProperty.Register(nameof(DatasSource), typeof(ObservableCollection<ShapeL>), typeof(CanvasEx), new PropertyMetadata(new ObservableCollection<ShapeL>()));
            ElementDragEvent = EventManager.RegisterRoutedEvent(nameof(ElementDrag), RoutingStrategy.Direct, typeof(CanvasEventHandler), typeof(CanvasEx));
            ElementResizeEvent = EventManager.RegisterRoutedEvent(nameof(ElementResize), RoutingStrategy.Direct, typeof(CanvasEventHandler), typeof(CanvasEx));
            DrawEvent = EventManager.RegisterRoutedEvent(nameof(Draw), RoutingStrategy.Direct, typeof(CanvasEventHandler), typeof(CanvasEx));
            DrawingEvent = EventManager.RegisterRoutedEvent(nameof(Drawing), RoutingStrategy.Direct, typeof(CanvasEventHandler), typeof(CanvasEx));
            DrawnEvent = EventManager.RegisterRoutedEvent(nameof(Drawn), RoutingStrategy.Direct, typeof(CanvasEventHandler), typeof(CanvasEx));
            ShapeClickEvent = EventManager.RegisterRoutedEvent(nameof(ShapeClick), RoutingStrategy.Direct, typeof(ShapeEventHandler), typeof(CanvasEx));
        }

        /// <summary>
        /// 变换矩阵
        /// </summary>
        private readonly MatrixTransform _matrixTransform;

        /// <summary>
        /// 形状列表
        /// </summary>
        private readonly IList<Shape> _shapes;

        /// <summary>
        /// 鼠标起始位置
        /// </summary>
        /// <remarks>基于Viewport坐标系</remarks>
        private Point? _startPosition;

        /// <summary>
        /// 鼠标实时位置
        /// </summary>
        /// <remarks>基于Viewport坐标系</remarks>
        private Point? _mousePosition;

        /// <summary>
        /// 矫正鼠标起始位置
        /// </summary>
        /// <remarks>基于CanvasEx坐标系</remarks>
        private Point? _rectifiedStartPosition;

        /// <summary>
        /// 矫正鼠标实时位置
        /// </summary>
        /// <remarks>基于CanvasEx坐标系</remarks>
        private Point? _rectifiedMousePosition;

        /// <summary>
        /// 拖拽偏移量
        /// </summary>
        /// <remarks>鼠标位置与元素边距的偏移</remarks>
        private Vector _draggingOffset;

        /// <summary>
        /// 选中元素
        /// </summary>
        private UIElement _selectedVisual;

        /// <summary>
        /// 实例构造器
        /// </summary>
        public CanvasEx()
        {
            //默认值
            this._matrixTransform = new MatrixTransform();
            this._shapes = new List<Shape>();
            base.ClipToBounds = true;
            base.HorizontalAlignment = HorizontalAlignment.Stretch;
            base.VerticalAlignment = VerticalAlignment.Stretch;
            base.Background = new SolidColorBrush(Colors.Transparent);

            //注册事件
            base.PreviewMouseDown += this.OnMouseDown;
            base.PreviewMouseMove += this.OnMouseMove;
            base.PreviewMouseWheel += this.OnMouseWheel;
            base.PreviewMouseUp += this.OnMouseUp;
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

        #region 依赖属性 - 边框厚度 —— double BorderThickness
        /// <summary>
        /// 依赖属性 - 边框厚度
        /// </summary>
        public double BorderThickness
        {
            get => (double)this.GetValue(BorderThicknessProperty);
            set => this.SetValue(BorderThicknessProperty, value);
        }
        #endregion

        #region 依赖属性 - 边框画刷 —— Brush BorderBrush
        /// <summary>
        /// 依赖属性 - 边框画刷
        /// </summary>
        public Brush BorderBrush
        {
            get => (Brush)this.GetValue(BorderBrushProperty);
            set => this.SetValue(BorderBrushProperty, value);
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

        #region 依赖属性 - 形状列表 —— ObservableCollection<Shape> ItemsSource
        /// <summary>
        /// 依赖属性 - 形状列表
        /// </summary>
        public ObservableCollection<Shape> ItemsSource
        {
            get => (ObservableCollection<Shape>)this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }
        #endregion

        #region 依赖属性 - 形状数据列表 —— ObservableCollection<ShapeL> DatasSource
        /// <summary>
        /// 依赖属性 - 形状数据列表
        /// </summary>
        public ObservableCollection<ShapeL> DatasSource
        {
            get => (ObservableCollection<ShapeL>)this.GetValue(DatasSourceProperty);
            set => this.SetValue(DatasSourceProperty, value);
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

        #region 路由事件 - 元素拖拽 —— event CanvasEventHandler ElementDrag
        /// <summary>
        /// 路由事件 - 元素拖拽
        /// </summary>
        public event CanvasEventHandler ElementDrag
        {
            add => base.AddHandler(ElementDragEvent, value);
            remove => base.RemoveHandler(ElementDragEvent, value);
        }
        #endregion

        #region 路由事件 - 元素改变尺寸 —— event CanvasEventHandler ElementResize
        /// <summary>
        /// 路由事件 - 元素改变尺寸
        /// </summary>
        public event CanvasEventHandler ElementResize
        {
            add => base.AddHandler(ElementResizeEvent, value);
            remove => base.RemoveHandler(ElementResizeEvent, value);
        }
        #endregion

        #region 路由事件 - 绘制开始 —— event CanvasEventHandler Draw
        /// <summary>
        /// 路由事件 - 绘制开始
        /// </summary>
        public event CanvasEventHandler Draw
        {
            add => base.AddHandler(DrawEvent, value);
            remove => base.RemoveHandler(DrawEvent, value);
        }
        #endregion

        #region 路由事件 - 绘制中 —— event CanvasEventHandler Drawing
        /// <summary>
        /// 路由事件 - 绘制中
        /// </summary>
        public event CanvasEventHandler Drawing
        {
            add => base.AddHandler(DrawingEvent, value);
            remove => base.RemoveHandler(DrawingEvent, value);
        }
        #endregion

        #region 路由事件 - 绘制完成 —— event CanvasEventHandler Drawn
        /// <summary>
        /// 路由事件 - 绘制完成
        /// </summary>
        public event CanvasEventHandler Drawn
        {
            add => base.AddHandler(DrawnEvent, value);
            remove => base.RemoveHandler(DrawnEvent, value);
        }
        #endregion

        #region 路由事件 - 形状点击 —— event ShapeEventHandler ShapeClick
        /// <summary>
        /// 路由事件 - 形状点击
        /// </summary>
        public event ShapeEventHandler ShapeClick
        {
            add => base.AddHandler(ShapeClickEvent, value);
            remove => base.RemoveHandler(ShapeClickEvent, value);
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

        #region 只读属性 - 鼠标起始位置 —— Point? StartPosition
        /// <summary>
        /// 只读属性 - 鼠标起始位置
        /// </summary>
        /// <remarks>基于Viewport坐标系</remarks>
        public Point? StartPosition
        {
            get => this._startPosition;
        }
        #endregion

        #region 只读属性 - 鼠标实时位置 —— Point? MousePosition
        /// <summary>
        /// 只读属性 - 鼠标实时位置
        /// </summary>
        /// <remarks>基于Viewport坐标系</remarks>
        public Point? MousePosition
        {
            get => this._mousePosition;
        }
        #endregion

        #region 只读属性 - 矫正鼠标起始位置 —— Point? RectifiedStartPosition
        /// <summary>
        /// 只读属性 - 矫正鼠标起始位置
        /// </summary>
        /// <remarks>基于CanvasEx坐标系</remarks>
        public Point? RectifiedStartPosition
        {
            get => this._rectifiedStartPosition;
        }
        #endregion

        #region 只读属性 - 矫正鼠标实时位置 —— Point? RectifiedMousePosition
        /// <summary>
        /// 只读属性 - 矫正鼠标实时位置
        /// </summary>
        /// <remarks>基于CanvasEx坐标系</remarks>
        public Point? RectifiedMousePosition
        {
            get => this._rectifiedMousePosition;
        }
        #endregion

        #region 只读属性 - 拖拽偏移量 —— Vector DraggingOffset
        /// <summary>
        /// 只读属性 - 拖拽偏移量
        /// </summary>
        /// <remarks>鼠标位置与元素位置的偏移</remarks>
        public Vector DraggingOffset
        {
            get => this._draggingOffset;
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

        #endregion

        #region # 方法

        //Events

        #region 边框厚度改变事件 —— static void OnBorderThicknessChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 边框厚度改变事件
        /// </summary>
        private static void OnBorderThicknessChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            CanvasEx canvas = (CanvasEx)dependencyObject;
            if (eventArgs.NewValue is double thickness)
            {
                double scaledRatio = canvas.ScaledRatio.Equals(0) ? 1 : canvas.ScaledRatio;

                //网格线粗细调整
                foreach (GridLinesVisual2D gridLines in canvas.Children.OfType<GridLinesVisual2D>())
                {
                    gridLines.StrokeThickness = thickness / scaledRatio;
                }

                //图形边框粗细调整
                foreach (Shape shape in canvas.ItemsSource)
                {
                    shape.StrokeThickness = thickness / scaledRatio;
                }
            }
        }
        #endregion

        #region 边框画刷改变事件 —— static void OnBorderBrushChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 边框画刷改变事件
        /// </summary>
        private static void OnBorderBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            CanvasEx canvas = (CanvasEx)dependencyObject;
            if (eventArgs.NewValue is Brush brush)
            {
                foreach (Shape shape in canvas.ItemsSource)
                {
                    shape.Stroke = brush;
                }
            }
        }
        #endregion

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

        #region 形状列表源改变事件 —— static void OnItemsSourceChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 形状列表源改变事件
        /// </summary>
        private static void OnItemsSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            CanvasEx canvas = (CanvasEx)dependencyObject;
            ObservableCollection<Shape> oldShapes = eventArgs.OldValue as ObservableCollection<Shape>;
            ObservableCollection<Shape> newShapes = eventArgs.NewValue as ObservableCollection<Shape>;
            if (oldShapes != null)
            {
                foreach (Shape shape in oldShapes)
                {
                    if (canvas.Children.Contains(shape))
                    {
                        canvas.Children.Remove(shape);
                        canvas._shapes.Remove(shape);
                        if (shape.Tag is ShapeL shapeL && canvas.DatasSource.Contains(shapeL))
                        {
                            canvas.DatasSource.Remove(shapeL);
                        }
                    }
                }
            }
            if (newShapes != null)
            {
                foreach (Shape shape in newShapes)
                {
                    if (shape.Parent is Panel panel)
                    {
                        panel.Children.Remove(shape);
                    }

                    shape.RenderTransform = canvas.MatrixTransform;
                    shape.MouseLeftButtonDown += canvas.OnShapeMouseLeftDown;
                    if (!canvas.Children.Contains(shape))
                    {
                        canvas.Children.Add(shape);
                        canvas._shapes.Add(shape);
                        if (shape.Tag is ShapeL shapeL && !canvas.DatasSource.Contains(shapeL))
                        {
                            canvas.DatasSource.Add(shapeL);
                        }
                    }
                }

                //注册集合元素改变事件
                newShapes.CollectionChanged += canvas.OnShapeItemsChanged;
            }
            if (newShapes == null)
            {
                foreach (Shape shape in canvas._shapes)
                {
                    if (canvas.Children.Contains(shape))
                    {
                        canvas.Children.Remove(shape);
                        if (shape.Tag is ShapeL shapeL && canvas.DatasSource.Contains(shapeL))
                        {
                            canvas.DatasSource.Remove(shapeL);
                        }
                    }
                }
                canvas._shapes.Clear();
            }
        }
        #endregion

        #region 形状列表元素改变事件 —— void OnShapeItemsChanged(object sender, NotifyCollectionChangedEventArgs...
        /// <summary>
        /// 形状列表元素改变事件
        /// </summary>
        private void OnShapeItemsChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (eventArgs.OldItems != null)
            {
                foreach (Shape shape in eventArgs.OldItems)
                {
                    if (this.Children.Contains(shape))
                    {
                        this.Children.Remove(shape);
                        this._shapes.Remove(shape);
                        if (shape.Tag is ShapeL shapeL && this.DatasSource.Contains(shapeL))
                        {
                            this.DatasSource.Remove(shapeL);
                        }
                    }
                }
            }
            if (eventArgs.NewItems != null)
            {
                foreach (Shape shape in eventArgs.NewItems)
                {
                    if (shape.Parent is Panel panel)
                    {
                        panel.Children.Remove(shape);
                    }

                    shape.RenderTransform = this.MatrixTransform;
                    shape.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                    if (!this.Children.Contains(shape))
                    {
                        this.Children.Add(shape);
                        this._shapes.Add(shape);
                        if (shape.Tag is ShapeL shapeL && !this.DatasSource.Contains(shapeL))
                        {
                            this.DatasSource.Add(shapeL);
                        }
                    }
                }
            }
            if (eventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (Shape shape in this._shapes)
                {
                    if (this.Children.Contains(shape))
                    {
                        this.Children.Remove(shape);
                        if (shape.Tag is ShapeL shapeL && this.DatasSource.Contains(shapeL))
                        {
                            this.DatasSource.Remove(shapeL);
                        }
                    }
                }
                this._shapes.Clear();
            }
        }
        #endregion

        #region 形状鼠标左击事件 —— void OnShapeMouseLeftDown(object sender, MouseButtonEventArgs eventArgs)
        /// <summary>
        /// 形状鼠标左击事件
        /// </summary>
        private void OnShapeMouseLeftDown(object sender, MouseButtonEventArgs eventArgs)
        {
            Shape shape = (Shape)sender;
            ShapeL shapeL = (ShapeL)shape.Tag;

            //挂起路由事件
            ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeClickEvent, shape, shapeL);
            this.RaiseEvent(shapeEventArgs);
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
            Point startPosition = eventArgs.GetPosition(this);
            this._startPosition = startPosition;
            this._rectifiedStartPosition = this._matrixTransform.Inverse!.Transform(startPosition);
            if (eventArgs.ChangedButton == MouseButton.Left)
            {
                if (eventArgs.Source is UIElement element && this.Children.Contains(element))
                {
                    this._selectedVisual = element;

                    //计算鼠标位置与元素边距偏移量
                    double marginX = GetLeft(element);
                    double marginY = GetTop(element);
                    marginX = double.IsNaN(marginX) ? 0 : marginX;
                    marginY = double.IsNaN(marginY) ? 0 : marginY;
                    Point elementMargin = new Point(marginX, marginY);
                    this._draggingOffset = elementMargin - startPosition;
                }
            }
            if (this.Mode == CanvasMode.Draw && eventArgs.ChangedButton == MouseButton.Left)
            {
                //设置光标
                Mouse.OverrideCursor = Cursors.Cross;

                //挂起路由事件
                this.RaiseEvent(new RoutedEventArgs(DrawEvent, this));
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
            this._mousePosition = mousePosition;
            this._rectifiedMousePosition = rectifiedMousePosition;
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

                    Canvas.SetLeft(this._selectedVisual, mousePosition.X + this._draggingOffset.X);
                    Canvas.SetTop(this._selectedVisual, mousePosition.Y + this._draggingOffset.Y);

                    //挂起路由事件
                    this.RaiseEvent(new RoutedEventArgs(ElementDragEvent, this));
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
                    this.RaiseEvent(new RoutedEventArgs(ElementResizeEvent, this));
                }
            }
            if (this.Mode == CanvasMode.Draw && eventArgs.LeftButton == MouseButtonState.Pressed && this._rectifiedStartPosition.HasValue)
            {
                //设置光标
                Mouse.OverrideCursor = Cursors.Cross;

                //挂起路由事件
                this.RaiseEvent(new RoutedEventArgs(DrawingEvent, this));
            }
        }
        #endregion

        #region 鼠标滚轮事件 —— void OnMouseWheel(object sender, MouseWheelEventArgs eventArgs)
        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        private void OnMouseWheel(object sender, MouseWheelEventArgs eventArgs)
        {
            float scaledFactor = eventArgs.Delta < 0
                ? 1f / this.ScaledFactor
                : this.ScaledFactor;
            Point mousePostion = eventArgs.GetPosition(this);

            this.Scale(scaledFactor, mousePostion.X, mousePostion.Y);
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

            this._startPosition = null;
            this._mousePosition = null;
            this._rectifiedStartPosition = null;
            this._rectifiedMousePosition = null;
            this._selectedVisual = null;

            if (this.Mode == CanvasMode.Draw)
            {
                //挂起路由事件
                this.RaiseEvent(new RoutedEventArgs(DrawnEvent, this));
            }
        }
        #endregion


        //Private

        #region 缩放 —— void Scale(double scaleRatio, double centerX, double centerY)
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scaleRatio">缩放率</param>
        /// <param name="centerX">缩放中心X坐标</param>
        /// <param name="centerY">缩放中心Y坐标</param>
        private void Scale(double scaleRatio, double centerX, double centerY)
        {
            Matrix scaledMatrix = this._matrixTransform.Matrix;
            scaledMatrix.ScaleAt(scaleRatio, scaleRatio, centerX, centerY);
            this._matrixTransform.Matrix = scaledMatrix;
            foreach (UIElement element in this.Children)
            {
                double x = GetLeft(element);
                double y = GetTop(element);
                double scaledX = x * scaleRatio;
                double scaledY = y * scaleRatio;
                SetLeft(element, scaledX);
                SetTop(element, scaledY);
                element.RenderTransform = this._matrixTransform;
            }
        }
        #endregion

        #endregion
    }
}
