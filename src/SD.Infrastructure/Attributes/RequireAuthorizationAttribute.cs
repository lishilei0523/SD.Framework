using System;

namespace SD.Infrastructure.Attributes
{
    /// <summary>
    /// 需授权特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequireAuthorizationAttribute : Attribute
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="authorityPath">权限路径</param>
        public RequireAuthorizationAttribute(string authorityPath)
        {
            this.AuthorityPath = authorityPath;
        }

        /// <summary>
        /// 权限路径
        /// </summary>
        public string AuthorityPath { get; set; }
    }
}
