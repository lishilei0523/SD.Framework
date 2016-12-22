using ShSoft.Infrastructure.Constants;
using ShSoft.Infrastructure.MVC.Toolkits;
using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;

// ReSharper disable once CheckNamespace
namespace ShSoft.Infrastructure.MVC
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public abstract class BaseController : Controller
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 登录页AppSetting键
        /// </summary>
        private const string LoginPageAppSettingKey = "LoginPage";

        /// <summary>
        /// 登录页地址
        /// </summary>
        private static readonly string _LoginPage;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static BaseController()
        {
            string loginPage = ConfigurationManager.AppSettings[LoginPageAppSettingKey];

            #region # 验证

            if (string.IsNullOrWhiteSpace(loginPage))
            {
                throw new ApplicationException("默认登录页未配置，请联系管理员！");
            }

            #endregion

            _LoginPage = loginPage;
        }

        #endregion

        #region # 属性

        #region 验证码 —— string ValidCode
        /// <summary>
        /// 验证码
        /// </summary>
        protected string ValidCode
        {
            get
            {
                return
                    base.Session[CacheConstants.ValidCodeKey] == null ?
                    null :
                    base.Session[CacheConstants.ValidCodeKey].ToString();
            }
            set
            {
                base.Session[CacheConstants.ValidCodeKey] = value;
            }
        }
        #endregion

        #region 当前登录用户 —— LoginInfo LoginInfo
        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected LoginInfo LoginInfo
        {
            get
            {
                return base.Session[CacheConstants.CurrentUserKey] as LoginInfo;
            }
            set
            {
                base.Session[CacheConstants.CurrentUserKey] = value;
            }
        }
        #endregion

        #region 只读属性 - 是否已登录 —— bool Logined
        /// <summary>
        /// 只读属性 - 是否已登录
        /// </summary>
        protected bool Logined
        {
            get
            {
                //校验Session
                return this.LoginInfo != null;
            }
        }
        #endregion

        #endregion

        #region # 方法

        //Action

        #region 注销 —— void Logout()
        /// <summary>
        /// 注销
        /// </summary>
        public void Logout()
        {
            this.LoginInfo = null;
        }
        #endregion

        #region 获取验证码图片 —— FileContentResult GetValidCode()
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns>验证码图片二进制内容</returns>
        [AllowAnonymous]
        public FileContentResult GetValidCode()
        {
            string validCode = ValidCodeGenerator.GenerateCode(4);
            byte[] buffer = ValidCodeGenerator.GenerateStream(validCode);

            this.ValidCode = validCode;

            return base.File(buffer, @"image/jpeg");
        }
        #endregion


        //Other

        #region 授权过滤器 —— override void OnAuthorization(AuthorizationContext filterContext)
        /// <summary>
        /// 授权过滤器
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //判断用户是否登录，Action上是否贴有无需过滤标签，
            if (!this.Logined &&
                !filterContext.ActionDescriptor.HasAttr<AllowAnonymousAttribute>())
            {
                //是不是Ajax请求
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    throw new InvalidOperationException("未登录，请重新登录！");
                }

                //构造脚本
                StringBuilder scriptBuilder = new StringBuilder();
                scriptBuilder.Append("<script type=\"text/javascript\">");
                scriptBuilder.Append("window.top.location.href=");
                scriptBuilder.Append(string.Format("\"{0}\"", _LoginPage));
                scriptBuilder.Append("</script>");

                //跳转至登录页
                filterContext.HttpContext.Response.Write(scriptBuilder.ToString());
            }
        }
        #endregion

        #region 清空验证码 —— void ClearValidCode()
        /// <summary>
        /// 清空验证码
        /// </summary>
        protected void ClearValidCode()
        {
            this.Session.Remove(CacheConstants.ValidCodeKey);
        }
        #endregion

        #endregion
    }
}