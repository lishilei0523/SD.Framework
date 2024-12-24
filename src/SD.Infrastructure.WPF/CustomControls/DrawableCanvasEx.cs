using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Enums;
using SD.Infrastructure.WPF.Extensions;
using SD.Infrastructure.WPF.Models;
using SD.Infrastructure.WPF.Visual2Ds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.CustomControls
{
    /// <summary>
    /// 绘图增强Canvas
    /// </summary>
    public class DrawableCanvasEx : CanvasEx
    {
        #region # 字段及构造器

        /// <summary>
        /// 线段
        /// </summary>
        private Line _line;

        /// <summary>
        /// 画刷
        /// </summary>
        private Polyline _brush;

        /// <summary>
        /// 矩形
        /// </summary>
        private RectangleVisual2D _rectangle;

        /// <summary>
        /// 圆形
        /// </summary>
        private CircleVisual2D _circle;

        /// <summary>
        /// 椭圆形
        /// </summary>
        private EllipseVisual2D _ellipse;

        /// <summary>
        /// 实时锚线
        /// </summary>
        private Line _realAnchorLine;

        /// <summary>
        /// 锚点集
        /// </summary>
        private readonly IList<PointVisual2D> _polyAnchors;

        /// <summary>
        /// 锚线集
        /// </summary>
        private readonly IList<Line> _polyAnchorLines;

        /// <summary>
        /// 水平参考线
        /// </summary>
        private readonly Line _horizontalLine;

        /// <summary>
        /// 垂直参考线
        /// </summary>
        private readonly Line _verticalLine;

        /// <summary>
        /// 参考线厚度
        /// </summary>
        public const double GuideLineThickness = 2.0d;

        /// <summary>
        /// 参考线可见性依赖属性
        /// </summary>
        public static readonly DependencyProperty GuideLinesVisibilityProperty;

        /// <summary>
        /// 选中缩放依赖属性
        /// </summary>
        public static readonly DependencyProperty ScaleCheckedProperty;

        /// <summary>
        /// 选中拖拽依赖属性
        /// </summary>
        public static readonly DependencyProperty DragCheckedProperty;

        /// <summary>
        /// 选中编辑依赖属性
        /// </summary>
        public static readonly DependencyProperty ResizeCheckedProperty;

        /// <summary>
        /// 选中点依赖属性
        /// </summary>
        public static readonly DependencyProperty PointCheckedProperty;

        /// <summary>
        /// 选中线段依赖属性
        /// </summary>
        public static readonly DependencyProperty LineCheckedProperty;

        /// <summary>
        /// 选中画刷依赖属性
        /// </summary>
        public static readonly DependencyProperty BrushCheckedProperty;

        /// <summary>
        /// 选中矩形依赖属性
        /// </summary>
        public static readonly DependencyProperty RectangleCheckedProperty;

        /// <summary>
        /// 选中圆形依赖属性
        /// </summary>
        public static readonly DependencyProperty CircleCheckedProperty;

        /// <summary>
        /// 选中椭圆形依赖属性
        /// </summary>
        public static readonly DependencyProperty EllipseCheckedProperty;

        /// <summary>
        /// 选中多边形依赖属性
        /// </summary>
        public static readonly DependencyProperty PolygonCheckedProperty;

        /// <summary>
        /// 选中折线段依赖属性
        /// </summary>
        public static readonly DependencyProperty PolylineCheckedProperty;

        /// <summary>
        /// 形状数据列表依赖属性
        /// </summary>
        public static readonly DependencyProperty DatasSourceProperty;

        /// <summary>
        /// 形状点击路由事件
        /// </summary>
        public static readonly RoutedEvent ShapeClickEvent;

        /// <summary>
        /// 形状绘制完成路由事件
        /// </summary>
        public static readonly RoutedEvent ShapeDrawnEvent;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DrawableCanvasEx()
        {
            GuideLinesVisibilityProperty = DependencyProperty.Register(nameof(GuideLinesVisibility), typeof(Visibility), typeof(DrawableCanvasEx), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsRender));
            ScaleCheckedProperty = DependencyProperty.Register(nameof(ScaleChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnScaleCheckedChanged));
            DragCheckedProperty = DependencyProperty.Register(nameof(DragChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnDragCheckedChanged));
            ResizeCheckedProperty = DependencyProperty.Register(nameof(ResizeChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnResizeCheckedChanged));
            PointCheckedProperty = DependencyProperty.Register(nameof(PointChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnPointCheckedChanged));
            LineCheckedProperty = DependencyProperty.Register(nameof(LineChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnLineCheckedChanged));
            BrushCheckedProperty = DependencyProperty.Register(nameof(BrushChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnBrushCheckedChanged));
            RectangleCheckedProperty = DependencyProperty.Register(nameof(RectangleChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnRectangleCheckedChanged));
            CircleCheckedProperty = DependencyProperty.Register(nameof(CircleChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnCircleCheckedChanged));
            EllipseCheckedProperty = DependencyProperty.Register(nameof(EllipseChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnEllipseCheckedChanged));
            PolygonCheckedProperty = DependencyProperty.Register(nameof(PolygonChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnPolygonCheckedChanged));
            PolylineCheckedProperty = DependencyProperty.Register(nameof(PolylineChecked), typeof(bool), typeof(DrawableCanvasEx), new PropertyMetadata(true, OnPolylineCheckedChanged));
            DatasSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(ObservableCollection<ShapeL>), typeof(DrawableCanvasEx), new PropertyMetadata(new ObservableCollection<ShapeL>()));
            ShapeClickEvent = EventManager.RegisterRoutedEvent(nameof(ShapeClick), RoutingStrategy.Direct, typeof(ShapeEventHandler), typeof(DrawableCanvasEx));
            ShapeDrawnEvent = EventManager.RegisterRoutedEvent(nameof(ShapeDrawn), RoutingStrategy.Direct, typeof(ShapeEventHandler), typeof(DrawableCanvasEx));
        }

        /// <summary>
        /// 实例构造器
        /// </summary>
        public DrawableCanvasEx()
        {
            //默认值
            this._polyAnchors = new List<PointVisual2D>();
            this._polyAnchorLines = new List<Line>();
            this._horizontalLine = new Line();
            this._verticalLine = new Line();

            //初始化参考线
            this.Children.Add(this._horizontalLine);
            this.Children.Add(this._verticalLine);
            this.InitGuidLines();

            //注册事件
            this.ElementDrag += this.OnDragElement;
            this.ElementResize += this.OnResizeElement;
            this.Draw += this.OnDraw;
            this.Drawing += this.OnDrawing;
            this.Drawn += this.OnDrawn;
            this.MouseMove += this.OnCanvasMouseMove;
            this.MouseWheel += this.OnCanvasMouseWheel;
            this.MouseMove += this.OnCanvasMouseMove;
        }

        #endregion

        #region # 属性

        #region 依赖属性 - 参考线可见性 —— Color GuideLinesVisibility
        /// <summary>
        /// 依赖属性 - 参考线可见性
        /// </summary>
        public Visibility GuideLinesVisibility
        {
            get => (Visibility)this.GetValue(GuideLinesVisibilityProperty);
            set => this.SetValue(GuideLinesVisibilityProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中缩放 —— bool ScaleChecked
        /// <summary>
        /// 依赖属性 - 选中缩放
        /// </summary>
        public bool ScaleChecked
        {
            get => (bool)this.GetValue(ScaleCheckedProperty);
            set => this.SetValue(ScaleCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中拖拽 —— bool DragChecked
        /// <summary>
        /// 依赖属性 - 选中拖拽
        /// </summary>
        public bool DragChecked
        {
            get => (bool)this.GetValue(DragCheckedProperty);
            set => this.SetValue(DragCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中编辑 —— bool ResizeChecked
        /// <summary>
        /// 依赖属性 - 选中编辑
        /// </summary>
        public bool ResizeChecked
        {
            get => (bool)this.GetValue(ResizeCheckedProperty);
            set => this.SetValue(ResizeCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中点 —— bool PointChecked
        /// <summary>
        /// 依赖属性 - 选中点
        /// </summary>
        public bool PointChecked
        {
            get => (bool)this.GetValue(PointCheckedProperty);
            set => this.SetValue(PointCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中线段 —— bool LineChecked
        /// <summary>
        /// 依赖属性 - 选中线段
        /// </summary>
        public bool LineChecked
        {
            get => (bool)this.GetValue(LineCheckedProperty);
            set => this.SetValue(LineCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中画刷 —— bool BrushChecked
        /// <summary>
        /// 依赖属性 - 选中画刷
        /// </summary>
        public bool BrushChecked
        {
            get => (bool)this.GetValue(BrushCheckedProperty);
            set => this.SetValue(BrushCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中矩形 —— bool RectangleChecked
        /// <summary>
        /// 依赖属性 - 选中矩形
        /// </summary>
        public bool RectangleChecked
        {
            get => (bool)this.GetValue(RectangleCheckedProperty);
            set => this.SetValue(RectangleCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中圆形 —— bool CircleChecked
        /// <summary>
        /// 依赖属性 - 选中圆形
        /// </summary>
        public bool CircleChecked
        {
            get => (bool)this.GetValue(CircleCheckedProperty);
            set => this.SetValue(CircleCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中椭圆形 —— bool EllipseChecked
        /// <summary>
        /// 依赖属性 - 选中椭圆形
        /// </summary>
        public bool EllipseChecked
        {
            get => (bool)this.GetValue(EllipseCheckedProperty);
            set => this.SetValue(EllipseCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中多边形 —— bool PolygonChecked
        /// <summary>
        /// 依赖属性 - 选中多边形
        /// </summary>
        public bool PolygonChecked
        {
            get => (bool)this.GetValue(PolygonCheckedProperty);
            set => this.SetValue(PolygonCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 选中折线段 —— bool PolylineChecked
        /// <summary>
        /// 依赖属性 - 选中折线段
        /// </summary>
        public bool PolylineChecked
        {
            get => (bool)this.GetValue(PolylineCheckedProperty);
            set => this.SetValue(PolylineCheckedProperty, value);
        }
        #endregion

        #region 依赖属性 - 形状数据列表 —— ObservableCollection<Shape> DatasSource
        /// <summary>
        /// 依赖属性 - 形状数据列表
        /// </summary>
        public ObservableCollection<ShapeL> DatasSource
        {
            get => (ObservableCollection<ShapeL>)this.GetValue(DatasSourceProperty);
            set => this.SetValue(DatasSourceProperty, value);
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

        #region 路由事件 - 形状绘制完成 —— event ShapeEventHandler ShapeDrawn
        /// <summary>
        /// 路由事件 - 形状绘制完成
        /// </summary>
        public event ShapeEventHandler ShapeDrawn
        {
            add => base.AddHandler(ShapeDrawnEvent, value);
            remove => base.RemoveHandler(ShapeDrawnEvent, value);
        }
        #endregion

        #endregion

        #region # 方法

        //Events

        #region 拖拽元素事件 —— void OnDragElement(CanvasEx canvas...
        /// <summary>
        /// 拖拽元素事件
        /// </summary>
        public void OnDragElement(CanvasEx canvas, RoutedEventArgs eventArgs)
        {
            double leftMargin = canvas.GetRectifiedLeft(canvas.SelectedVisual);
            double topMargin = canvas.GetRectifiedTop(canvas.SelectedVisual);

            if (canvas.SelectedVisual is PointVisual2D point)
            {
                this.RebuildPoint(point, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Line line)
            {
                this.RebuildLine(line, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is RectangleVisual2D rectangle)
            {
                this.RebuildRectangle(rectangle, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is CircleVisual2D circle)
            {
                this.RebuildCircle(circle, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is EllipseVisual2D ellipse)
            {
                this.RebuildEllipse(ellipse, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Polygon polygon)
            {
                this.RebuildPolygon(polygon, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Polyline polyline)
            {
                this.RebuildPolyline(polyline, leftMargin, topMargin);
            }
        }
        #endregion

        #region 改变元素尺寸事件 —— void OnResizeElement(CanvasEx canvas...
        /// <summary>
        /// 改变元素尺寸事件
        /// </summary>
        public void OnResizeElement(CanvasEx canvas, RoutedEventArgs eventArgs)
        {
            double leftMargin = canvas.GetRectifiedLeft(canvas.SelectedVisual);
            double topMargin = canvas.GetRectifiedTop(canvas.SelectedVisual);

            if (canvas.SelectedVisual is Line line)
            {
                Vector vectorA = canvas.RectifiedMousePosition!.Value - new Point(line.X1 + leftMargin, line.Y1 + topMargin);
                Vector vectorB = canvas.RectifiedMousePosition!.Value - new Point(line.X2 + leftMargin, line.Y2 + topMargin);
                if (vectorA.Length < vectorB.Length)
                {
                    line.X1 = canvas.RectifiedMousePosition!.Value.X - leftMargin;
                    line.Y1 = canvas.RectifiedMousePosition!.Value.Y - topMargin;
                }
                else
                {
                    line.X2 = canvas.RectifiedMousePosition!.Value.X - leftMargin;
                    line.Y2 = canvas.RectifiedMousePosition!.Value.Y - topMargin;
                }

                this.RebuildLine(line, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is RectangleVisual2D rectangle)
            {
                Point retifiedVertex = new Point(rectangle.Location.X + leftMargin, rectangle.Location.Y + topMargin);
                double width = canvas.RectifiedMousePosition!.Value.X - retifiedVertex.X;
                double height = canvas.RectifiedMousePosition!.Value.Y - retifiedVertex.Y;
                if (width > 0 && height > 0)
                {
                    rectangle.Size = new Size(width, height);
                }

                this.RebuildRectangle(rectangle, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is CircleVisual2D circle)
            {
                Point retifiedCenter = new Point(circle.Center.X + leftMargin, circle.Center.Y + topMargin);
                Vector vector = retifiedCenter - canvas.RectifiedMousePosition!.Value;
                circle.Radius = Math.Abs(vector.Length);

                this.RebuildCircle(circle, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is EllipseVisual2D ellipse)
            {
                Point retifiedCenter = new Point(ellipse.Center.X + leftMargin, ellipse.Center.Y + topMargin);
                ellipse.RadiusX = Math.Abs(retifiedCenter.X - canvas.RectifiedMousePosition!.Value.X);
                ellipse.RadiusY = Math.Abs(retifiedCenter.Y - canvas.RectifiedMousePosition!.Value.Y);

                this.RebuildEllipse(ellipse, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Polygon polygon)
            {
                double minDistance = int.MaxValue;
                Point? nearestPoint = null;
                foreach (Point point in polygon.Points)
                {
                    Vector vector = canvas.RectifiedMousePosition!.Value - new Point(point.X + leftMargin, point.Y + topMargin);
                    if (vector.Length < minDistance)
                    {
                        minDistance = vector.Length;
                        nearestPoint = point;
                    }
                }
                if (nearestPoint.HasValue)
                {
                    int index = polygon.Points.IndexOf(nearestPoint.Value);
                    polygon.Points.Remove(nearestPoint.Value);
                    Point newPoint = new Point(canvas.RectifiedMousePosition!.Value.X - leftMargin, canvas.RectifiedMousePosition!.Value.Y - topMargin);
                    polygon.Points.Insert(index, newPoint);
                }

                this.RebuildPolygon(polygon, leftMargin, topMargin);
            }
            if (canvas.SelectedVisual is Polyline polyline)
            {
                double minDistance = int.MaxValue;
                Point? nearestPoint = null;
                foreach (Point point in polyline.Points)
                {
                    Vector vector = canvas.RectifiedMousePosition!.Value - new Point(point.X + leftMargin, point.Y + topMargin);
                    if (vector.Length < minDistance)
                    {
                        minDistance = vector.Length;
                        nearestPoint = point;
                    }
                }
                if (nearestPoint.HasValue)
                {
                    int index = polyline.Points.IndexOf(nearestPoint.Value);
                    polyline.Points.Remove(nearestPoint.Value);
                    Point newPoint = new Point(canvas.RectifiedMousePosition!.Value.X - leftMargin, canvas.RectifiedMousePosition!.Value.Y - topMargin);
                    polyline.Points.Insert(index, newPoint);
                    polyline.Points = polyline.Points;
                }

                this.RebuildPolyline(polyline, leftMargin, topMargin);
            }
        }
        #endregion

        #region 绘制开始事件 —— void OnDraw(CanvasEx canvas...
        /// <summary>
        /// 绘制开始事件
        /// </summary>
        public void OnDraw(CanvasEx canvas, RoutedEventArgs eventArgs)
        {
            if (this.PointChecked)
            {
                this.DrawPoint(canvas);
            }
            if (this.PolygonChecked || this.PolylineChecked)
            {
                if (canvas.SelectedVisual is PointVisual2D element && this._polyAnchors.Any() && element == this._polyAnchors[0])
                {
                    //多边形
                    if (this.PolygonChecked)
                    {
                        this.DrawPolygon(canvas);
                    }
                    //折线段
                    if (this.PolylineChecked)
                    {
                        this.DrawPolyline(canvas);
                    }
                }
                else
                {
                    //锚点
                    this.DrawPolyAnchor(canvas);
                }
            }
        }
        #endregion

        #region 绘制中事件 —— void OnDrawing(CanvasEx canvas...
        /// <summary>
        /// 绘制中事件
        /// </summary>
        public void OnDrawing(CanvasEx canvas, RoutedEventArgs eventArgs)
        {
            if (this.LineChecked)
            {
                this.DrawLine(canvas);
            }
            if (this.BrushChecked)
            {
                this.DrawBrush(canvas);
            }
            if (this.RectangleChecked)
            {
                this.DrawRectangle(canvas);
            }
            if (this.CircleChecked)
            {
                this.DrawCircle(canvas);
            }
            if (this.EllipseChecked)
            {
                this.DrawEllipse(canvas);
            }
        }
        #endregion

        #region 绘制完成事件 —— void OnDrawn(CanvasEx canvas...
        /// <summary>
        /// 绘制完成事件
        /// </summary>
        public void OnDrawn(CanvasEx canvas, RoutedEventArgs eventArgs)
        {
            if (this._line != null)
            {
                int x1 = (int)Math.Ceiling(this._line.X1);
                int y1 = (int)Math.Ceiling(this._line.Y1);
                int x2 = (int)Math.Ceiling(this._line.X2);
                int y2 = (int)Math.Ceiling(this._line.Y2);
                LineL lineL = new LineL(new PointL(x1, y1), new PointL(x2, y2));

                this._line.Tag = lineL;
                lineL.Tag = this._line;
                this.DatasSource.Add(lineL);
                this.ItemsSource.Add(this._line);

                //挂起路由事件
                ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeDrawnEvent, this._line, lineL);
                this.RaiseEvent(shapeEventArgs);
            }
            if (this._brush != null)
            {
                //构建点集
                IList<PointL> pointLs = new List<PointL>();
                foreach (Point point in this._brush.Points)
                {
                    int x = (int)Math.Ceiling(point.X);
                    int y = (int)Math.Ceiling(point.Y);
                    PointL pointL = new PointL(x, y);
                    pointLs.Add(pointL);
                }

                PolylineL polylineL = new PolylineL(pointLs);
                this._brush.Tag = polylineL;
                polylineL.Tag = this._brush;
                this.DatasSource.Add(polylineL);
                this.ItemsSource.Add(this._brush);

                //挂起路由事件
                ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeDrawnEvent, this._brush, polylineL);
                this.RaiseEvent(shapeEventArgs);
            }
            if (this._rectangle != null)
            {
                int x = (int)Math.Ceiling(this._rectangle.Location.X);
                int y = (int)Math.Ceiling(this._rectangle.Location.Y);
                int width = (int)Math.Ceiling(this._rectangle.Size.Width);
                int height = (int)Math.Ceiling(this._rectangle.Size.Height);
                RectangleL rectangleL = new RectangleL(x, y, width, height);

                this._rectangle.Tag = rectangleL;
                rectangleL.Tag = this._rectangle;
                this.DatasSource.Add(rectangleL);
                this.ItemsSource.Add(this._rectangle);

                //挂起路由事件
                ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeDrawnEvent, this._rectangle, rectangleL);
                this.RaiseEvent(shapeEventArgs);
            }
            if (this._circle != null)
            {
                int x = (int)Math.Ceiling(this._circle.Center.X);
                int y = (int)Math.Ceiling(this._circle.Center.Y);
                int radius = (int)Math.Ceiling(this._circle.Radius);
                CircleL circleL = new CircleL(x, y, radius);

                this._circle.Tag = circleL;
                circleL.Tag = this._circle;
                this.DatasSource.Add(circleL);
                this.ItemsSource.Add(this._circle);

                //挂起路由事件
                ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeDrawnEvent, this._circle, circleL);
                this.RaiseEvent(shapeEventArgs);
            }
            if (this._ellipse != null)
            {
                int x = (int)Math.Ceiling(this._ellipse.Center.X);
                int y = (int)Math.Ceiling(this._ellipse.Center.Y);
                int radiusX = (int)Math.Ceiling(this._ellipse.RadiusX);
                int radiusY = (int)Math.Ceiling(this._ellipse.RadiusY);
                EllipseL ellipseL = new EllipseL(x, y, radiusX, radiusY);

                this._ellipse.Tag = ellipseL;
                ellipseL.Tag = this._ellipse;
                this.DatasSource.Add(ellipseL);
                this.ItemsSource.Add(this._ellipse);

                //挂起路由事件
                ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeDrawnEvent, this._ellipse, ellipseL);
                this.RaiseEvent(shapeEventArgs);
            }

            this._line = null;
            this._brush = null;
            this._rectangle = null;
            this._circle = null;
            this._ellipse = null;
        }
        #endregion

        #region 形状鼠标左击事件 —— void OnShapeMouseLeftDown(object sender...
        /// <summary>
        /// 形状鼠标左击事件
        /// </summary>
        private void OnShapeMouseLeftDown(object sender, MouseButtonEventArgs eventArgs)
        {
            Shape shape = (Shape)sender;
            ShapeL shapeL = (ShapeL)shape.Tag;
            ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeClickEvent, shape, shapeL);

            //挂起路由事件
            this.RaiseEvent(shapeEventArgs);
        }
        #endregion

        #region 画布鼠标移动事件 —— void OnCanvasMouseMove(object sender...
        /// <summary>
        /// 画布鼠标移动事件
        /// </summary>
        public void OnCanvasMouseMove(object sender, MouseEventArgs eventArgs)
        {
            CanvasEx canvas = (CanvasEx)sender;

            //十字参考线
            if (this.BackgroundImage != null)
            {
                Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;

                //参考线坐标调整
                this._horizontalLine.Y1 = rectifiedPosition.Y > this.BackgroundImage.Source.Height
                    ? this.BackgroundImage.Source.Height
                    : rectifiedPosition.Y < 0 ? 0 : rectifiedPosition.Y;
                this._horizontalLine.Y2 = this._horizontalLine.Y1;
                this._verticalLine.X1 = rectifiedPosition.X > this.BackgroundImage.Source.Width
                    ? this.BackgroundImage.Source.Width
                    : rectifiedPosition.X < 0 ? 0 : rectifiedPosition.X;
                this._verticalLine.X2 = this._verticalLine.X1;
                if (!canvas.ScaledRatio.Equals(0))
                {
                    this._horizontalLine.StrokeThickness = GuideLineThickness / canvas.ScaledRatio;
                    this._verticalLine.StrokeThickness = GuideLineThickness / canvas.ScaledRatio;
                }

                //设置光标
                Mouse.OverrideCursor = Cursors.Cross;
            }
            //实时锚线
            if ((this.PolygonChecked || this.PolylineChecked) && this._polyAnchors.Any())
            {
                if (this._realAnchorLine == null)
                {
                    this._realAnchorLine = new Line
                    {
                        Fill = new SolidColorBrush(Colors.Transparent),
                        Stroke = this.BorderBrush,
                        StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                        RenderTransform = canvas.MatrixTransform
                    };
                    canvas.Children.Add(this._realAnchorLine);
                }
                PointVisual2D startPoint = this._polyAnchors.Last();
                Point endPoint = canvas.RectifiedMousePosition!.Value;
                this._realAnchorLine.X1 = startPoint.X;
                this._realAnchorLine.Y1 = startPoint.Y;
                this._realAnchorLine.X2 = endPoint.X;
                this._realAnchorLine.Y2 = endPoint.Y;
            }
        }
        #endregion

        #region 画布鼠标滚轮事件 —— void OnCanvasMouseWheel(object sender...
        /// <summary>
        /// 画布鼠标滚轮事件
        /// </summary>
        public void OnCanvasMouseWheel(object sender, MouseEventArgs eventArgs)
        {
            CanvasEx canvas = (CanvasEx)sender;

            //参考线粗细调整
            if (!canvas.ScaledRatio.Equals(0))
            {
                this._horizontalLine.StrokeThickness = GuideLineThickness / canvas.ScaledRatio;
                this._verticalLine.StrokeThickness = GuideLineThickness / canvas.ScaledRatio;
            }
            //图形边框粗细调整
            foreach (Shape shape in canvas.Children.OfType<Shape>())
            {
                shape.StrokeThickness = this.BorderThickness / canvas.ScaledRatio;
            }
            foreach (PointVisual2D pointVisual2D in canvas.Children.OfType<PointVisual2D>())
            {
                pointVisual2D.Thickness = PointVisual2D.DefaultThickness / canvas.ScaledRatio;
            }
        }
        #endregion

        #region 画布鼠标离开事件 —— void OnCanvasMouseLeave(object sender...
        /// <summary>
        /// 画布鼠标离开事件
        /// </summary>
        public void OnCanvasMouseLeave(object sender, MouseEventArgs eventArgs)
        {
            //设置光标
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        #endregion

        #region 选中缩放改变事件 —— static void OnScaleCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中缩放改变事件
        /// </summary>
        private static void OnScaleCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool scaleChecked = (bool)eventArgs.NewValue;
            if (scaleChecked)
            {
                canvas.Mode = CanvasMode.Scale;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中拖拽改变事件 —— static void OnDragCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中拖拽改变事件
        /// </summary>
        private static void OnDragCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool dragChecked = (bool)eventArgs.NewValue;
            if (dragChecked)
            {
                canvas.Mode = CanvasMode.Drag;
                canvas.ScaleChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中编辑改变事件 —— static void OnResizeCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中编辑改变事件
        /// </summary>
        private static void OnResizeCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool resizeChecked = (bool)eventArgs.NewValue;
            if (resizeChecked)
            {
                canvas.Mode = CanvasMode.Resize;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中点改变事件 —— static void OnPointCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中点改变事件
        /// </summary>
        private static void OnPointCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool pointChecked = (bool)eventArgs.NewValue;
            if (pointChecked)
            {
                canvas.Mode = CanvasMode.Draw;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中线段改变事件 —— static void OnLineCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中线段改变事件
        /// </summary>
        private static void OnLineCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool lineChecked = (bool)eventArgs.NewValue;
            if (lineChecked)
            {
                canvas.Mode = CanvasMode.Draw;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中画刷改变事件 —— static void OnBrushCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中画刷改变事件
        /// </summary>
        private static void OnBrushCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool brushChecked = (bool)eventArgs.NewValue;
            if (brushChecked)
            {
                canvas.Mode = CanvasMode.Draw;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中矩形改变事件 —— static void OnRectangleCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中矩形改变事件
        /// </summary>
        private static void OnRectangleCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool rectangleChecked = (bool)eventArgs.NewValue;
            if (rectangleChecked)
            {
                canvas.Mode = CanvasMode.Draw;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中圆形改变事件 —— static void OnCircleCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中圆形改变事件
        /// </summary>
        private static void OnCircleCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool circleChecked = (bool)eventArgs.NewValue;
            if (circleChecked)
            {
                canvas.Mode = CanvasMode.Draw;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中椭圆形改变事件 —— static void OnEllipseCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中椭圆形改变事件
        /// </summary>
        private static void OnEllipseCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool ellipseChecked = (bool)eventArgs.NewValue;
            if (ellipseChecked)
            {
                canvas.Mode = CanvasMode.Draw;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.PolygonChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中多边形改变事件 —— static void OnPolygonCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中多边形改变事件
        /// </summary>
        private static void OnPolygonCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool polygonChecked = (bool)eventArgs.NewValue;
            if (polygonChecked)
            {
                canvas.Mode = CanvasMode.Draw;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolylineChecked = false;
            }
        }
        #endregion

        #region 选中折线段改变事件 —— static void OnPolylineCheckedChanged(DependencyObject dependencyObject...
        /// <summary>
        /// 选中折线段改变事件
        /// </summary>
        private static void OnPolylineCheckedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            DrawableCanvasEx canvas = (DrawableCanvasEx)dependencyObject;
            bool polylineChecked = (bool)eventArgs.NewValue;
            if (polylineChecked)
            {
                canvas.Mode = CanvasMode.Draw;
                canvas.ScaleChecked = false;
                canvas.DragChecked = false;
                canvas.ResizeChecked = false;
                canvas.PointChecked = false;
                canvas.LineChecked = false;
                canvas.BrushChecked = false;
                canvas.RectangleChecked = false;
                canvas.CircleChecked = false;
                canvas.EllipseChecked = false;
                canvas.PolygonChecked = false;
            }
        }
        #endregion


        //Private

        #region 初始化参考线 —— void InitGuidLines()
        /// <summary>
        /// 初始化参考线
        /// </summary>
        private void InitGuidLines()
        {
            DropShadowEffect effect = new DropShadowEffect
            {
                BlurRadius = 0,
                ShadowDepth = 0,
                Color = Colors.Transparent
            };
            SolidColorBrush lineBrush = new SolidColorBrush(Color.FromArgb(0xC8, 0, 0, 0));

            this._horizontalLine.X1 = 0;
            this._horizontalLine.Stroke = lineBrush;
            this._horizontalLine.StrokeThickness = GuideLineThickness;
            this._horizontalLine.RenderTransform = base.MatrixTransform;
            this._horizontalLine.Effect = effect;

            this._verticalLine.Y1 = 0;
            this._verticalLine.Stroke = lineBrush;
            this._verticalLine.StrokeThickness = GuideLineThickness;
            this._verticalLine.RenderTransform = base.MatrixTransform;
            this._verticalLine.Effect = effect;

            Binding horizontalLineX2Binding = new Binding(nameof(Width));
            Binding verticalLineY2Binding = new Binding(nameof(Height));
            Binding visibilityBinding = new Binding(nameof(GuideLinesVisibility));
            horizontalLineX2Binding.Source = this.BackgroundImage.Source;
            verticalLineY2Binding.Source = this.BackgroundImage.Source;
            visibilityBinding.Source = this;
            this._horizontalLine.SetBinding(Line.X2Property, horizontalLineX2Binding);
            this._horizontalLine.SetBinding(Line.VisibilityProperty, visibilityBinding);
            this._verticalLine.SetBinding(Line.Y2Property, verticalLineY2Binding);
            this._verticalLine.SetBinding(Line.VisibilityProperty, visibilityBinding);

            SetZIndex(this._horizontalLine, -1);
            SetZIndex(this._verticalLine, -1);
            SetDraggable(this._horizontalLine, false);
            SetDraggable(this._verticalLine, false);
            SetResizable(this._horizontalLine, false);
            SetResizable(this._verticalLine, false);
        }
        #endregion

        #region 重建点 —— void RebuildPoint(PointVisual2D point...
        /// <summary>
        /// 重建点
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildPoint(PointVisual2D point, double leftMargin, double topMargin)
        {
            PointL pointL = (PointL)point.Tag;
            int index = this.DatasSource.IndexOf(pointL);
            if (index != -1)
            {
                this.DatasSource.Remove(pointL);

                int x = (int)Math.Ceiling(point.X + leftMargin);
                int y = (int)Math.Ceiling(point.Y + topMargin);
                PointL newPointL = new PointL(x, y);

                point.Tag = newPointL;
                newPointL.Tag = point;
                this.DatasSource.Insert(index, newPointL);
            }
        }
        #endregion

        #region 重建线段 —— void RebuildLine(Line line...
        /// <summary>
        /// 重建线段
        /// </summary>
        /// <param name="line">线段</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildLine(Line line, double leftMargin, double topMargin)
        {
            LineL lineL = (LineL)line.Tag;
            int index = this.DatasSource.IndexOf(lineL);
            if (index != -1)
            {
                this.DatasSource.Remove(lineL);

                int x1 = (int)Math.Ceiling(line.X1 + leftMargin);
                int y1 = (int)Math.Ceiling(line.Y1 + topMargin);
                int x2 = (int)Math.Ceiling(line.X2 + leftMargin);
                int y2 = (int)Math.Ceiling(line.Y2 + topMargin);
                LineL newLineL = new LineL(new PointL(x1, y1), new PointL(x2, y2));

                line.Tag = newLineL;
                newLineL.Tag = line;
                this.DatasSource.Insert(index, newLineL);
            }
        }
        #endregion

        #region 重建矩形 —— void RebuildRectangle(RectangleVisual2D rectangle...
        /// <summary>
        /// 重建矩形
        /// </summary>
        /// <param name="rectangle">矩形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildRectangle(RectangleVisual2D rectangle, double leftMargin, double topMargin)
        {
            RectangleL rectangleL = (RectangleL)rectangle.Tag;
            int index = this.DatasSource.IndexOf(rectangleL);
            if (index != -1)
            {
                this.DatasSource.Remove(rectangleL);

                int x = (int)Math.Ceiling(rectangle.Location.X + leftMargin);
                int y = (int)Math.Ceiling(rectangle.Location.Y + topMargin);
                int width = (int)Math.Ceiling(rectangle.Size.Width);
                int height = (int)Math.Ceiling(rectangle.Size.Height);
                RectangleL newRectangleL = new RectangleL(x, y, width, height);

                rectangle.Tag = newRectangleL;
                newRectangleL.Tag = rectangle;
                this.DatasSource.Insert(index, newRectangleL);
            }
        }
        #endregion

        #region 重建圆形 —— void RebuildCircle(CircleVisual2D circle...
        /// <summary>
        /// 重建圆形
        /// </summary>
        /// <param name="circle">圆形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildCircle(CircleVisual2D circle, double leftMargin, double topMargin)
        {
            CircleL circleL = (CircleL)circle.Tag;
            int index = this.DatasSource.IndexOf(circleL);
            if (index != -1)
            {
                this.DatasSource.Remove(circleL);

                int x = (int)Math.Ceiling(circle.Center.X + leftMargin);
                int y = (int)Math.Ceiling(circle.Center.Y + topMargin);
                int radius = (int)Math.Ceiling(circle.Radius);
                CircleL newCircleL = new CircleL(x, y, radius);

                circle.Tag = newCircleL;
                newCircleL.Tag = circle;
                this.DatasSource.Insert(index, newCircleL);
            }
        }
        #endregion

        #region 重建椭圆形 —— void RebuildEllipse(EllipseVisual2D ellipse...
        /// <summary>
        /// 重建椭圆形
        /// </summary>
        /// <param name="ellipse">椭圆形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildEllipse(EllipseVisual2D ellipse, double leftMargin, double topMargin)
        {
            EllipseL ellipseL = (EllipseL)ellipse.Tag;
            int index = this.DatasSource.IndexOf(ellipseL);
            if (index != -1)
            {
                this.DatasSource.Remove(ellipseL);

                int x = (int)Math.Ceiling(ellipse.Center.X + leftMargin);
                int y = (int)Math.Ceiling(ellipse.Center.Y + topMargin);
                int radiusX = (int)Math.Ceiling(ellipse.RadiusX);
                int radiusY = (int)Math.Ceiling(ellipse.RadiusY);
                EllipseL newEllipseL = new EllipseL(x, y, radiusX, radiusY);

                ellipse.Tag = newEllipseL;
                newEllipseL.Tag = ellipse;
                this.DatasSource.Insert(index, newEllipseL);
            }
        }
        #endregion

        #region 重建多边形 —— void RebuildPolygon(Polygon polygon...
        /// <summary>
        /// 重建多边形
        /// </summary>
        /// <param name="polygon">多边形</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildPolygon(Polygon polygon, double leftMargin, double topMargin)
        {
            PolygonL polygonL = (PolygonL)polygon.Tag;
            int index = this.DatasSource.IndexOf(polygonL);
            if (index != -1)
            {
                this.DatasSource.Remove(polygonL);

                IList<PointL> pointLs = new List<PointL>();
                foreach (Point point in polygon.Points)
                {
                    int x = (int)Math.Ceiling(point.X + leftMargin);
                    int y = (int)Math.Ceiling(point.Y + topMargin);
                    PointL pointL = new PointL(x, y);
                    pointLs.Add(pointL);
                }
                PolygonL newPolygonL = new PolygonL(pointLs);

                polygon.Tag = newPolygonL;
                newPolygonL.Tag = polygon;
                this.DatasSource.Insert(index, newPolygonL);
            }
        }
        #endregion

        #region 重建折线段 —— void RebuildPolyline(Polyline polyline...
        /// <summary>
        /// 重建折线段
        /// </summary>
        /// <param name="polyline">折线段</param>
        /// <param name="leftMargin">左边距</param>
        /// <param name="topMargin">上边距</param>
        private void RebuildPolyline(Polyline polyline, double leftMargin, double topMargin)
        {
            PolylineL polylineL = (PolylineL)polyline.Tag;
            int index = this.DatasSource.IndexOf(polylineL);
            if (index != -1)
            {
                this.DatasSource.Remove(polylineL);

                IList<PointL> pointLs = new List<PointL>();
                foreach (Point point in polyline.Points)
                {
                    int x = (int)Math.Ceiling(point.X + leftMargin);
                    int y = (int)Math.Ceiling(point.Y + topMargin);
                    PointL pointL = new PointL(x, y);
                    pointLs.Add(pointL);
                }
                PolylineL newPolylineL = new PolylineL(pointLs);

                polyline.Tag = newPolylineL;
                newPolylineL.Tag = polyline;
                this.DatasSource.Insert(index, newPolylineL);
            }
        }
        #endregion

        #region 绘制点 —— void DrawPoint(CanvasEx canvas)
        /// <summary>
        /// 绘制点
        /// </summary>
        private void DrawPoint(CanvasEx canvas)
        {
            Point rectifiedVertex = canvas.RectifiedStartPosition!.Value;
            int x = (int)Math.Ceiling(rectifiedVertex.X);
            int y = (int)Math.Ceiling(rectifiedVertex.Y);

            PointL pointL = new PointL(x, y);
            PointVisual2D point = new PointVisual2D
            {
                X = rectifiedVertex.X,
                Y = rectifiedVertex.Y,
                Thickness = PointVisual2D.DefaultThickness / canvas.ScaledRatio,
                Fill = new SolidColorBrush(Colors.Black),
                Stroke = this.BorderBrush,
                StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                RenderTransform = canvas.MatrixTransform,
                Tag = pointL
            };

            pointL.Tag = point;
            this.DatasSource.Add(pointL);
            this.ItemsSource.Add(point);

            //事件处理
            point.MouseLeftButtonDown += this.OnShapeMouseLeftDown;

            //挂起路由事件
            ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeDrawnEvent, point, pointL);
            this.RaiseEvent(shapeEventArgs);
        }
        #endregion

        #region 绘制线段 —— void DrawLine(CanvasEx canvas)
        /// <summary>
        /// 绘制线段
        /// </summary>
        private void DrawLine(CanvasEx canvas)
        {
            if (this._line == null)
            {
                this._line = new Line
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = this.BorderBrush,
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._line.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._line);
            }

            Point rectifiedVertex = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;

            this._line.X1 = rectifiedVertex.X;
            this._line.Y1 = rectifiedVertex.Y;
            this._line.X2 = rectifiedPosition.X;
            this._line.Y2 = rectifiedPosition.Y;
        }
        #endregion

        #region 绘制画刷 —— void DrawBrush(CanvasEx canvas)
        /// <summary>
        /// 绘制画刷
        /// </summary>
        private void DrawBrush(CanvasEx canvas)
        {
            if (this._brush == null)
            {
                this._brush = new Polyline
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = this.BorderBrush,
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._brush.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._brush);
            }

            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            this._brush.Points.Add(rectifiedPosition);
        }
        #endregion

        #region 绘制矩形 —— void DrawRectangle(CanvasEx canvas)
        /// <summary>
        /// 绘制矩形
        /// </summary>
        private void DrawRectangle(CanvasEx canvas)
        {
            if (this._rectangle == null)
            {
                this._rectangle = new RectangleVisual2D()
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = this.BorderBrush,
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._rectangle.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._rectangle);
            }

            Point rectifiedVertex = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;

            if (rectifiedPosition.X > rectifiedVertex.X && rectifiedPosition.Y > rectifiedVertex.Y)
            {
                this._rectangle.Location = rectifiedVertex;
            }
            if (rectifiedPosition.X > rectifiedVertex.X && rectifiedPosition.Y < rectifiedVertex.Y)
            {
                this._rectangle.Location = new Point(rectifiedVertex.X, rectifiedPosition.Y);
            }
            if (rectifiedPosition.X < rectifiedVertex.X && rectifiedPosition.Y > rectifiedVertex.Y)
            {
                this._rectangle.Location = new Point(rectifiedPosition.X, rectifiedVertex.Y);
            }
            if (rectifiedPosition.X < rectifiedVertex.X && rectifiedPosition.Y < rectifiedVertex.Y)
            {
                this._rectangle.Location = rectifiedPosition;
            }

            double width = Math.Abs(rectifiedPosition.X - rectifiedVertex.X);
            double height = Math.Abs(rectifiedPosition.Y - rectifiedVertex.Y);
            this._rectangle.Size = new Size(width, height);
        }
        #endregion

        #region 绘制圆形 —— void DrawCircle(CanvasEx canvas)
        /// <summary>
        /// 绘制圆形
        /// </summary>
        private void DrawCircle(CanvasEx canvas)
        {
            if (this._circle == null)
            {
                this._circle = new CircleVisual2D
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = this.BorderBrush,
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._circle.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._circle);
            }

            Point rectifiedCenter = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;
            Vector vector = Point.Subtract(rectifiedPosition, rectifiedCenter);

            this._circle.Center = rectifiedCenter;
            this._circle.Radius = vector.Length;
        }
        #endregion

        #region 绘制椭圆形 —— void DrawEllipse(CanvasEx canvas)
        /// <summary>
        /// 绘制椭圆形
        /// </summary>
        private void DrawEllipse(CanvasEx canvas)
        {
            if (this._ellipse == null)
            {
                this._ellipse = new EllipseVisual2D()
                {
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = this.BorderBrush,
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._ellipse.MouseLeftButtonDown += this.OnShapeMouseLeftDown;
                canvas.Children.Add(this._ellipse);
            }

            Point rectifiedCenter = canvas.RectifiedStartPosition!.Value;
            Point rectifiedPosition = canvas.RectifiedMousePosition!.Value;

            this._ellipse.Center = rectifiedCenter;
            this._ellipse.RadiusX = Math.Abs(rectifiedPosition.X - rectifiedCenter.X);
            this._ellipse.RadiusY = Math.Abs(rectifiedPosition.Y - rectifiedCenter.Y);
        }
        #endregion

        #region 绘制锚点 —— void DrawPolyAnchor(CanvasEx canvas)
        /// <summary>
        /// 绘制锚点
        /// </summary>
        private void DrawPolyAnchor(CanvasEx canvas)
        {
            Point rectifiedPoint = canvas.RectifiedStartPosition!.Value;
            Brush fillBrush = new SolidColorBrush(Colors.Black);
            Brush borderBrush = this._polyAnchors.Any()
                ? this.BorderBrush
                : new SolidColorBrush(Colors.Yellow);
            PointVisual2D anchor = new PointVisual2D
            {
                X = rectifiedPoint.X,
                Y = rectifiedPoint.Y,
                Thickness = PointVisual2D.DefaultThickness / canvas.ScaledRatio,
                Fill = fillBrush,
                Stroke = borderBrush,
                StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                RenderTransform = canvas.MatrixTransform
            };

            //绘制锚线
            if (this._polyAnchors.Any())
            {
                PointVisual2D lastAnchor = this._polyAnchors.Last();
                Line polyAnchorLine = new Line
                {
                    X1 = lastAnchor.X,
                    Y1 = lastAnchor.Y,
                    X2 = anchor.X,
                    Y2 = anchor.Y,
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stroke = this.BorderBrush,
                    StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                    RenderTransform = canvas.MatrixTransform
                };
                this._polyAnchorLines.Add(polyAnchorLine);
                canvas.Children.Add(polyAnchorLine);
            }
            //设置起始点在最上层
            else
            {
                Panel.SetZIndex(anchor, short.MaxValue);
            }

            this._polyAnchors.Add(anchor);
            canvas.Children.Add(anchor);
        }
        #endregion

        #region 绘制多边形 —— void DrawPolygon(CanvasEx canvas)
        /// <summary>
        /// 绘制多边形
        /// </summary>
        private void DrawPolygon(CanvasEx canvas)
        {
            //点集排序
            IEnumerable<Point> point2ds =
                from anchor in this._polyAnchors
                let leftMargin = canvas.GetRectifiedLeft(anchor)
                let topMargin = canvas.GetRectifiedTop(anchor)
                select new Point(anchor.X + leftMargin, anchor.Y + topMargin);
            PointCollection points = new PointCollection(point2ds);
            points = points.Sequentialize();

            //构建点集
            IEnumerable<PointL> pointLs =
                from point in points
                let x = (int)Math.Ceiling(point.X)
                let y = (int)Math.Ceiling(point.Y)
                select new PointL(x, y);

            PolygonL polygonL = new PolygonL(pointLs);
            Polygon polygon = new Polygon
            {
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = this.BorderBrush,
                StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                Points = points,
                RenderTransform = canvas.MatrixTransform,
                Tag = polygonL
            };

            polygonL.Tag = polygon;
            this.DatasSource.Add(polygonL);
            this.ItemsSource.Add(polygon);

            //清空锚点、锚线
            foreach (PointVisual2D anchor in this._polyAnchors)
            {
                canvas.Children.Remove(anchor);
            }
            foreach (Line anchorLine in this._polyAnchorLines)
            {
                canvas.Children.Remove(anchorLine);
            }
            if (this._realAnchorLine != null)
            {
                canvas.Children.Remove(this._realAnchorLine);
                this._realAnchorLine = null;
            }
            this._polyAnchors.Clear();
            this._polyAnchorLines.Clear();

            //事件处理
            polygon.MouseLeftButtonDown += this.OnShapeMouseLeftDown;

            //挂起路由事件
            ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeDrawnEvent, polygon, polygonL);
            this.RaiseEvent(shapeEventArgs);
        }
        #endregion

        #region 绘制折线段 —— void DrawPolyline(CanvasEx canvas)
        /// <summary>
        /// 绘制折线段
        /// </summary>
        private void DrawPolyline(CanvasEx canvas)
        {
            //构建点集
            IEnumerable<Point> point2ds =
                from anchor in this._polyAnchors
                let leftMargin = canvas.GetRectifiedLeft(anchor)
                let topMargin = canvas.GetRectifiedTop(anchor)
                select new Point(anchor.X + leftMargin, anchor.Y + topMargin);
            PointCollection points = new PointCollection(point2ds);
            IEnumerable<PointL> pointLs =
                from point in points
                let x = (int)Math.Ceiling(point.X)
                let y = (int)Math.Ceiling(point.Y)
                select new PointL(x, y);

            PolylineL polylineL = new PolylineL(pointLs);
            Polyline polyline = new Polyline
            {
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = this.BorderBrush,
                StrokeThickness = this.BorderThickness / canvas.ScaledRatio,
                Points = points,
                RenderTransform = canvas.MatrixTransform,
                Tag = polylineL
            };

            polylineL.Tag = polyline;
            this.DatasSource.Add(polylineL);
            this.ItemsSource.Add(polyline);

            //清空锚点、锚线
            foreach (PointVisual2D anchor in this._polyAnchors)
            {
                canvas.Children.Remove(anchor);
            }
            foreach (Line anchorLine in this._polyAnchorLines)
            {
                canvas.Children.Remove(anchorLine);
            }
            if (this._realAnchorLine != null)
            {
                canvas.Children.Remove(this._realAnchorLine);
                this._realAnchorLine = null;
            }
            this._polyAnchors.Clear();
            this._polyAnchorLines.Clear();

            //事件处理
            polyline.MouseLeftButtonDown += this.OnShapeMouseLeftDown;

            //挂起路由事件
            ShapeEventArgs shapeEventArgs = new ShapeEventArgs(ShapeDrawnEvent, polyline, polylineL);
            this.RaiseEvent(shapeEventArgs);
        }
        #endregion

        #endregion
    }
}
