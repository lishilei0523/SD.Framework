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
        /// 构造器
        /// </summary>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //设置编号长度
            builder.Property(user => user.Number).HasMaxLength(20);
            builder.Property(user => user.PrivateKey).HasMaxLength(64);

            //设置索引
            builder.HasIndex(user => user.Number).HasDatabaseName("IX_Number").IsUnique();
            builder.HasIndex(user => user.PrivateKey).HasDatabaseName("IX_PrivateKey").IsUnique();
        }
    }
}
