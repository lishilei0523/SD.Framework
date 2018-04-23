using SD.Infrastructure.EntityBase;

namespace SD.Infrastructure.DomainServiceBase
{
    /// <summary>
    /// 领域服务基接口
    /// </summary>
    public interface IDomainService<in T> where T : AggregateRootEntity
    {

    }
}
