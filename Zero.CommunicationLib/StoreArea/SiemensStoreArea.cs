using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    public  class SiemensStoreArea
    {
        public SiemensStoreArea(DataType dataType,int dbNo)
        {
            this.DataType = dataType;
            this.DBNO = dbNo;
        }

        /// <summary>
        /// DB号
        /// </summary>
        public int DBNO { get; set; } = 0;

        /// <summary>
        /// 存储区
        /// </summary>
        public DataType DataType { get; set; } = DataType.DataBlock;

        /// <summary>
        /// I输入存储区
        /// </summary>
        public readonly static SiemensStoreArea I = new SiemensStoreArea(DataType.Input, 0);

        /// <summary>
        /// Q输出存储区
        /// </summary>
        public readonly static SiemensStoreArea Q = new SiemensStoreArea(DataType.Output, 0);

        /// <summary>
        /// M内部存储区
        /// </summary>
        public readonly static SiemensStoreArea M = new SiemensStoreArea(DataType.Memory, 0);

        /// <summary>
        /// V内部存储区
        /// </summary>
        public readonly static SiemensStoreArea V = new SiemensStoreArea(DataType.DataBlock, 1);

        /// <summary>
        /// DB数据存储区
        /// </summary>
        public readonly static SiemensStoreArea DB = new SiemensStoreArea(DataType.DataBlock, 1);


        /// <summary>
        /// T定时器存储区
        /// </summary>
        public readonly static SiemensStoreArea T = new SiemensStoreArea(DataType.Timer, 0);

        /// <summary>
        /// C计数器存储区
        /// </summary>
        public readonly static SiemensStoreArea C = new SiemensStoreArea(DataType.Counter, 0);


    }
}
