using ShSoft.Infrastructure.Repository.RedisTests.Entities;
using ShSoft.Infrastructure.RepositoryBase;

namespace ShSoft.Infrastructure.Repository.RedisTests.IRepositories
{
    /// <summary>
    /// 商品仓储接口
    /// </summary>
    public interface IProductRepository : ISimpleRepository<Product>
    {

    }
}
