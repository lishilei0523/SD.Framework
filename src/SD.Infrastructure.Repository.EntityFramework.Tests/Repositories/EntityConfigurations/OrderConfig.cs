using SD.Infrastructure.Repository.EntityFramework.Tests.Entities;
using SD.Toolkits.EntityFramework.Extensions;
using System.Data.Entity.ModelConfiguration;

namespace SD.Infrastructure.Repository.EntityFramework.Tests.Repositories.EntityConfigurations
{
    /// <summary>
    /// 单据数据映射配置
    /// </summary>
    public class OrderConfig : EntityTypeConfiguration<Order>
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public OrderConfig()
        {
            //设置编号长度
            this.Property(order => order.Number).HasMaxLength(32);

            //设置索引
            this.HasIndex("IX_Number", IndexType.Unique, table => table.Property(order => order.Number));
        }
    }
}
