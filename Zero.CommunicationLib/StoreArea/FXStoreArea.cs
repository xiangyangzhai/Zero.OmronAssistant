using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    /// <summary>
    /// FXϵ��PLC�Ĵ洢��
    /// </summary>
    public  class FXStoreArea
    {
        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="areaType"></param>
        /// <param name="fromBase"></param>
        public FXStoreArea(byte areaType, int fromBase)
        {
            AreaType = areaType;
            FromBase = fromBase;
        }

        /// <summary>
        /// �洢�����ͣ�0��ʾ�֣�1��ʾλ
        /// </summary>
        public byte AreaType { get; set; } = 0x00;

        /// <summary>
        /// �洢������
        /// </summary>
        public int FromBase { get; set; } = 10;

        /// <summary>
        /// X����̵���
        /// </summary>
        public readonly static FXStoreArea X = new FXStoreArea(0x01, 8);

        /// <summary>
        /// Y����̵���
        /// </summary>
        public readonly static FXStoreArea Y = new FXStoreArea(0x01, 8);

        /// <summary>
        /// M�ڲ��̵���
        /// </summary>
        public readonly static FXStoreArea M = new FXStoreArea(0x01, 10);

        /// <summary>
        /// S�ڲ��̵���
        /// </summary>
        public readonly static FXStoreArea S = new FXStoreArea(0x01, 10);

        /// <summary>
        /// D�ڲ��̵���
        /// </summary>
        public readonly static FXStoreArea D = new FXStoreArea(0x00, 10);

        /// <summary>
        /// TN��ʱ����ǰֵ
        /// </summary>
        public readonly static FXStoreArea TN = new FXStoreArea(0x00, 10);

       /// <summary>
        /// TS��ʱ������
        /// </summary>
        public readonly static FXStoreArea TS = new FXStoreArea(0x01, 10);

        /// <summary>
        /// CN��������ǰֵ
        /// </summary>
        public readonly static FXStoreArea CN = new FXStoreArea(0x00, 10);


        /// <summary>
        /// CS����������
        /// </summary>
        public readonly static FXStoreArea CS = new FXStoreArea(0x01, 10);
    }
}
