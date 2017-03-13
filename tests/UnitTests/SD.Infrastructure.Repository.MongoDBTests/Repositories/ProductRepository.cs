using SD.Infrastructure.Repository.MongoDB.Tests.Entities;
using SD.Infrastructure.Repository.MongoDB.Tests.IRepositories;

namespace SD.Infrastructure.Repository.MongoDB.Tests.Repositories
{
    /// <summary>
    /// 商品仓储实现
    /// </summary>
    public class ProductRepository : MongoRepositoryProvider<Product>, IProductRepository
    {

    }
}
