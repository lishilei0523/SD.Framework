using SD.Infrastructure.Repository.MongoDB.Tests.Entities;
using SD.Infrastructure.RepositoryBase;

namespace SD.Infrastructure.Repository.MongoDB.Tests.IRepositories
{
    /// <summary>
    /// 商品仓储接口
    /// </summary>
    public interface IProductRepository : ISimpleRepository<Product>
    {

    }
}
