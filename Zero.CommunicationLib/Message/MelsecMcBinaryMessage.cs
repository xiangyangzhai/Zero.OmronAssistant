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
        /// 包头的长度
        /// </summary>
        public int HeadDataLength { get; set; } = 9;

        /// <summary>
        /// 包头的内容
        /// </summary>
        public byte[] HeadData { get; set; }

        /// <summary>
        /// 后续报文的内容
        /// </summary>
        public byte[] ContentData { get; set; }

        /// <summary>
        /// 发送的报文
        /// </summary>
        public byte[] SendData { get; set; }

        /// <summary>
        /// 验证HeadData是否正确
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
        /// 获取后续报文的长度
        /// </summary>
        /// <returns></returns>
        public int GetContentLength()
        {
            return BitConverter.ToUInt16(HeadData, 7);
        }
    }
}
