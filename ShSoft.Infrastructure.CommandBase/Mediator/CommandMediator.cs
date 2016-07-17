using System;
using System.Reflection;
using System.Threading.Tasks;
using ShSoft.Infrastructure.CommandBase.Factories;

namespace ShSoft.Infrastructure.CommandBase.Mediator
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

            //或异步执行
            if (command.Asynchronous)
            {
                Task.Run(() => commandExecutor.Execute(command));
            }
            else
            {
                commandExecutor.Execute(command);
            }
        }
        #endregion

        #region # 执行命令 —— static void Execute(ICommand command)
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令对象</param>
        public static void Execute(ICommand command)
        {
            //获取相应命令执行者实例
            object commandExecutor = CommandExecutorFactory.GetCommandExecutorFor(command.GetType());

            //或异步执行
            if (command.Asynchronous)
            {
                Task.Run(() =>
                {
                    Type executorType = commandExecutor.GetType();

                    MethodInfo methodInfo = executorType.GetMethod("Execute", new[] { command.GetType() });

                    methodInfo.Invoke(commandExecutor, new object[] { command });
                });
            }
            else
            {
                Type executorType = commandExecutor.GetType();

                MethodInfo methodInfo = executorType.GetMethod("Execute", new[] { command.GetType() });

                methodInfo.Invoke(commandExecutor, new object[] { command });
            }
        }
        #endregion
    }
}
