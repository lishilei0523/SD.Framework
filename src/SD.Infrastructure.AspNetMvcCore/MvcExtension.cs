using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD.Infrastructure.Constants;
using SD.Toolkits.Image;
using SD.Toolkits.OwinCore.Extensions;

namespace SD.Infrastructure.AspNetMvcCore
{
    /// <summary>
    /// ASP.NET Core MVC扩展
    /// </summary>
    public static class MvcExtension
    {
        #region # 注销登录 —— static void Logout()
        /// <summary>
        /// 注销登录
        /// </summary>
        public static void Logout()
        {
            OwinContextReader.Current.Session.Remove(GlobalSetting.ApplicationId);
        }
        #endregion

        #region # 设置验证码 —— static void SetValidCode(string validCode)
        /// <summary>
        /// 设置验证码
        /// </summary>
        /// <param name="validCode">验证码</param>
        public static void SetValidCode(string validCode)
        {
            OwinContextReader.Current.Session.SetString(SessionKey.ValidCode, validCode);
        }
        #endregion

        #region # 获取验证码 —— static string GetValidCode()
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns>验证码</returns>
        public static string GetValidCode()
        {
            string validCode = OwinContextReader.Current.Session.GetString(SessionKey.ValidCode);

            return validCode;
        }
        #endregion

        #region # 清空验证码 —— static void ClearValidCode()
        /// <summary>
        /// 清空验证码
        /// </summary>
        public static void ClearValidCode()
        {
            OwinContextReader.Current.Session.Remove(SessionKey.ValidCode);
        }
        #endregion

        #region # 获取验证码图片 —— static FileContentResult GetValidCodeImage()
        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns>验证码图片二进制内容</returns>
        public static FileContentResult GetValidCodeImage()
        {
            string validCode = ValidCodeGenerator.GenerateCode(4);
            SetValidCode(validCode);

            byte[] buffer = ValidCodeGenerator.GenerateStream(validCode);
            FileContentResult validCodeImage = new FileContentResult(buffer, @"image/jpeg");

            return validCodeImage;
        }
        #endregion
    }
}
