using SD.Infrastructure.CommandBase.Factories;

namespace SD.Infrastructure.CommandBase.Mediator
{
    /// <summary>
    /// 命令中介者
    /// </summary>
    public static class CommandMediator
    {
        #region # 执行命令 —— static void Execute<T>(T command)
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        /// <param name="command">命令对象</param>
        public static void Execute<T>(T command) where T : class, ICommand
        {
            //获取相应命令执行者实例
            ICommandExecutor<T> commandExecutor = CommandExecutorFactory.GetCommandExecutorFor(command);

            //执行
            commandExecutor.Execute(command);
        }
        #endregion
    }
}
