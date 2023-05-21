using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.Models;
using Zero.DAL;
using System.Data;
using tools.Models;

namespace Zero.BLL
{
    public class SysLogManager
    {
        private SysLogService sysLogService = new SysLogService();
        public int AddSysLog(SysLog sysLog)
        {
            return sysLogService.AddSysLog(sysLog);
        }

        public DataTable QuerySysLogByCondition(string start, string end, string logType)
        {
            return sysLogService.QuerySysLogByCondition(start, end, logType);
        }
    }


}
