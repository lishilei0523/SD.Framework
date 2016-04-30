using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ShSoft.Framework2016.Common.PoweredByLee
{
    /// <summary>
    /// 常用扩展方法
    /// </summary>
    public static class CommonExtension
    {
        #region # 获取本机IP地址 —— static string GetLocalIPAddress()
        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP</returns>
        public static string GetLocalIPAddress()
        {
            StringBuilder buid = new StringBuilder();

            string hostName = Dns.GetHostName();//本机名   
            buid.Append(hostName + ";");
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   

            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    buid.Append(ip + ";");
                }
            }
            return buid.ToString();
        }
        #endregion
    }
}
