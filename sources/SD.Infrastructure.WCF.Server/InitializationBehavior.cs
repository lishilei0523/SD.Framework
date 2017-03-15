using SD.Infrastructure.Global;
using SD.IOC.Integration.WCF;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace SD.Infrastructure.WCF.Server
{
    /// <summary>
    /// 初始化行为
    /// </summary>
    internal class InitializationBehavior : IServiceBehavior
    {
        /// <summary>
        /// 适用行为
        /// </summary>
        /// <param name="serviceDescription">服务描述</param>
        /// <param name="serviceHostBase">服务主机</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //初始化SessionId
            Initializer.InitSessionId();

            //初始化数据库
            Initializer.InitDataBase();

            //注册事件
            InstanceProvider.OnGetInstance += InstanceProvider_OnGetInstance;
            InstanceProvider.OnReleaseInstance += InstanceProvider_OnReleaseInstance;
        }

        /// <summary>
        /// 获取服务实例事件
        /// </summary>
        private static void InstanceProvider_OnGetInstance(InstanceContext instanceContext)
        {
            //初始化SessionId
            Initializer.InitSessionId();
        }

        /// <summary>
        /// 销毁服务实例事件
        /// </summary>
        private static void InstanceProvider_OnReleaseInstance(InstanceContext instanceContext, object instance)
        {
            //清理数据库
            Finalizer.CleanDb();
        }


        //没有用
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }
}
