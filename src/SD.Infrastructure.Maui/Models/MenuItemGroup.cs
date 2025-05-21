using SD.Infrastructure.Membership;
using System.Collections.Generic;

namespace SD.Infrastructure.Maui.Models
{
    /// <summary>
    /// 菜单项组
    /// </summary>
    public class MenuItemGroup : List<LoginMenuInfo>
    {
        /// <summary>
        /// 菜单项组名称
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// 创建菜单项组构造器
        /// </summary>
        /// <param name="menuGroupName">菜单项组名称</param>
        /// <param name="menuItems">菜单项列表</param>
        public MenuItemGroup(string menuGroupName, IEnumerable<LoginMenuInfo> menuItems)
            : base(menuItems)
        {
            this.GroupName = menuGroupName;
        }
    }
}
