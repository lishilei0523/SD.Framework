using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.Common.PoweredByLee;
using ShSoft.Framework2015.CommonTests.StubDTOs;
using ShSoft.Framework2015.CommonTests.StubEntities;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ShSoft.Framework2015.CommonTests.TestCases
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
            int count = 0;


            Student student1 = new Student { Id = 1, Name = "张三", BirthDay = DateTime.Now };

            StudentInfo studentInfo1 = null;
            for (int i = 0; i < 10; i++)
            {
                studentInfo1 = Transform<Student, StudentInfo>.Map(student1, null, Test);
            }



            Assert.IsTrue(studentInfo1.Name == student1.Name);
        }


        private void Test(Student x, StudentInfo y)
        {
            Trace.WriteLine(DateTime.Now);
        }
    }
}
