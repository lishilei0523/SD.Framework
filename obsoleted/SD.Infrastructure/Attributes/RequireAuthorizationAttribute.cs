using System;

namespace SD.Infrastructure.Attributes
{
    /// <summary>
    /// 需要权限验证特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequireAuthorizationAttribute : Attribute
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="authorityName">权限名称</param>
        /// <param name="englishName">英文名称</param>
        /// <param name="description">描述</param>
        public RequireAuthorizationAttribute(string authorityName, string englishName = null, string description = null)
        {
            this.AuthorityName = authorityName;
            this.EnglishName = englishName;
            this.Description = description;
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string AuthorityName { get; private set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }
    }
}
