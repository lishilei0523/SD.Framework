using System;
using ShSoft.Framework2015.EntityFramework.Base;
using ShSoft.Framework2015.Infrastructure.IEntity;

namespace ShSoft.Framework2015.EntityFrameworkTests.Entities
{
    public class DbSession : BaseDbContext
    {
        /// <summary>
        /// 基础构造器
        /// </summary>
        public DbSession()
            : base("DefaultConnection")
        {
        }

        /// <summary>
        /// 实体所在程序集
        /// </summary>
        public override string EntityAssembly
        {
            get { return "ShSoft.Framework2015.EntityFrameworkTests"; }
        }

        /// <summary>
        /// 实体配置所在程序集
        /// </summary>
        public override string EntityConfigAssembly
        {
            get { return null; }
        }

        /// <summary>
        /// 类型查询条件
        /// </summary>
        public override Func<Type, bool> TypeQuery
        {
            get { return x => x.IsSubclassOf(typeof(PlainEntity)); }
        }

        /// <summary>
        /// 数据表名前缀
        /// </summary>
        public override string TablePrefix
        {
            get { return null; }
        }
    }
}
