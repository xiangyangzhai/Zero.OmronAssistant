using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.ToolsLib
{
    public class MySQLHelper
    {

        public static string connString = string.Empty;

        /// <summary>
        /// ִ��insert��update��delete���͵�SQL���
        /// </summary>
        /// <param name="cmdText">SQL����洢��������</param>
        /// <param name="paramArray">��������</param>
        /// <returns>��Ӱ�������</returns>
        public static int ExecuteNonQuery(string cmdText, MySqlParameter[] paramArray = null)
        {
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            if (paramArray != null)
            {
                cmd.Parameters.AddRange(paramArray);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string errorMsg = $"{DateTime.Now}  : ִ�� public static int ExecuteNonQuery(string cmdText, SqlParameter[] paramArray = null)���������쳣��{ ex.Message}";
                //������ط�д����־...

                throw new Exception("ִ��public static int ExecuteNonQuery(string cmdText, SqlParameter[] paramArray = null)���������쳣��" + ex.Message);
            }
            finally   //���ϲ����Ƿ����쳣������ִ�еĴ���
            {
                conn.Close();
            }
        }

        /// <summary>
        /// ���ص�һ����Ĳ�ѯ
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, MySqlParameter[] paramArray = null)
        {
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            if (paramArray != null)
            {
                cmd.Parameters.AddRange(paramArray);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //������ط�д����־...

                throw new Exception("ִ�� public object ExecuteScalar(string cmdText, SqlParameter[] paramArray = null���������쳣��" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// ִ�з���һ��ֻ��������Ĳ�ѯ
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static MySqlDataReader ExecuteReader(string cmdText, MySqlParameter[] paramArray = null)
        {
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand cmd = new MySqlCommand(cmdText, conn);
            if (paramArray != null)
            {
                cmd.Parameters.AddRange(paramArray);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection); //����������ö��
            }
            catch (Exception ex)
            {
                //������ط�д����־...

                throw new Exception("ִ�� public object SqlDataReader(string cmdText, SqlParameter[] paramArray = null)���������쳣��" + ex.Message);
            }
        }
        /// <summary>
        /// ���ذ���һ�����ݱ�����ݼ��Ĳ�ѯ
        /// </summary>
        /// <param name="sql">��ѯ���</param>
        /// <param name="tableName">���ݱ������</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql, MySqlParameter[] paramArray = null, string tableName = null)
        {
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            if (paramArray != null)
            {
                cmd.Parameters.AddRange(paramArray);
            }
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                if (tableName == null)
                    da.Fill(ds);
                else
                    da.Fill(ds, tableName);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("ִ�� public DataSet GetDataSet(string sql, string tableName = null)���������쳣��" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// ִ�в�ѯ������һ���������DataSet
        /// </summary>
        /// <param name="dicTableAndSql"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(Dictionary<string, string> dicTableAndSql)
        {
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                foreach (string tbName in dicTableAndSql.Keys)
                {
                    cmd.CommandText = dicTableAndSql[tbName];
                    da.Fill(ds, tbName);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("ִ�� public DataSet GetDataSet(Dictionary<string,string> dicTableAndSql)���������쳣��" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
