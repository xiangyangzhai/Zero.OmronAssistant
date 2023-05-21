using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.Interface
{
    /// <summary>
    /// ��Ϣ�ӿ�
    /// </summary>
    public  interface IMessage
    {
        /// <summary>
        /// ��ͷ����
        /// </summary>
        int HeadDataLength { get; set; }

        /// <summary>
        /// ��ͷ����
        /// </summary>
        byte[] HeadData { get; set; }

        /// <summary>
        /// ���ݱ���
        /// </summary>
        byte[] ContentData { get; set; }

        /// <summary>
        /// ���ͱ���
        /// </summary>
        byte[] SendData { get; set; }


        /// <summary>
        /// ��ȡ���ݳ���
        /// </summary>
        /// <returns>���س���</returns>
        int GetContentLength();

        /// <summary>
        /// ��֤��ͷ�Ƿ���ȷ
        /// </summary>
        /// <param name="headData">��ͷ����</param>
        /// <returns>�Ƿ���ȷ</returns>
        bool CheckHeadData(byte[] headData);

    }
}
