using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Interface;
using Zero.DataConvertLib;

namespace Zero.CommunicationLib.Message
{
    /// <summary>
    /// OmronFinsMessage��Ϣ��
    /// </summary>
    public class OmronFinsMessage : IMessage
    {
        /// <summary>
        /// ��ͷ����
        /// </summary>
        public int HeadDataLength { get; set; } = 8;

        /// <summary>
        /// ��ͷ����
        /// </summary>
        public byte[] HeadData { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        public byte[] ContentData { get; set; }

        /// <summary>
        /// ���ͱ���
        /// </summary>
        public byte[] SendData { get; set; }

        /// <summary>
        /// ��֤��ͷ�Ƿ���������
        /// </summary>
        /// <param name="headData"></param>
        /// <returns></returns>
        public bool CheckHeadData(byte[] headData)
        {
            //����֤�Ƿ�ΪNULL
            if (headData == null) return false;

            //��֤����
            if (headData.Length != HeadDataLength) return false;

            //��֤��ͷ�Ƿ���FINS��ͷ

            if (Encoding.ASCII.GetString(headData, 0, 4).ToUpper() != "FINS") return false;

            this.HeadData = headData;

            return true;
        }

        /// <summary>
        /// ��ȡ�������ĵĳ���
        /// </summary>
        /// <returns></returns>
        public int GetContentLength()
        {
            return IntLib.GetIntFromByteArray(this.HeadData, 4);
        }
    }
}
