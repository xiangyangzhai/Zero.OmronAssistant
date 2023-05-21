using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Base;
using Zero.CommunicationLib.Helper;
using Zero.CommunicationLib.Interface;
using Zero.CommunicationLib.Message;
using Zero.CommunicationLib.StoreArea;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib
{
    /// <summary>
    /// Melsec A1EЭ��
    /// </summary>
    public class MelsecMcA1E : NetDeviceBase
    {
        /// <summary>
        /// ���췽����ʼ���������ֽ���
        /// </summary>
        /// <param name="DataFormat"></param>
        public MelsecMcA1E(DataFormat DataFormat = DataFormat.DCBA)
        {
            this.DataFormat = DataFormat;
        }

        /// <summary>
        /// PLC���
        /// </summary>
        public byte PLCNumber { get; set; } = 0xFF;

        /// <summary>
        /// ��������ݶ����������ȡ��Ԫ��
        /// </summary>
        /// <param name="address">������ַ</param>
        /// <param name="length">����</param>
        /// <returns>������ݶ���</returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            // ��ȡЭ��֡
            var command = BuildReadMessageFrame(address, length);
            if (!command.IsSuccess) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage netMessage = new MelsecA1EMessage()
            {
                SendData = command.Content
            };

            var receive = SendAndReceive(command.Content, ref response, netMessage);

            // �������ݽ���
            if (receive.IsSuccess)
            {
                if (response != null)
                {
                    if (response.Length > 1)
                    {
                        // ���������֤
                        ushort errorCode = response[1];
                        if (errorCode != 0) return OperateResult.CreateFailResult<byte[]>(new OperateResult()
                        {
                            ErrorCode = errorCode
                        });

                        // �������ݽ����������Ƿ�λ����
                        return AnalysisResponseMessage(response, command.Content[0] == 0x00);
                    }
                    else
                    {
                        return OperateResult.CreateFailResult<byte[]>(new OperateResult(false, "���ر��ĳ��Ȳ�������"));
                    }
                }
                else
                {
                    return OperateResult.CreateFailResult<byte[]>(new OperateResult(false, "���ر���Ϊ��"));
                }
            }
            else
            {
                return OperateResult.CreateFailResult<byte[]>(receive);
            }
        }

        /// <summary>
        ///��������ݶ����������ȡλ��Ԫ��
        /// </summary>
        /// <param name="address">������ַ</param>
        /// <param name="length">����</param>
        /// <returns>������ݶ���</returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            // ����λ��ַ��֤
            var analysis = IsValidBitAddress(address);

            if (analysis.IsSuccess && analysis.Content)
            {
                // ���Ľ���
                var read = ReadByteArray(address, length);
                if (!read.IsSuccess) return OperateResult.CreateFailResult<bool[]>(read);

                // ת��bool����
                return OperateResult.CreateSuccessResult(read.Content.Select(c => c == 0x01).Take(length).ToArray());
            }
            else
            {
                return OperateResult.CreateFailResult<bool[]>(new OperateResult(false, "������ַ����λ������ַ"));
            }
        }

        /// <summary>
        /// ��������������д���ֽ�����
        /// </summary>
        /// <param name="address">������ַ</param>
        /// <param name="value">�ֽ�����</param>
        /// <returns>�������</returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            // ��ȡЭ��֡
            OperateResult<byte[]> command = BuildWriteMessageFrame(address, value);
            if (!command.IsSuccess) return command;

            byte[] response = null;

            IMessage netMessage = new MelsecA1EMessage()
            {
                SendData = command.Content
            };

            var receive = SendAndReceive(command.Content, ref response, netMessage);

            // �������ݽ���
            if (receive.IsSuccess)
            {
                if (response != null)
                {
                    if (response.Length > 1)
                    {
                        // ������У��
                        ushort errorCode = response[1];
                        if (errorCode != 0) return OperateResult.CreateFailResult<byte[]>(new OperateResult()
                        {
                            ErrorCode = errorCode
                        });

                        // ���سɹ����
                        return OperateResult.CreateSuccessResult();
                    }
                    else
                    {
                        return new OperateResult(false, "���ر��ĳ��Ȳ�������");
                    }
                }
                else
                {
                    return new OperateResult(false, "���ر���Ϊ��");
                }
            }
            else
            {
                // ����ʧ�ܽ��
                return new OperateResult(false, receive.Message);
            }
        }

        /// <summary>
        /// ��������������д�벼������
        /// </summary>
        /// <param name="address">������ַ</param>
        /// <param name="values">��������</param>
        /// <returns>�������</returns>
        public override OperateResult WriteBoolArray(string address, bool[] values)
        {
            // ����λ��ַ��֤
            var analysis = IsValidBitAddress(address);

            if (analysis.IsSuccess && analysis.Content)
            {
                return WriteByteArray(address, values.Select(c => c ? (byte)0x01 : (byte)0x00).ToArray());
            }
            else
            {
                return new OperateResult(false, "������ַ����λ������ַ");
            }
        }


        /// <summary>
        /// ������ʼ��ַ�����ȣ�ȷ����ȡЭ��֡
        /// </summary>
        /// <param name="address">��ʼ��ַ</param>
        /// <param name="length">����</param>
        /// <returns>Э��֡����</returns>
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length)
        {
            var result = MelsecHelper.MelsecA1EAddressAnalysis(address);
            if (!result.IsSuccess) return OperateResult.CreateFailResult<byte[]>(result);

            ByteArray sendCommand = new ByteArray();

            // ��ͷ����ָ�
            sendCommand.Add(result.Content1.AreaType == 0x01 ? (byte)0x00 : (byte)0x01);
            // �ɱ�̿��������
            sendCommand.Add(PLCNumber);
            // ACPU���Ӷ�ʱ�����ȴ�CPU���ص�ʱ��Ϊ10*250ms=2.5��
            sendCommand.Add(0x0A, 0x00);
            // ��ʼ��Ԫ��
            byte[] startAddress = BitConverter.GetBytes(result.Content2);
            sendCommand.Add(startAddress[0], startAddress[1], startAddress[2], startAddress[3]);
            // ��Ԫ������
            sendCommand.Add(result.Content1.AreaBinaryCode[1], result.Content1.AreaBinaryCode[0]);
            // ��Ԫ������
            sendCommand.Add((byte)(length % 256), (byte)(length / 256));

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }

        /// <summary>
        /// ������ʼ��ַ�����ȣ�ȷ��д��Э��֡
        /// </summary>
        /// <param name="address">��ʼ��ַ</param>
        /// <param name="value">д������</param>
        /// <returns>Э��֡����</returns>
        private OperateResult<byte[]> BuildWriteMessageFrame(string address, byte[] value)
        {
            var result = MelsecHelper.MelsecA1EAddressAnalysis(address);
            if (!result.IsSuccess) return OperateResult.CreateFailResult<byte[]>(result);

            byte[] writeValue = null;

            if (result.Content1.AreaType == 0x01)
            {
                List<byte> data = new List<byte>();

                int realLength = value.Length % 2 == 0 ? value.Length / 2 : value.Length / 2 + 1;

                for (int i = 0; i < realLength; i++)
                {
                    byte b = 0;
                    b += value[2 * i] != 0x00 ? (byte)0x10 : (byte)0x00;

                    //���value.Length��������ʱ�򣬱���Ҫ��������ᱨ��������
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
            else
            {
                if (value.Length % 2 != 0)
                {
                    return OperateResult.CreateFailResult<byte[]>("д���ֽ���������Ϊż��");
                }
                else
                {
                    writeValue = value;
                }
            }

            ByteArray sendCommand = new ByteArray();
            // ��ͷ����ָ�
            sendCommand.Add(result.Content1.AreaType == 0x01 ? (byte)0x02 : (byte)0x03);
            // �ɱ�̿��������
            sendCommand.Add(PLCNumber);
            // ACPU���Ӷ�ʱ�����ȴ�CPU���ص�ʱ��Ϊ10*250ms=2.5��
            sendCommand.Add(0x0A, 0x00);
            // ��ʼ��Ԫ��
            byte[] startAddress = BitConverter.GetBytes(result.Content2);
            sendCommand.Add(startAddress[0], startAddress[1], startAddress[2], startAddress[3]);
            // ��Ԫ������
            sendCommand.Add(result.Content1.AreaBinaryCode[1], result.Content1.AreaBinaryCode[0]);
            // ��Ԫ������������λ��Ҫ����һ��
            //��Ԫ�����������λ�洢�����ִ洢����������ͬ
            if (result.Content1.AreaType == 0x01)
            {
                sendCommand.Add((byte)(value.Length % 256), (byte)(value.Length / 256));
            }
            else
            {
                sendCommand.Add((byte)(value.Length / 2 % 256), (byte)(value.Length / 2 / 256));
            }

            // ��Ԫ������
            sendCommand.Add(writeValue);

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }

        /// <summary>
        /// �������ص���Ӧ����
        /// </summary>
        /// <param name="response">��Ӧ����</param>
        /// <param name="isBit">�Ƿ�Ϊλ��ַ</param>
        /// <returns>������ֵ�Ĳ������</returns>
        private OperateResult<byte[]> AnalysisResponseMessage(byte[] response, bool isBit)
        {
            byte[] content = ByteArrayLib.GetByteArrayFromByteArray(response, 2);

            if (isBit)
            {
                // λ��ȡ
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
                // �ֶ�ȡ
                return OperateResult.CreateSuccessResult(content);
            }
        }

        /// <summary>
        /// �Ƿ�Ϊ��Ч��λ��ַ
        /// </summary>
        /// <param name="address">������ַ</param>
        /// <returns>������ֵ�Ĳ������</returns>
        private OperateResult<bool> IsValidBitAddress(string address)
        {
            // ������ַ����
            OperateResult<MelsecStoreArea, int> analysis = MelsecHelper.MelsecAddressAnalysis(address, false);
            if (!analysis.IsSuccess) return OperateResult.CreateFailResult<bool>(analysis);

            // λ��ַУ��
            return OperateResult.CreateSuccessResult(analysis.Content1.AreaType == 0x01);
        }
    }
}
