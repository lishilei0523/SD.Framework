using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD.Common.PoweredByLee
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
            ICollection<string> ips = GetIPs();
            string ipsStr = ips.ToSplicString();

            return ipsStr;
        }
        #endregion

        #region # 获取IP地址列表 —— static ICollection<string> GetIPs()
        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP</returns>
        public static ICollection<string> GetIPs()
        {
            ICollection<string> ips = new HashSet<string>();

            string hostName = Dns.GetHostName();//本机名   

            ips.Add(hostName);

            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   

            foreach (IPAddress ipAddress in ipAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(ipAddress.ToString());
                }
            }

            return ips;
        }
        #endregion

        #region # 获取CPU唯一码列表 —— static ICollection<string> GetCpuIds()
        /// <summary>
        /// 获取CPU唯一码列表
        /// </summary>
        /// <returns>CPU唯一码列表</returns>
        public static ICollection<string> GetCpuIds()
        {
            ManagementClass managementClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection managementObjects = managementClass.GetInstances();

            ICollection<string> cpuIds = new HashSet<string>();

            foreach (ManagementBaseObject managementBase in managementObjects)
            {
                ManagementObject management = (ManagementObject)managementBase;
                PropertyData propertyData = management.Properties["ProcessorId"];
                object dataValue = propertyData.Value;

                if (dataValue != null)
                {
                    string cpuId = dataValue.ToString().Trim();
                    cpuIds.Add(cpuId);
                }
            }

            return cpuIds;
        }
        #endregion

        #region # 获取硬盘唯一码列表 —— static ICollection<string> GetHardDiskIds()
        /// <summary>
        /// 获取硬盘唯一码列表
        /// </summary>
        /// <returns>硬盘唯一码列表</returns>
        public static ICollection<string> GetHardDiskIds()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            ManagementObjectCollection managementObjects = searcher.Get();

            ICollection<string> hardDiskIds = new HashSet<string>();

            foreach (ManagementBaseObject managementBase in managementObjects)
            {
                ManagementObject management = (ManagementObject)managementBase;
                object serial = management["SerialNumber"];

                if (serial != null)
                {
                    string hardDiskId = serial.ToString().Trim();
                    hardDiskIds.Add(hardDiskId);
                }
            }

            return hardDiskIds;
        }
        #endregion

        #region # 获取MAC地址列表 —— static ICollection<string> GetMacAddresses()
        /// <summary>
        /// 获取MAC地址列表
        /// </summary>
        /// <returns>MAC地址列表</returns>
        public static ICollection<string> GetMacAddresses()
        {
            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection managementObjects = managementClass.GetInstances();

            ICollection<string> macAddresses = new HashSet<string>();

            foreach (ManagementBaseObject managementBase in managementObjects)
            {
                ManagementObject management = (ManagementObject)managementBase;
                object mac = management["MacAddress"];

                if (mac != null)
                {
                    string macAddress = mac.ToString().Trim();
                    macAddresses.Add(macAddress);
                }
            }

            return macAddresses;
        }
        #endregion

        #region # 获取机器唯一码 —— static string GetMachineCode()
        /// <summary>
        /// 获取机器唯一码
        /// </summary>
        /// <returns>机器唯一码</returns>
        public static string GetMachineCode()
        {
            ICollection<string> hardDiskIds = GetHardDiskIds();
            ICollection<string> macs = GetMacAddresses();

            StringBuilder builder = new StringBuilder();

            if (hardDiskIds.Any())
            {
                builder.Append(hardDiskIds.First());
            }
            if (macs.Any())
            {
                builder.Append(macs.First());
            }

            string machineCode = builder.ToString().ToMD5();

            return machineCode;
        }
        #endregion
    }
}
