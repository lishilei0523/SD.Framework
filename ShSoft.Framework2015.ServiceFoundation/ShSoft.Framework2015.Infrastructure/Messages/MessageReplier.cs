using System.Collections.Generic;
using ShSoft.Framework2015.Infrastructure.IDTO;

namespace ShSoft.Framework2015.Infrastructure.Messages
{
    /// <summary>
    /// 消息答复者
    /// </summary>
    public static class MessageReplier
    {
        #region 01.响应状态消息 —— ResponseMessage Response(ResponseStatus status, string message)
        /// <summary>
        /// 响应状态消息
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="message">消息</param>
        /// <returns>响应消息模型</returns>
        public static ResponseMessage Response(ResponseStatus status, string message)
        {
            return new ResponseMessage { ResponseStatus = status, Message = message };
        }
        #endregion

        #region 02.响应数据信息 —— ResponseData<T> Response<T>(ResponseStatus status, string message, T single)
        /// <summary>
        /// 响应数据信息
        /// </summary>
        /// <typeparam name="T">DTO类型</typeparam>
        /// <param name="status">响应状态</param>
        /// <param name="message">响应消息</param>
        /// <param name="single">数据信息</param>
        /// <returns>响应数据模型</returns>
        public static ResponseData<T> Response<T>(ResponseStatus status, string message, T single) where T : BaseDTO
        {
            return new ResponseData<T> { ResponseStatus = status, Message = message, Single = single };
        }
        #endregion

        #region 03.响应数据集 —— ResponseData<T> Response<T>(ResponseStatus status, string message, IEnumerable<T> list)
        /// <summary>
        /// 响应数据集
        /// </summary>
        /// <typeparam name="T">DTO类型</typeparam>
        /// <param name="status">响应状态</param>
        /// <param name="message">响应消息</param>
        /// <param name="list">数据集</param>
        /// <returns>响应数据模型</returns>
        public static ResponseData<T> Response<T>(ResponseStatus status, string message, IEnumerable<T> list) where T : BaseDTO
        {
            return new ResponseData<T> { ResponseStatus = status, Message = message, DataList = list };
        }
        #endregion

        #region 04.响应分页数据模型 —— ResponseData<T> Response<T>(ResponseStatus status, string message, PageModel<T> pageModel)
        /// <summary>
        /// 响应分页数据模型
        /// </summary>
        /// <typeparam name="T">DTO类型</typeparam>
        /// <param name="status">响应状态</param>
        /// <param name="message">响应消息</param>
        /// <param name="pageModel">分页数据模型</param>
        /// <returns>响应数据模型</returns>
        public static ResponseData<T> Response<T>(ResponseStatus status, string message, PageModel<T> pageModel) where T : BaseDTO
        {
            return new ResponseData<T> { ResponseStatus = status, Message = message, PageModel = pageModel };
        }
        #endregion

        #region 05.响应一对多数据模型 —— ResponseData<T> Response<T>(ResponseStatus status, string message, T single, IEnumerable<T>...
        /// <summary>
        /// 响应一对多数据模型
        /// </summary>
        /// <typeparam name="T">DTO类型</typeparam>
        /// <param name="status">响应状态</param>
        /// <param name="message">响应消息</param>
        /// <param name="single">数据信息</param>
        /// <param name="list">数据集</param>
        /// <returns>响应数据模型</returns>
        public static ResponseData<T> Response<T>(ResponseStatus status, string message, T single, IEnumerable<T> list) where T : BaseDTO
        {
            return new ResponseData<T> { ResponseStatus = status, Message = message, Single = single, DataList = list };
        }
        #endregion
    }
}
