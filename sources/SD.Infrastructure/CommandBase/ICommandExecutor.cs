namespace SD.Infrastructure.CommandBase
{
    /// <summary>
    /// 命令执行者基接口
    /// </summary>
    public interface ICommandExecutor<in T> where T : class, ICommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令对象</param>
        void Execute(T command);
    }
}
