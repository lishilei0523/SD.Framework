using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Infrastructure.EntityBase;
using SD.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SD.Infrastructure.Tests.TestCases
{
    /// <summary>
    /// 分页测试
    /// </summary>
    [TestClass]
    public class PageTests
    {
        #region # 测试分页返回集合 —— void TestToPageReturnList()
        /// <summary>
        /// 测试分页返回集合
        /// </summary>
        [TestMethod]
        public void TestToPageReturnList()
        {
            string[] numbers = { "3", "1", "X", "A", "f", ",", "4", "t", "3g" };

            int pageIndex = 1;
            int pageSize = 5;
            IList<string> pagedResult = numbers.ToPage(pageIndex, pageSize, out int rowCount, out int pageCount);
            foreach (string number in pagedResult)
            {
                Trace.WriteLine(number);
            }

            Assert.AreEqual(rowCount, numbers.Length);
            Assert.AreEqual(pageCount, (int)Math.Ceiling(rowCount * 1.0 / pageSize));
        }
        #endregion

        #region # 测试分页返回分页集合 —— void TestToPageReturnPage()
        /// <summary>
        /// 测试分页返回分页集合
        /// </summary>
        [TestMethod]
        public void TestToPageReturnPage()
        {
            string[] numbers = { "3", "1", "X", "A", "f", ",", "4", "t", "3g" };

            int pageIndex = 1;
            int pageSize = 5;
            Page<string> page = numbers.ToPage(pageIndex, pageSize);
            foreach (string number in page)
            {
                Trace.WriteLine(number);
            }

            Assert.AreEqual(page.RowCount, numbers.Length);
            Assert.AreEqual(page.PageCount, (int)Math.Ceiling(page.RowCount * 1.0 / pageSize));
        }
        #endregion
    }
}
