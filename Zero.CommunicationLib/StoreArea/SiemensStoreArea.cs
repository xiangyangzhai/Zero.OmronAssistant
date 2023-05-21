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
        /// DB��
        /// </summary>
        public int DBNO { get; set; } = 0;

        /// <summary>
        /// �洢��
        /// </summary>
        public DataType DataType { get; set; } = DataType.DataBlock;

        /// <summary>
        /// I����洢��
        /// </summary>
        public readonly static SiemensStoreArea I = new SiemensStoreArea(DataType.Input, 0);

        /// <summary>
        /// Q����洢��
        /// </summary>
        public readonly static SiemensStoreArea Q = new SiemensStoreArea(DataType.Output, 0);

        /// <summary>
        /// M�ڲ��洢��
        /// </summary>
        public readonly static SiemensStoreArea M = new SiemensStoreArea(DataType.Memory, 0);

        /// <summary>
        /// V�ڲ��洢��
        /// </summary>
        public readonly static SiemensStoreArea V = new SiemensStoreArea(DataType.DataBlock, 1);

        /// <summary>
        /// DB���ݴ洢��
        /// </summary>
        public readonly static SiemensStoreArea DB = new SiemensStoreArea(DataType.DataBlock, 1);


        /// <summary>
        /// T��ʱ���洢��
        /// </summary>
        public readonly static SiemensStoreArea T = new SiemensStoreArea(DataType.Timer, 0);

        /// <summary>
        /// C�������洢��
        /// </summary>
        public readonly static SiemensStoreArea C = new SiemensStoreArea(DataType.Counter, 0);


    }
}
