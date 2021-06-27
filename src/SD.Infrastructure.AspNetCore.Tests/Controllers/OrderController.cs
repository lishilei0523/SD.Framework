using Microsoft.AspNetCore.Mvc;
using SD.Infrastructure.Constants;
using SD.Infrastructure.StubIAppService.Interfaces;
using System;
using System.Diagnostics;

namespace SD.Infrastructure.AspNetCore.Tests.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    [ApiController]
    [Route("Api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// 商品接口
        /// </summary>
        private readonly IProductContract _productContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public OrderController(IProductContract productContract)
        {
            this._productContract = productContract;
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <returns>订单</returns>
        [HttpGet]
        public string GetOrder()
        {
            Trace.WriteLine($"当前会话Id：\"{GlobalSetting.CurrentSessionId}\"");
            Console.WriteLine($"当前会话Id：\"{GlobalSetting.CurrentSessionId}\"");
            return this._productContract.GetProducts();
        }
    }
}
