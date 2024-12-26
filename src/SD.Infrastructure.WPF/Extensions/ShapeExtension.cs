using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Animations;
using SD.Infrastructure.WPF.Visual2Ds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SD.Infrastructure.WPF.Extensions
{
    /// <summary>
    /// 形状扩展
    /// </summary>
    public static class ShapeExtension
    {
        #region # 顺序化点集 —— static PointCollection Sequentialize(this PointCollection points)
        /// <summary>
        /// 顺序化点集
        /// </summary>
        /// <param name="points">点集</param>
        /// <returns>顺序化点集</returns>
        /// <remarks>用于排列多边形点集</remarks>
        public static PointCollection Sequentialize(this PointCollection points)
        {
            #region # 验证

            if (points == null)
            {
                throw new ArgumentNullException(nameof(points), "点集不可为null！");
            }
            if (!points.Any())
            {
                return new PointCollection();
            }

            #endregion

            double meanX = points.Average(point => point.X);
            double meanY = points.Average(point => point.Y);
            IOrderedEnumerable<Point> orderedPoints = points.OrderBy(point => Math.Atan2(point.Y - meanY, point.X - meanX));

            return new PointCollection(orderedPoints);
        }
        #endregion

        #region # 顺序化点集 —— static IList<Point> Sequentialize(this IEnumerable<Point> points)
        /// <summary>
        /// 顺序化点集
        /// </summary>
        /// <param name="points">点集</param>
        /// <returns>顺序化点集</returns>
        /// <remarks>用于排列多边形点集</remarks>
        public static IList<Point> Sequentialize(this IEnumerable<Point> points)
        {
            #region # 验证

            points = points?.ToArray() ?? new Point[0];
            if (!points.Any())
            {
                return new List<Point>();
            }

            #endregion

            double meanX = points.Average(point => point.X);
            double meanY = points.Average(point => point.Y);
            IOrderedEnumerable<Point> orderedPoints = points.OrderBy(point => Math.Atan2(point.Y - meanY, point.X - meanX));

            return orderedPoints.ToList();
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
            Point point = new Point
            {
                X = pointL.X,
                Y = pointL.Y
            };

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

        #region # 线映射 —— static Line ToLine(this LineL lineL)
        /// <summary>
        /// 线映射
        /// </summary>
        public static Line ToLine(this LineL lineL)
        {
            Line line = new Line
            {
                X1 = lineL.A.X,
                Y1 = lineL.A.Y,
                X2 = lineL.B.X,
                Y2 = lineL.B.Y,
                Fill = new SolidColorBrush(lineL.Fill.ToColor()),
                Stroke = new SolidColorBrush(lineL.Stroke.ToColor()),
                StrokeThickness = lineL.StrokeThickness
            };

            return line;
        }
        #endregion

        #region # 线映射 —— static LineL ToLineL(this Line line)
        /// <summary>
        /// 线映射
        /// </summary>
        public static LineL ToLineL(this Line line)
        {
            SolidColorBrush fillBrush = (SolidColorBrush)line.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)line.Stroke;
            LineL lineL = new LineL
            {
                A = new PointL((int)Math.Ceiling(line.X1), (int)Math.Ceiling(line.Y1)),
                B = new PointL((int)Math.Ceiling(line.X2), (int)Math.Ceiling(line.Y2)),
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(line.StrokeThickness)
            };

            return lineL;
        }
        #endregion

        #region # 矩形映射 —— static Rectangle ToRectangle(this RectangleL rectangleL)
        /// <summary>
        /// 矩形映射
        /// </summary>
        public static Rectangle ToRectangle(this RectangleL rectangleL)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = rectangleL.Width,
                Height = rectangleL.Height,
                Fill = new SolidColorBrush(rectangleL.Fill.ToColor()),
                Stroke = new SolidColorBrush(rectangleL.Stroke.ToColor()),
                StrokeThickness = rectangleL.StrokeThickness
            };
            Canvas.SetLeft(rectangle, rectangleL.X);
            Canvas.SetTop(rectangle, rectangleL.Y);

            return rectangle;
        }
        #endregion

        #region # 矩形映射 —— static RectangleVisual2D ToRectangleVisual2D(this RectangleL rectangleL)
        /// <summary>
        /// 矩形映射
        /// </summary>
        public static RectangleVisual2D ToRectangleVisual2D(this RectangleL rectangleL)
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

        #region # 矩形映射 —— static RectangleL ToRectangleL(this Rectangle rectangle)
        /// <summary>
        /// 矩形映射
        /// </summary>
        public static RectangleL ToRectangleL(this Rectangle rectangle)
        {
            int leftMargin = (int)Math.Ceiling(Canvas.GetLeft(rectangle));
            int topMargin = (int)Math.Ceiling(Canvas.GetTop(rectangle));
            SolidColorBrush fillBrush = (SolidColorBrush)rectangle.Fill;
            SolidColorBrush strokeBrush = (SolidColorBrush)rectangle.Stroke;
            RectangleL rectangleL = new RectangleL
            {
                X = leftMargin,
                Y = topMargin,
                Width = (int)Math.Ceiling(rectangle.Width),
                Height = (int)Math.Ceiling(rectangle.Height),
                Fill = fillBrush.Color.ToColorL(),
                Stroke = strokeBrush.Color.ToColorL(),
                StrokeThickness = (int)Math.Ceiling(rectangle.StrokeThickness)
            };

            return rectangleL;
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

        #region # 多边形映射 —— static Polygon ToPolygon(this PolygonL polygonL)
        /// <summary>
        /// 多边形映射
        /// </summary>
        public static Polygon ToPolygon(this PolygonL polygonL)
        {
            PointCollection points = new PointCollection();
            foreach (PointL pointL in polygonL.Points)
            {
                Point point = pointL.ToPoint();
                points.Add(point);
            }
            Polygon polygon = new Polygon
            {
                Points = points,
                Fill = new SolidColorBrush(polygonL.Fill.ToColor()),
                Stroke = new SolidColorBrush(polygonL.Stroke.ToColor()),
                StrokeThickness = polygonL.StrokeThickness
            };

            return polygon;
        }
        #endregion

        #region # 多边形映射 —— static PolygonL ToPolygonL(this Polygon polygon)
        /// <summary>
        /// 多边形映射
        /// </summary>
        public static PolygonL ToPolygonL(this Polygon polygon)
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

        #region # 折线段映射 —— static Polyline ToPolyline(this PolylineL polylineL)
        /// <summary>
        /// 折线段映射
        /// </summary>
        public static Polyline ToPolyline(this PolylineL polylineL)
        {
            PointCollection points = new PointCollection();
            foreach (PointL pointL in polylineL.Points)
            {
                Point point = pointL.ToPoint();
                points.Add(point);
            }
            Polyline polyline = new Polyline
            {
                Points = points,
                Fill = new SolidColorBrush(polylineL.Fill.ToColor()),
                Stroke = new SolidColorBrush(polylineL.Stroke.ToColor()),
                StrokeThickness = polylineL.StrokeThickness
            };

            return polyline;
        }
        #endregion

        #region # 折线段映射 —— static PolylineL ToPolylineL(this Polyline polyline)
        /// <summary>
        /// 折线段映射
        /// </summary>
        public static PolylineL ToPolylineL(this Polyline polyline)
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

        #region # 闪烁形状边框 —— static void BlinkStroke(this Shape shape, int duration)
        /// <summary>
        /// 闪烁形状边框
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="duration">持续时长(毫秒)</param>
        public static void BlinkStroke(this Shape shape, int duration = 2000)
        {
            BrushAnimation brushAnimation;
            Duration animationDuration = new Duration(TimeSpan.FromMilliseconds(duration));
            if (shape.Stroke is SolidColorBrush solidColorBrush)
            {
                brushAnimation = new BrushAnimation
                {
                    From = new SolidColorBrush(solidColorBrush.Color.Invert()),
                    To = shape.Stroke,
                    Duration = animationDuration
                };
            }
            else if (shape.Stroke is LinearGradientBrush linearGradientBrush)
            {
                GradientStop firstStop = linearGradientBrush.GradientStops.First();
                GradientStop lastStop = linearGradientBrush.GradientStops.Last();
                LinearGradientBrush invertBrush = new LinearGradientBrush(lastStop.Color, firstStop.Color,
                    linearGradientBrush.StartPoint, linearGradientBrush.EndPoint);
                brushAnimation = new BrushAnimation
                {
                    From = invertBrush,
                    To = shape.Stroke,
                    Duration = animationDuration
                };
            }
            else if (shape.Stroke is RadialGradientBrush radialGradientBrush)
            {
                GradientStop firstStop = radialGradientBrush.GradientStops.First();
                GradientStop lastStop = radialGradientBrush.GradientStops.Last();
                RadialGradientBrush invertBrush = new RadialGradientBrush(lastStop.Color, firstStop.Color);
                brushAnimation = new BrushAnimation
                {
                    From = invertBrush,
                    To = shape.Stroke,
                    Duration = animationDuration
                };
            }
            else
            {
                return;
            }

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(brushAnimation, shape);
            Storyboard.SetTargetProperty(brushAnimation, new PropertyPath(Shape.StrokeProperty));
            storyboard.Children.Add(brushAnimation);
            storyboard.Begin();
        }
        #endregion

        #region # 闪烁形状填充 —— static void BlinkFill(this Shape shape, int duration)
        /// <summary>
        /// 闪烁形状填充
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="duration">持续时长(毫秒)</param>
        public static void BlinkFill(this Shape shape, int duration = 2000)
        {
            BrushAnimation brushAnimation;
            Duration animationDuration = new Duration(TimeSpan.FromMilliseconds(duration));
            if (shape.Fill is SolidColorBrush solidColorBrush)
            {
                brushAnimation = new BrushAnimation
                {
                    From = new SolidColorBrush(solidColorBrush.Color.Invert()),
                    To = shape.Fill,
                    Duration = animationDuration
                };
            }
            else if (shape.Fill is LinearGradientBrush linearGradientBrush)
            {
                GradientStop firstStop = linearGradientBrush.GradientStops.First();
                GradientStop lastStop = linearGradientBrush.GradientStops.Last();
                LinearGradientBrush invertBrush = new LinearGradientBrush(lastStop.Color, firstStop.Color,
                    linearGradientBrush.StartPoint, linearGradientBrush.EndPoint);
                brushAnimation = new BrushAnimation
                {
                    From = invertBrush,
                    To = shape.Fill,
                    Duration = animationDuration
                };
            }
            else if (shape.Fill is RadialGradientBrush radialGradientBrush)
            {
                GradientStop firstStop = radialGradientBrush.GradientStops.First();
                GradientStop lastStop = radialGradientBrush.GradientStops.Last();
                RadialGradientBrush invertBrush = new RadialGradientBrush(lastStop.Color, firstStop.Color);
                brushAnimation = new BrushAnimation
                {
                    From = invertBrush,
                    To = shape.Fill,
                    Duration = animationDuration
                };
            }
            else
            {
                return;
            }

            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(brushAnimation, shape);
            Storyboard.SetTargetProperty(brushAnimation, new PropertyPath(Shape.FillProperty));
            storyboard.Children.Add(brushAnimation);
            storyboard.Begin();
        }
        #endregion
    }
}
