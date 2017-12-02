using System;
using System.Collections;
using System.Web.Script.Serialization;

namespace SD.Infrastructure.WPF.Base
{
    public static class ExceptionExtension
    {

        #region # 字段及构造器

        /// <summary>
        /// JSON序列化器
        /// </summary>
        private static readonly JavaScriptSerializer _JsonSerializer;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ExceptionExtension()
        {
            _JsonSerializer = new JavaScriptSerializer();
        }

        #endregion

        public static Exception GetInnerException(this Exception exception)
        {
            if (exception != null)
            {
                if (exception.InnerException != null)
                {
                    return exception.InnerException.GetInnerException();
                }

                return exception;
            }

            return null;
        }

        public static string GetErrorMessage(this Exception exception)
        {
            Exception innerException = exception.GetInnerException();

            string errorMessage = string.Empty;

            if (innerException != null)
            {
                errorMessage = GetErrorMessage(innerException.Message, ref errorMessage);
                return errorMessage;
            }

            return errorMessage;
        }

        #region # 递归获取错误消息 —— static string GetErrorMessage(string exceptionMessage...
        /// <summary>
        /// 递归获取错误消息
        /// </summary>
        /// <param name="exceptionMessage">异常消息</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>错误消息</returns>
        private static string GetErrorMessage(string exceptionMessage, ref string errorMessage)
        {
            try
            {
                IDictionary json = _JsonSerializer.DeserializeObject(exceptionMessage) as IDictionary;

                if (json != null && json.Contains("ErrorMessage"))
                {
                    errorMessage = json["ErrorMessage"].ToString();
                }
                else
                {
                    errorMessage = exceptionMessage;
                }

                GetErrorMessage(errorMessage, ref errorMessage);

                return errorMessage;
            }
            catch
            {

                return exceptionMessage;
            }
        }
        #endregion
    }
}
