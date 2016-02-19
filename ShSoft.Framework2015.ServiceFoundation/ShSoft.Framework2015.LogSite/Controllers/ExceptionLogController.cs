using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShSoft.Framework2015.AOP.Models.Entities;
using ShSoft.Framework2015.LogSite.Model.ExceptionLogs;
using ShSoft.Framework2015.LogSite.Model.Format;

namespace ShSoft.Framework2015.LogSite.Controllers
{
    /// <summary>
    /// 异常日志管理控制器
    /// </summary>
    public class ExceptionLogController : Controller
    {
        #region 00.字段及构造器
        /// <summary>
        /// 异常日志业务对象
        /// </summary>
        private readonly ExceptionLogBll _exceptionLogBll;

        /// <summary>
        /// 构造器
        /// </summary>
        public ExceptionLogController()
        {
            this._exceptionLogBll = ExceptionLogBll.CreateInstance();
        }
        #endregion

        #region 01.异常日志视图 —— ActionResult Index()
        /// <summary>
        /// 异常日志视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return this.View();
        }
        #endregion

        #region 02.加载异常日志列表 —— ActionResult List()
        /// <summary>
        /// 加载异常日志列表
        /// </summary>
        /// <returns>异常日志列表</returns>
        public ActionResult List()
        {
            //1.获取EasyUI请求的数据
            int pageIndex = string.IsNullOrEmpty(this.Request["page"]) ? 1 : int.Parse(this.Request["page"]);
            int pageSize = string.IsNullOrEmpty(this.Request["rows"]) ? 15 : int.Parse(this.Request["rows"]);
            Guid logId = string.IsNullOrEmpty(this.Request["logId"]) ? Guid.Empty : Guid.Parse(this.Request["logId"]);

            //2.查询参数处理
            DateTime startDate, endDate;
            string startTime = DateTime.TryParse(this.Request["startTime"], out startDate) ? this.Request["startTime"] : null;
            string endTime = DateTime.TryParse(this.Request["endTime"], out endDate) ? this.Request["endTime"] : null;

            //3.调用业务层查询数据并转换视图模型返回
            int rowCount, pageCount;
            List<ExceptionLog> list = this._exceptionLogBll.GetModelList(pageIndex, pageSize, out pageCount, out rowCount,
                logId, startTime, endTime);

            //4.封装EasyUI模型
            GridModel gridModel = new GridModel(rowCount, list);

            //5.返回客户端数据
            return this.Json(gridModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 03.查看异常日志明细 —— ActionResult Detail(Guid id)
        /// <summary>
        /// 查看异常日志明细
        /// </summary>
        /// <param name="id">异常日志Id</param>
        /// <returns>异常日志明细</returns>
        public ActionResult Detail(Guid id)
        {
            ExceptionLog model = this._exceptionLogBll.GetModel(id);
            return this.View(model);
        }
        #endregion

        #region 04.删除选中异常日志 —— ActionResult DeleteSelected(string selectedIds)
        /// <summary>
        /// 删除选中异常日志
        /// </summary>
        /// <param name="selectedIds">选中行的Id</param>
        /// <returns>JsonModel</returns>
        public ActionResult DeleteSelected(string selectedIds)
        {
            try                 //涉及多次类型转换等操作，应处理异常
            {
                //1.处理前台选中的所有要删除的实体Id信息
                List<Guid> idList = selectedIds.Split(',').Select(x => Guid.Parse(x)).ToList();
                //2.执行批量删除操作
                idList.ForEach(x => this._exceptionLogBll.PhysicalDelete(x));
                return OperateContext.Current.JsonModel(1, "删除成功！");
            }
            catch (Exception ex)
            {
                //返回异常信息
                return OperateContext.Current.JsonModel(0, string.Format("删除失败，{0}", ex.Message));
            }
        }
        #endregion
    }
}
