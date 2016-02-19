using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using ShSoft.Framework2015.EntityFramework.Attributes;

namespace ShSoft.Framework2015.EntityFramework.Base
{
    /// <summary>
    /// EF上下文基类
    /// </summary>
    public abstract class BaseDbContext : DbContext
    {
        #region # 构造器

        #region 01.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        protected BaseDbContext(string connectionName)
            : base(connectionName)
        {
            this.Database.CreateIfNotExists();
        }
        #endregion

        #endregion

        #region # 属性

        #region 实体所在程序集 —— abstract string EntityAssembly
        /// <summary>
        /// 实体所在程序集
        /// </summary>
        public abstract string EntityAssembly { get; }
        #endregion

        #region 实体配置所在程序集 —— abstract string EntityConfigAssembly
        /// <summary>
        /// 实体配置所在程序集
        /// </summary>
        public abstract string EntityConfigAssembly { get; }
        #endregion

        #region 类型查询条件 —— abstract Func<Type, bool> TypeQuery
        /// <summary>
        /// 类型查询条件
        /// </summary>
        public abstract Func<Type, bool> TypeQuery { get; }
        #endregion

        #region 单独注册的类型集 —— virtual IEnumerable<Type> TypesToRegister
        /// <summary>
        /// 单独注册的类型集
        /// </summary>
        public virtual IEnumerable<Type> TypesToRegister
        {
            get { return null; }
        }
        #endregion

        #region 数据表名前缀 —— abstract string TablePrefix
        /// <summary>
        /// 数据表名前缀
        /// </summary>
        public abstract string TablePrefix { get; }
        #endregion

        #endregion

        #region # 方法

        #region # 模型创建事件 —— override void OnModelCreating(DbModelBuilder modelBuilder)
        /// <summary>
        /// 模型创建事件
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region # 验证程序集

            if (string.IsNullOrWhiteSpace(this.EntityAssembly))
            {
                throw new ApplicationException("实体所在程序集未配置！");
            }

            #endregion

            //加载模型所在程序集查询出所有符合条件的实体类型
            IEnumerable<Type> types = Assembly.Load(this.EntityAssembly).GetTypes().Where(x => !x.IsInterface && !x.IsDefined(typeof(NonMapAttribute)));

            #region # 验证类型查询条件

            if (this.TypeQuery != null)
            {
                types = types.Where(this.TypeQuery);
            }

            #endregion

            #region # 合并类型集

            HashSet<Type> entityTypes = new HashSet<Type>(types);

            if (this.TypesToRegister != null)
            {
                foreach (Type entityType in this.TypesToRegister)
                {
                    entityTypes.Add(entityType);
                }
            }

            #endregion

            //注册实体配置
            this.RegisterEntityConfigurations(modelBuilder);

            //注册实体
            this.RegisterEntityTypes(modelBuilder, entityTypes);

            //注册数据表前缀
            this.RegisterTableMaps(modelBuilder);

            //调用基类方法
            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region # 注册实体配置 —— void RegisterEntityConfigurations(DbModelBuilder modelBuilder)
        /// <summary>
        /// 注册实体配置
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        private void RegisterEntityConfigurations(DbModelBuilder modelBuilder)
        {
            if (!string.IsNullOrWhiteSpace(this.EntityConfigAssembly))
            {
                Assembly entityConfigAssembly = Assembly.Load(this.EntityConfigAssembly);
                modelBuilder.Configurations.AddFromAssembly(entityConfigAssembly);
            }
        }
        #endregion

        #region # 注册实体类型 —— void RegisterEntityTypes(DbModelBuilder modelBuilder...
        /// <summary>
        /// 注册实体类型
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        /// <param name="entityTypes">实体类型集</param>
        private void RegisterEntityTypes(DbModelBuilder modelBuilder, IEnumerable<Type> entityTypes)
        {
            foreach (Type entityType in entityTypes)
            {
                modelBuilder.RegisterEntityType(entityType);
            }
        }
        #endregion

        #region # 注册数据表名前缀 —— void RegisterTableMaps(DbModelBuilder modelBuilder...
        /// <summary>
        /// 注册数据表名前缀
        /// </summary>
        /// <param name="modelBuilder">模型建造者</param>
        private void RegisterTableMaps(DbModelBuilder modelBuilder)
        {
            string tablePrefix = string.IsNullOrWhiteSpace(this.TablePrefix) ? string.Empty : this.TablePrefix;
            modelBuilder.Types().Configure(entity => entity.ToTable(tablePrefix + entity.ClrType.Name));
        }
        #endregion

        #endregion
    }
}
