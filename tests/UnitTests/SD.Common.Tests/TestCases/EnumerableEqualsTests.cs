using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common.PoweredByLee;
using SD.Common.Tests.StubEntities;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.Common.Tests.TestCases
{
    /// <summary>
    /// 比较序列相等测试
    /// </summary>
    [TestClass]
    public class EnumerableEqualsTests
    {
        #region # 字段与初始化器

        /// <summary>
        /// 源学生集
        /// </summary>
        private IList<Student> _sourceList;

        /// <summary>
        /// 目标学生集
        /// </summary>
        private IList<Student> _targetList;

        /// <summary>
        /// 源字典
        /// </summary>
        private IDictionary<string, Student> _sourceDict;

        /// <summary>
        /// 目标字典
        /// </summary>
        private IDictionary<string, Student> _targetDict;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            this._sourceList = new List<Student>
            {
                new Student{Id = 3,Name = "学生3",BirthDay = new DateTime(1996,2,20)},
                new Student{Id = 1,Name = "学生1",BirthDay = new DateTime(1992,4,18)},
                new Student{Id = 2,Name = "学生2",BirthDay = new DateTime(1991,4,20)}
         
            };
            this._targetList = new List<Student>
            {
                new Student{Id = 1,Name = "学生1",BirthDay = new DateTime(1992,4,18)},
                new Student{Id = 2,Name = "学生2",BirthDay = new DateTime(1991,4,20)},
                new Student{Id = 3,Name = "学生3",BirthDay = new DateTime(1996,2,20)}
            };

            this._sourceDict = new Dictionary<string, Student>
            {
                {"001", this._sourceList[0]},
                {"002", this._sourceList[1]},
                {"003", this._sourceList[2]}
            };
            this._targetDict = new Dictionary<string, Student>
            {
                {"001", this._sourceList[0]},
                {"002", this._sourceList[1]},
                {"003", this._sourceList[2]}
            };
        }

        #endregion

        /// <summary>
        /// 测试集合是否值相等
        /// </summary>
        [TestMethod]
        public void TestCollectionEquals()
        {
            Assert.IsTrue(this._sourceList.OrderBy(x => x.Id).EqualsTo(this._targetList.OrderBy(x => x.Id)));
        }

        /// <summary>
        /// 测试字典是否值相等
        /// </summary>
        [TestMethod]
        public void TestDictionaryEquals()
        {
            Assert.IsTrue(this._sourceDict.EqualsTo(this._targetDict));
        }

        /// <summary>
        /// 测试字典是否值相等
        /// </summary>
        [TestMethod]
        public void TestDictionaryEquals2()
        {
            IDictionary<Guid, float> source = new Dictionary<Guid, float>();
            source.Add(new Guid("494bcada-3377-476e-912f-1eecef3eb9fc"), 10.0f);

            IDictionary<Guid, float> target = new Dictionary<Guid, float>();
            target.Add(new Guid("494bcada-3377-476e-912f-1eecef3eb9fc"), 10.0f);

            Assert.IsTrue(source.EqualsTo(target));
        }
    }
}
