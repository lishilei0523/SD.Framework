using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.Shapes;

namespace SD.Infrastructure.Tests.TestCases
{
    /// <summary>
    /// 形状测试
    /// </summary>
    [TestClass]
    public class ShapeTests
    {
        #region # 测试三角形 —— void TestTriangle()
        /// <summary>
        /// 测试三角形
        /// </summary>
        [TestMethod]
        public void TestTriangle()
        {
            PointL pointA = new PointL(0, 0);
            PointL pointB = new PointL(50, 0);
            PointL pointC = new PointL(0, 120);
            TriangleL triangle = new TriangleL(pointA, pointB, pointC);

            Assert.AreEqual(triangle.AngleA + triangle.AngleB + triangle.AngleC, 180);
            Assert.AreEqual(triangle.SideBC, 130);
        }
        #endregion
    }
}
