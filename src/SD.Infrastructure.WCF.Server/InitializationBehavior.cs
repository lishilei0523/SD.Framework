using SD.Infrastructure.Global;
using SD.IOC.Integration.WCF.Providers;
using System.Collections.ObjectModel;
#if NET40_OR_GREATER
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
#endif
#if NETSTANDARD2_0_OR_GREATER
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Description;
#endif

namespace SD.Infrastructure.WCF.Server
{
    /// <summary>
    /// 初始化行为
    /// </summary>
    public class InitializationBehavior : IServiceBehavior
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        /// <summary>
        /// 是否已初始化过
        /// </summary>
        private static bool _Initialized;

        /// <summary>
        /// 初始化
        /// </summary>
        private static void Initialize()
        {
            lock (_Sync)
            {
                _Initialized = true;
            }
        }

        /// <summary>
        /// 适用行为
        /// </summary>
        /// <param name="serviceDescription">服务描述</param>
        /// <param name="serviceHostBase">服务主机</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (!_Initialized)
            {
                //初始化SessionId
                Initializer.InitSessionId();

                //初始化数据库
                Initializer.InitDataBase();

                //注册事件
                ServiceInstanceProvider.OnGetInstance += InstanceProvider_OnGetInstance;

                //初始化完毕
                Initialize();
            }
        }

        /// <summary>
        /// 获取服务实例事件
        /// </summary>
        private static void InstanceProvider_OnGetInstance(InstanceContext instanceContext)
        {
            //初始化SessionId
            Initializer.InitSessionId();
        }


        //没有用
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) { }
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }
    }
}
