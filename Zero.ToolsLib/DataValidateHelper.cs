using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zero.ToolsLib
{
    public  class DataValidateHelper
    {
        /// <summary>
        /// ��֤�Ƿ�Ϊ����
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsInteger(string text)
        {
            Regex regex = new Regex(@"^[0-9]\d*$");

            return regex.IsMatch(text);       
        }


        /// <summary>
        /// ��֤�Ƿ���Email
        /// </summary>
        /// <param name="txt">�ַ���</param>
        /// <returns>���</returns>
        public static bool IsEmail(string txt)
        {
            Regex objReg = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return objReg.IsMatch(txt);
        }
        /// <summary>
        /// ��֤���֤
        /// </summary>
        /// <param name="txt">�ַ���</param>
        /// <returns>���</returns>
        public static bool IsIdentityCard(string txt)
        {
            Regex objReg = new Regex(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
            return objReg.IsMatch(txt);
        }

        /// <summary>
        /// �Ƿ�Ϊ��ЧIP��ַ
        /// </summary>
        /// <param name="ip">�ַ���</param>
        /// <returns>���</returns>
        public static bool IsIPAddress(string ip)
        {

            if (string.IsNullOrEmpty(ip) || ip.Length < 7 || ip.Length > 15) return false;

            string regformat = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);

            return regex.IsMatch(ip);

        }

        /// <summary>
        /// �Ƿ�Ϊ��Ч�˿�
        /// </summary>
        /// <param name="port">�ַ���</param>
        /// <returns>���</returns>
        public static bool IsIPPort(string port)
        {
            bool isPort = false;
            int portNum;
            isPort = Int32.TryParse(port, out portNum);
            if (isPort && portNum >= 0 && portNum <= 65535)
            {
                isPort = true;
            }
            else
            {
                isPort = false;
            }
            return isPort;
        }

    }
}
