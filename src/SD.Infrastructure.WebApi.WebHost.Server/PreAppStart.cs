using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.ComponentModel;

namespace SD.Infrastructure.WebApi.WebHost.Server
{
    /// <summary>
    /// WebApi应用程序初始化
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PreAppStart
    {
        /// <summary>
        /// 是否已初始化
        /// </summary>
        private static bool _InitWasCalled;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            if (!PreAppStart._InitWasCalled)
            {
                PreAppStart._InitWasCalled = true;
                DynamicModuleUtility.RegisterModule(typeof(InitializationHttpModule));
            }
        }
    }
}
