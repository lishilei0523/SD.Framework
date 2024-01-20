using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Entities;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.EntityConfigurations
{
    /// <summary>
    /// 单据实体映射配置
    /// </summary>
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        /// <summary>
        /// 配置
        /// </summary>
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //配置属性
            builder.HasKey(order => order.Id).IsClustered(false);
            builder.Property(order => order.Number).HasMaxLength(32);

            //配置索引
            builder.HasIndex(order => order.AddedTime).IsUnique(false).IsClustered().HasDatabaseName("IX_AddedTime");
            builder.HasIndex(order => order.Number).IsUnique().Metadata.SetDatabaseName("IX_Number");
        }
    }
}
