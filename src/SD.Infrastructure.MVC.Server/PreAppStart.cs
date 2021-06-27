using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using SD.Infrastructure.MVC.Server.Modules;
using System.ComponentModel;

namespace SD.Infrastructure.MVC.Server
{
    /// <summary>
    /// ASP.NET MVC应用程序初始化
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
            if (!_InitWasCalled)
            {
                _InitWasCalled = true;
                DynamicModuleUtility.RegisterModule(typeof(InitializationModule));
            }
        }
    }
}
