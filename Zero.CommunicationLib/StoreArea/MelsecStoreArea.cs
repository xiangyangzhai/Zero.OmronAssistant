using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    /// <summary>
    /// Melsec��Ԫ���洢�����ο�Э���ֲ�125ҳ
    /// </summary>
    public  class MelsecStoreArea
    {
        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="areaBinaryCode"></param>
        /// <param name="areaASCIICode"></param>
        /// <param name="areaType"></param>
        /// <param name="fromBase"></param>
        public MelsecStoreArea(byte areaBinaryCode, string areaASCIICode,byte areaType, int fromBase )
        {
            AreaBinaryCode = areaBinaryCode;
            AreaASCIICode = areaASCIICode;   
            AreaType = areaType;
            FromBase = fromBase;
        }


        /// <summary>
        /// ��Ԫ�������ƴ���
        /// </summary>
        public byte AreaBinaryCode { get; set; } = 0x00;

        /// <summary>
        /// ��Ԫ��ASCII����
        /// </summary>
        public string AreaASCIICode { get; set; }

        /// <summary>
        /// ��Ԫ������
        /// </summary>
        public int FromBase { get; set; }

        /// <summary>
        /// ��Ԫ�����ͣ�0��ʾ�֣�1��ʾλ
        /// </summary>
        public byte AreaType { get; set; } = 0x00;

        /// <summary>
        /// X����̵���
        /// </summary>
        public readonly static MelsecStoreArea X = new MelsecStoreArea(0x9C,"X*",0x01,16);

        /// <summary>
        /// Y����̵���
        /// </summary>
        public readonly static MelsecStoreArea Y = new MelsecStoreArea(0x9D, "Y*", 0x01, 16);

        /// <summary>
        /// X����̵������˽��ƣ�
        /// </summary>
        public readonly static MelsecStoreArea X8 = new MelsecStoreArea(0x9C, "X*", 0x01, 8);

        /// <summary>
        /// Y����̵������˽��ƣ�
        /// </summary>
        public readonly static MelsecStoreArea Y8 = new MelsecStoreArea(0x9D, "Y*", 0x01, 8);

        /// <summary>
        /// M�ڲ��̵���
        /// </summary>
        public readonly static MelsecStoreArea M = new MelsecStoreArea(0x90, "M*", 0x01, 10);

        /// <summary>
        /// L����̵���
        /// </summary>
        public readonly static MelsecStoreArea L = new MelsecStoreArea(0x92, "L*", 0x01, 10);

        /// <summary>
        /// F�����̵���
        /// </summary>
        public readonly static MelsecStoreArea F = new MelsecStoreArea(0x93, "F*", 0x01, 10);

        /// <summary>
        /// V���ؼ̵���
        /// </summary>
        public readonly static MelsecStoreArea V = new MelsecStoreArea(0x94, "V*", 0x01, 10);

        /// <summary>
        /// B���Ӽ̵���
        /// </summary>
        public readonly static MelsecStoreArea B = new MelsecStoreArea(0x95, "B*", 0x01, 10);

        /// <summary>
        /// D���ݼĴ���
        /// </summary>
        public readonly static MelsecStoreArea D = new MelsecStoreArea(0xA8, "D*", 0x00, 10);

        /// <summary>
        /// W���ӼĴ���
        /// </summary>
        public readonly static MelsecStoreArea W = new MelsecStoreArea(0xB4, "W*", 0x00, 16);

        /// <summary>
        /// ��ַ�Ĵ���
        /// </summary>
        public readonly static MelsecStoreArea Z = new MelsecStoreArea(0xCC, "Z*", 0x00, 10);

        /// <summary>
        /// ��ʱ���Ĵ���
        /// </summary>
        public readonly static MelsecStoreArea TN = new MelsecStoreArea(0xC2, "TN", 0x00, 10);

        /// <summary>
        /// �ۼƶ�ʱ���Ĵ���
        /// </summary>
        public readonly static MelsecStoreArea SN = new MelsecStoreArea(0xC8, "SN", 0x00, 10);

        /// <summary>
        /// �������Ĵ���
        /// </summary>
        public readonly static MelsecStoreArea CN = new MelsecStoreArea(0xC5, "CN", 0x00, 10);
    }
}
