using System;

namespace ShSoft.Infrastructure.CommandBase
{
    /// <summary>
    /// 命令队列基接口
    /// </summary>
    public interface ICommandQueue : IDisposable
    {
        #region # 挂起命令 —— void Suspend<T>(T command)
        /// <summary>
        /// 挂起命令
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        /// <param name="command">命令对象</param>
        void Suspend<T>(T command) where T : class, ICommand;
        #endregion

        #region # 处理未处理的命令 —— void HandleUncompletedCommands()
        /// <summary>
        /// 处理未处理的命令
        /// </summary>
        void HandleUncompletedCommands();
        #endregion
    }
}
