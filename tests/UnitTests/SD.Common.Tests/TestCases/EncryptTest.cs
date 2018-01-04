using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common.PoweredByLee;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace SD.Common.Tests.TestCases
{
    /// <summary>
    /// 字符串解密解密测试
    /// </summary>
    [TestClassAttribute]
    public class EncryptTest
    {
        /// <summary>
        /// 无Key测试
        /// </summary>
        [TestMethodAttribute]
        public void TestNoKey()
        {
            const string text = "Hello World";

            string password = text.Encrypt();
            string source = password.Decrypt();

            Assert.AreEqual(text, source);
        }

        /// <summary>
        /// 含Key测试
        /// </summary>
        [TestMethodAttribute]
        public void TestContainsKey()
        {
            const string text = "Hello World";
            const string key = "123456";

            string password = text.Encrypt(key);
            string source = password.Decrypt(key);

            Assert.AreEqual(text, source);
        }
    }
}
