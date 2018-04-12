using System;
using System.ServiceModel.Configuration;

namespace SD.Infrastructure.WCF.Server
{
    /// <summary>
    /// 初始化行为扩展元素
    /// </summary>
    internal class InitializationBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// 行为类型
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(InitializationBehavior); }
        }

        /// <summary>
        /// 创建行为
        /// </summary>
        /// <returns>行为实例</returns>
        protected override object CreateBehavior()
        {
            return new InitializationBehavior();
        }
    }
}
