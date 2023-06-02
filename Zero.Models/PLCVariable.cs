using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Models
{
    public class PLCVariable
    {
        public string Name { get; set; } = "";
        public string DataType { get; set; } = "";
        public string Address { get; set; } = "";
        public string Comment { get; set; } = "";
        public string TagLink { get; set; } ="";
        public string RW { get; set; } = "";
        public string POU { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
