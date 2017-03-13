using System.Web;
using System.Web.SessionState;

namespace SD.Common.PoweredByLee
{
    /// <summary>
    /// Http助手
    /// </summary>
    public static class HttpAssitant
    {
        #region # HttpContext对象 —— static HttpContext HttpContext
        /// <summary>
        /// HttpContext对象
        /// </summary>
        public static HttpContext HttpContext
        {
            get
            {
                return HttpContext.Current;
            }
        }
        #endregion

        #region # Session对象 —— static HttpSessionState Session
        /// <summary>
        /// Session对象
        /// </summary>
        public static HttpSessionState Session
        {
            get
            {
                return HttpContext.Session;
            }
        }
        #endregion

        #region # Application对象 —— static HttpApplicationState Application
        /// <summary>
        /// Application对象
        /// </summary>
        public static HttpApplicationState Application
        {
            get
            {
                return HttpContext.Application;
            }
        }
        #endregion

        #region # Request对象 —— static HttpRequest Request
        /// <summary>
        /// Request对象
        /// </summary>
        public static HttpRequest Request
        {
            get
            {
                return HttpContext.Request;
            }
        }
        #endregion

        #region # Response对象 —— static HttpResponse Response
        /// <summary>
        /// Response对象
        /// </summary>
        public static HttpResponse Response
        {
            get
            {
                return HttpContext.Response;
            }
        }
        #endregion
    }
}
