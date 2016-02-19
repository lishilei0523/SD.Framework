using System;

namespace ShSoft.Framework2015.AOP.Models.Entities
{
    /// <summary>
    /// 日志基类
    /// </summary>
    [Serializable]
    public abstract class BaseLog
    {
        #region 01.标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        public Guid Id { get; set; }
        #endregion

        #region 02.命名空间 —— string Namespace
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }
        #endregion

        #region 03.类名 —— string ClassName
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
        #endregion

        #region 04.方法名 —— string MethodName
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
        #endregion

        #region 05.方法类型 —— string MethodType
        /// <summary>
        /// 方法类型
        /// </summary>
        public string MethodType { get; set; }
        #endregion

        #region 06.方法参数Json —— string ArgsJson
        /// <summary>
        /// 方法参数Json
        /// </summary>
        public string ArgsJson { get; set; }
        #endregion

        #region 07.IP地址 —— string IPAddress
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }
        #endregion
    }
}
