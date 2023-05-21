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
    /// Melsec A1E协议
    /// </summary>
    public class MelsecMcA1E : NetDeviceBase
    {
        /// <summary>
        /// 构造方法初始化，传入字节序
        /// </summary>
        /// <param name="DataFormat"></param>
        public MelsecMcA1E(DataFormat DataFormat = DataFormat.DCBA)
        {
            this.DataFormat = DataFormat;
        }

        /// <summary>
        /// PLC编号
        /// </summary>
        public byte PLCNumber { get; set; } = 0xFF;

        /// <summary>
        /// 带结果数据对象的批量读取软元件
        /// </summary>
        /// <param name="address">变量地址</param>
        /// <param name="length">长度</param>
        /// <returns>结果数据对象</returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            // 获取协议帧
            var command = BuildReadMessageFrame(address, length);
            if (!command.IsSuccess) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage netMessage = new MelsecA1EMessage()
            {
                SendData = command.Content
            };

            var receive = SendAndReceive(command.Content, ref response, netMessage);

            // 核心数据交互
            if (receive.IsSuccess)
            {
                if (response != null)
                {
                    if (response.Length > 1)
                    {
                        // 错误代码验证
                        ushort errorCode = response[1];
                        if (errorCode != 0) return OperateResult.CreateFailResult<byte[]>(new OperateResult()
                        {
                            ErrorCode = errorCode
                        });

                        // 返回数据解析，传入是否位参数
                        return AnalysisResponseMessage(response, command.Content[0] == 0x00);
                    }
                    else
                    {
                        return OperateResult.CreateFailResult<byte[]>(new OperateResult(false, "返回报文长度不够解析"));
                    }
                }
                else
                {
                    return OperateResult.CreateFailResult<byte[]>(new OperateResult(false, "返回报文为空"));
                }
            }
            else
            {
                return OperateResult.CreateFailResult<byte[]>(receive);
            }
        }

        /// <summary>
        ///带结果数据对象的批量读取位软元件
        /// </summary>
        /// <param name="address">变量地址</param>
        /// <param name="length">长度</param>
        /// <returns>结果数据对象</returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            // 变量位地址验证
            var analysis = IsValidBitAddress(address);

            if (analysis.IsSuccess && analysis.Content)
            {
                // 核心交互
                var read = ReadByteArray(address, length);
                if (!read.IsSuccess) return OperateResult.CreateFailResult<bool[]>(read);

                // 转化bool数组
                return OperateResult.CreateSuccessResult(read.Content.Select(c => c == 0x01).Take(length).ToArray());
            }
            else
            {
                return OperateResult.CreateFailResult<bool[]>(new OperateResult(false, "变量地址不是位变量地址"));
            }
        }

        /// <summary>
        /// 带结果对象的批量写入字节数组
        /// </summary>
        /// <param name="address">变量地址</param>
        /// <param name="value">字节数组</param>
        /// <returns>结果对象</returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            // 获取协议帧
            OperateResult<byte[]> command = BuildWriteMessageFrame(address, value);
            if (!command.IsSuccess) return command;

            byte[] response = null;

            IMessage netMessage = new MelsecA1EMessage()
            {
                SendData = command.Content
            };

            var receive = SendAndReceive(command.Content, ref response, netMessage);

            // 核心数据交互
            if (receive.IsSuccess)
            {
                if (response != null)
                {
                    if (response.Length > 1)
                    {
                        // 错误码校验
                        ushort errorCode = response[1];
                        if (errorCode != 0) return OperateResult.CreateFailResult<byte[]>(new OperateResult()
                        {
                            ErrorCode = errorCode
                        });

                        // 返回成功结果
                        return OperateResult.CreateSuccessResult();
                    }
                    else
                    {
                        return new OperateResult(false, "返回报文长度不够解析");
                    }
                }
                else
                {
                    return new OperateResult(false, "返回报文为空");
                }
            }
            else
            {
                // 返回失败结果
                return new OperateResult(false, receive.Message);
            }
        }

        /// <summary>
        /// 带结果对象的批量写入布尔数组
        /// </summary>
        /// <param name="address">变量地址</param>
        /// <param name="values">布尔数组</param>
        /// <returns>结果对象</returns>
        public override OperateResult WriteBoolArray(string address, bool[] values)
        {
            // 变量位地址验证
            var analysis = IsValidBitAddress(address);

            if (analysis.IsSuccess && analysis.Content)
            {
                return WriteByteArray(address, values.Select(c => c ? (byte)0x01 : (byte)0x00).ToArray());
            }
            else
            {
                return new OperateResult(false, "变量地址不是位变量地址");
            }
        }


        /// <summary>
        /// 根据起始地址及长度，确定读取协议帧
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">长度</param>
        /// <returns>协议帧数据</returns>
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length)
        {
            var result = MelsecHelper.MelsecA1EAddressAnalysis(address);
            if (!result.IsSuccess) return OperateResult.CreateFailResult<byte[]>(result);

            ByteArray sendCommand = new ByteArray();

            // 副头部（指令）
            sendCommand.Add(result.Content1.AreaType == 0x01 ? (byte)0x00 : (byte)0x01);
            // 可编程控制器编号
            sendCommand.Add(PLCNumber);
            // ACPU监视定时器：等待CPU返回的时间为10*250ms=2.5秒
            sendCommand.Add(0x0A, 0x00);
            // 起始软元件
            byte[] startAddress = BitConverter.GetBytes(result.Content2);
            sendCommand.Add(startAddress[0], startAddress[1], startAddress[2], startAddress[3]);
            // 软元件代码
            sendCommand.Add(result.Content1.AreaBinaryCode[1], result.Content1.AreaBinaryCode[0]);
            // 软元件点数
            sendCommand.Add((byte)(length % 256), (byte)(length / 256));

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }

        /// <summary>
        /// 根据起始地址及长度，确定写入协议帧
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">写入数据</param>
        /// <returns>协议帧数据</returns>
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

                    //如果value.Length是奇数的时候，必须要处理，否则会报索引超限
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
                    return OperateResult.CreateFailResult<byte[]>("写入字节数量必须为偶数");
                }
                else
                {
                    writeValue = value;
                }
            }

            ByteArray sendCommand = new ByteArray();
            // 副头部（指令）
            sendCommand.Add(result.Content1.AreaType == 0x01 ? (byte)0x02 : (byte)0x03);
            // 可编程控制器编号
            sendCommand.Add(PLCNumber);
            // ACPU监视定时器：等待CPU返回的时间为10*250ms=2.5秒
            sendCommand.Add(0x0A, 0x00);
            // 起始软元件
            byte[] startAddress = BitConverter.GetBytes(result.Content2);
            sendCommand.Add(startAddress[0], startAddress[1], startAddress[2], startAddress[3]);
            // 软元件代码
            sendCommand.Add(result.Content1.AreaBinaryCode[1], result.Content1.AreaBinaryCode[0]);
            // 软元件点数，对于位，要处理一下
            //软元件点数：针对位存储区或字存储区会有所不同
            if (result.Content1.AreaType == 0x01)
            {
                sendCommand.Add((byte)(value.Length % 256), (byte)(value.Length / 256));
            }
            else
            {
                sendCommand.Add((byte)(value.Length / 2 % 256), (byte)(value.Length / 2 / 256));
            }

            // 软元件数据
            sendCommand.Add(writeValue);

            return OperateResult.CreateSuccessResult(sendCommand.array);
        }

        /// <summary>
        /// 解析返回的响应报文
        /// </summary>
        /// <param name="response">响应报文</param>
        /// <param name="isBit">是否为位地址</param>
        /// <returns>带返回值的操作结果</returns>
        private OperateResult<byte[]> AnalysisResponseMessage(byte[] response, bool isBit)
        {
            byte[] content = ByteArrayLib.GetByteArrayFromByteArray(response, 2);

            if (isBit)
            {
                // 位读取
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
                // 字读取
                return OperateResult.CreateSuccessResult(content);
            }
        }

        /// <summary>
        /// 是否为有效的位地址
        /// </summary>
        /// <param name="address">变量地址</param>
        /// <returns>带返回值的操作结果</returns>
        private OperateResult<bool> IsValidBitAddress(string address)
        {
            // 变量地址解析
            OperateResult<MelsecStoreArea, int> analysis = MelsecHelper.MelsecAddressAnalysis(address, false);
            if (!analysis.IsSuccess) return OperateResult.CreateFailResult<bool>(analysis);

            // 位地址校验
            return OperateResult.CreateSuccessResult(analysis.Content1.AreaType == 0x01);
        }
    }
}
