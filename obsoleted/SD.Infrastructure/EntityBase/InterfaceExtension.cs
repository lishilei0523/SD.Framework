using SD.Infrastructure.Constants;
using System;

namespace SD.Infrastructure.EntityBase
{
    /// <summary>
    /// 接口扩展工具类
    /// </summary>
    public static class InterfaceExtension
    {
        #region # 提交审核 —— static void DoSubmit<T>(this T checkable)
        /// <summary>
        /// 提交审核
        /// </summary>
        /// <typeparam name="T">需要审核类型</typeparam>
        /// <param name="checkable">需要审核接口实例</param>
        public static void DoSubmit<T>(this T checkable) where T : ICheckable
        {
            #region # 验证状态

            if (checkable.CheckStatus != CheckStatus.Unchecked && checkable.CheckStatus != CheckStatus.Rejected)
            {
                throw new InvalidOperationException("只有当审核状态为未审核或审核未通过时，才可以提交审核！");
            }

            #endregion

            checkable.CheckStatus = CheckStatus.Checking;
        }
        #endregion

        #region # 审核 —— static void DoCheck<T>(this T checkable, bool passed...
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="checkable">需要审核接口实例</param>
        /// <param name="passed">是否通过</param>
        /// <param name="checkedRemark">审核意见</param>
        /// <param name="checkerAccount">审核人账号</param>
        /// <param name="checkerName">审核人姓名</param>
        public static void DoCheck<T>(this T checkable, bool passed, string checkedRemark, string checkerAccount, string checkerName) where T : ICheckable
        {
            #region # 验证状态

            if (checkable.CheckStatus != CheckStatus.Checking)
            {
                throw new InvalidOperationException("只有待审核的可以审核！");
            }
            if (checkable.CheckStatus == CheckStatus.Passed)
            {
                throw new InvalidOperationException("已审核通过的不可再审核！");
            }

            #endregion

            checkable.CheckedRemark = checkedRemark;
            checkable.CheckerAccount = checkerAccount;
            checkable.CheckerName = checkerName;
            checkable.CheckStatus = passed ? CheckStatus.Passed : CheckStatus.Rejected;
            checkable.CheckedTime = DateTime.Now;
        }
        #endregion

        #region # 启用 —— static void DoEnable<T>(this T enable)
        /// <summary>
        /// 启用
        /// </summary>
        public static void DoEnable<T>(this T enable) where T : IDisable
        {
            if (enable.Enabled)
            {
                return;
            }

            enable.Enabled = true;
        }
        #endregion

        #region # 停用 —— static void DoDisable<T>(this T enable)
        /// <summary>
        /// 停用
        /// </summary>
        public static void DoDisable<T>(this T enable) where T : IDisable
        {
            if (!enable.Enabled)
            {
                return;
            }

            enable.Enabled = false;
        }
        #endregion
    }
}
