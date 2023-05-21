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
    /// 三菱MC协议Qna兼容3E帧，二进制格式
    /// </summary>
    public class MelsecMcBinary : NetDeviceBase
    {
        /// <summary>
        /// 构造方法初始化，传入字节序
        /// </summary>
        /// <param name="dataFormat"></param>
        public MelsecMcBinary(DataFormat dataFormat = DataFormat.DCBA)
        {
            this.DataFormat = dataFormat;
        }

        /// <summary>
        /// 网络编号，默认为0
        /// </summary>
        public byte NetworkNo { get; set; } = 0x00;

        /// <summary>
        /// 请求目标模块站号
        /// </summary>
        public byte DstModuleNo { get; set; } = 0x00;

        /// <summary>
        /// 是否为FX5U，FX5U的XY存储区为8进制
        /// </summary>
        public bool IsFx5U { get; set; } = false;

        /// <summary>
        /// 批量读取字软元件
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            //获取协议帧：根据协议格式，获取发送报文
            var command = BuildReadMessageFrame(address, length);

            if (!command.IsSuccess) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage netMessage = new MelsecMcBinaryMessage();

            //核心数据交互：发送报文并获取返回报文
            var receive = SendAndReceive(command.Content, ref response, netMessage);

            if (receive.IsSuccess)
            {
                if (response != null && response.Length > 10)
                {
                    //验证返回报文
                    ushort errorCode = BitConverter.ToUInt16(response, 9);

                    if (errorCode != 0) return OperateResult.CreateFailResult<byte[]>(new OperateResult()
                    {
                        ErrorCode = errorCode,
                    });

                    //数据解析
                    return AnalysisResponseMessage(response, command.Content[13] == 0x01);
                }

                return OperateResult.CreateFailResult<byte[]>(new OperateResult(false, "返回报文长度小于11"));
            }
            else
            {
                return OperateResult.CreateFailResult<byte[]>(receive);
            }
        }

        /// <summary>
        /// 批量读取位软元件
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            var analysis = IsValidBitAddress(address);

            if (!analysis.IsSuccess) return OperateResult.CreateFailResult<bool[]>("变量地址不是有效的地址");

            if (!analysis.Content) return OperateResult.CreateFailResult<bool[]>("变量地址不是位地址");

            var read = ReadByteArray(address, length);

            if (!read.IsSuccess) return OperateResult.CreateFailResult<bool[]>(read);

            return OperateResult.CreateSuccessResult(read.Content.Select(c => c == 0x01).Take(length).ToArray());
        }

        /// <summary>
        /// 批量写入字软元件
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            //获取协议帧
            var command = BuildWriteMessageFrame(address, value);

            if (!command.IsSuccess) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage netMessage = new MelsecMcBinaryMessage();

            //发送报文并接收报文
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
                    Message = "接收报文长度小于11"
                };
            }
            else
            {
                return OperateResult.CreateFailResult(receive.Message);
            }
        }

        /// <summary>
        /// 批量写入位软元件
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteBoolArray(string address, bool[] value)
        {
            var analysis=IsValidBitAddress(address);

            if (!analysis.IsSuccess) return OperateResult.CreateFailResult<bool[]>("变量地址不是有效的地址");

            if(!analysis.Content) return OperateResult.CreateFailResult<bool[]>("变量地址不是位地址");

            return WriteByteArray(address, value.Select(c => c ? (byte)0x01 : (byte)0x00).ToArray());
        }

        /// <summary>
        /// 拼接读取报文
        /// </summary>
        /// <param name="address">PLC地址</param>
        /// <param name="length">长度</param>
        /// <returns>拼接之后的报文</returns>
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length)
        {
            var result = MelsecHelper.MelsecAddressAnalysis(address,IsFx5U);

            //排除地址解析不正确的情况
            if (!result.IsSuccess) return OperateResult.CreateFailResult<byte[]>(result);

            //拼接报文
            ByteArray sendCommand = new ByteArray();

            //Q头部
            //副头部
            sendCommand.Add(0x50, 0x00);

            //网络编号
            sendCommand.Add(NetworkNo);

            //PLC编号
            sendCommand.Add(0xFF);

            //请求目标模块IO编号
            sendCommand.Add(0xFF, 0x03);

            //请求目标模块站号
            sendCommand.Add(DstModuleNo);

            //请求数据长度
            sendCommand.Add(0x0C, 0x00);

            //CPU监视定时器
            sendCommand.Add(0x0A, 0x00);

            //指令
            sendCommand.Add(0x01, 0x04);

            //子指令
            sendCommand.Add(result.Content1.AreaType, 0x00);

            //请求部分数据
            //起始软元件
            byte[] startAddress = BitConverter.GetBytes(result.Content2);
            sendCommand.Add(startAddress[0], startAddress[1], startAddress[2]);

            //软元件代号
            sendCommand.Add(result.Content1.AreaBinaryCode);

            //软元件点数
            sendCommand.Add((byte)(length % 256), (byte)(length / 256));

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }

        /// <summary>
        /// 拼接写入报文
        /// </summary>
        /// <param name="address">PLC地址</param>
        /// <param name="length">长度</param>
        /// <returns>拼接之后的报文</returns>
        private OperateResult<byte[]> BuildWriteMessageFrame(string address, byte[] value)
        {
            var result = MelsecHelper.MelsecAddressAnalysis(address, IsFx5U);

            //排除地址解析不正确的情况
            if (!result.IsSuccess) return OperateResult.CreateFailResult<byte[]>(result);

            //对写入的字节数组做一定处理

            byte[] writeValue = null;

            //表示为 位软元件
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
            //表示为 字软元件
            else
            {
                if (value.Length % 2 != 0)
                {
                    return OperateResult.CreateFailResult<byte[]>("对字软件，写入的字节数组长度必须为偶数");
                }
                else
                {
                    writeValue = value;
                }
            }

            //拼接报文
            ByteArray sendCommand = new ByteArray();

            //Q头部
            //副头部
            sendCommand.Add(0x50, 0x00);

            //网络编号
            sendCommand.Add(NetworkNo);

            //PLC编号
            sendCommand.Add(0xFF);

            //请求目标模块IO编号
            sendCommand.Add(0xFF, 0x03);

            //请求目标模块站号
            sendCommand.Add(DstModuleNo);

            //请求数据长度
            sendCommand.Add((byte)((writeValue.Length + 12) % 256), (byte)((writeValue.Length + 12) / 256));

            //CPU监视定时器
            sendCommand.Add(0x0A, 0x00);

            //指令
            sendCommand.Add(0x01, 0x14);

            //子指令
            sendCommand.Add(result.Content1.AreaType, 0x00);

            //请求部分数据
            //起始软元件
            byte[] startAddress = BitConverter.GetBytes(result.Content2);
            sendCommand.Add(startAddress[0], startAddress[1], startAddress[2]);

            //软元件代号
            sendCommand.Add(result.Content1.AreaBinaryCode);

            //软元件点数

            if (result.Content1.AreaType == 0x01)
            {
                sendCommand.Add((byte)(value.Length % 256), (byte)(value.Length / 256));
            }
            else
            {
                sendCommand.Add((byte)(value.Length / 2 % 256), (byte)(value.Length / 2 / 256));
            }

            //软元件数据
            sendCommand.Add(writeValue);

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }


        /// <summary>
        /// 解析响应报文
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
        /// 判断地址是不是一个有效的位地址
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
