using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.ToolsLib;

namespace Zero.DAL
{
    public class SQLiteService
    {
        public void SetConnStr(string connString)
        {
            SQLiteHelper.connString = connString;
        }

        /// <summary>
        /// 判断数据表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsTableExists(string tableName)
        {
            string sql = "select count(*) from sqlite_master where type='table' and name=@table";
            SQLiteParameter[] parameters = new SQLiteParameter[] {
                new SQLiteParameter("@table",tableName),
            };

            try
            {
                return int.Parse( SQLiteHelper.ExecuteScalar(sql, parameters).ToString()) == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除数据表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool DeleteTable(string tableName)
        {
            string sql = "drop table @table";
            SQLiteParameter[] parameters = new SQLiteParameter[] {
                new SQLiteParameter("@table",tableName),
            };

            try
            {
                SQLiteHelper.ExecuteNonQuery(sql, parameters);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 清除表数据内存
        /// </summary>
        /// <returns></returns>
        public bool VacuumTable()
        {
            string sql = "Vacuum";
            try
            {
                SQLiteHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colums"></param>
        /// <param name="isPrimaryKey"></param>
        /// <returns></returns>
        public bool CreateTable(string tableName, List<string> colums, bool isPrimaryKey = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("create table ");
            stringBuilder.Append(tableName + "(");

            if (isPrimaryKey)
            {
                stringBuilder.Append("ID Integer primary key autoincrement not null,");

            }
            for (int i = 0; i < colums.Count; i++)
            {
                //如果是最后一个字段
                if (i == colums.Count - 1)
                {
                    stringBuilder.Append(colums[i] + " varchar(20)");
                }
                else
                {
                    stringBuilder.Append(colums[i] + " varchar(20),");
                }
            }
            stringBuilder.Append(")");
            try
            {
                SQLiteHelper.ExecuteNonQuery(stringBuilder.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
