using System;
using System.Web;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// 路径扩展
    /// </summary>
    public static class PathExtension
    {
        /// <summary>
        /// 将相对路径转换为绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="appType">应用程序类型</param>
        /// <returns>绝对路径</returns>
        public static string MapPath(string relativePath, ApplicationType appType = ApplicationType.WebApp)
        {
            switch (appType)
            {
                case ApplicationType.WebApp:
                    return HttpContext.Current.Server.MapPath(relativePath);
                case ApplicationType.WindowsApp:
                    return string.Format("{0}{1}", Environment.CurrentDirectory, relativePath);
                default:
                    return HttpContext.Current.Server.MapPath(relativePath);
            }
        }
    }

    /// <summary>
    /// 应用程序类型
    /// </summary>
    public enum ApplicationType
    {
        /// <summary>
        /// Web应用程序
        /// </summary>
        WebApp = 0,

        /// <summary>
        /// Windows应用程序
        /// </summary>
        WindowsApp = 1
    }
}
