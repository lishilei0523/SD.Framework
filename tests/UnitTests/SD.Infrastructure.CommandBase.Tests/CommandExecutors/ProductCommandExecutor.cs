using SD.Infrastructure.AppServiceBase;
using SD.Infrastructure.CommandBase.Tests.Commands;

namespace SD.Infrastructure.CommandBase.Tests.CommandExecutors
{
    /// <summary>
    /// 商品相关命令执行者
    /// </summary>
    public class ProductCommandExecutor : IApplicationService, ICommandExecutor<CreateProductCommand>
    {
        /// <summary>
        /// 执行创建商品命令
        /// </summary>
        /// <param name="command">命令对象</param>
        public void Execute(CreateProductCommand command)
        {
            //TODO 创建商品
            command.Price = 20;
        }
    }
}
