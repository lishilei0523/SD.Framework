using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD.Infrastructure.AspNetCore.Toolkits;
using SD.Infrastructure.Constants;
using SD.Infrastructure.MemberShip;
using SD.IOC.Core.Mediators;
using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure.AspNetCore
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
        /// Http上下文
        /// </summary>
        private static readonly HttpContext _HttpContext;

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
            IHttpContextAccessor httpContextAccessor = ResolveMediator.ResolveOptional<IHttpContextAccessor>();
            _LoginPage = ConfigurationManager.AppSettings[LoginPageAppSettingKey] ?? "/Home/Index";
            _ErrorPage = ConfigurationManager.AppSettings[ErrorPageAppSettingKey] ?? "/Home/Index";

            #region # 验证

            if (httpContextAccessor == null)
            {
                throw new ApplicationException("\"IHttpContextAccessor\"的实现未注册！");
            }

            #endregion

            _HttpContext = httpContextAccessor.HttpContext;
        }

        #endregion

        #region # 属性

        #region Http上下文 —— static HttpContext HttpContext
        /// <summary>
        /// Http上下文
        /// </summary>
        public static HttpContext HttpContext
        {
            get { return _HttpContext; }
        }
        #endregion

        #region 验证码 —— static string ValidCode
        /// <summary>
        /// 验证码
        /// </summary>
        public static string ValidCode
        {
            get
            {
                return HttpContext.Session.GetString(SessionKey.ValidCode);
            }
            set
            {
                HttpContext.Session.SetString(SessionKey.ValidCode, value);
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
                string loginInfoJson = HttpContext.Session.GetString(SessionKey.CurrentUser);

                if (string.IsNullOrWhiteSpace(loginInfoJson))
                {
                    return null;
                }

                LoginInfo loginInfo = JsonConvert.DeserializeObject<LoginInfo>(loginInfoJson);
                return loginInfo;
            }
            set
            {
                HttpContext.Session.SetString(SessionKey.CurrentUser, value?.ToString());
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
            HttpContext.Session.Remove(SessionKey.ValidCode);
        }
        #endregion

        #endregion
    }
}
