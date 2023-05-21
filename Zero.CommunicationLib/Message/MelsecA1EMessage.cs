using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Interface;

namespace Zero.CommunicationLib.Message
{
    /// <summary>
    /// MelsecA1EMessage
    /// </summary>
    public class MelsecA1EMessage : IMessage
    {
        /// <summary>
        /// ��ͷ����
        /// </summary>
        public int HeadDataLength { get; set; } = 2;

        /// <summary>
        /// ��ͷ����
        /// </summary>
        public byte[] HeadData { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public byte[] ContentData { get; set; }

        /// <summary>
        /// ���ͱ���
        /// </summary>
        public byte[] SendData { get; set; }

        /// <summary>
        /// ��֤��ͷ�Ƿ���ȷ
        /// </summary>
        /// <param name="headData"></param>
        /// <returns></returns>
        public bool CheckHeadData(byte[] headData)
        {
            if (headData == null) return false;

            if (headData.Length != HeadDataLength) return false;

            if (headData.Length < 2) return false;

            this.HeadData = headData;
            return true;
        }

        /// <summary>
        /// ��ȡ���ݳ���
        /// </summary>
        /// <returns></returns>
        public int GetContentLength()
        {
            if (HeadData.Length < 2) return 0;

            if (HeadData[1] == 0x58) return 2;

            if (HeadData[1] == 0x00)
            {
                switch (HeadData[0])
                {
                    case 0x80: return SendData[10] != 0x00 ? (SendData[10] + 1) / 2 : 128;              // λ��λ��������
                    case 0x81: return SendData[10] * 2;                                                 // �ֵ�λ��������
                    case 0x82:                                                                          // λ��λ����д��
                    case 0x83: return 0;                                                                // �ֵ�λ����д��
                    default: return 0;
                }
            }
            return 0;
        }
    }
}
