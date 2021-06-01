using SD.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SD.Infrastructure.MemberShip
{
    /// <summary>
    /// 登录菜单信息
    /// </summary>
    [DataContract]
    [Serializable]
    public class LoginMenuInfo
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        public LoginMenuInfo()
        {
            this.SubMenuInfos = new HashSet<LoginMenuInfo>();
        }


        /// <summary>
        /// 信息系统编号
        /// </summary>
        [DataMember]
        public string SystemNo { get; set; }

        /// <summary>
        /// 应用程序类型
        /// </summary>
        [DataMember]
        public ApplicationType ApplicationType { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// 上级菜单Id
        /// </summary>
        [DataMember]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        [DataMember]
        public int Sort { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        [DataMember]
        public string Icon { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// 子菜单集
        /// </summary>
        [DataMember]
        public ICollection<LoginMenuInfo> SubMenuInfos { get; set; }
    }
}
