using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using SD.Avalonia.Controls.Shapes;
using SD.Infrastructure.Avalonia.Enums;
using SD.Infrastructure.Avalonia.Extensions;
using SD.Infrastructure.Avalonia.Models;
using SD.Infrastructure.Avalonia.Visual2Ds;
using SD.Infrastructure.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace SD.Infrastructure.Avalonia.CustomControls
{
    /// <summary>
    /// 增强Canvas
    /// </summary>
    public class CanvasEx : Canvas
    {
        #region # 字段及构造器

        /// <summary>
        /// 默认缩放因子
        /// </summary>
        public const float DefaultScaledFactor = 1.1f;

        /// <summary>
        /// 默认边框厚度
        /// </summary>
        public const double DefaultBorderThickness = 2.0d;

        /// <summary>
        /// 操作模式依赖属性
        /// </summary>
        public static readonly StyledProperty<CanvasMode> ModeProperty;

        /// <summary>
        /// 缩放因子依赖属性
        /// </summary>
        public static readonly StyledProperty<float> ScaledFactorProperty;

        /// <summary>
        /// 边框厚度依赖属性
        /// </summary>
        public static readonly StyledProperty<double> BorderThicknessProperty;

        /// <summary>
        /// 边框画刷依赖属性
        /// </summary>
        public static readonly StyledProperty<Brush> BorderBrushProperty;

        /// <summary>
        /// 背景图像依赖属性
        /// </summary>
        public static readonly StyledProperty<Image> BackgroundImageProperty;

        /// <summary>
        /// 显示网格线依赖属性
        /// </summary>
        public static readonly StyledProperty<bool> ShowGridLinesProperty;

        /// <summary>
        /// 形状列表依赖属性
        /// </summary>
        public static readonly StyledProperty<ObservableCollection<Shape>> ItemsSourceProperty;

        /// <summary>
        /// 形状数据列表依赖属性
        /// </summary>
        public static readonly StyledProperty<ObservableCollection<ShapeL>> DatasSourceProperty;

        /// <summary>
        /// 形状上下文菜单依赖属性
        /// </summary>
        public static readonly StyledProperty<ContextMenu> ShapeContextMenuProperty;

        /// <summary>
        /// 元素可拖拽附加属性
        /// </summary>
        public static readonly AttachedProperty<bool> DraggableProperty;

        /// <summary>
        /// 元素可改变尺寸附加属性
        /// </summary>
        public static readonly AttachedProperty<bool> ResizableProperty;

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
            //注册依赖属性
            ModeProperty = AvaloniaProperty.Register<CanvasEx, CanvasMode>(nameof(Mode), CanvasMode.Scale);
            ScaledFactorProperty = AvaloniaProperty.Register<CanvasEx, float>(nameof(ScaledFactor), DefaultScaledFactor);
            BorderThicknessProperty = AvaloniaProperty.Register<CanvasEx, double>(nameof(BorderThickness), DefaultBorderThickness);
            BorderBrushProperty = AvaloniaProperty.Register<CanvasEx, Brush>(nameof(BorderBrush), new SolidColorBrush(Colors.Red));
            BackgroundImageProperty = AvaloniaProperty.Register<CanvasEx, Image>(nameof(BackgroundImage), null);
            ShowGridLinesProperty = AvaloniaProperty.Register<CanvasEx, bool>(nameof(ShowGridLines), false);
            ItemsSourceProperty = AvaloniaProperty.Register<CanvasEx, ObservableCollection<Shape>>(nameof(ItemsSource), new ObservableCollection<Shape>());
            DatasSourceProperty = AvaloniaProperty.Register<CanvasEx, ObservableCollection<ShapeL>>(nameof(DatasSource), new ObservableCollection<ShapeL>());
            ShapeContextMenuProperty = AvaloniaProperty.Register<CanvasEx, ContextMenu>(nameof(ShapeContextMenu), null);

            //注册附加属性
            DraggableProperty = AvaloniaProperty.RegisterAttached<CanvasEx, Shape, bool>("Draggable", true);
            ResizableProperty = AvaloniaProperty.RegisterAttached<CanvasEx, Shape, bool>("Resizable", true);

            //注册路由事件
            ElementDragEvent = RoutedEvent.Register<CanvasEx, RoutedEventArgs>(nameof(ElementDrag), RoutingStrategies.Direct);
            ElementResizeEvent = RoutedEvent.Register<CanvasEx, RoutedEventArgs>(nameof(ElementResize), RoutingStrategies.Direct);
            DrawEvent = RoutedEvent.Register<CanvasEx, RoutedEventArgs>(nameof(Draw), RoutingStrategies.Direct);
            DrawingEvent = RoutedEvent.Register<CanvasEx, RoutedEventArgs>(nameof(Drawing), RoutingStrategies.Direct);
            DrawnEvent = RoutedEvent.Register<CanvasEx, RoutedEventArgs>(nameof(Drawn), RoutingStrategies.Direct);
            ShapeClickEvent = RoutedEvent.Register<CanvasEx, ShapeEventArgs>(nameof(ShapeClick), RoutingStrategies.Direct);

            //属性值改变回调函数
            BorderThicknessProperty.Changed.AddClassHandler<CanvasEx>(OnBorderThicknessChanged);
            BorderBrushProperty.Changed.AddClassHandler<CanvasEx>(OnBorderBrushChanged);
            BackgroundImageProperty.Changed.AddClassHandler<CanvasEx>(OnBackgroundImageChanged);
            ShowGridLinesProperty.Changed.AddClassHandler<CanvasEx>(OnShowGridLinesChanged);
            ItemsSourceProperty.Changed.AddClassHandler<CanvasEx>(OnItemsSourceChanged);
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
        private Control _selectedVisual;

        /// <summary>
        /// 默认构造器
        /// </summary>
        public CanvasEx()
        {
            //默认值
            this._matrixTransform = new MatrixTransform();
            this._shapes = new List<Shape>();
            this.ClipToBounds = true;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.Background = new SolidColorBrush(Colors.Transparent);
            this.RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute);

            //注册事件
            this.PointerPressed += this.OnMouseDown;
            this.PointerMoved += this.OnMouseMove;
            this.PointerWheelChanged += this.OnMouseWheel;
            this.PointerReleased += this.OnMouseUp;
            this.PointerExited += this.OnMouseLeave;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 操作模式 —— CanvasMode Mode
        /// <summary>
        /// 依赖属性 - 操作模式
        /// </summary>
        public CanvasMode Mode
        {
            get => this.GetValue(ModeProperty);
            set => this.SetValue(ModeProperty, value);
        }
        #endregion

        #region 依赖属性 - 缩放因子 —— float ScaledFactor
        /// <summary>
        /// 依赖属性 - 缩放因子
        /// </summary>
        public float ScaledFactor
        {
            get => this.GetValue(ScaledFactorProperty);
            set => this.SetValue(ScaledFactorProperty, value);
        }
        #endregion

        #region 依赖属性 - 边框厚度 —— double BorderThickness
        /// <summary>
        /// 依赖属性 - 边框厚度
        /// </summary>
        public double BorderThickness
        {
            get => this.GetValue(BorderThicknessProperty);
            set => this.SetValue(BorderThicknessProperty, value);
        }
        #endregion

        #region 依赖属性 - 边框画刷 —— Brush BorderBrush
        /// <summary>
        /// 依赖属性 - 边框画刷
        /// </summary>
        public Brush BorderBrush
        {
            get => this.GetValue(BorderBrushProperty);
            set => this.SetValue(BorderBrushProperty, value);
        }
        #endregion

        #region 依赖属性 - 背景图像 —— Image BackgroundImage
        /// <summary>
        /// 依赖属性 - 背景图像
        /// </summary>
        public Image BackgroundImage
        {
            get => this.GetValue(BackgroundImageProperty);
            set => this.SetValue(BackgroundImageProperty, value);
        }
        #endregion

        #region 依赖属性 - 显示网格线 —— bool ShowGridLines
        /// <summary>
        /// 依赖属性 - 显示网格线
        /// </summary>
        public bool ShowGridLines
        {
            get => this.GetValue(ShowGridLinesProperty);
            set => this.SetValue(ShowGridLinesProperty, value);
        }
        #endregion

        #region 依赖属性 - 形状列表 —— ObservableCollection<Shape> ItemsSource
        /// <summary>
        /// 依赖属性 - 形状列表
        /// </summary>
        public ObservableCollection<Shape> ItemsSource
        {
            get => this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }
        #endregion

        #region 依赖属性 - 形状数据列表 —— ObservableCollection<ShapeL> DatasSource
        /// <summary>
        /// 依赖属性 - 形状数据列表
        /// </summary>
        public ObservableCollection<ShapeL> DatasSource
        {
            get => this.GetValue(DatasSourceProperty);
            set => this.SetValue(DatasSourceProperty, value);
        }
        #endregion

        #region 依赖属性 - 形状上下文菜单 —— ContextMenu ShapeContextMenu
        /// <summary>
        /// 依赖属性 - 形状上下文菜单
        /// </summary>
        public ContextMenu ShapeContextMenu
        {
            get => this.GetValue(ShapeContextMenuProperty);
            set => this.SetValue(ShapeContextMenuProperty, value);
        }
        #endregion

        #region 附加属性 - 元素可拖拽 —— bool Draggable

        /// <summary>
        /// 获取元素可拖拽
        /// </summary>
        public static bool GetDraggable(AvaloniaObject dependencyObject)
        {
            return dependencyObject.GetValue(DraggableProperty);
        }

        /// <summary>
        /// 设置元素可拖拽
        /// </summary>
        public static void SetDraggable(AvaloniaObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(DraggableProperty, value);
        }

        #endregion

        #region 附加属性 - 元素可改变尺寸 —— bool Resizable

        /// <summary>
        /// 获取元素可改变尺寸
        /// </summary>
        public static bool GetResizable(AvaloniaObject dependencyObject)
        {
            return dependencyObject.GetValue(ResizableProperty);
        }

        /// <summary>
        /// 设置元素可改变尺寸
        /// </summary>
        public static void SetResizable(AvaloniaObject dependencyObject, bool value)
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

        #region 只读属性 - 选中元素 —— Control SelectedVisual
        /// <summary>
        /// 只读属性 - 选中元素
        /// </summary>
        public Control SelectedVisual
        {
            get => this._selectedVisual;
        }
        #endregion

        #endregion

        #region # 方法

        #region 边框厚度改变事件 —— static void OnBorderThicknessChanged(CanvasEx canvas...
        /// <summary>
        /// 边框厚度改变事件
        /// </summary>
        private static void OnBorderThicknessChanged(CanvasEx canvas, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue is double thickness)
            {
                //网格线粗细调整
                foreach (GridLinesVisual2D gridLines in canvas.Children.OfType<GridLinesVisual2D>())
                {
                    gridLines.StrokeThickness = GridLinesVisual2D.DefaultStrokeThickness / canvas.ScaledRatio;
                }

                //图形边框粗细调整
                foreach (Shape shape in canvas.ItemsSource)
                {
                    shape.StrokeThickness = thickness / canvas.ScaledRatio;
                }
            }
        }
        #endregion

        #region 边框画刷改变事件 —— static void OnBorderBrushChanged(CanvasEx canvas...
        /// <summary>
        /// 边框画刷改变事件
        /// </summary>
        private static void OnBorderBrushChanged(CanvasEx canvas, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue is Brush brush)
            {
                foreach (Shape shape in canvas.ItemsSource)
                {
                    shape.Stroke = brush;
                }
            }
        }
        #endregion

        #region 背景图像改变事件 —— static void OnBackgroundImageChanged(CanvasEx canvas...
        /// <summary>
        /// 背景图像改变事件
        /// </summary>
        private static void OnBackgroundImageChanged(CanvasEx canvas, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            Image oldImage = eventArgs.OldValue as Image;
            Image newImage = eventArgs.NewValue as Image;
            if (oldImage != null && canvas.Children.Contains(oldImage))
            {
                canvas.Children.Remove(oldImage);
            }
            if (newImage != null)
            {
                newImage.Stretch = Stretch.Uniform;
                newImage.ZIndex = int.MinValue;
                SetDraggable(newImage, false);
                SetResizable(newImage, false);
                canvas.Children.Add(newImage);
            }
        }
        #endregion

        #region 显示网格线改变事件 —— static void OnShowGridLinesChanged(CanvasEx canvas...
        /// <summary>
        /// 显示网格线改变事件
        /// </summary>
        private static void OnShowGridLinesChanged(CanvasEx canvas, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            bool oldShow = (bool)eventArgs.OldValue;
            bool newShow = (bool)eventArgs.NewValue;
            if (oldShow && newShow)
            {
                return;
            }
            if (oldShow && !newShow)
            {
                foreach (Control element in canvas.Children)
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
                gridLines.ZIndex = int.MaxValue;
                SetDraggable(gridLines, false);
                SetResizable(gridLines, false);
                canvas.Children.Add(gridLines);
            }
        }
        #endregion

        #region 形状列表源改变事件 —— static void OnItemsSourceChanged(CanvasEx canvas...
        /// <summary>
        /// 形状列表源改变事件
        /// </summary>
        private static void OnItemsSourceChanged(CanvasEx canvas, AvaloniaPropertyChangedEventArgs eventArgs)
        {
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
                    shape.ContextMenu = canvas.ShapeContextMenu;
                    shape.PointerPressed += canvas.OnShapeMouseLeftDown;
                    shape.PointerEntered += canvas.OnShapeMouseEnter;
                    shape.PointerExited += canvas.OnShapeMouseLeave;
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
                    shape.ContextMenu = this.ShapeContextMenu;
                    shape.PointerPressed += this.OnShapeMouseLeftDown;
                    shape.PointerEntered += this.OnShapeMouseEnter;
                    shape.PointerExited += this.OnShapeMouseLeave;
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

        #region 形状鼠标左击事件 —— void OnShapeMouseLeftDown(object sender, PointerPressedEventArgs eventArgs)
        /// <summary>
        /// 形状鼠标左击事件
        /// </summary>
        private void OnShapeMouseLeftDown(object sender, PointerPressedEventArgs eventArgs)
        {
            PointerPoint pointerPoint = eventArgs.GetCurrentPoint((Visual)sender);
            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                Shape shape = (Shape)sender;

                //挂起路由事件
                ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeClickEvent, shape);
                this.RaiseEvent(shapeEventArgs);
            }
        }
        #endregion

        #region 形状鼠标进入事件 —— void OnShapeMouseEnter(object sender, PointerEventArgs e)
        /// <summary>
        /// 形状鼠标进入事件
        /// </summary>
        private void OnShapeMouseEnter(object sender, PointerEventArgs e)
        {
            Shape shape = (Shape)sender;
            if (shape is TextVisual2D)
            {
                shape.StrokeThickness = TextVisual2D.DefaultStrokeThickness / this.ScaledRatio * 10;
            }
            else
            {
                shape.StrokeThickness = this.BorderThickness / this.ScaledRatio * 1.5;
            }
        }
        #endregion

        #region 形状鼠标离开事件 —— void OnShapeMouseLeave(object sender, PointerEventArgs e)
        /// <summary>
        /// 形状鼠标离开事件
        /// </summary>
        private void OnShapeMouseLeave(object sender, PointerEventArgs e)
        {
            Shape shape = (Shape)sender;
            if (shape is TextVisual2D)
            {
                shape.StrokeThickness = TextVisual2D.DefaultStrokeThickness / this.ScaledRatio;
            }
            else
            {
                shape.StrokeThickness = this.BorderThickness / this.ScaledRatio;
            }
        }
        #endregion

        #region 获取矫正左边距 —— double GetRectifiedLeft(Control element)
        /// <summary>
        /// 获取矫正左边距
        /// </summary>
        /// <param name="element">UI元素</param>
        /// <returns>矫正左边距</returns>
        public double GetRectifiedLeft(Control element)
        {
            double left = GetLeft(element);
            left = double.IsNaN(left) ? 0 : left;
            double retifiedLeft = left / this.ScaledRatio;

            return retifiedLeft;
        }
        #endregion

        #region 获取矫正上边距 —— double GetRectifiedTop(Control element)
        /// <summary>
        /// 获取矫正上边距
        /// </summary>
        /// <param name="element">UI元素</param>
        /// <returns>矫正上边距</returns>
        public double GetRectifiedTop(Control element)
        {
            double top = GetTop(element);
            top = double.IsNaN(top) ? 0 : top;
            double retifiedTop = top / this.ScaledRatio;

            return retifiedTop;
        }
        #endregion

        #region 鼠标按下事件 —— void OnMouseDown(object sender, PointerPressedEventArgs eventArgs)
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        private void OnMouseDown(object sender, PointerPressedEventArgs eventArgs)
        {
            Point startPosition = eventArgs.GetPosition(this);
            this._startPosition = startPosition;
            this._rectifiedStartPosition = this._matrixTransform.Matrix.Invert()!.Transform(startPosition);

            PointerPoint pointerPoint = eventArgs.GetCurrentPoint((Visual)sender);
            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                if (eventArgs.Source is Control element && this.Children.Contains(element))
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
            if (this.Mode == CanvasMode.Draw && pointerPoint.Properties.IsLeftButtonPressed)
            {
                //设置光标
                this.Cursor = new Cursor(StandardCursorType.Cross);

                //挂起路由事件
                this.RaiseEvent(new RoutedEventArgs(DrawEvent, this));
            }
        }
        #endregion

        #region 鼠标移动事件 —— void OnMouseMove(object sender, PointerEventArgs eventArgs)
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        private void OnMouseMove(object sender, PointerEventArgs eventArgs)
        {
            Point mousePosition = eventArgs.GetPosition(this);
            Point rectifiedMousePosition = this._matrixTransform.Value.Invert()!.Transform(mousePosition);
            this._mousePosition = mousePosition;
            this._rectifiedMousePosition = rectifiedMousePosition;

            PointerPoint pointerPoint = eventArgs.GetCurrentPoint((Visual)sender);
            if (pointerPoint.Properties.IsMiddleButtonPressed && this._rectifiedStartPosition.HasValue)
            {
                //设置光标
                this.Cursor = new Cursor(StandardCursorType.Hand);

                Vector delta = Vector.Subtract(rectifiedMousePosition, this._rectifiedStartPosition.Value);
                TranslateTransform transform = new TranslateTransform(delta.X, delta.Y);
                this._matrixTransform.Matrix = transform.Value * this._matrixTransform.Matrix;
                foreach (Control element in this.Children)
                {
                    element.RenderTransform = this._matrixTransform;
                }
            }
            if (this.Mode == CanvasMode.Drag && pointerPoint.Properties.IsLeftButtonPressed && this._selectedVisual != null)
            {
                bool elementIsDraggable = GetDraggable(this._selectedVisual);
                if (elementIsDraggable)
                {
                    //设置光标
                    this.Cursor = new Cursor(StandardCursorType.Hand);

                    Canvas.SetLeft(this._selectedVisual, mousePosition.X + this._draggingOffset.X);
                    Canvas.SetTop(this._selectedVisual, mousePosition.Y + this._draggingOffset.Y);

                    //挂起路由事件
                    this.RaiseEvent(new RoutedEventArgs(ElementDragEvent, this));
                }
            }
            if (this.Mode == CanvasMode.Resize && pointerPoint.Properties.IsLeftButtonPressed && this._selectedVisual != null)
            {
                bool elementIsResizable = GetResizable(this._selectedVisual);
                if (elementIsResizable)
                {
                    //设置光标
                    this.Cursor = new Cursor(StandardCursorType.SizeAll);

                    //挂起路由事件
                    this.RaiseEvent(new RoutedEventArgs(ElementResizeEvent, this));
                }
            }
            if (this.Mode == CanvasMode.Draw && pointerPoint.Properties.IsLeftButtonPressed && this._rectifiedStartPosition.HasValue)
            {
                //设置光标
                this.Cursor = new Cursor(StandardCursorType.Cross);

                //挂起路由事件
                this.RaiseEvent(new RoutedEventArgs(DrawingEvent, this));
            }
        }
        #endregion

        #region 鼠标滚轮事件 —— void OnMouseWheel(object sender, MouseWheelEventArgs eventArgs)
        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        private void OnMouseWheel(object sender, PointerWheelEventArgs eventArgs)
        {
            float scaledFactor = eventArgs.Delta.Y < 0
                ? 1f / this.ScaledFactor
                : this.ScaledFactor;
            Point mousePostion = eventArgs.GetPosition(this);

            this.Scale(scaledFactor, mousePostion.X, mousePostion.Y);

            CanvasEx canvas = (CanvasEx)sender;
            if (!canvas.ScaledRatio.Equals(0))
            {
                //网格线粗细调整
                foreach (GridLinesVisual2D gridLines in canvas.Children.OfType<GridLinesVisual2D>())
                {
                    gridLines.StrokeThickness = GridLinesVisual2D.DefaultStrokeThickness / canvas.ScaledRatio;
                }

                //图形边框粗细调整
                foreach (Shape shape in this.ItemsSource)
                {
                    shape.StrokeThickness = this.BorderThickness / canvas.ScaledRatio;
                }
                foreach (PointVisual2D pointVisual2D in this.ItemsSource.OfType<PointVisual2D>())
                {
                    pointVisual2D.Thickness = PointVisual2D.DefaultThickness / canvas.ScaledRatio;
                }
                foreach (TextVisual2D textVisual2D in this.ItemsSource.OfType<TextVisual2D>())
                {
                    textVisual2D.StrokeThickness = TextVisual2D.DefaultStrokeThickness / canvas.ScaledRatio;
                }
            }
        }
        #endregion

        #region 鼠标松开事件 —— void OnMouseUp(object sender, PointerReleasedEventArgs eventArgs)
        /// <summary>
        /// 鼠标松开事件
        /// </summary>
        private void OnMouseUp(object sender, PointerReleasedEventArgs eventArgs)
        {
            //设置光标
            this.Cursor = new Cursor(StandardCursorType.Arrow);

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

        #region 鼠标离开事件 —— void OnMouseLeave(object sender, PointerEventArgs eventArgs)
        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        private void OnMouseLeave(object sender, PointerEventArgs eventArgs)
        {
            //设置光标
            this.Cursor = new Cursor(StandardCursorType.Arrow);
        }
        #endregion


        //Private

        #region 缩放 —— void Scale(float scaleRatio, double centerX, double centerY)
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scaleRatio">缩放率</param>
        /// <param name="centerX">缩放中心X坐标</param>
        /// <param name="centerY">缩放中心Y坐标</param>
        private void Scale(float scaleRatio, double centerX, double centerY)
        {
            this._matrixTransform.Matrix = this._matrixTransform.Matrix.ScaleAt(scaleRatio, scaleRatio, centerX, centerY);
            foreach (Control element in this.Children)
            {
                double x = GetLeft(element);
                double y = GetTop(element);
                double scaledX = x * scaleRatio;
                double scaledY = y * scaleRatio;
                SetLeft(element, scaledX);
                SetTop(element, scaledY);
                element.RenderTransformOrigin = new RelativePoint(0, 0, RelativeUnit.Absolute);
                element.RenderTransform = this._matrixTransform;
            }
        }
        #endregion

        #endregion
    }
}
