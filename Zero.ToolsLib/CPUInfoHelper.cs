using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Zero.ToolsLib
{
    /// <summary>
    /// CPUInfoHelper
    /// </summary>
    public class CPUInfoHelper
    {

        private static string defaultValue = "abcdefgh";

        /// <summary>
        /// ��ȡ�������Ϣ
        /// </summary>
        /// <returns>��ȡ���</returns>
        public static string GetComputerInfo()
        {
            string cpu = GetCPUInfo();
            string bios = GetBIOSInfo();
            return string.Concat(cpu, bios);
        }

        public static string GetCPUInfo()
        {
            return GetHardWareInfo("Win32_Processor", "ProcessorId");
        }

        public static string GetBIOSInfo()
        {
            string info = GetHardWareInfo("Win32_BIOS", "SerialNumber");
            if (!string.IsNullOrEmpty(info) && info != "To be filled by O.E.M" && !info.Contains("O.E.M") && !info.Contains("OEM") && !info.Contains("Default"))
            {
                return info;
            }
            else
            {
                return defaultValue;
            }
        }

        private static string GetHardWareInfo(string typePath, string key)
        {
            ManagementClass managementClass = new ManagementClass(typePath);
            ManagementObjectCollection mn = managementClass.GetInstances();
            PropertyDataCollection properties = managementClass.Properties;
            foreach (PropertyData property in properties)
            {
                if (property.Name == key)
                {
                    foreach (ManagementObject m in mn)
                    {
                        return m.Properties[property.Name].Value.ToString();
                    }
                }
            }
            return string.Empty;
        }


        /// <summary>
        /// ���ܽ��
        /// </summary>
        /// <returns>���ؼ��ܽ��</returns>
        public static string Encrypt()
        {
            return StringSecurityHelper.DESEncrypt(StringSecurityHelper.MD5Encrypt(GetComputerInfo()));
        }

        /// <summary>
        /// ��֤�Ƿ���ȷ
        /// </summary>
        /// <param name="Code">�ַ���</param>
        /// <returns>���</returns>
        public static bool Check(string Code)
        {
            if (StringSecurityHelper.DESDecrypt(Code) == StringSecurityHelper.MD5Encrypt(GetComputerInfo()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
