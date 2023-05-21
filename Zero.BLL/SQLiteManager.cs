using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.DAL;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.BLL
{
    public class SQLiteManager
    {
        private SQLiteService sQLiteService = new SQLiteService();
        public void SetConnStr(string connString)
        {
            sQLiteService.SetConnStr(connString);
        }

        /// <summary>
        /// 判断数据表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsTableExists(string tableName)
        {
            return sQLiteService.IsTableExists(tableName);
        }

        /// <summary>
        /// 删除数据表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool DeleteTable(string tableName)
        {
            return sQLiteService.DeleteTable(tableName);

        }

        /// <summary>
        /// 清除表数据内存
        /// </summary>
        /// <returns></returns>
        public bool VacuumTable()
        {
            return sQLiteService.VacuumTable();
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
            return sQLiteService.CreateTable(tableName, colums, isPrimaryKey);
        }

        /// <summary>
        /// 重新创建数据表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colums"></param>
        /// <returns></returns>
        public OperateResult ReCreateActualTable(string tableName,List<string> colums)
        {
            if (IsTableExists(tableName))
            {
                if (DeleteTable(tableName)==false)
                {
                    return OperateResult.CreateFailResult("删除数据表失败");
                }
            }

            if (CreateTable(tableName, colums)==false)
            {
                return OperateResult.CreateFailResult("创建数据表失败");

            }

            VacuumTable();

            return OperateResult.CreateSuccessResult();
        }
    }
}
