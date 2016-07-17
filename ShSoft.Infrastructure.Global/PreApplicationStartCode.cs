using System.ComponentModel;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using ShSoft.Framework2016.Infrastructure.Global.HttpModules;

namespace ShSoft.Framework2016.Infrastructure.Global
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PreApplicationStart
    {
        /// <summary>
        /// 是否已初始化
        /// </summary>
        private static bool _InitWasCalled;

        /// <summary>
        /// 初始化依赖注入
        /// </summary>
        public static void Initialize()
        {
            if (!_InitWasCalled)
            {
                _InitWasCalled = true;
                DynamicModuleUtility.RegisterModule(typeof(InitializationHttpModule));
            }
        }
    }
}
