using SD.Infrastructure.Constants;
using System;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 需要审核接口
    /// </summary>
    public interface ICheckable
    {
        #region # 属性

        #region 审核状态 —— CheckStatus CheckStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        CheckStatus CheckStatus { get; set; }
        #endregion

        #region 审核意见 —— string CheckedRemark
        /// <summary>
        /// 审核意见
        /// </summary>
        string CheckedRemark { get; set; }
        #endregion

        #region 审核人账号 —— string CheckerAccount
        /// <summary>
        /// 审核人账号
        /// </summary>
        string CheckerAccount { get; set; }
        #endregion

        #region 审核人姓名 —— string CheckerName
        /// <summary>
        /// 审核人姓名
        /// </summary>
        string CheckerName { get; set; }
        #endregion

        #region 审核时间 —— DateTime? CheckedTime
        /// <summary>
        /// 审核时间
        /// </summary>
        DateTime? CheckedTime { get; set; }
        #endregion

        #endregion

        #region # 方法

        #region 提交审核 —— void Submit()
        /// <summary>
        /// 提交审核
        /// </summary>
        void Submit();
        #endregion

        #region 审核 —— void Check(bool passed, string checkedRemark...
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="passed">是否通过</param>
        /// <param name="checkedRemark">审核意见</param>
        /// <param name="checkedAccount">审核人账号</param>
        /// <param name="checkedName">审核人姓名</param>
        void Check(bool passed, string checkedRemark, string checkedAccount, string checkedName);
        #endregion

        #endregion
    }
}
