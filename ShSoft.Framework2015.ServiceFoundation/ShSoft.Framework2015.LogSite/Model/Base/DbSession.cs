using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using ShSoft.Framework2015.LogSite.Migrations;

namespace ShSoft.Framework2015.LogSite.Model.Base
{
    /// <summary>
    /// EF上下文
    /// </summary>
    public class DbSession : DbContext
    {
        #region # 构造器

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DbSession()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbSession, Configuration>());
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public DbSession()
            : base("name=LogConnection")
        {

        }

        /// <summary>
        /// 单例模式创建对象
        /// </summary>
        /// <returns>EF上下文对象</returns>
        public static DbSession Current
        {
            get
            {
                DbSession dbSession = CallContext.GetData(typeof(DbSession).Name) as DbSession;
                if (dbSession == null)
                {
                    dbSession = new DbSession();
                    CallContext.SetData(typeof(DbSession).Name, dbSession);
                }
                return dbSession;
            }
        }

        #endregion

        #region # 属性

        #region 菜单 —— DbSet<Menu> Menus
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<Menu> Menus { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 初始化菜单 —— void InitMenu()
        /// <summary>
        /// 初始化菜单
        /// </summary>
        public void InitMenu()
        {
            //01.创建数据库
            this.Database.CreateIfNotExists();

            //02.判断菜单根节点是否存在，不存在则创建菜单节点
            if (!this.Set<Menu>().Any())
            {
                //创建根菜单
                Menu rootMenu = new Menu
                {
                    IsLink = false,
                    Level = 0,
                    MenuName = "六月版日志管理后台",
                    PId = 0,
                    Sort = int.MaxValue
                };

                this.Set<Menu>().Add(rootMenu);
                this.SaveChanges();

                //创建异常日志管理菜单
                Menu exceptionLogMenu = new Menu
                {
                    IsLink = true,
                    Level = 1,
                    MenuName = "异常日志管理",
                    PId = rootMenu.Id,
                    Sort = 2,
                    Url = "/ExceptionLog/Index"
                };

                //创建程序日志管理菜单
                Menu runningLogMenu = new Menu
                {
                    IsLink = true,
                    Level = 1,
                    MenuName = "程序日志管理",
                    PId = rootMenu.Id,
                    Sort = 1,
                    Url = "/RunningLog/Index"
                };

                //创建菜单管理菜单
                Menu menuManage = new Menu
                {
                    IsLink = true,
                    Level = 1,
                    MenuName = "菜单管理",
                    PId = rootMenu.Id,
                    Sort = 0,
                    Url = "/Menu/Index"
                };

                this.Set<Menu>().Add(exceptionLogMenu);
                this.Set<Menu>().Add(runningLogMenu);
                this.Set<Menu>().Add(menuManage);
                this.SaveChanges();
            }
        }
        #endregion

        #endregion
    }
}