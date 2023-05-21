using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Base;
using Zero.CommunicationLib.Helper;
using Zero.CommunicationLib.Interface;
using Zero.CommunicationLib.Message;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib
{
    public class ModbusRTUOverTCP : NetDeviceBase
    {
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="dataFormat">��С�����ݸ�ʽ</param>
        public ModbusRTUOverTCP(DataFormat dataFormat = DataFormat.ABCD)
        {
            this.DataFormat = dataFormat;
        }

        /// <summary>
        /// ��վ��ַ
        /// </summary>
        public byte DevAddress { get; set; } = 1;

        /// <summary>
        /// �Ƿ�Ϊ�̵�ַģ��
        /// </summary>
        public bool IsShortAddress { get; set; } = true;

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            //1.ƴ�ӱ���
            var command = BuildReadMessageFrame(address, length);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<bool[]>(command);

            //2.���ͱ���
            //3.���ձ���

            byte[] response = null;

            //1.�����ȷ�����س��ȣ��ǾͿ��Բ���д��Ϣ��
            //2.���ֻȷ�����س��ȣ��ǾͿ���д��Ϣ�࣬���ǽ�HeadDataLength=0
            //3.�����Ҫͨ��HeadDataȥȷ�������ĳ��ȣ��ǿ��԰���Э�����ú�HeadDataLength


            IMessage message = new ModbusRTUMessage()
            {
                NumberOfPoints = length,
                FunctionCode = FunctionCode.ReadOutputStatus
            };

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                //4.��֤����
                receive = CheckResponse(response, command.Content[0], true, UShortLib.GetByteLengthFromBoolLength(length));

                if (receive.IsSuccess)
                {
                    //5.��������
                    command = AnalysisResponseMessage(Response, true);

                    if (command.IsSuccess)
                    {
                        return OperateResult.CreateSuccessResult(command.Content.Select(c => c == 0x01).Take(length).ToArray());
                    }
                    else
                    {
                        return OperateResult.CreateFailResult<bool[]>(command);
                    }
                }
                else
                {
                    return OperateResult.CreateFailResult<bool[]>(receive);
                }
            }
            else
            {
                return OperateResult.CreateFailResult<bool[]>(receive);
            }
        }

        /// <summary>
        /// ��ȡ�ֽ�����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            //1.ƴ�ӱ���
            var command = BuildReadMessageFrame(address, length);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(command);

            //2.���ͱ���
            //3.���ձ���

            byte[] response = null;

            IMessage message = new ModbusRTUMessage()
            {
                NumberOfPoints = length,
                FunctionCode = FunctionCode.ReadOutputRegister
            };

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                //4.��֤����
                receive = CheckResponse(response, command.Content[0], true, (ushort)(length * 2));

                if (receive.IsSuccess)
                {
                    //5.��������
                    return AnalysisResponseMessage(Response, false);
                }
                else
                {
                    return OperateResult.CreateFailResult<byte[]>(receive);
                }
            }
            else
            {
                return OperateResult.CreateFailResult<byte[]>(receive);
            }
        }

        /// <summary>
        /// д�벼������
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteBoolArray(string address, bool[] value)
        {
            var command = BuildWriteMessageFrame(address, ByteArrayLib.GetByteArrayFromBoolArray(value), true, (ushort)value.Length);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage message = new ModbusRTUMessage()
            {
                FunctionCode = FunctionCode.ForceMultiCoils
            };

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                receive = CheckResponse(response, command.Content[0], false);

                if (receive.IsSuccess)
                {
                    bool compare = ByteArrayLib.GetByteArrayEquals(response.Take(6).ToArray(), command.Content.Take(6).ToArray());

                    return compare ? OperateResult.CreateSuccessResult() : OperateResult.CreateFailResult("����ǰ6���ֽڶԱȲ�һ��");
                }
                else
                {
                    return OperateResult.CreateFailResult(receive.Message);
                }
            }
            else
            {
                return OperateResult.CreateFailResult(receive.Message);
            }
        }


        /// <summary>
        /// д���ֽ�����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            var command = BuildWriteMessageFrame(address, value, false);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage message = new ModbusRTUMessage()
            {
                FunctionCode = FunctionCode.PreSetMultiRegisters
            };

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                receive = CheckResponse(response, command.Content[0], false);

                if (receive.IsSuccess)
                {
                    bool compare = ByteArrayLib.GetByteArrayEquals(response.Take(6).ToArray(), command.Content.Take(6).ToArray());

                    return compare ? OperateResult.CreateSuccessResult() : OperateResult.CreateFailResult("����ǰ6���ֽڶԱȲ�һ��");
                }
                else
                {
                    return OperateResult.CreateFailResult(receive.Message);
                }
            }
            else
            {
                return OperateResult.CreateFailResult(receive.Message);
            }
        }


        /// <summary>
        /// ƴ�ӱ��ģ��齨��ȡ����֡
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length)
        {
            //������ַ

            var result = ModbusHelper.ModbusAddressAnalysis(address, DevAddress, IsShortAddress);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            //����ByteArray

            ByteArray sendCommand = new ByteArray();

            //��վ��ַ
            sendCommand.Add(result.Content2);

            //������
            sendCommand.Add((byte)result.Content1.ReadFunctionCode);

            //��ʼ��ַ
            sendCommand.Add(result.Content3);

            //����
            sendCommand.Add(length);

            //CRC
            sendCommand.Add(ParityHelper.CalculateCRC(sendCommand.array, sendCommand.Length));

            return OperateResult.CreateSuccessResult(sendCommand.array);

        }



        /// <summary>
        /// ƴ�ӱ��ģ��齨д�뱨��֡
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="isBit"></param>
        /// <param name="coilLength"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildWriteMessageFrame(string address, byte[] value, bool isBit, ushort coilLength = 0)
        {
            //������ַ

            var result = ModbusHelper.ModbusAddressAnalysis(address, DevAddress, IsShortAddress);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            //����ByteArray

            ByteArray sendCommand = new ByteArray();

            //��վ��ַ
            sendCommand.Add(result.Content2);

            //������
            sendCommand.Add((byte)result.Content1.WriteFunctionCode);

            //��ʼ��ַ
            sendCommand.Add(result.Content3);

            //��Ȧ��Ĵ�������
            sendCommand.Add(isBit ? coilLength : (ushort)(value.Length / 2));

            //�ֽ���
            sendCommand.Add((byte)value.Length);

            //д��ֵ
            sendCommand.Add(value);

            //CRC
            sendCommand.Add(ParityHelper.CalculateCRC(sendCommand.array, sendCommand.Length));

            return OperateResult.CreateSuccessResult(sendCommand.array);

        }

        /// <summary>
        /// ��֤����
        /// </summary>
        /// <param name="response"></param>
        /// <param name="devAdd"></param>
        /// <param name="isRead"></param>
        /// <param name="byteLength"></param>
        /// <returns></returns>
        private OperateResult CheckResponse(byte[] response, byte devAdd, bool isRead, ushort byteLength=0)
        {
            int reqLength = isRead ? byteLength + 5 : 8;

            //��֤���س���
            if (response.Length == reqLength)
            {
                //��֤վ��ַ�Ƿ���ȷ
                if (response[0] == devAdd)
                {
                    //��֤CRCУ���Ƿ���ȷ
                    if (ParityHelper.CheckCRC(response))
                    {
                        return OperateResult.CreateSuccessResult();
                    }
                    else
                    {
                        return OperateResult.CreateFailResult("���ر���У����֤��ͨ��");
                    }
                }
                else
                {
                    return OperateResult.CreateFailResult("���ر��Ĵ�վ��ַ��һ�£�" + response[0]);
                }
            }
            else
            {
                return OperateResult.CreateFailResult("���ر��ĳ��Ȳ�����Ҫ��" + response.Length);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="response"></param>
        /// <param name="isBit"></param>
        /// <returns></returns>
        private OperateResult<byte[]> AnalysisResponseMessage(byte[] response, bool isBit)
        {
            byte[] data = ByteArrayLib.GetByteArrayFromByteArray(response, 3, response.Length - 5);

            if (isBit)
            {
                bool[] values = BitLib.GetBitArrayFromByteArray(data);

                return OperateResult.CreateSuccessResult(values.Select(c => c == true ? (byte)0x01 : (byte)0x00).ToArray());
            }
            else
            {
                return OperateResult.CreateSuccessResult(data);
            }
        }
    }
}
