using ShSoft.Infrastructure.Repository.Redis;
using ShSoft.Infrastructure.Repository.RedisTests.Entities;
using ShSoft.Infrastructure.Repository.RedisTests.IRepositories;

namespace ShSoft.Infrastructure.Repository.RedisTests.Repositories
{
    /// <summary>
    /// 商品仓储实现
    /// </summary>
    public class ProductRepository : RedisRepositoryProvider<Product>, IProductRepository
    {

    }
}
