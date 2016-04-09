using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ShSoft.Framework2016.Infrastructure.WCF.IOC
{
    /// <summary>
    /// WCF依赖注入特性类
    /// </summary>
    public class IocServiceBehavior : Attribute, IServiceBehavior
    {
        #region # 适用依赖注入 —— void ApplyDispatchBehavior(ServiceDescription serviceDescription...
        /// <summary>
        /// 适用依赖注入
        /// </summary>
        /// <param name="serviceDescription">服务描述</param>
        /// <param name="serviceHostBase">服务主机</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher dispatcher = (ChannelDispatcher)channelDispatcherBase;
                foreach (EndpointDispatcher endpoint in dispatcher.Endpoints)
                {
                    if (!endpoint.IsSystemEndpoint)
                    {
                        endpoint.DispatchRuntime.InstanceProvider = new InstanceProvider(serviceDescription.ServiceType);
                    }
                }
            }
        }
        #endregion

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }
}
