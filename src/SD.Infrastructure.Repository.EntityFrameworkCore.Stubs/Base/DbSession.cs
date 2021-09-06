﻿using Microsoft.EntityFrameworkCore;
using SD.Infrastructure.Constants;
using SD.Infrastructure.Repository.EntityFrameworkCore.Base;

namespace SD.Infrastructure.Repository.EntityFrameworkCore.Stubs.Base
{
    /// <summary>
    /// EF Core上下文
    /// </summary>
    public class DbSession : BaseDbSession
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(NetCoreSetting.DefaultConnectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}