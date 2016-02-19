using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using ShSoft.Framework2015.Common.PoweredByLee;
using ShSoft.Framework2015.LogSite.Model;
using ShSoft.Framework2015.LogSite.Model.Base;
using ShSoft.Framework2015.LogSite.Model.Format;

namespace ShSoft.Framework2015.LogSite.Controllers
{
    /// <summary>
    /// 菜单管理控制器
    /// </summary>
    public class MenuController : Controller
    {
        #region 00.构造器
        /// <summary>
        /// EF上下文对象
        /// </summary>
        private readonly DbContext _dbSession;

        /// <summary>
        /// 构造器
        /// </summary>
        public MenuController()
        {
            this._dbSession = DbSession.Current;
        }
        #endregion

        #region 01.加载视图 —— ActionResult Index()
        /// <summary>
        /// 加载视图
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Index()
        {
            return this.View();
        }
        #endregion

        #region 02.加载菜单树 —— ActionResult List()
        /// <summary>
        /// 加载菜单树
        /// </summary>
        /// <returns>菜单列表树形集合</returns>
        public ActionResult List()
        {
            //1.接收EasyUI请求
            int pId = this.Request["id"] == null ? 0 : int.Parse(this.Request["id"]);
            //2.查询所有菜单
            List<Menu> menuList = this._dbSession.Set<Menu>().OrderByDescending(x => x.Sort).ToList();
            //3.将菜单集合转换为树形
            List<TreeModel> treeList = OperateContext.Current.MenuListToTree(menuList, 0);
            //4.返回Json字符串
            return this.Json(treeList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 03.添加 - 加载视图 —— ActionResult Add(int id)
        /// <summary>
        /// 添加 - 加载视图
        /// </summary>
        /// <param name="id">父Id</param>
        /// <returns>添加视图</returns>
        public ActionResult Add(int id)
        {
            //接收客户端提供的父Id，根据Id查询Name，并赋值给ViewBag
            this.ViewBag.PId = id;
            this.ViewBag.PName = this._dbSession.Set<Menu>().Single(x => x.Id == id).MenuName;
            return this.View();
        }
        #endregion

        #region 04.添加 - 提交添加 —— ActionResult AddConfirm(Menu model)
        /// <summary>
        /// 添加——提交添加
        /// </summary>
        /// <param name="model">视图模型</param>
        /// <returns>JsonModel</returns>
        public ActionResult AddConfirm(Menu model)
        {
            //校验模型
            if (this.ModelState.IsValid)
            {
                Menu newModel = this._dbSession.Set<Menu>().Add(model);

                if (this._dbSession.SaveChanges() > 0)
                {
                    return OperateContext.Current.JsonModel(1, "添加成功！");
                }
                else
                {
                    return OperateContext.Current.JsonModel(0, "添加失败！");
                }
            }
            else    //模型校验失败
            {
                return OperateContext.Current.JsonModel(0, "您输入的数据有误，请重试！");
            }
        }
        #endregion

        #region 05.递归删除菜单（单个） —— ActionResult DeleteSingle(int id)
        /// <summary>
        /// 递归删除菜单（单个）
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>JsonModel</returns>
        public ActionResult DeleteSingle(int id)
        {
            try             //涉及外键关联，须处理异常
            {
                //1.查询该实体对象是否存在
                Menu model = this._dbSession.Set<Menu>().Single(x => x.Id == id);
                if (model == null)
                {
                    return OperateContext.Current.JsonModel(0, "该菜单节点不存在！");
                }
                else //2.如果存在，则递归删除
                {
                    this.DeleteRecursion(id);     //删除给定菜单节点以及其所有下级菜单节点
                    return OperateContext.Current.JsonModel(1, "删除成功！");
                }
            }
            catch (Exception ex)
            {
                //返回客户端错误消息
                return OperateContext.Current.JsonModel(0, ex.Message);
            }
        }

        /// <summary>
        /// 递归删除给定栏目及其所有下级栏目
        /// </summary>
        /// <param name="id">要删除的栏目Id</param>
        private void DeleteRecursion(int id)
        {
            //1.根据给定栏目Id获取所有下级栏目
            List<Menu> list = this._dbSession.Set<Menu>().Where(x => x.PId == id).ToList();
            if (list.Count > 0)
            {
                //2.遍历下级栏目
                foreach (Menu menu in list)
                {
                    this.DeleteRecursion(menu.Id);      //递归
                }
            }

            //3.删除给定部栏目及下级栏目
            Menu currentMenu = this._dbSession.Set<Menu>().Single(x => x.Id == id);
            this._dbSession.Set<Menu>().Remove(currentMenu);
            this._dbSession.SaveChanges();
        }
        #endregion

        #region 06.递归删除菜单（勾选） —— ActionResult DeleteSelected(string selectedIds)
        /// <summary>
        /// 删除选中的菜单节点
        /// </summary>
        /// <param name="selectedIds">选中树节点的Id</param>
        /// <returns>JsonModel</returns>
        public ActionResult DeleteSelected(string selectedIds)
        {
            try             //涉及多次类型转换与外键关系等操作，应处理异常
            {
                //1.处理前台选中的所有要删除的实体Id信息
                List<int> idList = selectedIds.Split(',').Select(x => int.Parse(x)).ToList();
                //2.执行批量删除操作
                idList.ForEach(id =>
                {
                    Menu currentMenu = this._dbSession.Set<Menu>().Single(x => x.Id == id);
                    this._dbSession.Set<Menu>().Remove(currentMenu);
                });
                return OperateContext.Current.JsonModel(1, "删除成功！");
            }
            catch (Exception ex)
            {
                //写入日志

                //返回异常信息
                return OperateContext.Current.JsonModel(0, string.Format("删除失败，{0}", ex.Message));
            }
        }
        #endregion

        #region 07.修改 - 加载视图 —— ActionResult Edit(int id)
        /// <summary>
        /// 修改菜单节点
        /// </summary>
        /// <param name="id">菜单节点Id</param>
        /// <returns>修改视图</returns>
        public ActionResult Edit(int id)
        {
            Menu currentMenu = this._dbSession.Set<Menu>().Single(x => x.Id == id);
            Menu pMenu = this._dbSession.Set<Menu>().SingleOrDefault(x => x.Id == currentMenu.PId);
            this.ViewBag.PName = pMenu == null ? null : pMenu.MenuName;
            return this.View(currentMenu);
        }
        #endregion

        #region 08.修改 - 提交修改 —— ActionResult EditConfirm(Menu model)
        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="model">视图模型</param>
        /// <returns>JsonModel</returns>
        public ActionResult EditConfirm(Menu model)
        {
            //校验模型
            if (this.ModelState.IsValid)
            {
                Menu currentMenu = this._dbSession.Set<Menu>().Single(x => x.Id == model.Id);

                Transform<Menu, Menu>.Fill(model, currentMenu);

                //2.执行修改操作
                DbEntityEntry entry = this._dbSession.Entry<Menu>(currentMenu);
                entry.State = EntityState.Modified;
                this._dbSession.SaveChanges();

                return OperateContext.Current.JsonModel(1, "修改成功！");
            }
            else    //模型校验失败
            {
                return OperateContext.Current.JsonModel(0, "您输入的数据有误，请重试！");
            }
        }
        #endregion
    }
}
