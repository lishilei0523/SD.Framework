using SD.Infrastructure.RepositoryBase;
using System;

namespace SD.Infrastructure.WCF.Tests.Stubs
{
    /// <summary>
    /// 数据初始化桩
    /// </summary>
    public class StubDataInitializer : IDataInitializer
    {
        /// <summary>
        /// 初始化基础数据
        /// </summary>
        public void Initialize()
        {
            Console.WriteLine("数据初始化已执行..");
        }
    }
}