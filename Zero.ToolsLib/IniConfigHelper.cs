using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zero.ToolsLib
{
    public class IniConfigHelper
    {
        /// <summary>
        /// �ļ�·��
        /// </summary>
        public static string IniPath = string.Empty;


        #region API��������

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key,
    string val, string filePath);

        //��Ҫ����GetPrivateProfileString������
        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(string section, string key,
            string def, Byte[] retVal, int size, string filePath);

        #endregion

        #region ��ȡINI�ļ�
        /// <summary>
        /// ���ݽڵ㼰Key��ֵ��������
        /// </summary>
        /// <param name="Section">�ڵ�</param>
        /// <param name="Key">��</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <param name="path">·��</param>
        /// <returns>����ֵ</returns>
        public static string ReadIniData(string Section, string Key, string defaultValue, string path)
        {
            if (File.Exists(path))
            {
                StringBuilder stringBuilder = new StringBuilder();

                GetPrivateProfileString(Section, Key, defaultValue, stringBuilder, 1024, path);

                return stringBuilder.ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// ���ݽڵ㼰Key��ֵ��������
        /// </summary>
        /// <param name="Section">�ڵ�</param>
        /// <param name="Key">��</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>����ֵ</returns>
        public static string ReadIniData(string Section, string Key, string defaultValue)
        {
            return ReadIniData(Section, Key, defaultValue, IniPath);
        }

        #endregion

        #region д��Ini�ļ�

        /// <summary>
        /// ���ݽڵ㼰Key��ֵд������
        /// </summary>
        /// <param name="Section">�ڵ�</param>
        /// <param name="Key">��</param>
        /// <param name="Value">ֵ</param>
        /// <param name="path">·��</param>
        /// <returns>�������</returns>
        public static bool WriteIniData(string Section, string Key, string Value, string path)
        {
            long result = WritePrivateProfileString(Section, Key, Value, path);

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// ���ݽڵ㼰Key��ֵд������
        /// </summary>
        /// <param name="Section">�ڵ�</param>
        /// <param name="Key">��</param>
        /// <param name="Value">ֵ</param>
        /// <returns>�������</returns>
        public static bool WriteIniData(string Section, string Key, string Value)
        {
            return WriteIniData(Section, Key, Value, IniPath);
        }

        #endregion

        #region ��ȡ���е�Sections

        /// <summary>
        /// ��ȡ���е�Section
        /// </summary>
        /// <param name="path">·��</param>
        /// <returns>Section����</returns>
        public static List<string> ReadSections(string path)
        {
            byte[] buffer = new byte[65536];

            uint length = GetPrivateProfileStringA(null, null, null, buffer, buffer.Length, path);

            int startIndex = 0;

            List<string> sections = new List<string>();

            for (int i = 0; i < length; i++)
            {
                if (buffer[i] == 0)
                {
                    sections.Add(Encoding.Default.GetString(buffer, startIndex, i - startIndex));
                    startIndex = i + 1;
                }
            }

            return sections;
        }

        /// <summary>
        /// ��ȡ���е�Section
        /// </summary>
        /// <param name="path">·��</param>
        /// <returns>Section����</returns>
        public static List<string> ReadSections()
        {
            byte[] buffer = new byte[65536];

            uint length = GetPrivateProfileStringA(null, null, null, buffer, buffer.Length, IniPath);

            int startIndex = 0;

            List<string> sections = new List<string>();

            for (int i = 0; i < length; i++)
            {
                if (buffer[i] == 0)
                {
                    sections.Add(Encoding.Default.GetString(buffer, startIndex, i - startIndex));
                    startIndex = i + 1;
                }
            }

            return sections;
        }



        #endregion

        #region ����ĳ��Section��ȡ���е�Keys

        /// <summary>
        /// ����ĳ��Section��ȡ���е�Keys
        /// </summary>
        /// <param name="section">ĳ��section</param>
        /// <param name="path">·��</param>
        /// <returns>key�ļ���</returns>
        public static List<string> ReadKeys(string section, string path)
        {
            byte[] buffer = new byte[65536];

            uint length = GetPrivateProfileStringA(section, null, null, buffer, buffer.Length, path);

            int startIndex = 0;

            List<string> keys = new List<string>();

            for (int i = 0; i < length; i++)
            {
                if (buffer[i] == 0)
                {
                    keys.Add(Encoding.Default.GetString(buffer, startIndex, i - startIndex));
                    startIndex = i + 1;
                }
            }

            return keys;

        }

        /// <summary>
        /// ����ĳ��Section��ȡ���е�Keys
        /// </summary>
        /// <param name="section">ĳ��section</param>
        /// <param name="path">·��</param>
        /// <returns>key�ļ���</returns>
        public static List<string> ReadKeys(string section)
        {
            byte[] buffer = new byte[65536];

            uint length = GetPrivateProfileStringA(section, null, null, buffer, buffer.Length, IniPath);

            int startIndex = 0;

            List<string> keys = new List<string>();

            for (int i = 0; i < length; i++)
            {
                if (buffer[i] == 0)
                {
                    keys.Add(Encoding.Default.GetString(buffer, startIndex, i - startIndex));
                    startIndex = i + 1;
                }
            }

            return keys;

        }

        #endregion

    }
}
