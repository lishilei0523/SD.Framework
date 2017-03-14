using SD.Infrastructure.Repository.Redis.Tests.Entities;
using SD.Infrastructure.Repository.Redis.Tests.IRepositories;

namespace SD.Infrastructure.Repository.Redis.Tests.Repositories
{
    /// <summary>
    /// 商品仓储实现
    /// </summary>
    public class ProductRepository : RedisRepositoryProvider<Product>, IProductRepository
    {

    }
}
