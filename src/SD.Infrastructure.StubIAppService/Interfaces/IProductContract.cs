namespace SD.Infrastructure.StubIAppService.Interfaces
{
    /// <summary>
    /// 商品管理接口
    /// </summary>
    public interface IProductContract
    {
        /// <summary>
        /// 获取商品集
        /// </summary>
        /// <returns>商品集</returns>
        string GetProducts();
    }
}
