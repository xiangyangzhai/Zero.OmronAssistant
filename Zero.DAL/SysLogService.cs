using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tools.Models;
using Zero.Models;
using Zero.ToolsLib;

namespace Zero.DAL
{
    public class SysLogService
    {
        /// <summary>
        /// 添加一条系统日志
        /// </summary>
        /// <param name="sysLog"></param>
        /// <returns></returns>
        public int AddSysLog(SysLog sysLog)
        {
            string sql = "insert into SysLog(InsertTime,Note,LogType,Operator,VarName,AlarmSet,AlarmValue,AlarmType) " +
                "values(@InsertTime,@Note,@LogType,@Operator,@VarName,@AlarmSet,@AlarmValue,@AlarmType)";
            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@InsertTime",sysLog.InsertTime),
                new SQLiteParameter("@Note",sysLog.Note),
                new SQLiteParameter("@LogType",sysLog.LogType),
                new SQLiteParameter("@Operator",sysLog.Operator),
                new SQLiteParameter("@VarName",sysLog.VarName),
                new SQLiteParameter("@AlarmSet",sysLog.AlarmSet),
                new SQLiteParameter("@AlarmValue",sysLog.AlarmValue),
                new SQLiteParameter("@AlarmType",sysLog.AlarmType),
            };
            return SQLiteHelper.ExecuteNonQuery(sql, parameters);
        }

        /// <summary>
        /// 根据时间和日志类型进行查询
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        public DataTable QuerySysLogByCondition(string start,string end,string logType)
        {
            string sql = "select InsertTime,Note,LogType,Operator,VarName,AlarmSet,AlarmValue,AlarmType " +
                "from SysLog where InsertTime between @start and @end";
            List<SQLiteParameter> parameters = new List<SQLiteParameter>()
            {
                new SQLiteParameter("@start",start),
                new SQLiteParameter("@end",end),
            };

            if (logType.Length>0)
            {
                sql += " and LogType=@LogType";
                parameters.Add(new SQLiteParameter("@LogType",logType));
            }

            DataSet dataSet = SQLiteHelper.GetDataSet(sql,parameters.ToArray());

            if (dataSet!=null&&dataSet.Tables.Count>0)
            {
                return dataSet.Tables[0];
            }
            else
            {
                return null;
            }
        }
    }
}
