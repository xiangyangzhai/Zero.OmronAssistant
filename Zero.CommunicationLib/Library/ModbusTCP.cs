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
    public class ModbusTCP : NetDeviceBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dataFormat">大小端数据格式</param>
        public ModbusTCP(DataFormat dataFormat = DataFormat.ABCD)
        {
            this.DataFormat = dataFormat;
        }

        /// <summary>
        /// 从站地址
        /// </summary>
        public byte DevAddress { get; set; } = 1;

        /// <summary>
        /// 是否为短地址模型
        /// </summary>
        public bool IsShortAddress { get; set; } = true;

        //锁对象
        private static readonly object transactionIdLock = new object();

        private ushort transactionId = 0;

        /// <summary>
        /// 事务处理标识符
        /// </summary>
        public ushort TransactionId
        {
            get
            {
                lock (transactionIdLock)
                {
                    return transactionId == ushort.MaxValue ? (ushort)1 : ++transactionId;
                }
            }
            set 
            { 
                transactionId = value; 
            }
        }


        /// <summary>
        /// 读取布尔数组
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            //1.拼接报文
            var command = BuildReadMessageFrame(address, length);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<bool[]>(command);

            //2.发送报文
            //3.接收报文

            byte[] response = null;

            //1.如果不确定返回长度，那就可以不用写消息类
            //2.如果只确定返回长度，那就可以写消息类，但是将HeadDataLength=0
            //3.如果需要通过HeadData去确定后续的长度，那可以按照协议设置好HeadDataLength


            IMessage message = new ModbusTCPMessage()
            {
                NumberOfPoints = length,
                FunctionCode = FunctionCode.ReadOutputStatus
            };

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                //4.验证报文
                receive = CheckResponse(response, command.Content[6], true, UShortLib.GetByteLengthFromBoolLength(length));

                if (receive.IsSuccess)
                {
                    //5.解析报文
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
        /// 读取字节数组
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            //1.拼接报文
            var command = BuildReadMessageFrame(address, length);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(command);

            //2.发送报文
            //3.接收报文

            byte[] response = null;

            IMessage message = new ModbusTCPMessage()
            {
                NumberOfPoints = length,
                FunctionCode = FunctionCode.ReadOutputRegister
            };

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                //4.验证报文
                receive = CheckResponse(response, command.Content[6], true, (ushort)(length * 2));

                if (receive.IsSuccess)
                {
                    //5.解析报文
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
        /// 写入布尔数组
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteBoolArray(string address, bool[] value)
        {
            var command = BuildWriteMessageFrame(address, ByteArrayLib.GetByteArrayFromBoolArray(value), true, (ushort)value.Length);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage message = new ModbusTCPMessage()
            {
                FunctionCode = FunctionCode.ForceMultiCoils
            };

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                receive = CheckResponse(response, command.Content[6], false);

                if (receive.IsSuccess)
                {
                    //验证后6个字节
                    bool compare = ByteArrayLib.GetByteArrayEquals(response.Take(12).Skip(6).ToArray(), command.Content.Take(12).Skip(6).ToArray());

                    //验证前4个字节
                    compare&= ByteArrayLib.GetByteArrayEquals(response.Take(4).ToArray(), command.Content.Take(4).ToArray());

                    return compare ? OperateResult.CreateSuccessResult() : OperateResult.CreateFailResult("报文字节对比不一致");
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
        /// 写入字节数组
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            var command = BuildWriteMessageFrame(address, value, false);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(command);

            byte[] response = null;

            IMessage message = new ModbusTCPMessage()
            {
                FunctionCode = FunctionCode.PreSetMultiRegisters
            };

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                receive = CheckResponse(response, command.Content[6], false);

                if (receive.IsSuccess)
                {
                    //验证后6个字节
                    bool compare = ByteArrayLib.GetByteArrayEquals(response.Take(12).Skip(6).ToArray(), command.Content.Take(12).Skip(6).ToArray());

                    //验证前4个字节
                    compare &= ByteArrayLib.GetByteArrayEquals(response.Take(4).ToArray(), command.Content.Take(4).ToArray());

                    return compare ? OperateResult.CreateSuccessResult() : OperateResult.CreateFailResult("报文字节对比不一致");
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
        /// 拼接报文：组建读取报文帧
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length)
        {
            //解析地址

            var result = ModbusHelper.ModbusAddressAnalysis(address, DevAddress, IsShortAddress);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            //创建ByteArray

            ByteArray sendCommand = new ByteArray();

            //事务处理标识符

            //sendCommand.Add(0x00, 0x00);

            sendCommand.Add(TransactionId);

            //协议标识符
            sendCommand.Add(0x00, 0x00);

            //长度
            sendCommand.Add(0x00, 0x06);

            //单元标识符
            sendCommand.Add(result.Content2);

            //功能码
            sendCommand.Add((byte)result.Content1.ReadFunctionCode);

            //起始地址
            sendCommand.Add(result.Content3);

            //长度
            sendCommand.Add(length);

            return OperateResult.CreateSuccessResult(sendCommand.array);

        }


        /// <summary>
        /// 拼接报文：组建写入报文帧
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="isBit"></param>
        /// <param name="coilLength"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildWriteMessageFrame(string address, byte[] value, bool isBit, ushort coilLength = 0)
        {
            //解析地址

            var result = ModbusHelper.ModbusAddressAnalysis(address, DevAddress, IsShortAddress);

            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            //创建ByteArray

            ByteArray sendCommand = new ByteArray();

            //事务处理标识符
            sendCommand.Add(TransactionId);

            //协议标识符
            sendCommand.Add(0x00, 0x00);

            //长度
            sendCommand.Add((ushort)(7+value.Length));

            //单元标识符
            sendCommand.Add(result.Content2);

            //功能码
            sendCommand.Add((byte)result.Content1.WriteFunctionCode);

            //起始地址
            sendCommand.Add(result.Content3);

            //线圈或寄存器长度
            sendCommand.Add(isBit ? coilLength : (ushort)(value.Length / 2));

            //字节数
            sendCommand.Add((byte)value.Length);

            //写入值
            sendCommand.Add(value);

            return OperateResult.CreateSuccessResult(sendCommand.array);

        }

        /// <summary>
        /// 验证报文
        /// </summary>
        /// <param name="response"></param>
        /// <param name="devAdd"></param>
        /// <param name="isRead"></param>
        /// <param name="byteLength"></param>
        /// <returns></returns>
        private OperateResult CheckResponse(byte[] response, byte devAdd, bool isRead, ushort byteLength = 0)
        {
            int reqLength = isRead ? byteLength + 9 : 12;

            //验证返回长度
            if (response.Length == reqLength)
            {
                //验证站地址是否正确
                if (response[6] == devAdd)
                {
                    return OperateResult.CreateSuccessResult();                  
                }
                else
                {
                    return OperateResult.CreateFailResult("返回报文从站地址不一致：" + response[0]);
                }
            }
            else
            {
                return OperateResult.CreateFailResult("返回报文长度不满足要求：" + response.Length);
            }
        }

        /// <summary>
        /// 解析报文
        /// </summary>
        /// <param name="response"></param>
        /// <param name="isBit"></param>
        /// <returns></returns>
        private OperateResult<byte[]> AnalysisResponseMessage(byte[] response, bool isBit)
        {
            byte[] data = ByteArrayLib.GetByteArrayFromByteArray(response, 9, response.Length - 9);

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
