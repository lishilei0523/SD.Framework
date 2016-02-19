namespace ShSoft.Framework2015.LogSite.Model.Format
{
    /// <summary>
    /// 响对象模型
    /// </summary>
    public class ResponseModel
    {
        #region 01.数据
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
        #endregion

        #region 02.状态码
        /// <summary>
        /// 状态，1正常，0错误
        /// </summary>
        public int Status { get; set; }
        #endregion

        #region 03.消息
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        #endregion

        #region 04.返回Url
        /// <summary>
        /// 返回Url
        /// </summary>
        public string ReturnUrl { get; set; }
        #endregion
    }
}
