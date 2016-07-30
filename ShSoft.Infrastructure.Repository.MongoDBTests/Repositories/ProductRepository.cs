using ShSoft.Infrastructure.Repository.MongoDB;
using ShSoft.Infrastructure.Repository.MongoDBTests.Entities;
using ShSoft.Infrastructure.Repository.MongoDBTests.IRepositories;

namespace ShSoft.Infrastructure.Repository.MongoDBTests.Repositories
{
    /// <summary>
    /// 商品仓储实现
    /// </summary>
    public class ProductRepository : MongoRepositoryProvider<Product>, IProductRepository
    {

    }
}
