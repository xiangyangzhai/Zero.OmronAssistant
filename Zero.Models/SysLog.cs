using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tools.Models
{
    public class SysLog
    {
        /// <summary>
        /// 插入时间
        /// </summary>
        public string InsertTime { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 报警名称
        /// </summary>
        public string VarName { get; set; }

        /// <summary>
        /// 报警设定值
        /// </summary>
        public string AlarmSet { get; set; }

        /// <summary>
        /// 报警值
        /// </summary>
        public string AlarmValue { get; set; }

        /// <summary>
        /// 报警类型，触发/消除
        /// </summary>
        public string  AlarmType { get; set; }


    }
}
