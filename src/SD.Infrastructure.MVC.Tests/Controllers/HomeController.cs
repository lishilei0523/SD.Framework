using SD.Infrastructure.Constants;
using SD.Infrastructure.StubIAppService.Interfaces;
using System.Web.Mvc;

namespace SD.Infrastructure.MVC.Tests.Controllers
{
    /// <summary>
    /// 首页控制器
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 商品接口
        /// </summary>
        private readonly IProductContract _productContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public HomeController(IProductContract productContract)
        {
            this._productContract = productContract;
        }

        /// <summary>
        /// 获取首页视图
        /// </summary>
        /// <returns>首页视图</returns>
        [HttpGet]
        public ViewResult Index()
        {
            this.ViewBag.SessionId = GlobalSetting.CurrentSessionId;
            this.ViewBag.Order = this._productContract.GetProducts();

            return this.View();
        }
    }
}
