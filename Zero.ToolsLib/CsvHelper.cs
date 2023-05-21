using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zero.ToolsLib
{
    public class CsvHelper
    {
        /// <summary>
        /// ��CSV�ļ������ݶ�ȡ��DataTable��
        /// </summary>
        /// <param name="path">CSV�ļ�·��</param>
        /// <param name="hasTitle">�Ƿ�CSV�ļ��ĵ�һ�ж�ȡΪDataTable������</param>
        /// <returns></returns>
        public static DataTable CsvToDataTable(string path, bool hasTitle = false)
        {
            DataTable dt = new DataTable();           //Ҫ��������ݱ�
            StreamReader sr = new StreamReader(path, Encoding.Default); //�ļ�������
            bool bFirst = true;                       //ָʾ�Ƿ��һ�ζ�ȡ����

            //���ж�ȡ
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(',');

                //��һ�ζ�ȡ����ʱ��Ҫ����������
                if (bFirst)
                {
                    for (int i = 0; i < elements.Length; i++)
                    {
                        dt.Columns.Add();
                    }
                    bFirst = false;
                }

                //�б�����ʱ����һ�е��������д���
                if (hasTitle)
                {
                    for (int i = 0; i < dt.Columns.Count && i < elements.Length; i++)
                    {
                        dt.Columns[i].ColumnName = elements[i];
                    }
                    hasTitle = false;
                }
                else //��ȡһ������
                {
                    if (elements.Length == dt.Columns.Count)
                    {
                        dt.Rows.Add(elements);
                    }
                    else
                    {
                        throw new Exception("CSV��ʽ���󣺱�����������һ��");
                    }
                }
            }
            sr.Close();

            return dt;
        }

        /// <summary>
        /// ��CSV�ļ��е����ݶ�ȡת����List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="hasTitle"></param>
        /// <returns></returns>
        public static List<T> CsvToList<T>(string path, bool hasTitle = false)
        {
            DataTable dataTable = CsvToDataTable(path, hasTitle);

            return DataTableToList<T>(dataTable);
        }


        /// <summary>
        /// ��DataTable���ݱ��浽CSV�ļ���
        /// </summary>
        /// <param name="dt">���ݱ�</param>
        /// <param name="path">CSV�ļ���ַ</param>
        /// <param name="hasTitle">�Ƿ�Ҫ������ݱ����������ΪCSV�ļ���һ��</param>
        public static void DataTableToCsv(DataTable dt, string path, bool hasTitle = false)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(path, false, Encoding.Default);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //��������У�����У�
            if (hasTitle)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i].ColumnName);
                    if (i != dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.WriteLine();
            }

            //����ļ�����
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sw.Write(dt.Rows[i][j].ToString());
                    if (j != dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.WriteLine();
            }

            sw.Close();
        }

        /// <summary>
        /// ��List����ת��CSV�ļ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <param name="hasTitle"></param>
        public static void ListToCsv<T>(List<T> list, string path, bool hasTitle = false)
        {
            DataTable dataTable = ListToDataTable<T>(list);

            DataTableToCsv(dataTable, path, hasTitle);
        }


        #region DataTableToList
        public static List<T> DataTableToList<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        private static List<T> ConvertTo<T>(IList<DataRow> rows)
        {
            List<T> list = null;
            if (rows != null)
            {

                list = new List<T>();
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }

            }
            return list;
        }

        private static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);

            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);

                    try
                    {
                        object value = row[column.ColumnName];

                        SetObjectPropertyValue(obj, column.ColumnName, value.ToString());
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// ͨ��������������ֵ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="obj">����</param>
        /// <param name="propertyName">��������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private static bool SetObjectPropertyValue<T>(T obj, string propertyName, string value)
        {
            try
            {
                Type type = typeof(T);

                object t = Convert.ChangeType(value, type.GetProperty(propertyName).PropertyType);

                type.GetProperty(propertyName).SetValue(obj, t, null);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region ListToDataTable

        public static DataTable ListToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();

            var dt = new DataTable();

            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());

            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();

                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }

                    object[] array = tempList.ToArray();

                    dt.LoadDataRow(array, true);

                }
            }
            return dt;
        }

        #endregion

        #region д�뵽CSV

        /// <summary>
        /// д�뵽CSV
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="dataList">���ݼ���</param>
        /// <param name="title">���⼯��</param>
        public static void WriteToCsv(string path, List<string> dataList, List<string> title = null, string remark = null)
        {
            StreamWriter streamWriter = null;

            try
            {
                //˵���ǵ�һ�Σ���Ϊ��һ����Ҫд��title
                if (!File.Exists(path))
                {
                    streamWriter = new StreamWriter(path, false, Encoding.Default);

                    if (remark != null)
                    {
                        //д��һЩ��ע��Ϣ
                        streamWriter.WriteLine(remark, Encoding.Default);
                    }

                    if (title != null)
                    {
                        //д�����
                        streamWriter.WriteLine(string.Join(",", title), Encoding.Default);
                    }
                }
                else
                {
                    //д������
                    streamWriter = new StreamWriter(path, true, Encoding.Default);
                }

                streamWriter.WriteLine(string.Join(",", dataList), Encoding.Default);

                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Csvд��ʧ�ܣ�" + ex.Message);
            }
        }

        #endregion

    }
}
