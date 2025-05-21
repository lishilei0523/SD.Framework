using Avalonia;
using Avalonia.Animation;
using Avalonia.Media;
using Avalonia.Styling;
using SD.Avalonia.Controls.Shapes;
using SD.Infrastructure.Avalonia.Visual2Ds;
using SD.Infrastructure.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Infrastructure.Avalonia.Extensions
{
    /// <summary>
    /// 形状扩展
    /// </summary>
    public static class ShapeExtension
    {
        #region # 顺序化点集 —— static IList<Point> Sequentialize(this IList<Point> points)
        /// <summary>
        /// 顺序化点集
        /// </summary>
        /// <param name="points">点集</param>
        /// <returns>顺序化点集</returns>
        /// <remarks>用于排列多边形点集</remarks>
        public static IList<Point> Sequentialize(this IList<Point> points)
        {
            #region # 验证

            if (points == null)
            {
                throw new ArgumentNullException(nameof(points), "点集不可为null！");
            }
            if (!points.Any())
            {
                return new List<Point>();
            }

            #endregion

            double meanX = points.Average(point => point.X);
            double meanY = points.Average(point => point.Y);
            IOrderedEnumerable<Point> orderedPoints = points.OrderBy(point => Math.Atan2(point.Y - meanY, point.X - meanX));

            return new List<Point>(orderedPoints);
        }
        #endregion

        #region # 颜色映射 —— static Color ToColor(this ColorL colorL)
        /// <summary>
        /// 颜色映射
        /// </summary>
        public static Color ToColor(this ColorL colorL)
        {
            Color color = Color.FromArgb(colorL.A, colorL.R, colorL.G, colorL.B);

            return color;
        }
        #endregion

        #region # 颜色映射 —— static ColorL ToColorL(this Color color)
        /// <summary>
        /// 颜色映射
        /// </summary>
        public static ColorL ToColorL(this Color color)
        {
            ColorL colorL = new ColorL(color.R, color.G, color.B, color.A);

            return colorL;
        }
        #endregion

        #region # 点映射 —— static Point ToPoint(this PointL pointL)
        /// <summary>
        /// 点映射
        /// </summary>
        public static Point ToPoint(this PointL pointL)
        {
            Point point = new Point(pointL.X, pointL.Y);

            return point;
        }
        #endregion

        #region # 点映射 —— static PointVisual2D ToPointVisual2D(this PointL pointL)
        /// <summary>
        /// 点映射
        /// </summary>
        public static PointVisual2D ToPointVisual2D(this PointL pointL)
        {
            PointVisual2D point = new PointVisual2D
            {
                X = pointL.X,
                Y = pointL.Y,
                Thickness = pointL.Thickness,
                Fill = new SolidColorBrush(pointL.Fill.ToColor()),
                Stroke = new SolidColorBrush(pointL.Stroke.ToColor()),
                StrokeThickness = pointL.StrokeThickness
            };

            return point;
        }
        #endregion

        #region # 点映射 —— static PointL ToPointL(this Point point)
        /// <summary>
        /// 点映射
        /// </summary>
        public static PointL ToPointL(this Point point)
        {
            PointL pointL = new PointL
            {
                X = (int)Math.Ceiling(point.X),
                Y = (int)Math.Ceiling(point.Y)
            };

            return pointL;
        }
        #endregion

        #region # 点映射 —— static PointL ToPointL(this PointVisual2D point)
        /// <summary>
        /// 点映射
        /// </summary>
        public static PointL ToPointL(this PointVisual2D point)
        {
            SolidColorBrush fillBrush = (SolidColorBrush)point.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)point.Stroke;
            PointL pointL = new PointL
            {
                X = (int)Math.Ceiling(point.X),
                Y = (int)Math.Ceiling(point.Y),
                Thickness = (int)Math.Ceiling(point.Thickness),
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(point.StrokeThickness)
            };

            return pointL;
        }
        #endregion

        #region # 线映射 —— static LineVisual2D ToLine(this LineL lineL)
        /// <summary>
        /// 线映射
        /// </summary>
        public static LineVisual2D ToLine(this LineL lineL)
        {
            LineVisual2D line = new LineVisual2D
            {
                StartPoint = new Point(lineL.A.X, lineL.A.Y),
                EndPoint = new Point(lineL.B.X, lineL.B.Y),
                Fill = new SolidColorBrush(lineL.Fill.ToColor()),
                Stroke = new SolidColorBrush(lineL.Stroke.ToColor()),
                StrokeThickness = lineL.StrokeThickness
            };

            return line;
        }
        #endregion

        #region # 线映射 —— static LineL ToLineL(this LineVisual2D line)
        /// <summary>
        /// 线映射
        /// </summary>
        public static LineL ToLineL(this LineVisual2D line)
        {
            SolidColorBrush fillBrush = (SolidColorBrush)line.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)line.Stroke;
            LineL lineL = new LineL
            {
                A = new PointL((int)Math.Ceiling(line.StartPoint.X), (int)Math.Ceiling(line.StartPoint.Y)),
                B = new PointL((int)Math.Ceiling(line.EndPoint.X), (int)Math.Ceiling(line.EndPoint.Y)),
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(line.StrokeThickness)
            };

            return lineL;
        }
        #endregion

        #region # 矩形映射 —— static RectangleVisual2D ToRectangle(this RectangleL rectangleL)
        /// <summary>
        /// 矩形映射
        /// </summary>
        public static RectangleVisual2D ToRectangle(this RectangleL rectangleL)
        {
            RectangleVisual2D rectangle = new RectangleVisual2D
            {
                Location = new Point(rectangleL.X, rectangleL.Y),
                Size = new Size(rectangleL.Width, rectangleL.Height),
                Fill = new SolidColorBrush(rectangleL.Fill.ToColor()),
                Stroke = new SolidColorBrush(rectangleL.Stroke.ToColor()),
                StrokeThickness = rectangleL.StrokeThickness
            };

            return rectangle;
        }
        #endregion

        #region # 矩形映射 —— static RectangleL ToRectangleL(this RectangleVisual2D rectangle)
        /// <summary>
        /// 矩形映射
        /// </summary>
        public static RectangleL ToRectangleL(this RectangleVisual2D rectangle)
        {
            SolidColorBrush fillBrush = (SolidColorBrush)rectangle.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)rectangle.Stroke;
            RectangleL rectangleL = new RectangleL
            {
                X = (int)Math.Ceiling(rectangle.Location.X),
                Y = (int)Math.Ceiling(rectangle.Location.Y),
                Width = (int)Math.Ceiling(rectangle.Size.Width),
                Height = (int)Math.Ceiling(rectangle.Size.Height),
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(rectangle.StrokeThickness)
            };

            return rectangleL;
        }
        #endregion

        #region # 圆形映射 —— static CircleVisual2D ToCircle(this CircleL circleL)
        /// <summary>
        /// 圆形映射
        /// </summary>
        public static CircleVisual2D ToCircle(this CircleL circleL)
        {
            CircleVisual2D circle = new CircleVisual2D
            {
                Center = new Point(circleL.X, circleL.Y),
                Radius = circleL.Radius,
                Fill = new SolidColorBrush(circleL.Fill.ToColor()),
                Stroke = new SolidColorBrush(circleL.Stroke.ToColor()),
                StrokeThickness = circleL.StrokeThickness
            };

            return circle;
        }
        #endregion

        #region # 圆形映射 —— static CircleL ToCircleL(this CircleVisual2D circle)
        /// <summary>
        /// 圆形映射
        /// </summary>
        public static CircleL ToCircleL(this CircleVisual2D circle)
        {
            SolidColorBrush fillBrush = (SolidColorBrush)circle.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)circle.Stroke;
            CircleL circleL = new CircleL
            {
                X = (int)Math.Ceiling(circle.Center.X),
                Y = (int)Math.Ceiling(circle.Center.Y),
                Radius = (int)Math.Ceiling(circle.Radius),
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(circle.StrokeThickness)
            };

            return circleL;
        }
        #endregion

        #region # 椭圆形映射 —— static EllipseVisual2D ToEllipse(this EllipseL ellipseL)
        /// <summary>
        /// 椭圆形映射
        /// </summary>
        public static EllipseVisual2D ToEllipse(this EllipseL ellipseL)
        {
            EllipseVisual2D ellipse = new EllipseVisual2D
            {
                Center = new Point(ellipseL.X, ellipseL.Y),
                RadiusX = ellipseL.RadiusX,
                RadiusY = ellipseL.RadiusY,
                Fill = new SolidColorBrush(ellipseL.Fill.ToColor()),
                Stroke = new SolidColorBrush(ellipseL.Stroke.ToColor()),
                StrokeThickness = ellipseL.StrokeThickness
            };

            return ellipse;
        }
        #endregion

        #region # 椭圆形映射 —— static EllipseL ToEllipseL(this EllipseVisual2D ellipse)
        /// <summary>
        /// 椭圆形映射
        /// </summary>
        public static EllipseL ToEllipseL(this EllipseVisual2D ellipse)
        {
            SolidColorBrush fillBrush = (SolidColorBrush)ellipse.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)ellipse.Stroke;
            EllipseL ellipseL = new EllipseL
            {
                X = (int)Math.Ceiling(ellipse.Center.X),
                Y = (int)Math.Ceiling(ellipse.Center.Y),
                RadiusX = (int)Math.Ceiling(ellipse.RadiusX),
                RadiusY = (int)Math.Ceiling(ellipse.RadiusY),
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(ellipse.StrokeThickness)
            };

            return ellipseL;
        }
        #endregion

        #region # 多边形映射 —— static PolygonVisual2D ToPolygon(this PolygonL polygonL)
        /// <summary>
        /// 多边形映射
        /// </summary>
        public static PolygonVisual2D ToPolygon(this PolygonL polygonL)
        {
            IList<Point> points = new List<Point>();
            foreach (PointL pointL in polygonL.Points)
            {
                Point point = pointL.ToPoint();
                points.Add(point);
            }
            PolygonVisual2D polygon = new PolygonVisual2D
            {
                Points = points,
                Fill = new SolidColorBrush(polygonL.Fill.ToColor()),
                Stroke = new SolidColorBrush(polygonL.Stroke.ToColor()),
                StrokeThickness = polygonL.StrokeThickness
            };

            return polygon;
        }
        #endregion

        #region # 多边形映射 —— static PolygonL ToPolygonL(this PolygonVisual2D polygon)
        /// <summary>
        /// 多边形映射
        /// </summary>
        public static PolygonL ToPolygonL(this PolygonVisual2D polygon)
        {
            SolidColorBrush fillBrush = (SolidColorBrush)polygon.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)polygon.Stroke;
            IEnumerable<PointL> pointLs = polygon.Points.Select(point => point.ToPointL());
            PolygonL polygonL = new PolygonL(pointLs)
            {
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(polygon.StrokeThickness)
            };

            return polygonL;
        }
        #endregion

        #region # 折线段映射 —— static PolylineVisual2D ToPolyline(this PolylineL polylineL)
        /// <summary>
        /// 折线段映射
        /// </summary>
        public static PolylineVisual2D ToPolyline(this PolylineL polylineL)
        {
            IList<Point> points = new List<Point>();
            foreach (PointL pointL in polylineL.Points)
            {
                Point point = pointL.ToPoint();
                points.Add(point);
            }
            PolylineVisual2D polyline = new PolylineVisual2D
            {
                Points = points,
                Fill = new SolidColorBrush(polylineL.Fill.ToColor()),
                Stroke = new SolidColorBrush(polylineL.Stroke.ToColor()),
                StrokeThickness = polylineL.StrokeThickness
            };

            return polyline;
        }
        #endregion

        #region # 折线段映射 —— static PolylineL ToPolylineL(this PolylineVisual2D polyline)
        /// <summary>
        /// 折线段映射
        /// </summary>
        public static PolylineL ToPolylineL(this PolylineVisual2D polyline)
        {
            SolidColorBrush fillBrush = (SolidColorBrush)polyline.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)polyline.Stroke;
            IEnumerable<PointL> pointLs = polyline.Points.Select(point => point.ToPointL());
            PolylineL polylineL = new PolylineL(pointLs)
            {
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(polyline.StrokeThickness)
            };

            return polylineL;
        }
        #endregion

        #region # 闪烁形状边框 —— static Task BlinkStroke(this Shape shape, int duration)
        /// <summary>
        /// 闪烁形状边框
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="duration">持续时长(毫秒)</param>
        public static async Task BlinkStroke(this Shape shape, int duration = 2000)
        {
            Animation animation = new Animation
            {
                Duration = TimeSpan.FromMilliseconds(duration)
            };
            if (shape.Stroke is SolidColorBrush solidColorBrush)
            {
                animation.Children.Add(new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters = { new Setter(Shape.StrokeProperty, new SolidColorBrush(solidColorBrush.Color.Invert())) }
                });
                animation.Children.Add(new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters = { new Setter(Shape.StrokeProperty, solidColorBrush) }
                });
            }

            await animation.RunAsync(shape);
        }
        #endregion

        #region # 闪烁形状填充 —— static Task BlinkFill(this Shape shape, int duration)
        /// <summary>
        /// 闪烁形状填充
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="duration">持续时长(毫秒)</param>
        public static async Task BlinkFill(this Shape shape, int duration = 2000)
        {
            Animation animation = new Animation
            {
                Duration = TimeSpan.FromMilliseconds(duration)
            };
            if (shape.Fill is SolidColorBrush solidColorBrush)
            {
                animation.Children.Add(new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters = { new Setter(Shape.FillProperty, new SolidColorBrush(solidColorBrush.Color.Invert())) }
                });
                animation.Children.Add(new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters = { new Setter(Shape.FillProperty, solidColorBrush) }
                });
            }

            await animation.RunAsync(shape);
        }
        #endregion
    }
}
