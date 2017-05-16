using SD.CacheManager;
using SD.Infrastructure.Constants;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

// ReSharper disable once CheckNamespace
namespace SD.Infrastructure
{
    /// <summary>
    /// Membership管理工具类
    /// </summary>
    public static class Membership
    {
        /// <summary>
        /// 当前登录信息
        /// </summary>
        /// <returns></returns>
        public static LoginInfo LoginInfo
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    //获取消息头
                    MessageHeaders headers = OperationContext.Current.IncomingMessageHeaders;

                    if (!headers.Any(x => x.Name == CommonConstants.WcfAuthHeaderName && x.Namespace == CommonConstants.WcfAuthHeaderNamespace))
                    {
                        return null;
                    }

                    //读取消息头中的公钥
                    Guid publicKey = headers.GetHeader<Guid>(CommonConstants.WcfAuthHeaderName, CommonConstants.WcfAuthHeaderNamespace);

                    //以公钥为键，查询分布式缓存
                    LoginInfo loginInfo = CacheMediator.Get<LoginInfo>(publicKey.ToString());

                    return loginInfo;
                }

                return null;
            }
        }
    }
}
