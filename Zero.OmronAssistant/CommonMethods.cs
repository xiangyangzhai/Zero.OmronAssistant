using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.Models;

namespace Zero.OmronAssistant
{
    public class CommonMethods
    {
        // 创建一个全局plc对象，包含所有的Variables.txt中的全局变量
        public static List<PLCVariable> PLCVariables = new List<PLCVariable>();

        //创建一个全局变量用来作为datagridview的数据源
        public static List<PLCVariable> PLCVariables_Show;
    }
}
