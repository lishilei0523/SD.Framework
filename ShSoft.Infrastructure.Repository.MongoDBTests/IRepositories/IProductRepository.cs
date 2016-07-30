using ShSoft.Infrastructure.Repository.MongoDBTests.Entities;
using ShSoft.Infrastructure.RepositoryBase;

namespace ShSoft.Infrastructure.Repository.MongoDBTests.IRepositories
{
    /// <summary>
    /// 商品仓储接口
    /// </summary>
    public interface IProductRepository : ISimpleRepository<Product>
    {

    }
}
