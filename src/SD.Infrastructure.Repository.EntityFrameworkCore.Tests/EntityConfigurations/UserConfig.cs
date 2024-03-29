﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD.Infrastructure.Repository.EntityFrameworkCore.Tests.Entities;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Tests.EntityConfigurations
{
    /// <summary>
    /// 用户实体映射配置
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
            builder.Property(user => user.Number).HasMaxLength(20);
            builder.Property(user => user.PrivateKey).HasMaxLength(64);
            builder.Property(user => user.Age).HasPrecision(10, 3);

            //配置索引
            builder.HasIndex(user => user.AddedTime).IsUnique(false).IsClustered().HasDatabaseName("IX_AddedTime");
            builder.HasIndex(user => user.Number).IsUnique().Metadata.SetDatabaseName("IX_Number");
            builder.HasIndex(user => user.PrivateKey).IsUnique().Metadata.SetDatabaseName("IX_PrivateKey");
        }
    }
}
