using System.Web.Mvc;
using ShSoft.Framework2015.Infrastructure.MVCTests.IAppService;

namespace ShSoft.Framework2015.Infrastructure.MVCTests.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductContract _productContract;

        public HomeController(IProductContract productContract)
        {
            this._productContract = productContract;
        }

        public ActionResult Index()
        {
            this.ViewBag.Hello = this._productContract.Test();
            return this.View();
        }

    }
}
