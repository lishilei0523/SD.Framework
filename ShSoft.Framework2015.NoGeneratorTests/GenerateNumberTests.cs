using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.NoGenerator.Facade;

namespace ShSoft.Framework2015.NoGeneratorTests
{
    /// <summary>
    /// 测试生成编号
    /// </summary>
    [TestClass]
    public class GenerateNumberTests
    {
        /// <summary>
        /// 生成编号
        /// </summary>
        [TestMethod]
        public void GenerateNumber()
        {
            //生成当前时间格式
            string currentTime = DateTime.Now.ToString("yyyyMMdd");

            //生成编号
            NumberGenerator generator = new NumberGenerator();
            string number = generator.GenerateNumber("NO", currentTime, typeof(GenerateNumberTests).Name, 3, null);

            //预期编号前缀
            string expectedNoPrefix = string.Format("NO{0}", currentTime);

            //断言
            Assert.IsTrue(number.Contains(expectedNoPrefix));
        }
    }
}
