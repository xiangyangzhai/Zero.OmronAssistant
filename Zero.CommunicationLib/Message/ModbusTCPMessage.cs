using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Interface;
using Zero.DataConvertLib;

namespace Zero.CommunicationLib.Message
{
    public class ModbusTCPMessage : IMessage
    {
        /// <summary>
        /// ��ͷ����
        /// </summary>
        public int HeadDataLength { get; set; } = 0;
        public byte[] HeadData { get; set; }
        public byte[] ContentData { get; set; }
        public byte[] SendData { get; set; }

        /// <summary>
        /// ���ݵ�����
        /// </summary>
        public int NumberOfPoints { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public FunctionCode FunctionCode { get; set; }


        public bool CheckHeadData(byte[] headData)
        {
            return true;
        }

        public int GetContentLength()
        {
            switch (FunctionCode)
            {
                case FunctionCode.ReadOutputStatus:
                case FunctionCode.ReadInputStatus:
                    return IntLib.GetByteLengthFromBoolLength(NumberOfPoints) + 9;
                case FunctionCode.ReadOutputRegister:
                case FunctionCode.ReadInputRegister:
                    return NumberOfPoints * 2 + 9;
                case FunctionCode.ForceCoil:
                case FunctionCode.PreSetRegister:
                case FunctionCode.ForceMultiCoils:
                case FunctionCode.PreSetMultiRegisters:
                    return 12;
                default:
                    return 0;
            }
        }
    }
}
