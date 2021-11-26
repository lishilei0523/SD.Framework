using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD.Infrastructure.Repository.EntityFrameworkCore.Stubs.Entities;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Stubs.EntityConfigurations
{
    /// <summary>
    /// 用户数据映射配置
    /// </summary>
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// 配置
        /// </summary>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //配置属性
            builder.HasKey(user => user.Id).IsClustered(false);

            //设置编号长度
            builder.Property(user => user.Number).HasMaxLength(20);
            builder.Property(user => user.PrivateKey).HasMaxLength(64);
            builder.Property(user => user.Age).HasPrecision(10, 3);

            //设置索引
            builder.HasIndex(user => user.AddedTime).IsUnique(false).IsClustered().HasDatabaseName("IX_AddedTime");
            builder.HasIndex(user => user.Number).IsUnique().Metadata.SetName("IX_Number");
            builder.HasIndex(user => user.PrivateKey).IsUnique().Metadata.SetName("IX_PrivateKey");
        }
    }
}
