using System;
using System.ServiceModel.Configuration;

namespace ShSoft.Framework2016.Infrastructure.WCF.IOC
{
    /// <summary>
    /// 依赖注入行为扩展元素
    /// </summary>
    internal class IocServiceBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// 行为类型
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(IocServiceBehavior); }
        }

        /// <summary>
        /// 创建行为
        /// </summary>
        /// <returns>行为实例</returns>
        protected override object CreateBehavior()
        {
            return new IocServiceBehavior();
        }
    }
}
