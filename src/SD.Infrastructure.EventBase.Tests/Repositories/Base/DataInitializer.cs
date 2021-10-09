using SD.Infrastructure.RepositoryBase;
using System;

namespace SD.Infrastructure.EventBase.Tests.Repositories.Base
{
    /// <summary>
    /// 数据初始化器实现
    /// </summary>
    public class DataInitializer : IDataInitializer
    {
        /// <summary>
        /// 初始化基础数据
        /// </summary>
        public void Initialize()
        {
            Console.WriteLine("初始化数据");
        }
    }
}
