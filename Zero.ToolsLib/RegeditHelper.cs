using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.ToolsLib
{
    /// <summary>
    /// ע���
    /// </summary>
    public class RegeditHelper
    {
        private const string SUB_KEY = "SOFTWARE";
        private static readonly RegistryKey HKML;
        private static readonly RegistryKey SOFTWARE;

        static RegeditHelper()
        {
            //Win10 ��дLocalMachineȨ�ޣ�û�з���Ȩ��
            HKML = Registry.CurrentUser;
            SOFTWARE = HKML.OpenSubKey(SUB_KEY, true);
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="name">����</param>
        /// <returns>��������</returns>
        public static object ReadData(string node, string name)
        {
            RegistryKey tmp = SOFTWARE.OpenSubKey(node, true);
            object result = tmp?.GetValue(name);
            tmp?.Close();
            return result;
        }

        /// <summary>
        /// д��ֵ
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="item">����</param>
        /// <param name="value">ֵ</param>
        public static void WriteData(string node, string item, object value)
        {
            RegistryKey tmp = SOFTWARE.CreateSubKey(node);
            tmp?.SetValue(item, value);
            tmp?.Close();
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="item">����</param>
        /// <param name="value">ֵ</param>
        public static void DeleteNode(string node)
        {
            SOFTWARE.DeleteSubKey(node);
        }

    }
}
