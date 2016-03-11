using ShSoft.Framework2015.Infrastructure.IEntity;

namespace ShSoft.Framework2015.EntityFrameworkTests.StubEntities
{
    /// <summary>
    /// 班级
    /// </summary>
    public class Class : AggregateRootEntity
    {
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Class() { }

        /// <summary>
        /// 创建班级构造器
        /// </summary>
        /// <param name="classNo">班级编号</param>
        /// <param name="className">班级名称</param>
        public Class(string classNo, string className)
            : this()
        {
            base.Number = classNo;
            base.Name = className;
        }
    }
}
