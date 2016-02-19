using System;
using System.ServiceModel;

namespace ShSoft.Framework2015.Infrastructure.WCF
{
    /// <summary>
    /// WCF扩展工具类
    /// </summary>
    public static class WcfExtension
    {
        /// <summary>
        /// 关闭信道扩展方法
        /// </summary>
        /// <param name="channel">信道实例</param>
        public static void CloseChannel(this object channel)
        {
            ICommunicationObject communicationObject = channel as ICommunicationObject;

            if (communicationObject == null)
            {
                return;
            }

            try
            {
                if (communicationObject.State == CommunicationState.Faulted)
                {
                    communicationObject.Abort();
                }
                else
                {
                    communicationObject.Close();
                }
            }
            catch (TimeoutException)
            {
                communicationObject.Abort();
            }
            catch (CommunicationException)
            {
                communicationObject.Abort();
            }
            catch (Exception)
            {
                communicationObject.Abort();
                throw;
            }
        }
    }
}
