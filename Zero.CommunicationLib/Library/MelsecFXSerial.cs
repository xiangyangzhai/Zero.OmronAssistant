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
    /// FX编程口协议通信类
    /// </summary>
    public class MelsecFXSerial : SerialDeviceBase
    {
        /// <summary>
        /// 报文开始标识符
        /// </summary>
        private const byte STX = 0x02;

        /// <summary>
        /// 报文结束标识符
        /// </summary>
        private const byte ETX = 0x03;

        /// <summary>
        /// 正确响应标识符
        /// </summary>
        private const byte ACK = 0x06;


        /// <summary>
        /// 异常响应标识符
        /// </summary>
        private const byte NAK = 0x15;

        /// <summary>
        /// 读取指令
        /// </summary>
        private const byte ReadCMD = 0x30;

        /// <summary>
        /// 写入指令
        /// </summary>
        private const byte WriteCMD = 0x31;

        /// <summary>
        /// 强制True 指令
        /// </summary>
        private const byte ForceON = 0x37;

        /// <summary>
        /// 强制False 指令
        /// </summary>
        private const byte ForceOFF = 0x38;

        /// <summary>
        /// 构造方法初始化
        /// </summary>
        /// <param name="dataFormat"></param>
        public MelsecFXSerial(DataFormat dataFormat = DataFormat.DCBA)
        {
            this.DataFormat = dataFormat;
        }

        /// <summary>
        /// 针对字存储区，批量读取的方法
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            //获取指令
            var command = BuildReadMessageFrameForWord(address, length);

            if (command.IsSuccess == false) return command;

            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response);

            //发送并接收
            if (receive.IsSuccess)
            {
                //验证报文
                if (CheckResponse(response, true).IsSuccess)
                {
                    //解析报文
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
        /// 针对位存储区，批量读取的方法
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            // 拼接报文
            var command = BuildReadMessageFrameForBool(address, length);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<bool[]>(command);

            byte[] response = null;

            // 发送并接收
            var receive = SendAndReceive(command.Content1, ref response);

            if (receive.IsSuccess)
            {
                //验证报文
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
        /// 针对字存储区，批量写入的方法
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            //获取指令
            var command = BuildWriteMessageFrameForWord(address, value);

            if (command.IsSuccess == false) return command;

            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response);

            if (receive.IsSuccess)
            {
                //验证报文
                return CheckResponse(response, false);
            }
            else
            {
                return receive;
            }
        }

        /// <summary>
        /// 针对位存储区，批量写入方法，必须保证地址为M0  M8 这种格式，再者必须保证value的长度是8个整数，否则会覆盖其他位
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
                //获取指令
                var command = BuildWriteMessageFrameForBool(address, value);

                if (command.IsSuccess == false) return command;

                byte[] response = null;

                var receive = SendAndReceive(command.Content, ref response);

                if (receive.IsSuccess)
                {
                    //验证报文
                    return CheckResponse(response, false);
                }
                else
                {
                    return receive;
                }
            }
        }

        /// <summary>
        /// 强制写入单个布尔值
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperateResult ForceBool(string address, bool value)
        {
            //获取指令
            var command = BuildWriteMessageFrameForBool(address, value);

            if (command.IsSuccess == false) return command;

            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response);

            if (receive.IsSuccess)
            {
                //验证报文
                return CheckResponse(response, false);
            }
            else
            {
                return receive;
            }
        }

        /// <summary>
        /// 针对字存储区，读取报文拼接
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
        /// 针对字存储区，写入报文拼接
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildWriteMessageFrameForWord(string address, byte[] value)
        {
            var result = WordAddressHandle(address);
            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            if (value.Length % 2 != 0) return OperateResult.CreateFailResult<byte[]>("写入字节数量必须为偶数");

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
        /// 针对位存储区，读取报文拼接
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[], ushort> BuildReadMessageFrameForBool(string address, ushort length)
        {
            var addressResult = BoolAddressHandle(address);

            if (addressResult.IsSuccess == false) return OperateResult.CreateFailResult<byte[], ushort>(addressResult);

            ushort Address = addressResult.Content1;

            // 位的总数量
            int total = addressResult.Content2 + length;

            // 根据位总数算出涉及的字节数
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
        /// 针对位存储区，写入报文拼接
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildWriteMessageFrameForBool(string address, bool[] value)
        {
            var addressResult = BoolAddressHandle(address);

            if (addressResult.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(addressResult);

            ushort Address = addressResult.Content1;

            //获取ByteLength
            int total = addressResult.Content2 + value.Length;
            byte byteLength = total % 8 == 0 ? (byte)(total / 8) : (byte)(total / 8 + 1);

            //组织写入Data
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
        /// 针对位存储区，强制报文拼接
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
        /// 针对字存储区的地址处理
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private OperateResult<ushort> WordAddressHandle(string address)
        {
            //先解析地址，判断地址是否满足要求
            var result = MelsecHelper.MelsecFXAddressAnalysis(address);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<ushort>(result);

            //判断是否为字存储区
            if (result.Content1.AreaType != 0x00) return OperateResult.CreateFailResult<ushort>("地址不是有效的字存储区");

            //获取存储区类型及存储区地址，进行二次处理

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
                return OperateResult.CreateFailResult<ushort>("地址不是有效的字存储区");
            }

            return OperateResult.CreateSuccessResult(startAddress);
        }

        /// <summary>
        /// 针对位存储区的地址处理
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private OperateResult<ushort, ushort> BoolAddressHandle(string address)
        {
            //先解析地址，判断地址是否满足要求
            var result = MelsecHelper.MelsecFXAddressAnalysis(address);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<ushort, ushort>(result);

            //判断是否为位存储区
            if (result.Content1.AreaType != 0x01) return OperateResult.CreateFailResult<ushort, ushort>("地址不是有效的位存储区");

            //获取存储区类型及存储区地址，进行二次处理

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
                return OperateResult.CreateFailResult<ushort, ushort>("地址不是有效的位存储区");
            }

            return OperateResult.CreateSuccessResult(startAddress, (ushort)(result.Content2 % 8));
        }

        /// <summary>
        /// 针对位存储区强制地址处理
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private OperateResult<ushort> BoolForceAddressHandle(string address)
        {
            var result = MelsecHelper.MelsecFXAddressAnalysis(address);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<ushort>(result);

            //判断是否为位存储区
            if (result.Content1.AreaType != 0x01) return OperateResult.CreateFailResult<ushort>("地址不是有效的位存储区");

            //获取存储区类型及存储区地址，进行二次处理

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
                return OperateResult.CreateFailResult<ushort>("地址不是有效的位存储区");
            }

            return OperateResult.CreateSuccessResult(startAddress);
        }


        /// <summary>
        /// 验证响应报文
        /// </summary>
        /// <param name="response"></param>
        /// <param name="isRead"></param>
        /// <returns></returns>
        private OperateResult CheckResponse(byte[] response, bool isRead = true)
        {
            if (response == null || response.Length == 0) return OperateResult.CreateFailResult("返回报文为空");

            if (isRead)
            {
                if (response[0] == NAK) return OperateResult.CreateFailResult($"返回报文首字节为{NAK}");

                if (response[0] != STX) return OperateResult.CreateFailResult($"返回报文首字节不为{STX}");

                if (ParityHelper.CheckSUM(response) == false) return OperateResult.CreateFailResult($"返回报文校验不通过");

                return OperateResult.CreateSuccessResult();
            }
            else
            {
                if (response[0] != ACK) return OperateResult.CreateFailResult($"返回报文首字节不为{ACK}");

                return OperateResult.CreateSuccessResult();
            }
        }

        /// <summary>
        /// 解析报文
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
                return OperateResult.CreateFailResult<byte[]>("解析出错：" + ex.Message);
            }

        }
    }
}
