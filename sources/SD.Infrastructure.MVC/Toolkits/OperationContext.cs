using SD.Infrastructure.Constants;
using SD.Infrastructure.MVC.Toolkits;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.MVC
{
    /// <summary>
    /// MVC操作上下文
    /// </summary>
    public static class OperationContext
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 开启身份认证AppSetting键
        /// </summary>
        public const string EnableAuthAppSettingKey = "EnableAuth";

        /// <summary>
        /// 登录页AppSetting键
        /// </summary>
        public const string LoginPageAppSettingKey = "LoginPage";

        /// <summary>
        /// 错误页AppSetting键
        /// </summary>
        public const string ErrorPageAppSettingKey = "ErrorPage";

        /// <summary>
        /// 登录页地址
        /// </summary>
        private static readonly string _LoginPage;

        /// <summary>
        /// 错误页地址
        /// </summary>
        private static readonly string _ErrorPage;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static OperationContext()
        {
            string loginPage = ConfigurationManager.AppSettings[LoginPageAppSettingKey];
            string errorPage = ConfigurationManager.AppSettings[ErrorPageAppSettingKey];

            #region # 验证

            if (string.IsNullOrWhiteSpace(loginPage))
            {
                throw new ApplicationException("默认登录页未配置，请联系管理员！");
            }
            if (string.IsNullOrWhiteSpace(errorPage))
            {
                throw new ApplicationException("默认错误页未配置，请联系管理员！");
            }

            #endregion

            _LoginPage = loginPage;
            _ErrorPage = errorPage;
        }

        #endregion

        #region # 属性

        #region 验证码 —— static string ValidCode
        /// <summary>
        /// 验证码
        /// </summary>
        public static string ValidCode
        {
            get
            {
                return
                    HttpContext.Current.Session[SessionKey.ValidCode] == null ?
                        null :
                        HttpContext.Current.Session[SessionKey.ValidCode].ToString();
            }
            set
            {
                HttpContext.Current.Session[SessionKey.ValidCode] = value;
            }
        }
        #endregion

        #region 当前登录用户 —— static LoginInfo LoginInfo
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static LoginInfo LoginInfo
        {
            get
            {
                return HttpContext.Current.Session[SessionKey.CurrentUser] as LoginInfo;
            }
            set
            {
                HttpContext.Current.Session[SessionKey.CurrentUser] = value;
            }
        }
        #endregion

        #region 登录页地址 —— static string LoginPage
        /// <summary>
        /// 登录页地址
        /// </summary>
        public static string LoginPage
        {
            get { return _LoginPage; }
        }
        #endregion

        #region 错误页地址 —— static string ErrorPage
        /// <summary>
        /// 错误页地址
        /// </summary>
        public static string ErrorPage
        {
            get { return _ErrorPage; }
        }
        #endregion

        #region 只读属性 - 是否已登录 —— static bool Logined
        /// <summary>
        /// 只读属性 - 是否已登录
        /// </summary>
        public static bool Logined
        {
            get
            {
                //校验Session
                return LoginInfo != null;
            }
        }
        #endregion

        #endregion

        #region # 方法

        #region 注销 —— static void Logout()
        /// <summary>
        /// 注销
        /// </summary>
        public static void Logout()
        {
            LoginInfo = null;
        }
        #endregion

        #region 获取验证码图片 —— static FileContentResult GetValidCode()
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns>验证码图片二进制内容</returns>
        public static FileContentResult GetValidCode()
        {
            string validCode = ValidCodeGenerator.GenerateCode(4);
            byte[] buffer = ValidCodeGenerator.GenerateStream(validCode);
            ValidCode = validCode;

            FileContentResult fileResult = new FileContentResult(buffer, @"image/jpeg");
            return fileResult;
        }
        #endregion

        #region 清空验证码 —— static void ClearValidCode()
        /// <summary>
        /// 清空验证码
        /// </summary>
        public static void ClearValidCode()
        {
            HttpContext.Current.Session.Remove(SessionKey.ValidCode);
        }
        #endregion

        #endregion
    }
}
