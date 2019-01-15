using pde.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace pde.pub
{
    public static class HardWare
    {
        //取机器名     
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }

        //取CPU编号    
        public static string GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch(Exception e)
            {
                LogLocal.log().SaveLog(new LogEntity("获取CPU ID失败：" + e.Message, LogType.Plat, LogLevel.ERROR));
                return "";
            } 
        }

        /// <summary>
        /// 获取主板序列号
        /// </summary>
        /// <returns></returns>
        private static string GetMbId()
        {
            try
            {
                var myMb = new ManagementClass("Win32_BaseBoard").GetInstances();
                var serial = "";
                foreach (ManagementObject mb in myMb)
                {
                    var val = mb.Properties["SerialNumber"].Value;
                    serial += val == null ? "" : val.ToString();
                }
                return serial;
            }
            catch (Exception e)
            {
                LogLocal.log().SaveLog(new LogEntity("获取主板序列号失败：" + e.Message, LogType.Plat, LogLevel.ERROR));
                return "";
            }
        }

        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        private static string GetHdId()
        {
            try
            {
                var lpm = new ManagementClass("Win32_PhysicalMedia").GetInstances();
                var serial = "";
                foreach (ManagementObject hd in lpm)
                {
                    var val = hd.Properties["SerialNumber"].Value;
                    serial += val == null ? "" : val.ToString().Trim();
                }
                return serial;
            }
            catch (Exception e)
            {
                LogLocal.log().SaveLog(new LogEntity("获取硬盘序列号失败：" + e.Message, LogType.Plat, LogLevel.ERROR));
                return "";
            }
        }

        /// <summary>
        /// 获取BIOS编号
        /// </summary>
        /// <returns></returns>
        private static string GetBIOSCode()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_BIOS");
                ManagementObjectCollection moc = mc.GetInstances();
                string strID = null;
                foreach (ManagementObject mo in moc)
                {
                    strID = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                } 
                return strID;
            }
            catch (Exception e)
            {
                LogLocal.log().SaveLog(new LogEntity("获取硬盘序列号失败：" + e.Message, LogType.Plat, LogLevel.ERROR));
                return "";
            }
        }

        /// <summary>
        /// 获取本地在用的IP地址
        /// </summary>
        /// <returns></returns>
        public static string getLocalUsingIP()
        {
            try
            {
                var ni = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface item in ni)
                {
                    if (item.OperationalStatus == OperationalStatus.Up) //&& item.NetworkInterfaceType == ?
                    {
                        foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork & !IPAddress.IsLoopback(ip.Address))
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                } 
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
            catch (Exception e)
            {
                LogLocal.log().SaveLog(new LogEntity("获取当前IP失败：" + e.Message, LogType.Plat, LogLevel.ERROR));
                return "";
            }
        }
    }
}
