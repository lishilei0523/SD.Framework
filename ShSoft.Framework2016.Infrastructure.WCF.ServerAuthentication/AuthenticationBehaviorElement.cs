using System;
using System.ServiceModel.Configuration;

namespace ShSoft.Framework2016.Infrastructure.WCF.ServerAuthentication
{
    /// <summary>
    /// 服务端身份认证行为扩展元素
    /// </summary>
    internal class AuthenticationBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// 行为类型
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(AuthenticationBehavior); }
        }

        /// <summary>
        /// 创建行为
        /// </summary>
        /// <returns>行为实例</returns>
        protected override object CreateBehavior()
        {
            return new AuthenticationBehavior();
        }
    }
}
