using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using ShSoft.Framework2015.Common.PoweredByLee;
using ShSoft.Framework2015.LogSite.Model;
using ShSoft.Framework2015.LogSite.Model.Base;
using ShSoft.Framework2015.LogSite.Model.Format;

namespace ShSoft.Framework2015.LogSite.Controllers
{
    /// <summary>
    /// MVC全局操作上下文
    /// </summary>
    public sealed class OperateContext
    {
        #region # 访问器 —— static OperateContext Current

        /// <summary>
        /// 将构造方法私有化
        /// </summary>
        private OperateContext() { }

        /// <summary>
        /// 当前OperateContext实例，类似HttpContext.Current
        /// </summary>
        public static OperateContext Current
        {
            get
            {
                //从线程缓存中取，如果没有new一个，然后再存入线程缓存
                OperateContext context = CallContext.GetData(typeof(OperateContext).Name) as OperateContext;
                if (context == null)
                {
                    context = new OperateContext();
                    CallContext.SetData(typeof(OperateContext).Name, context);
                }
                return context;
            }
        }
        #endregion

        #region # 用户菜单Json字符串 —— string MainMenuTree
        /// <summary>
        /// 当前登录用户的菜单Json字符串
        /// </summary>
        public string MainMenuTree
        {
            get
            {
                List<Menu> menuList = DbSession.Current.Set<Menu>().OrderByDescending(x => x.Sort).ToList();
                return this.MenuListToTree(menuList, 0).ToJson();
            }
        }
        #endregion

        #region # JsonModel统一处理方法 JsonModel
        /// <summary>
        /// JsonModel统一处理方法
        /// </summary>
        /// <param name="data">返回客户端数据</param>
        /// <param name="status">返回客户端状态码</param>
        /// <param name="message">返回客户端消息</param>
        /// <param name="returnUrl">返回客户端重定向url地址</param>
        /// <returns>返回客户端JsonResult</returns>
        public JsonResult JsonModel(object data, int status, string message, string returnUrl)
        {
            ResponseModel jsonModel = new ResponseModel()
            {
                Data = data,
                Status = status,
                Message = message,
                ReturnUrl = returnUrl
            };
            JsonResult jr = new JsonResult()
            {
                Data = jsonModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return jr;
        }
        public JsonResult JsonModel(int status, string message)
        {
            return this.JsonModel(null, status, message, null);
        }
        public JsonResult JsonModel(object data, int status, string message)
        {
            return this.JsonModel(data, status, message, null);
        }
        public JsonResult JsonModel(int status, string message, string returnUrl)
        {
            return this.JsonModel(null, status, message, returnUrl);
        }
        #endregion

        #region # 转换EasyUI树形菜单集合方法 —— List<TreeModel> MenuListToTree(List<Menu> menuList, int pId)
        /// <summary>
        /// 将菜单集合转换EasyUI树形集合方法 MenuListToTree
        /// </summary>
        /// <param name="menuList">菜单集合</param>
        /// <param name="pId">父Id</param>
        /// <returns>EasyUI树形集合</returns>
        public List<TreeModel> MenuListToTree(List<Menu> menuList, int pId)
        {
            //1.创建TreeModel集合
            List<TreeModel> menuTree = new List<TreeModel>();
            //2.遍历菜单集合
            foreach (Menu menu in menuList)
            {
                //3.筛选集合中PId等于给定pId的菜单实体
                if (menu.PId == pId)
                {
                    //4.将菜单实体转换为TreeModel
                    TreeModel treeModel = menu.ToTreeNode();
                    //5.在树集合中添加转换后的TreeModel
                    menuTree.Add(treeModel);
                    //6.递归查询子节点
                    treeModel.children = this.MenuListToTree(menuList, treeModel.id);
                }
            }
            //7.返回TreeModel集合
            return menuTree;
        }
        #endregion
    }
}
