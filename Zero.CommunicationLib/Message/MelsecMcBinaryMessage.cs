using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Interface;

namespace Zero.CommunicationLib.Message
{
    /// <summary>
    /// MelsecMcBinaryMessage
    /// </summary>
    public class MelsecMcBinaryMessage : IMessage
    {
        /// <summary>
        /// ��ͷ�ĳ���
        /// </summary>
        public int HeadDataLength { get; set; } = 9;

        /// <summary>
        /// ��ͷ������
        /// </summary>
        public byte[] HeadData { get; set; }

        /// <summary>
        /// �������ĵ�����
        /// </summary>
        public byte[] ContentData { get; set; }

        /// <summary>
        /// ���͵ı���
        /// </summary>
        public byte[] SendData { get; set; }

        /// <summary>
        /// ��֤HeadData�Ƿ���ȷ
        /// </summary>
        /// <param name="headData"></param>
        /// <returns></returns>
        public bool CheckHeadData(byte[] headData)
        {
            if (headData == null) return false;

            if (headData.Length != HeadDataLength) return false;

           this.HeadData= headData;
            return headData[0] == 0xD0 && headData[1] == 0x00;
        }

        /// <summary>
        /// ��ȡ�������ĵĳ���
        /// </summary>
        /// <returns></returns>
        public int GetContentLength()
        {
            return BitConverter.ToUInt16(HeadData, 7);
        }
    }
}
