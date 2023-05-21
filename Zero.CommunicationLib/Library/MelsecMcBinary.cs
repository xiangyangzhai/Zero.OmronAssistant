using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Base;
using Zero.CommunicationLib.Helper;
using Zero.CommunicationLib.Interface;
using Zero.CommunicationLib.Message;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib.Library
{
    /// <summary>
    /// ����MCЭ��Qna����3E֡�������Ƹ�ʽ
    /// </summary>
    public class MelsecMcBinary : NetDeviceBase
    {
        /// <summary>
        /// ���췽����ʼ���������ֽ���
        /// </summary>
        /// <param name="dataFormat"></param>
        public MelsecMcBinary(DataFormat dataFormat = DataFormat.DCBA)
        {
            this.DataFormat = dataFormat;
        }

        /// <summary>
        /// �����ţ�Ĭ��Ϊ0
        /// </summary>
        public byte NetworkNo { get; set; } = 0x00;

        /// <summary>
        /// ����Ŀ��ģ��վ��
        /// </summary>
        public byte DstModuleNo { get; set; } = 0x00;

        /// <summary>
        /// �Ƿ�ΪFX5U��FX5U��XY�洢��Ϊ8����
        /// </summary>
        public bool IsFx5U { get; set; } = false;

        /// <summary>
        /// ������ȡ����Ԫ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            //��ȡЭ��֡������Э���ʽ����ȡ���ͱ���
            var command = BuildReadMessageFrame(address, length);

            if (!command.IsSuccess) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage netMessage = new MelsecMcBinaryMessage();

            //�������ݽ��������ͱ��Ĳ���ȡ���ر���
            var receive = SendAndReceive(command.Content, ref response, netMessage);

            if (receive.IsSuccess)
            {
                if (response != null && response.Length > 10)
                {
                    //��֤���ر���
                    ushort errorCode = BitConverter.ToUInt16(response, 9);

                    if (errorCode != 0) return OperateResult.CreateFailResult<byte[]>(new OperateResult()
                    {
                        ErrorCode = errorCode,
                    });

                    //���ݽ���
                    return AnalysisResponseMessage(response, command.Content[13] == 0x01);
                }

                return OperateResult.CreateFailResult<byte[]>(new OperateResult(false, "���ر��ĳ���С��11"));
            }
            else
            {
                return OperateResult.CreateFailResult<byte[]>(receive);
            }
        }

        /// <summary>
        /// ������ȡλ��Ԫ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            var analysis = IsValidBitAddress(address);

            if (!analysis.IsSuccess) return OperateResult.CreateFailResult<bool[]>("������ַ������Ч�ĵ�ַ");

            if (!analysis.Content) return OperateResult.CreateFailResult<bool[]>("������ַ����λ��ַ");

            var read = ReadByteArray(address, length);

            if (!read.IsSuccess) return OperateResult.CreateFailResult<bool[]>(read);

            return OperateResult.CreateSuccessResult(read.Content.Select(c => c == 0x01).Take(length).ToArray());
        }

        /// <summary>
        /// ����д������Ԫ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            //��ȡЭ��֡
            var command = BuildWriteMessageFrame(address, value);

            if (!command.IsSuccess) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage netMessage = new MelsecMcBinaryMessage();

            //���ͱ��Ĳ����ձ���
            var receive = SendAndReceive(command.Content, ref response, netMessage);

            if (receive.IsSuccess)
            {
                if (response != null && response.Length > 10)
                {
                    ushort errorCode = BitConverter.ToUInt16(response, 9);

                    if (errorCode != 0)
                    {
                        return new OperateResult()
                        {
                            IsSuccess = false,
                            ErrorCode = errorCode
                        };
                    }
                    return OperateResult.CreateSuccessResult();
                }
                return new OperateResult()
                {
                    IsSuccess = false,
                    Message = "���ձ��ĳ���С��11"
                };
            }
            else
            {
                return OperateResult.CreateFailResult(receive.Message);
            }
        }

        /// <summary>
        /// ����д��λ��Ԫ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteBoolArray(string address, bool[] value)
        {
            var analysis=IsValidBitAddress(address);

            if (!analysis.IsSuccess) return OperateResult.CreateFailResult<bool[]>("������ַ������Ч�ĵ�ַ");

            if(!analysis.Content) return OperateResult.CreateFailResult<bool[]>("������ַ����λ��ַ");

            return WriteByteArray(address, value.Select(c => c ? (byte)0x01 : (byte)0x00).ToArray());
        }

        /// <summary>
        /// ƴ�Ӷ�ȡ����
        /// </summary>
        /// <param name="address">PLC��ַ</param>
        /// <param name="length">����</param>
        /// <returns>ƴ��֮��ı���</returns>
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length)
        {
            var result = MelsecHelper.MelsecAddressAnalysis(address,IsFx5U);

            //�ų���ַ��������ȷ�����
            if (!result.IsSuccess) return OperateResult.CreateFailResult<byte[]>(result);

            //ƴ�ӱ���
            ByteArray sendCommand = new ByteArray();

            //Qͷ��
            //��ͷ��
            sendCommand.Add(0x50, 0x00);

            //������
            sendCommand.Add(NetworkNo);

            //PLC���
            sendCommand.Add(0xFF);

            //����Ŀ��ģ��IO���
            sendCommand.Add(0xFF, 0x03);

            //����Ŀ��ģ��վ��
            sendCommand.Add(DstModuleNo);

            //�������ݳ���
            sendCommand.Add(0x0C, 0x00);

            //CPU���Ӷ�ʱ��
            sendCommand.Add(0x0A, 0x00);

            //ָ��
            sendCommand.Add(0x01, 0x04);

            //��ָ��
            sendCommand.Add(result.Content1.AreaType, 0x00);

            //���󲿷�����
            //��ʼ��Ԫ��
            byte[] startAddress = BitConverter.GetBytes(result.Content2);
            sendCommand.Add(startAddress[0], startAddress[1], startAddress[2]);

            //��Ԫ������
            sendCommand.Add(result.Content1.AreaBinaryCode);

            //��Ԫ������
            sendCommand.Add((byte)(length % 256), (byte)(length / 256));

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }

        /// <summary>
        /// ƴ��д�뱨��
        /// </summary>
        /// <param name="address">PLC��ַ</param>
        /// <param name="length">����</param>
        /// <returns>ƴ��֮��ı���</returns>
        private OperateResult<byte[]> BuildWriteMessageFrame(string address, byte[] value)
        {
            var result = MelsecHelper.MelsecAddressAnalysis(address, IsFx5U);

            //�ų���ַ��������ȷ�����
            if (!result.IsSuccess) return OperateResult.CreateFailResult<byte[]>(result);

            //��д����ֽ�������һ������

            byte[] writeValue = null;

            //��ʾΪ λ��Ԫ��
            if (result.Content1.AreaType == 0x01)
            {
                List<byte> data = new List<byte>();

                int realLength = value.Length % 2 == 0 ? value.Length / 2 : value.Length / 2 + 1;

                for (int i = 0; i < realLength; i++)
                {
                    byte b = 0;

                    b += value[2 * i] != 0x00 ? (byte)0x10 : (byte)0x00;

                    if (value.Length > (2 * i + 1))
                    {
                        b += value[2 * i + 1] != 0x00 ? (byte)0x01 : (byte)0x00;
                    }
                    else
                    {
                        b += 0x00;
                    }
                    data.Add(b);
                }
                writeValue = data.ToArray();
            }
            //��ʾΪ ����Ԫ��
            else
            {
                if (value.Length % 2 != 0)
                {
                    return OperateResult.CreateFailResult<byte[]>("���������д����ֽ����鳤�ȱ���Ϊż��");
                }
                else
                {
                    writeValue = value;
                }
            }

            //ƴ�ӱ���
            ByteArray sendCommand = new ByteArray();

            //Qͷ��
            //��ͷ��
            sendCommand.Add(0x50, 0x00);

            //������
            sendCommand.Add(NetworkNo);

            //PLC���
            sendCommand.Add(0xFF);

            //����Ŀ��ģ��IO���
            sendCommand.Add(0xFF, 0x03);

            //����Ŀ��ģ��վ��
            sendCommand.Add(DstModuleNo);

            //�������ݳ���
            sendCommand.Add((byte)((writeValue.Length + 12) % 256), (byte)((writeValue.Length + 12) / 256));

            //CPU���Ӷ�ʱ��
            sendCommand.Add(0x0A, 0x00);

            //ָ��
            sendCommand.Add(0x01, 0x14);

            //��ָ��
            sendCommand.Add(result.Content1.AreaType, 0x00);

            //���󲿷�����
            //��ʼ��Ԫ��
            byte[] startAddress = BitConverter.GetBytes(result.Content2);
            sendCommand.Add(startAddress[0], startAddress[1], startAddress[2]);

            //��Ԫ������
            sendCommand.Add(result.Content1.AreaBinaryCode);

            //��Ԫ������

            if (result.Content1.AreaType == 0x01)
            {
                sendCommand.Add((byte)(value.Length % 256), (byte)(value.Length / 256));
            }
            else
            {
                sendCommand.Add((byte)(value.Length / 2 % 256), (byte)(value.Length / 2 / 256));
            }

            //��Ԫ������
            sendCommand.Add(writeValue);

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }


        /// <summary>
        /// ������Ӧ����
        /// </summary>
        /// <param name="response"></param>
        /// <param name="isBit"></param>
        /// <returns></returns>
        private OperateResult<byte[]> AnalysisResponseMessage(byte[] response, bool isBit)
        {
            byte[] content = ByteArrayLib.GetByteArrayFromByteArray(response, 11);

            if (isBit)
            {
                List<byte> data = new List<byte>();

                for (int i = 0; i < content.Length; i++)
                {
                    data.Add((content[i] & 0x10) == 0x10 ? (byte)0x01 : (byte)0x00);
                    data.Add((content[i] & 0x01) == 0x01 ? (byte)0x01 : (byte)0x00);
                }
                return OperateResult.CreateSuccessResult(data.ToArray());
            }
            else
            {
                return OperateResult.CreateSuccessResult(content);
            }
        }

        /// <summary>
        /// �жϵ�ַ�ǲ���һ����Ч��λ��ַ
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private OperateResult<bool> IsValidBitAddress(string address)
        {
            var analysis = MelsecHelper.MelsecAddressAnalysis(address);

            if (!analysis.IsSuccess) return OperateResult.CreateFailResult<bool>(analysis);

            return OperateResult.CreateSuccessResult(analysis.Content1.AreaType == 0x01);
        }
    }
}
