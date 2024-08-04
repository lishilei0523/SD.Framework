using SD.Infrastructure.Shapes;
using SD.Infrastructure.WPF.Visual2Ds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                Fill = new SolidColorBrush(Color.FromArgb(pointL.Fill.A, pointL.Fill.R, pointL.Fill.G, pointL.Fill.B)),
                Stroke = new SolidColorBrush(Color.FromArgb(pointL.Stroke.A, pointL.Stroke.R, pointL.Stroke.G, pointL.Stroke.B)),
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
                Fill = new ColorL(fillBrush.Color.R, fillBrush.Color.G, fillBrush.Color.B, fillBrush.Color.A),
                Stroke = new ColorL(strokeBrush.Color.R, strokeBrush.Color.G, strokeBrush.Color.B, strokeBrush.Color.A),
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
                Fill = new SolidColorBrush(Color.FromArgb(lineL.Fill.A, lineL.Fill.R, lineL.Fill.G, lineL.Fill.B)),
                Stroke = new SolidColorBrush(Color.FromArgb(lineL.Stroke.A, lineL.Stroke.R, lineL.Stroke.G, lineL.Stroke.B)),
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
                Fill = new ColorL(fillBrush.Color.R, fillBrush.Color.G, fillBrush.Color.B, fillBrush.Color.A),
                Stroke = new ColorL(strokeBrush.Color.R, strokeBrush.Color.G, strokeBrush.Color.B, strokeBrush.Color.A),
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
                Fill = new SolidColorBrush(Color.FromArgb(rectangleL.Fill.A, rectangleL.Fill.R, rectangleL.Fill.G, rectangleL.Fill.B)),
                Stroke = new SolidColorBrush(Color.FromArgb(rectangleL.Stroke.A, rectangleL.Stroke.R, rectangleL.Stroke.G, rectangleL.Stroke.B)),
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
                Fill = new SolidColorBrush(Color.FromArgb(rectangleL.Fill.A, rectangleL.Fill.R, rectangleL.Fill.G, rectangleL.Fill.B)),
                Stroke = new SolidColorBrush(Color.FromArgb(rectangleL.Stroke.A, rectangleL.Stroke.R, rectangleL.Stroke.G, rectangleL.Stroke.B)),
                StrokeThickness = rectangleL.StrokeThickness
            };

            return rectangle;
        }
        #endregion

        //TODO 颜色、圆、椭圆、多边形、折线段映射
    }
}
