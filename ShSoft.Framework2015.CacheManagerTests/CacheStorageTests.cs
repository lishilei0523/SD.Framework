using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShSoft.Framework2015.CacheManager.Mediator;

namespace ShSoft.Framework2015.CacheManagerTests
{
    /// <summary>
    /// 缓存测试
    /// </summary>
    [TestClass]
    public class CacheStorageTests
    {
        /// <summary>
        /// 初始化测试
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            //清空缓存
            CacheMediator.RemoveAll();
        }

        /// <summary>
        /// 测试插入与读取缓存
        /// </summary>
        [TestMethod]
        public void TestSetAndGetCache()
        {
            string cacheKey = "key";
            string cacheValue = "value";

            CacheMediator.Set(cacheKey, cacheValue);

            string value = CacheMediator.Get<string>(cacheKey);
            Assert.IsTrue(value == cacheValue);
        }

        /// <summary>
        /// 测试移除缓存
        /// </summary>
        [TestMethod]
        public void TestRemoveCache()
        {
            string cacheKey = "key";
            string cacheValue = "value";

            CacheMediator.Set(cacheKey, cacheValue);

            //移除
            CacheMediator.Remove(cacheKey);

            string value = CacheMediator.Get<string>(cacheKey);
            Assert.IsNull(value);
        }

        /// <summary>
        /// 测试清空缓存
        /// </summary>
        [TestMethod]
        public void TestRemoveAllCache()
        {
            string cacheKey1 = "key1";
            string cacheKey2 = "key2";
            string cacheValue1 = "value1";
            string cacheValue2 = "value2";

            CacheMediator.Set(cacheKey1, cacheValue1);
            CacheMediator.Set(cacheKey2, cacheValue2);

            //移除全部
            CacheMediator.RemoveAll();

            string value1 = CacheMediator.Get<string>(cacheKey1);
            string value2 = CacheMediator.Get<string>(cacheKey2);

            Assert.IsTrue(value1 == null && value2 == null);
        }
    }
}
