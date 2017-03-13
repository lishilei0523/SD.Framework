using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common.PoweredByLee;
using SD.Common.Tests.StubDTOs;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.Common.Tests.TestCases
{
    /// <summary>
    /// JSON序列化测试
    /// </summary>
    [TestClass]
    public class JsonSerializationTests
    {
        /// <summary>
        /// 测试序列化
        /// </summary>
        [TestMethod]
        public void TestSerialize()
        {
            StudentInfo studentInfo = new StudentInfo
            {
                Id = 1,
                Name = "张三",
                BirthDay = new DateTime(2016, 9, 1, 9, 33, 10)
            };

            string json = studentInfo.ToJson("yyyy-MM-dd HH:mm:ss");
            string dateStr = "2016-09-01 09:33:10";

            Assert.IsTrue(json.Contains(dateStr));
        }

        /// <summary>
        /// 测试反序列化
        /// </summary>
        [TestMethod]
        public void TestDeserialize()
        {
            StudentInfo studentInfo = new StudentInfo
            {
                Id = 1,
                Name = "张三",
                BirthDay = new DateTime(2016, 9, 1, 9, 33, 10)
            };

            string json = studentInfo.ToJson("yyyy-MM-dd HH:mm:ss");

            StudentInfo student = json.JsonToObject<StudentInfo>();

            Assert.IsTrue(studentInfo.Id == student.Id);
            Assert.IsTrue(studentInfo.Name == student.Name);
            Assert.IsTrue(studentInfo.BirthDay == student.BirthDay);
        }
    }
}
