using System;

namespace SD.Infrastructure.Shapes
{
    /// <summary>
    /// 三角形
    /// </summary>
    public class TriangleL : ShapeL
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public TriangleL() { }
        #endregion

        #region 01.创建三角形构造器
        /// <summary>
        /// 创建三角形构造器
        /// </summary>
        /// <param name="vertexA">顶点A</param>
        /// <param name="vertexB">顶点B</param>
        /// <param name="vertexC">顶点C</param>
        public TriangleL(PointL vertexA, PointL vertexB, PointL vertexC)
            : this()
        {
            this.VertexA = vertexA;
            this.VertexB = vertexB;
            this.VertexC = vertexC;
        }
        #endregion

        #region 02.创建三角形构造器
        /// <summary>
        /// 创建三角形构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="vertexA">顶点A</param>
        /// <param name="vertexB">顶点B</param>
        /// <param name="vertexC">顶点C</param>
        public TriangleL(string name, PointL vertexA, PointL vertexB, PointL vertexC)
            : this(vertexA, vertexB, vertexC)
        {
            base.Name = name;
        }
        #endregion

        #endregion

        #region # 属性

        #region 顶点A —— PointL VertexA
        /// <summary>
        /// 顶点A
        /// </summary>
        public PointL VertexA { get; set; }
        #endregion

        #region 顶点B —— PointL VertexB
        /// <summary>
        /// 顶点B
        /// </summary>
        public PointL VertexB { get; set; }
        #endregion

        #region 顶点C —— PointL VertexC
        /// <summary>
        /// 顶点C
        /// </summary>
        public PointL VertexC { get; set; }
        #endregion

        #region 只读属性 - 边AB长度 —— double SideAB
        /// <summary>
        /// 只读属性 - 边AB长度
        /// </summary>
        /// <remarks>边c</remarks>
        public double SideAB
        {
            get
            {
                double x0 = Math.Pow(this.VertexA.X - this.VertexB.X, 2);
                double y0 = Math.Pow(this.VertexA.Y - this.VertexB.Y, 2);
                double distance = Math.Sqrt(x0 + y0);

                return distance;
            }
        }
        #endregion

        #region 只读属性 - 边AC长度 —— double SideAC
        /// <summary>
        /// 只读属性 - 边AC长度
        /// </summary>
        /// <remarks>边b</remarks>
        public double SideAC
        {
            get
            {
                double x0 = Math.Pow(this.VertexA.X - this.VertexC.X, 2);
                double y0 = Math.Pow(this.VertexA.Y - this.VertexC.Y, 2);
                double distance = Math.Sqrt(x0 + y0);

                return distance;
            }
        }
        #endregion

        #region 只读属性 - 边BC长度 —— double SideBC
        /// <summary>
        /// 只读属性 - 边BC长度
        /// </summary>
        /// <remarks>边a</remarks>
        public double SideBC
        {
            get
            {
                double x0 = Math.Pow(this.VertexB.X - this.VertexC.X, 2);
                double y0 = Math.Pow(this.VertexB.Y - this.VertexC.Y, 2);
                double distance = Math.Sqrt(x0 + y0);

                return distance;
            }
        }
        #endregion

        #region 只读属性 - 角A度数 —— double AngleA
        /// <summary>
        /// 只读属性 - 角A度数
        /// </summary>
        /// <remarks>角α</remarks>
        public double AngleA
        {
            get
            {
                double numerator = Math.Pow(this.SideAC, 2) + Math.Pow(this.SideAB, 2) - Math.Pow(this.SideBC, 2);
                double denominator = 2 * this.SideAC * this.SideAB;
                double radians = Math.Acos(numerator / denominator);
                double degrees = radians * 180 / Math.PI;

                return degrees;
            }
        }
        #endregion

        #region 只读属性 - 角B度数 —— double AngleB
        /// <summary>
        /// 只读属性 - 角B度数
        /// </summary>
        /// <returns>角β</returns>
        public double AngleB
        {
            get
            {
                double numerator = Math.Pow(this.SideAB, 2) + Math.Pow(this.SideBC, 2) - Math.Pow(this.SideAC, 2);
                double denominator = 2 * this.SideBC * this.SideAB;
                double radians = Math.Acos(numerator / denominator);
                double degrees = radians * 180 / Math.PI;

                return degrees;
            }
        }
        #endregion

        #region 只读属性 - 角C度数 —— double AngleC
        /// <summary>
        /// 只读属性 - 角C度数
        /// </summary>
        /// <remarks>角γ</remarks>
        public double AngleC
        {
            get
            {
                double numerator = Math.Pow(this.SideBC, 2) + Math.Pow(this.SideAC, 2) - Math.Pow(this.SideAB, 2);
                double denominator = 2 * this.SideBC * this.SideAC;
                double radians = Math.Acos(numerator / denominator);
                double degrees = radians * 180 / Math.PI;

                return degrees;
            }
        }
        #endregion

        #endregion

        #region # 方法

        #region 是否相等 —— override bool Equals(object instance)
        /// <summary>
        /// 是否相等
        /// </summary>
        public override bool Equals(object instance)
        {
            if (instance is TriangleL triangle)
            {
                return triangle == this;
            }

            return false;
        }
        #endregion

        #region 获取哈希码 —— override int GetHashCode()
        /// <summary>
        /// 获取哈希码
        /// </summary>
        public override int GetHashCode()
        {
            return this.VertexA.GetHashCode() ^ this.VertexB.GetHashCode() ^ this.VertexC.GetHashCode();
        }
        #endregion

        #region 转换字符串 —— override string ToString()
        /// <summary>
        /// 转换字符串
        /// </summary>
        public override string ToString()
        {
            string vertexPoints = $"A({this.VertexA.X},{this.VertexA.Y})|B({this.VertexB.X},{this.VertexB.Y})|C({this.VertexC.X},{this.VertexC.Y})";

            return vertexPoints;
        }
        #endregion

        #region 比较两个三角形是否相等 —— static bool operator ==(TriangleL source, TriangleL target)
        /// <summary>
        /// 比较两个三角形是否相等
        /// </summary>
        /// <param name="source">源三角形</param>
        /// <param name="target">目标三角形</param>
        /// <returns>是否相等</returns>
        public static bool operator ==(TriangleL source, TriangleL target)
        {
            if (source is null && target is null)
            {
                return true;
            }
            if (source is null)
            {
                return false;
            }
            if (target is null)
            {
                return false;
            }

            return source.VertexA == target.VertexA &&
                   source.VertexB == target.VertexB &&
                   source.VertexC == target.VertexC;
        }
        #endregion

        #region 比较两个三角形是否不等 —— static bool operator !=(TriangleL source, TriangleL target)
        /// <summary>
        /// 比较两个三角形是否不等
        /// </summary>
        /// <param name="source">源三角形</param>
        /// <param name="target">目标三角形</param>
        /// <returns>是否不等</returns>
        public static bool operator !=(TriangleL source, TriangleL target)
        {
            return !(source == target);
        }
        #endregion

        #endregion
    }
}
