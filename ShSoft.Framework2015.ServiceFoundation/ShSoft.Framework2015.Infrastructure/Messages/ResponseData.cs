using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.Framework2015.Infrastructure.IDTO;

namespace ShSoft.Framework2015.Infrastructure.Messages
{
    /// <summary>
    /// 应用程序服务层响应数据模型
    /// </summary>
    /// <typeparam name="T">DTO类型</typeparam>
    [DataContract]
    public class ResponseData<T> : ResponseMessage where T : BaseDTO
    {
        #region 01.分页数据模型 —— PageModel<T> PageModel
        /// <summary>
        /// 分页数据模型
        /// </summary>
        [DataMember]
        public PageModel<T> PageModel { get; set; }
        #endregion

        #region 02.数据集 —— IEnumerable<T> DataList
        /// <summary>
        /// 数据集
        /// </summary>
        [DataMember]
        public IEnumerable<T> DataList { get; set; }
        #endregion

        #region 03.数据对象 —— T Single
        /// <summary>
        /// 数据对象
        /// </summary>
        [DataMember]
        public T Single { get; set; }
        #endregion
    }
}