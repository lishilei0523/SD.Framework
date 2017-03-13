using System;
using SD.IOC.Core.Mediator;

namespace SD.Infrastructure.CommandBase.Factories
{
    /// <summary>
    /// 命令执行者工厂
    /// </summary>
    internal static class CommandExecutorFactory
    {
        #region # 获取命令执行者 —— ICommandExecutor<T> GetCommandExecutorFor...
        /// <summary>
        /// 获取命令执行者
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <returns>命令执行者</returns>
        public static ICommandExecutor<T> GetCommandExecutorFor<T>(T command) where T : class, ICommand
        {
            return ResolveMediator.Resolve<ICommandExecutor<T>>();
        }
        #endregion

        #region # 获取命令执行者 —— object GetCommandExecutorFor(Type commandType)
        /// <summary>
        /// 命令执行者
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>命令执行者</returns>
        public static object GetCommandExecutorFor(Type commandType)
        {
            #region # 验证类型

            if (!typeof(ICommand).IsAssignableFrom(commandType))
            {
                throw new InvalidOperationException(string.Format("类型\"{0}\"不实现命令基接口！", commandType.FullName));
            }

            #endregion

            Type executorType = typeof(ICommandExecutor<>).MakeGenericType(commandType);

            return ResolveMediator.Resolve(executorType);
        }
        #endregion
    }
}
