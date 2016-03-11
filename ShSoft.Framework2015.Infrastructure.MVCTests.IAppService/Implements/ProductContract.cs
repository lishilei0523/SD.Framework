using ShSoft.Framework2015.Infrastructure.MVCTests.IAppService.Interfaces;

namespace ShSoft.Framework2015.Infrastructure.MVCTests.IAppService.Implements
{
    /// <summary>
    /// 商品管理实现
    /// </summary>
    public class ProductContract : IProductContract
    {
        /// <summary>
        /// 获取商品集
        /// </summary>
        /// <returns>商品集</returns>
        public string GetProducts()
        {
            return "Hello World";
        }
    }
}
