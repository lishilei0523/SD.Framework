using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SD.Infrastructure.Constants
{
    /// <summary>
    /// 应用程序类型
    /// </summary>
    [Serializable]
    [DataContract]
    public enum ApplicationType
    {
        /// <summary>
        /// Web应用程序
        /// </summary>
        [EnumMember]
        [Description("Web应用程序")]
        Web = 0,

        /// <summary>
        /// Windows应用程序
        /// </summary>
        [EnumMember]
        [Description("Windows应用程序")]
        Windows = 1,

        /// <summary>
        /// Android应用程序
        /// </summary>
        [EnumMember]
        [Description("Android应用程序")]
        Android = 2,

        /// <summary>
        /// IOS应用程序
        /// </summary>
        [EnumMember]
        [Description("IOS应用程序")]
        IOS = 3,

        /// <summary>
        /// Windows Phone应用程序
        /// </summary>
        [EnumMember]
        [Description("Windows Phone应用程序")]
        WindowsPhone = 4
    }
}
