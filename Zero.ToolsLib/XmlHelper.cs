using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Zero.ToolsLib
{
    public  class XmlHelper
    {
        #region 1.Xml���л������л���صķ���
        /// <summary>
        /// ʹ��XmlSerializer���л�����
        /// </summary>
        /// <typeparam name="T">��Ҫ���л��Ķ������ͣ���������[Serializable]����</typeparam>
        /// <param name="obj">��Ҫ���л��Ķ���</param>
        /// <param name="omitXmlDeclaration">true:ʡ��XML����;����Ϊfalse.Ĭ��false������д XML ������</param>
        /// <returns>���л�����ַ���</returns>
        public static string EntityToXml<T>(T obj, bool omitXmlDeclaration = false)
        {

            /* This property only applies to XmlWriter instances that output text content to a stream; otherwise, this setting is ignored.
            ���ܺܶ����������� ����ת����Xml���ܷ����л���ΪUTF8XML������������������ԭ��
            */
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.OmitXmlDeclaration = omitXmlDeclaration;
            xmlSettings.Encoding = new System.Text.UTF8Encoding(false);
            MemoryStream stream = new MemoryStream();
            XmlWriter xmlwriter = XmlWriter.Create(stream, xmlSettings); //�������ֱ��д�ɣ�Encoding = Encoding.UTF8 �������ɵ�xml�м���BOM(Byte-order Mark) ��Ϣ(Unicode �ֽ�˳����) �� ����new System.Text.UTF8Encoding(false)����ѷ�ʽ��ʡ�������滻���鷳
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add(String.Empty, String.Empty); //��XML���л�ʱȥ��Ĭ�������ռ�xmlns:xsd��xmlns:xsi
            XmlSerializer ser = new XmlSerializer(typeof(T));
            ser.Serialize(xmlwriter, obj, xmlns);

            return Encoding.UTF8.GetString(stream.ToArray());//writer.ToString();
        }

        /// <summary>
        /// ʹ��XmlSerializer���л�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">�ļ�·��</param>
        /// <param name="obj">��Ҫ���л��Ķ���</param>
        /// <param name="omitXmlDeclaration">true:ʡ��XML����;����Ϊfalse.Ĭ��false������д XML ������</param>
        /// <param name="removeDefaultNamespace">�Ƿ��Ƴ�Ĭ�����ƿռ�(���������ʱָ����:XmlRoot(Namespace = "http://www.xxx.com/xsd")����Ҫ��falseֵ����)</param>
        /// <returns>���л�����ַ���</returns>
        public static void EntityToXmlFile<T>(string path, T obj, bool omitXmlDeclaration = false, bool removeDefaultNamespace = true)
        {
            XmlWriterSettings xmlSetings = new XmlWriterSettings();
            xmlSetings.OmitXmlDeclaration = omitXmlDeclaration;
            using (XmlWriter xmlwriter = XmlWriter.Create(path, xmlSetings))
            {
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                if (removeDefaultNamespace)
                    xmlns.Add(String.Empty, String.Empty); //��XML���л�ʱȥ��Ĭ�������ռ�xmlns:xsd��xmlns:xsi
                XmlSerializer ser = new XmlSerializer(typeof(T));
                ser.Serialize(xmlwriter, obj, xmlns);
            }
        }

        private static byte[] ReadFile(string filePath)
        {
            byte[] bytes;
            //����"������һ����ʹ��,��˸ý����޷����ʴ��ļ�"����쳣 ������ flieShare����ΪReadWrite����������ļ������ڵĻ������ǻ�����쳣���������ﲻ�ܳԵ��κ��쳣��������Ҫ���ǵ���Щ����
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bytes = new byte[fs.Length];
                int numBytesToRead = (int)fs.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    int n = fs.Read(bytes, numBytesRead, numBytesToRead);
                    if (n == 0)
                        break;
                    numBytesRead += n;
                    numBytesToRead -= n;
                }
            }
            return bytes;
        }


        /// <summary>
        /// ʹ��XmlSerializer�����л�����
        /// </summary>
        /// <param name="xmlOfObject">��Ҫ�����л���xml�ַ���</param>
        /// <returns>�����л���Ķ���</returns>
        public static T XmlToEntity<T>(string xmlOfObject) where T : class
        {
            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlOfObject), new XmlReaderSettings());
            return (T)new XmlSerializer(typeof(T)).Deserialize(xmlReader);
        }


        /// <summary>
        /// ���ļ���ȡ�������л�Ϊ���� �����: ���̻߳������¶�д�������⣩
        /// </summary>
        /// <typeparam name="T">���صĶ�������</typeparam>
        /// <param name="path">�ļ���ַ</param>
        /// <returns></returns>
        public static T XmlFileToEntity<T>(string path)
        {
            byte[] bytes = ReadFile(path);
            if (bytes.Length < 1)//���ļ����ڱ�д������ʱ�����ܶ���Ϊ0
                for (int i = 0; i < 5; i++)
                { 
                    //5�λ���
                    bytes = ReadFile(path); // ����������������������ռ�ļ����ļ����ڱ�д��ʱ������������Ϊ0�ֽڵ����⡣
                    if (bytes.Length > 0) break;
                    System.Threading.Thread.Sleep(50); //����������ܹ��������1/4�룬��ȡ�ļ�
                }
            XmlDocument doc = new XmlDocument();
            doc.Load(new MemoryStream(bytes));
            if (doc.DocumentElement != null)
                return (T)new XmlSerializer(typeof(T)).Deserialize(new XmlNodeReader(doc.DocumentElement));
            return default(T);
        }


        #endregion

        #region 2.Xml�ļ���д�����ķ���

        /// <summary>
        /// ����XML�ĵ�
        /// </summary>
        /// <param name="name">���ڵ�����</param>
        /// <param name="type">���ڵ��һ������ֵ</param>
        /// <returns></returns>
        /// .net�е��÷�����д���ļ���,��
        ///          document = XmlOperate.CreateXmlDocument("sex", "sexy");
        ///          document.Save("c:/bookstore.xml");        
        public static XmlDocument CreateXmlDocument(string name, string type)
        {
            XmlDocument doc = null;
            XmlElement rootEle = null;
            try
            {
                doc = new XmlDocument();
                doc.LoadXml("<" + name + "/>");
                rootEle = doc.DocumentElement;
                rootEle.SetAttribute("type", type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return doc;
        }


        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="node">�ڵ�</param>
        /// <param name="attribute">���������ǿ�ʱ���ظ�����ֵ�����򷵻ش���ֵ</param>
        /// <returns>string</returns>
        /**************************************************
         * ʹ��ʾ��:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="node">�ڵ�</param>
        /// <param name="element">Ԫ�������ǿ�ʱ������Ԫ�أ������ڸ�Ԫ���в�������</param>
        /// <param name="attribute">���������ǿ�ʱ�����Ԫ������ֵ���������Ԫ��ֵ</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        /**************************************************
         * ʹ��ʾ��:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="node">�ڵ�</param>
        /// <param name="attribute">���������ǿ�ʱ�޸ĸýڵ�����ֵ�������޸Ľڵ�ֵ</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        /**************************************************
         * ʹ��ʾ��:
         * XmlHelper.Insert(path, "/Node", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="node">�ڵ�</param>
        /// <param name="attribute">���������ǿ�ʱɾ���ýڵ�����ֵ������ɾ���ڵ�ֵ</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        /**************************************************
         * ʹ��ʾ��:
         * XmlHelper.Delete(path, "/Node", "")
         * XmlHelper.Delete(path, "/Node", "Attribute")
         ************************************************/
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}
