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
    /// OmronFinsMessage消息类
    /// </summary>
    public class OmronFinsMessage : IMessage
    {
        /// <summary>
        /// 包头长度
        /// </summary>
        public int HeadDataLength { get; set; } = 8;

        /// <summary>
        /// 包头数据
        /// </summary>
        public byte[] HeadData { get; set; }

        /// <summary>
        /// 后续报文数据
        /// </summary>
        public byte[] ContentData { get; set; }

        /// <summary>
        /// 发送报文
        /// </summary>
        public byte[] SendData { get; set; }

        /// <summary>
        /// 验证包头是否满足条件
        /// </summary>
        /// <param name="headData"></param>
        /// <returns></returns>
        public bool CheckHeadData(byte[] headData)
        {
            //先验证是否为NULL
            if (headData == null) return false;

            //验证长度
            if (headData.Length != HeadDataLength) return false;

            //验证包头是否以FINS开头

            if (Encoding.ASCII.GetString(headData, 0, 4).ToUpper() != "FINS") return false;

            this.HeadData = headData;

            return true;
        }

        /// <summary>
        /// 获取后续报文的长度
        /// </summary>
        /// <returns></returns>
        public int GetContentLength()
        {
            return IntLib.GetIntFromByteArray(this.HeadData, 4);
        }
    }
}
