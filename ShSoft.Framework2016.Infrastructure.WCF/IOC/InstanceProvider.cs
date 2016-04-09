using System;
using System.ServiceModel;
using SD.IOC.Integration.WCF;
using ShSoft.Framework2016.Infrastructure.Global;
using ShSoft.Framework2016.Infrastructure.Global.Finalization;

namespace ShSoft.Framework2016.Infrastructure.WCF.IOC
{
    /// <summary>
    /// WCF实例提供者
    /// </summary>
    internal class InstanceProvider : WcfInstanceProvider
    {
        #region # 字段及构造器

        /// <summary>
        /// 服务契约类型
        /// </summary>
        private readonly Type _serviceType;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="serviceType">服务契约类型</param>
        public InstanceProvider(Type serviceType)
            : base(serviceType)
        {
            this._serviceType = serviceType;
        }

        #endregion

        #region # 获取服务契约实例 —— override object GetInstance(InstanceContext instanceContext)
        /// <summary>
        /// 获取服务契约实例
        /// </summary>
        /// <param name="instanceContext">WCF上下文对象</param>
        /// <returns>服务契约实例</returns>
        public override object GetInstance(InstanceContext instanceContext)
        {
            //初始化SessionId
            InitSessionId.Register();

            return base.GetInstance(instanceContext);
        }
        #endregion

        #region # 清理实例契约 —— override void ReleaseInstance(InstanceContext instanceContext...
        /// <summary>
        /// 清理实例契约
        /// </summary>
        /// <param name="instanceContext">WCF上下文对象</param>
        /// <param name="instance">服务契约实例</param>
        public override void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            //清理数据库
            FinalizeDatabase.Register();

            //清理WCF上下文
            base.ReleaseInstance(instanceContext, instance);
        }
        #endregion
    }
}