using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Zero.ToolsLib
{
    public class FileHelper
    {
        #region 1.�ı��ļ���д


        public static void WriteToTxt(string path, string content, bool isAppend = false)
        {
            //��1�������ļ���
            FileStream fileStream = new FileStream(path, isAppend ? FileMode.Append : FileMode.Create);

            //��2������д����
            StreamWriter streamWriter = new StreamWriter(fileStream);

            //��3����������ʽд������
            streamWriter.Write(content);

            //��4���ر�д����
            streamWriter.Close();

            //��5���ر��ļ���
            fileStream.Close();
        }


        public static string ReadFromTxt(string path)
        {
            //�ж��ļ��Ƿ����
            if (!File.Exists(path))
            {
                return string.Empty;
            }

            //��1�������ļ���
            FileStream fileStream = new FileStream(path, FileMode.Open);

            //��2��������ȡ��
            StreamReader streamReader = new StreamReader(fileStream);

            //��3�������ķ�ʽ��ȡ
            string content = streamReader.ReadToEnd();

            //��4���رն�ȡ��
            streamReader.Close();

            //��5���ر��ļ���
            fileStream.Close();

            return content;

        }

        #endregion

        #region 2.�������л��ļ�

        /// <summary>
        /// ���л�һ������һ���ļ���
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void SerializeObject(object obj, string path)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(path, FileMode.Create);

                BinaryFormatter binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(fileStream, obj);

            }
            catch (Exception ex)
            {
                throw new Exception("���л��������" + ex.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }


        public static T DeSerializeObject<T>(string path)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(path, FileMode.Open);

                BinaryFormatter binaryFormatter = new BinaryFormatter();

                return (T)binaryFormatter.Deserialize(fileStream);

            }
            catch (Exception ex)
            {
                throw new Exception("�����л��������" + ex.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }


        #endregion

        #region 3.�������л��ַ���

        /// <summary>
        /// ���������л����ַ���
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObjToString(object obj)
        {
            IFormatter formatter = new BinaryFormatter();

            string result = string.Empty;

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);

                byte[] bytes = stream.ToArray();

                result = Convert.ToBase64String(bytes);

                stream.Flush();

            }
            return result;
        }

        /// <summary>
        /// ���ַ��������л��ɶ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeSerializeObjFromString<T>(string str) where T : class
        {
            IFormatter formatter = new BinaryFormatter();

            byte[] bytes = Convert.FromBase64String(str);

            T obj = null;

            using (MemoryStream stream = new MemoryStream(bytes, 0, bytes.Length))
            {
                obj = (T)formatter.Deserialize(stream);
            }

            return obj;
        }

        #endregion

        #region 4.�ļ��Ļ�������

        /// <summary>
        /// �ļ�����
        /// </summary>
        /// <param name="srcFileName"></param>
        /// <param name="desFileName"></param>
        public static void CopyFile(string srcFileName, string desFileName)
        {
            if (File.Exists(desFileName))
            {
                File.Delete(desFileName);
            }

            File.Copy(srcFileName, desFileName);

        }


        /// <summary>
        /// �ļ��ƶ�
        /// </summary>
        /// <param name="srcFileName"></param>
        /// <param name="desFileName"></param>
        public static void MoveFile(string srcFileName, string desFileName)
        {
            if (File.Exists(srcFileName))
            {
                if (File.Exists(desFileName))
                {
                    File.Delete(desFileName);
                }

                File.Move(srcFileName, desFileName);
            }
        }

        /// <summary>
        /// �ļ�ɾ��
        /// </summary>
        /// <param name="fileName"></param>
        public static void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        #endregion


        #region 5.�ļ��е���ز���

        /// <summary>
        /// ��ȡָ��Ŀ¼�µ������ļ�
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        /// <summary>
        /// ��ȡָ��Ŀ¼�µ�������Ŀ¼
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
             Directory.CreateDirectory(path);
        }

        /// <summary>
        /// ɾ��ָ��Ŀ¼�µ�������Ŀ¼���ļ�
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            directory.Delete(true);
        }


        #endregion

    }
}
