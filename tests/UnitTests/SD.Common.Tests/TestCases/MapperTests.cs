using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common.PoweredByLee;
using SD.Common.Tests.StubDTOs;
using SD.Common.Tests.StubEntities;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.Common.Tests.TestCases
{
    /// <summary>
    /// 映射工具测试类
    /// </summary>
    [TestClass]
    public class MapperTests
    {
        /// <summary>
        /// 测试正常映射
        /// </summary>
        [TestMethod]
        public void TestMap_Normal()
        {
            Student student = new Student { Id = 1, Name = "张三", BirthDay = DateTime.Now };
            StudentInfo studentInfo = Transform<Student, StudentInfo>.Map(student);

            Assert.IsTrue(studentInfo.Name == student.Name);
        }

        /// <summary>
        /// 测试映射后事件执行次数
        /// </summary>
        [TestMethod]
        public void TestMap_AfterMap()
        {
            Student student = new Student { Id = 1, Name = "张三", BirthDay = DateTime.Now };

            StudentInfo studentInfo = null;
            for (int i = 0; i < 10; i++)
            {
                studentInfo = Transform<Student, StudentInfo>.Map(student, null, this.AfterMap);
            }

            Assert.IsTrue(studentInfo.Name == student.Name);
        }

        /// <summary>
        /// 映射后事件方法
        /// </summary>
        /// <param name="source">源实例</param>
        /// <param name="target">目标实例</param>
        private void AfterMap(Student source, StudentInfo target)
        {
            Trace.WriteLine(DateTime.Now);
        }
    }
}
