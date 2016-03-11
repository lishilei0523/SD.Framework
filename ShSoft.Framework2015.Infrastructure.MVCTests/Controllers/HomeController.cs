using System.Web.Mvc;
using ShSoft.Framework2015.Infrastructure.MVCTests.IAppService.Interfaces;

namespace ShSoft.Framework2015.Infrastructure.MVCTests.Controllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 商品管理服务接口
        /// </summary>
        private readonly IProductContract _productContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="productContract">商品管理服务接口</param>
        public HomeController(IProductContract productContract)
        {
            this._productContract = productContract;
        }

        /// <summary>
        /// 商品管理首页
        /// </summary>
        public ActionResult Index()
        {
            this.ViewBag.Hello = this._productContract.GetProducts();
            return this.View();
        }

    }
}
