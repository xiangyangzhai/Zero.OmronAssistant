using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Base;
using Zero.CommunicationLib.Helper;
using Zero.CommunicationLib.StoreArea;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib.Library
{
    /// <summary>
    /// FX��̿�Э��ͨ����
    /// </summary>
    public class MelsecFXSerial : SerialDeviceBase
    {
        /// <summary>
        /// ���Ŀ�ʼ��ʶ��
        /// </summary>
        private const byte STX = 0x02;

        /// <summary>
        /// ���Ľ�����ʶ��
        /// </summary>
        private const byte ETX = 0x03;

        /// <summary>
        /// ��ȷ��Ӧ��ʶ��
        /// </summary>
        private const byte ACK = 0x06;


        /// <summary>
        /// �쳣��Ӧ��ʶ��
        /// </summary>
        private const byte NAK = 0x15;

        /// <summary>
        /// ��ȡָ��
        /// </summary>
        private const byte ReadCMD = 0x30;

        /// <summary>
        /// д��ָ��
        /// </summary>
        private const byte WriteCMD = 0x31;

        /// <summary>
        /// ǿ��True ָ��
        /// </summary>
        private const byte ForceON = 0x37;

        /// <summary>
        /// ǿ��False ָ��
        /// </summary>
        private const byte ForceOFF = 0x38;

        /// <summary>
        /// ���췽����ʼ��
        /// </summary>
        /// <param name="dataFormat"></param>
        public MelsecFXSerial(DataFormat dataFormat = DataFormat.DCBA)
        {
            this.DataFormat = dataFormat;
        }

        /// <summary>
        /// ����ִ洢����������ȡ�ķ���
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            //��ȡָ��
            var command = BuildReadMessageFrameForWord(address, length);

            if (command.IsSuccess == false) return command;

            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response);

            //���Ͳ�����
            if (receive.IsSuccess)
            {
                //��֤����
                if (CheckResponse(response, true).IsSuccess)
                {
                    //��������
                    return AnalysisResponseMessage(response);
                }
                else
                {
                    return OperateResult.CreateFailResult<byte[]>(receive);
                }
            }

            return OperateResult.CreateFailResult<byte[]>(receive);
        }

        /// <summary>
        /// ���λ�洢����������ȡ�ķ���
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            // ƴ�ӱ���
            var command = BuildReadMessageFrameForBool(address, length);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<bool[]>(command);

            byte[] response = null;

            // ���Ͳ�����
            var receive = SendAndReceive(command.Content1, ref response);

            if (receive.IsSuccess)
            {
                //��֤����
                receive = CheckResponse(response, true);

                if (receive.IsSuccess)
                {
                    var data = AnalysisResponseMessage(response);

                    if (data.IsSuccess)
                    {
                        return OperateResult.CreateSuccessResult(BitLib.GetBitArrayFromByteArray(data.Content).Skip(command.Content2).Take(length).ToArray());
                    }
                    else
                    {
                        return OperateResult.CreateFailResult<bool[]>(data);
                    }
                }
                else
                {
                    return OperateResult.CreateFailResult<bool[]>(receive);
                }
            }
            return OperateResult.CreateFailResult<bool[]>(receive);
        }


        /// <summary>
        /// ����ִ洢��������д��ķ���
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            //��ȡָ��
            var command = BuildWriteMessageFrameForWord(address, value);

            if (command.IsSuccess == false) return command;

            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response);

            if (receive.IsSuccess)
            {
                //��֤����
                return CheckResponse(response, false);
            }
            else
            {
                return receive;
            }
        }

        /// <summary>
        /// ���λ�洢��������д�뷽�������뱣֤��ַΪM0  M8 ���ָ�ʽ�����߱��뱣֤value�ĳ�����8������������Ḳ������λ
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteBoolArray(string address, bool[] value)
        {
            if (value.Length == 1)
            {
                return ForceBool(address, value[0]);
            }
            else
            {
                //��ȡָ��
                var command = BuildWriteMessageFrameForBool(address, value);

                if (command.IsSuccess == false) return command;

                byte[] response = null;

                var receive = SendAndReceive(command.Content, ref response);

                if (receive.IsSuccess)
                {
                    //��֤����
                    return CheckResponse(response, false);
                }
                else
                {
                    return receive;
                }
            }
        }

        /// <summary>
        /// ǿ��д�뵥������ֵ
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperateResult ForceBool(string address, bool value)
        {
            //��ȡָ��
            var command = BuildWriteMessageFrameForBool(address, value);

            if (command.IsSuccess == false) return command;

            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response);

            if (receive.IsSuccess)
            {
                //��֤����
                return CheckResponse(response, false);
            }
            else
            {
                return receive;
            }
        }

        /// <summary>
        /// ����ִ洢������ȡ����ƴ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildReadMessageFrameForWord(string address, ushort length)
        {
            var result = WordAddressHandle(address);
            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            ushort Address = result.Content;
            ByteArray sendCommand = new ByteArray();

            //STX
            sendCommand.Add(STX);
            //CMD
            sendCommand.Add(ReadCMD);
            //Address
            sendCommand.Add(ByteArrayLib.GetAsciiByteArrayFromValue(Address));
            //ByteLength
            sendCommand.Add(ByteArrayLib.GetAsciiByteArrayFromValue((byte)(length * 2)));
            //ETX
            sendCommand.Add(ETX);
            //SUM
            sendCommand.Add(ParityHelper.CalculateSUM(sendCommand.array));

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }

        /// <summary>
        /// ����ִ洢����д�뱨��ƴ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildWriteMessageFrameForWord(string address, byte[] value)
        {
            var result = WordAddressHandle(address);
            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            if (value.Length % 2 != 0) return OperateResult.CreateFailResult<byte[]>("д���ֽ���������Ϊż��");

            ushort Address = result.Content;
            ByteArray sendCommand = new ByteArray();

            //STX
            sendCommand.Add(STX);
            //CMD
            sendCommand.Add(WriteCMD);
            //Address
            sendCommand.Add(ByteArrayLib.GetAsciiByteArrayFromValue(Address));
            //ByteLength
            sendCommand.Add(ByteArrayLib.GetAsciiByteArrayFromValue((byte)(value.Length)));
            //Data
            sendCommand.Add(ByteArrayLib.GetAsciiBytesFromByteArray(value));
            //ETX
            sendCommand.Add(ETX);
            //SUM
            sendCommand.Add(ParityHelper.CalculateSUM(sendCommand.array));

            return OperateResult.CreateSuccessResult(sendCommand.array);

        }


        /// <summary>
        /// ���λ�洢������ȡ����ƴ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[], ushort> BuildReadMessageFrameForBool(string address, ushort length)
        {
            var addressResult = BoolAddressHandle(address);

            if (addressResult.IsSuccess == false) return OperateResult.CreateFailResult<byte[], ushort>(addressResult);

            ushort Address = addressResult.Content1;

            // λ��������
            int total = addressResult.Content2 + length;

            // ����λ��������漰���ֽ���
            byte byteLength = (total % 8 == 0) ? (byte)(total / 8) : (byte)(total / 8 + 1);

            ByteArray sendCommand = new ByteArray();
            //STX
            sendCommand.Add(STX);
            // Cmd
            sendCommand.Add(ReadCMD);
            // Address
            sendCommand.Add(ByteArrayLib.GetAsciiByteArrayFromValue(Address));
            // ByteLength
            sendCommand.Add(ByteArrayLib.GetAsciiByteArrayFromValue(byteLength));
            // ETX
            sendCommand.Add(ETX);
            // SUM
            sendCommand.Add(ParityHelper.CalculateSUM(sendCommand.array));

            return OperateResult.CreateSuccessResult(sendCommand.array, addressResult.Content2);
        }

        /// <summary>
        /// ���λ�洢����д�뱨��ƴ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildWriteMessageFrameForBool(string address, bool[] value)
        {
            var addressResult = BoolAddressHandle(address);

            if (addressResult.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(addressResult);

            ushort Address = addressResult.Content1;

            //��ȡByteLength
            int total = addressResult.Content2 + value.Length;
            byte byteLength = total % 8 == 0 ? (byte)(total / 8) : (byte)(total / 8 + 1);

            //��֯д��Data
            bool[] preBools = new bool[addressResult.Content2];

            List<bool> writeBools = new List<bool>();
            writeBools.AddRange(preBools);
            writeBools.AddRange(value);

            ByteArray sendCommand = new ByteArray();

            //STX
            sendCommand.Add(STX);
            //CMD
            sendCommand.Add(WriteCMD);

            //Address
            sendCommand.Add(ByteArrayLib.GetAsciiByteArrayFromValue(Address));

            //ByteLength
            sendCommand.Add(ByteArrayLib.GetAsciiByteArrayFromValue(byteLength));

            //Data
            sendCommand.Add(ByteArrayLib.GetAsciiBytesFromByteArray(ByteArrayLib.GetByteArrayFromBoolArray(writeBools.ToArray())));

            //ETX
            sendCommand.Add(ETX);

            //SUM
            sendCommand.Add(ParityHelper.CalculateSUM(sendCommand.array));

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }

        /// <summary>
        /// ���λ�洢����ǿ�Ʊ���ƴ��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperateResult<byte[]> BuildWriteMessageFrameForBool(string address, bool value)
        {
            var addressResult = BoolForceAddressHandle(address);

            if (addressResult.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(addressResult);

            ushort Address = addressResult.Content;

            ByteArray sendCommand = new ByteArray();

            //STX
            sendCommand.Add(STX);

            //CMD
            sendCommand.Add(value ? ForceON : ForceOFF);

            //Address
            byte[] AddressTemp = ByteArrayLib.GetAsciiByteArrayFromValue(Address);

            sendCommand.Add(AddressTemp[2]);
            sendCommand.Add(AddressTemp[3]);
            sendCommand.Add(AddressTemp[0]);
            sendCommand.Add(AddressTemp[1]);

            //EXT
            sendCommand.Add(ETX);

            //SUM
            sendCommand.Add(ParityHelper.CalculateSUM(sendCommand.array));

            return OperateResult.CreateSuccessResult(sendCommand.array);

        }


        /// <summary>
        /// ����ִ洢���ĵ�ַ����
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private OperateResult<ushort> WordAddressHandle(string address)
        {
            //�Ƚ�����ַ���жϵ�ַ�Ƿ�����Ҫ��
            var result = MelsecHelper.MelsecFXAddressAnalysis(address);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<ushort>(result);

            //�ж��Ƿ�Ϊ�ִ洢��
            if (result.Content1.AreaType != 0x00) return OperateResult.CreateFailResult<ushort>("��ַ������Ч���ִ洢��");

            //��ȡ�洢�����ͼ��洢����ַ�����ж��δ���

            ushort startAddress = result.Content2;

            if (result.Content1 == FXStoreArea.D)
            {
                if (result.Content2 >= 8000)
                {
                    startAddress = (ushort)((startAddress - 8000) * 2 + 0x0E00);
                }
                else
                {
                    startAddress = (ushort)(startAddress * 2 + 0x1000);
                }
            }
            else if (result.Content1 == FXStoreArea.TN)
            {
                startAddress = (ushort)(startAddress * 2 + 0x0800);
            }
            else if (result.Content1 == FXStoreArea.CN)
            {
                if (result.Content2 > 200)
                {
                    startAddress = (ushort)((startAddress - 200) * 4 + 0x0C00);
                }
                else
                {
                    startAddress = (ushort)(startAddress * 2 + 0x0A00);
                }
            }
            else
            {
                return OperateResult.CreateFailResult<ushort>("��ַ������Ч���ִ洢��");
            }

            return OperateResult.CreateSuccessResult(startAddress);
        }

        /// <summary>
        /// ���λ�洢���ĵ�ַ����
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private OperateResult<ushort, ushort> BoolAddressHandle(string address)
        {
            //�Ƚ�����ַ���жϵ�ַ�Ƿ�����Ҫ��
            var result = MelsecHelper.MelsecFXAddressAnalysis(address);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<ushort, ushort>(result);

            //�ж��Ƿ�Ϊλ�洢��
            if (result.Content1.AreaType != 0x01) return OperateResult.CreateFailResult<ushort, ushort>("��ַ������Ч��λ�洢��");

            //��ȡ�洢�����ͼ��洢����ַ�����ж��δ���

            ushort startAddress = result.Content2;

            if (result.Content1 == FXStoreArea.M)
            {
                if (startAddress >= 8000)
                {
                    startAddress = (ushort)((startAddress - 8000) / 8 + 0x01E0);
                }
                else
                {
                    startAddress = (ushort)(startAddress / 8 + 0x0100);
                }
            }

            else if (result.Content1 == FXStoreArea.X)
            {
                startAddress = (ushort)(startAddress / 8 + 0x0080);
            }
            else if (result.Content1 == FXStoreArea.Y)
            {
                startAddress = (ushort)(startAddress / 8 + 0x00A0);
            }
            else if (result.Content1 == FXStoreArea.S)
            {
                startAddress = (ushort)(startAddress / 8);
            }
            else if (result.Content1 == FXStoreArea.TS)
            {
                startAddress = (ushort)(startAddress / 8 + 0x00C0);
            }
            else if (result.Content1 == FXStoreArea.CS)
            {
                startAddress = (ushort)(startAddress / 8 + 0x01C0);
            }
            else
            {
                return OperateResult.CreateFailResult<ushort, ushort>("��ַ������Ч��λ�洢��");
            }

            return OperateResult.CreateSuccessResult(startAddress, (ushort)(result.Content2 % 8));
        }

        /// <summary>
        /// ���λ�洢��ǿ�Ƶ�ַ����
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private OperateResult<ushort> BoolForceAddressHandle(string address)
        {
            var result = MelsecHelper.MelsecFXAddressAnalysis(address);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<ushort>(result);

            //�ж��Ƿ�Ϊλ�洢��
            if (result.Content1.AreaType != 0x01) return OperateResult.CreateFailResult<ushort>("��ַ������Ч��λ�洢��");

            //��ȡ�洢�����ͼ��洢����ַ�����ж��δ���

            ushort startAddress = result.Content2;

            if (result.Content1 == FXStoreArea.M)
            {
                if (startAddress >= 8000)
                {
                    startAddress = (ushort)((startAddress - 8000) + 0x0F00);
                }
                else
                {
                    startAddress = (ushort)(startAddress + 0x0800);
                }
            }
            else if (result.Content1 == FXStoreArea.X)
            {
                startAddress = (ushort)(startAddress + 0x0400);
            }
            else if (result.Content1 == FXStoreArea.Y)
            {
                startAddress = (ushort)(startAddress + 0x0500);
            }
            else if (result.Content1 == FXStoreArea.S)
            {
                startAddress = (ushort)(startAddress + 0x0000);
            }
            else if (result.Content1 == FXStoreArea.CS)
            {
                startAddress = (ushort)(startAddress + 0x0E00);
            }
            else if (result.Content1 == FXStoreArea.TS)
            {
                startAddress = (ushort)(startAddress + 0x0600);
            }
            else
            {
                return OperateResult.CreateFailResult<ushort>("��ַ������Ч��λ�洢��");
            }

            return OperateResult.CreateSuccessResult(startAddress);
        }


        /// <summary>
        /// ��֤��Ӧ����
        /// </summary>
        /// <param name="response"></param>
        /// <param name="isRead"></param>
        /// <returns></returns>
        private OperateResult CheckResponse(byte[] response, bool isRead = true)
        {
            if (response == null || response.Length == 0) return OperateResult.CreateFailResult("���ر���Ϊ��");

            if (isRead)
            {
                if (response[0] == NAK) return OperateResult.CreateFailResult($"���ر������ֽ�Ϊ{NAK}");

                if (response[0] != STX) return OperateResult.CreateFailResult($"���ر������ֽڲ�Ϊ{STX}");

                if (ParityHelper.CheckSUM(response) == false) return OperateResult.CreateFailResult($"���ر���У�鲻ͨ��");

                return OperateResult.CreateSuccessResult();
            }
            else
            {
                if (response[0] != ACK) return OperateResult.CreateFailResult($"���ر������ֽڲ�Ϊ{ACK}");

                return OperateResult.CreateSuccessResult();
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private OperateResult<byte[]> AnalysisResponseMessage(byte[] response)
        {
            try
            {
                byte[] data = new byte[(response.Length - 4) / 2];

                for (int i = 0; i < data.Length; i++)
                {
                    byte[] buffer = new byte[2];
                    buffer[0] = response[2 * i + 1];
                    buffer[1] = response[2 * i + 2];

                    data[i] = Convert.ToByte(Encoding.ASCII.GetString(buffer), 16);
                }

                return OperateResult.CreateSuccessResult(data);
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult<byte[]>("��������" + ex.Message);
            }

        }
    }
}
