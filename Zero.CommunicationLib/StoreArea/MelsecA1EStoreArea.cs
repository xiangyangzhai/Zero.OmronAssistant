using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    /// <summary>
    /// Melsec��Ԫ���洢�����ο��ֲ�125ҳ
    /// </summary>
    public class MelsecA1EStoreArea
    {
        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="areaBinaryCode">�����ƴ���</param>
        /// <param name="areaASCIICode">ASCII����</param>
        /// <param name="areaType">����</param>
        /// <param name="fromBase">����</param>
        public MelsecA1EStoreArea(byte[] areaBinaryCode, string areaASCIICode, byte areaType, int fromBase)
        {
            AreaBinaryCode = areaBinaryCode;
            AreaASCIICode = areaASCIICode;
            AreaType = areaType;
            FromBase = fromBase;
        }

        /// <summary>
        /// �����ƴ���
        /// </summary>
        public byte[] AreaBinaryCode { get; set; } = { 0x00, 0x00 };

        /// <summary>
        /// ASCII����
        /// </summary>
        public string AreaASCIICode { get; set; }

        /// <summary>
        /// ���ͣ�λ/�֣�0�����֣�1����λ
        /// </summary>
        public byte AreaType { get; set; } = 0x00;

        /// <summary>
        /// ����
        /// </summary>
        public int FromBase { get; set; }

        /// <summary>
        /// X����̵���
        /// </summary>
        public readonly static MelsecA1EStoreArea X = new MelsecA1EStoreArea(new byte[] { 0x58, 0x20 }, "X*", 0x01, 8);
        /// <summary>
        /// Y����̵���
        /// </summary>
        public readonly static MelsecA1EStoreArea Y = new MelsecA1EStoreArea(new byte[] { 0x59, 0x20 }, "Y*", 0x01, 8);
        /// <summary>
        /// M�ڲ��̵���
        /// </summary>
        public readonly static MelsecA1EStoreArea M = new MelsecA1EStoreArea(new byte[] { 0x4D, 0x20 }, "M*", 0x01, 10);
        /// <summary>
        /// W���Ӽ̵���
        /// </summary>
        public readonly static MelsecA1EStoreArea W = new MelsecA1EStoreArea(new byte[] { 0x57, 0x20 }, "W*", 0x01, 16);
        /// <summary>
        /// D���ݼĴ���
        /// </summary>
        public readonly static MelsecA1EStoreArea D = new MelsecA1EStoreArea(new byte[] { 0x44, 0x20 }, "D*", 0x00, 10);


    }
}
