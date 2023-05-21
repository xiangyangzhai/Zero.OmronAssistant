using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Base;
using Zero.CommunicationLib.Helper;
using Zero.CommunicationLib.Message;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib.Library
{
    public class OmronFinsTCP : NetDeviceBase
    {
        /// <summary>
        /// 构造方法，欧姆龙PLC的大小端默认是CDAB
        /// </summary>
        /// <param name="dataFormat"></param>
        public OmronFinsTCP(DataFormat dataFormat = DataFormat.CDAB)
        {
            this.DataFormat = dataFormat;
        }

        /// <summary>
        /// FinsHeader
        /// </summary>
        private byte[] FinsHeader = new byte[] { 0X46, 0x49, 0x4E, 0x53 };

        /// <summary>
        /// 格式为 1?00000?，其中 Bit0：0 表示需要回复，1 表示不需要回复；
        /// Bit7：0 表示命令，1 表示响应
        /// </summary>
        private byte ICF = 0x80;

        /// <summary>
        /// 备用值
        /// </summary>
        private byte RSV = 0x00;

        /// <summary>
        /// 网络层信息 
        /// </summary>
        private byte GCT = 0x02;

        /// <summary>
        /// 目标网络号
        /// </summary>
        public byte DNA { get; set; } = 0x00;

        /// <summary>
        /// 目标节点号
        /// </summary>
        public byte DA1 { get; set; } = 0x00;

        /// <summary>
        /// 目标单元号
        /// </summary>
        public byte DA2 { get; set; } = 0x00;

        /// <summary>
        /// 源网络号
        /// </summary>
        public byte SNA { get; set; } = 0x00;

        /// <summary>
        /// 源节点号
        /// </summary>
        public byte SA1 { get; set; } = 0x00;

        /// <summary>
        /// 源单元号
        /// </summary>
        public byte SA2 { get; set; } = 0x00;

        /// <summary>
        /// 服务ID
        /// </summary>
        public byte SID { get; set; } = 0x00;

        /// <summary>
        /// 握手报文
        /// </summary>
        private readonly byte[] HandMessage = new byte[]
        {
            0x46,0x49,0x4E,0x53,  // FINS
            0x00,0x00,0x00,0x0C, // 长度 Length
            0x00,0x00,0x00,0x00, // 命名码 Command
            0x00,0x00,0x00,0x00, // 错误码 Error Code
            0x00,0x00,0x00,0x00  // PC节点号
        };


        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="iporhost"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public override bool Connect(string iporhost, int port)
        {
            bool connected = base.Connect(iporhost, port);

            if (connected == false) return false;

            byte[] response = null;

            var receive = SendAndReceive(HandMessage, ref response);

            if (receive.IsSuccess && response.Length == 24)
            {
                int errorCode = IntLib.GetIntFromByteArray(response, 12);

                if (errorCode == 0)
                {
                    SA1 = response[19];
                    DA1= response[23];
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 读取字节数组的方法
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            //拼接报文
            var command = BuildReadMessageFrame(address, length, false);

            if (command.IsSuccess == false) return command;

            //发送报文  接收报文
            byte[] response = null;
            var message = new OmronFinsMessage();

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                //验证报文 解析报文
                return AnalysisReadResponseMessage(response);
            }
            else
            {
                return OperateResult.CreateFailResult<byte[]>(receive);
            }
        }


        /// <summary>
        /// 读取布尔数组的方法
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            //拼接报文
            var command = BuildReadMessageFrame(address, length, true);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<bool[]>( command);

            //发送报文  接收报文
            byte[] response = null;
            var message = new OmronFinsMessage();

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                //验证报文 解析报文
                var data= AnalysisReadResponseMessage(response);

                if (data.IsSuccess == false) return OperateResult.CreateFailResult<bool[]>(data);

                return OperateResult.CreateSuccessResult(data.Content.Select(c => c != 0x00 ? true : false).ToArray());
            }
            else
            {
                return OperateResult.CreateFailResult<bool[]>(receive);
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
            //拼接报文
            var command = BuildWriteMessageFrame(address, value, false);

            if (command.IsSuccess == false) return command;

            //发送报文  接收报文
            byte[] response = null;
            var message = new OmronFinsMessage();

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                return AnalysisWriteResponseMessage(response);
            }
            else
            {
                return receive;
            }
        }

        /// <summary>
        /// 写入布尔数组的方法
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteBoolArray(string address, bool[] value)
        {
            //拼接报文
            var command = BuildWriteMessageFrame(address, value.Select(c=>c?(byte)0x01: (byte)0x00).ToArray(), true);

            if (command.IsSuccess == false) return command;

            //发送报文  接收报文
            byte[] response = null;
            var message = new OmronFinsMessage();

            var receive = SendAndReceive(command.Content, ref response, message);

            if (receive.IsSuccess)
            {
                return AnalysisWriteResponseMessage(response);
            }
            else
            {
                return receive;
            }
        }


        /// <summary>
        /// 获取读取报文
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length,bool isBit)
        {
            var result = OmronHelper.OmronFinsAnalysisAddress(address, isBit);

            //如果地址解析失败
            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            ByteArray SendCommand = new ByteArray();

            //Header
            SendCommand.Add(FinsHeader);

            //Length
            SendCommand.Add(new byte[] { 0x00, 0x00, 0x00, 0x1A });

            //Command
            SendCommand.Add(new byte[] { 0x00, 0x00, 0x00, 0x02 });

            //ErrorCode
            SendCommand.Add(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            //ICF RSV GCT
            SendCommand.Add(new byte[] { ICF, RSV, GCT });

            //DNA DA1 DA2
            SendCommand.Add(new byte[] { DNA, DA1, DA2 });

            //SNA SA1 SA2 SID
            SendCommand.Add(new byte[] { SNA, SA1, SA2,SID });

            //MRC SRC
            SendCommand.Add(new byte[] { 0x01, 0x01 });

            //AREA
            SendCommand.Add(isBit ? result.Content1.BitCode : result.Content1.WordCode);

            //Address
            SendCommand.Add(result.Content2);

            //Length
            SendCommand.Add(new byte[] { (byte)(length / 256), (byte)(length % 256) });

            return OperateResult.CreateSuccessResult(SendCommand.array);
        }


        /// <summary>
        /// 获取写入报文
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildWriteMessageFrame(string address, byte[] value, bool isBit)
        {
            var result = OmronHelper.OmronFinsAnalysisAddress(address, isBit);

            //如果地址解析失败
            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            ByteArray SendCommand = new ByteArray();

            //Header
            SendCommand.Add(FinsHeader);

            //Length
            SendCommand.Add( ByteArrayLib.GetByteArrayFromInt( value.Length+26));

            //Command
            SendCommand.Add(new byte[] { 0x00, 0x00, 0x00, 0x02 });

            //ErrorCode
            SendCommand.Add(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            //ICF RSV GCT
            SendCommand.Add(new byte[] { ICF, RSV, GCT });

            //DNA DA1 DA2
            SendCommand.Add(new byte[] { DNA, DA1, DA2 });

            //SNA SA1 SA2 SID
            SendCommand.Add(new byte[] { SNA, SA1, SA2 ,SID});

            //MRC SRC
            SendCommand.Add(new byte[] { 0x01, 0x02 });

            //AREA
            SendCommand.Add(isBit ? result.Content1.BitCode : result.Content1.WordCode);

            //Address
            SendCommand.Add(result.Content2);

            //Length
            SendCommand.Add(isBit?  new byte[] { (byte)(value.Length / 256), (byte)(value.Length % 256) }: new byte[] { (byte)(value.Length/2 / 256), (byte)(value.Length/2 % 256) });

            //Value
            SendCommand.Add(value);

            return OperateResult.CreateSuccessResult(SendCommand.array);
        }

        /// <summary>
        /// 读取数据的解析方法
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private OperateResult<byte[]> AnalysisReadResponseMessage(byte[] response)
        {
            if (response.Length >= 16)
            {
                //获取错误码
                int errorCode = IntLib.GetIntFromByteArray(response, 12);

                if (errorCode > 0) return new OperateResult<byte[]>(false, errorCode, OmronHelper.GetErrorCodeText(errorCode));

                byte[] data = ByteArrayLib.GetByteArrayFromByteArray(response, 16, response.Length - 16);

                if (data.Length >= 14)
                {
                    int endCode = data[12] * 256 + data[13];

                    if(endCode > 0) return new OperateResult<byte[]>(false, endCode, OmronHelper.GetEndCodeText(endCode));

                    //截取
                    return OperateResult.CreateSuccessResult(ByteArrayLib.GetByteArrayFromByteArray(data, 14, data.Length - 14));

                }
            }

            return OperateResult.CreateFailResult<byte[]>("Receive Error");
        }

        /// <summary>
        /// 读取数据的解析方法
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private OperateResult AnalysisWriteResponseMessage(byte[] response)
        {
            if (response.Length >= 16)
            {
                //获取错误码
                int errorCode = IntLib.GetIntFromByteArray(response, 12);

                if (errorCode > 0) return new OperateResult<byte[]>(false, errorCode, OmronHelper.GetErrorCodeText(errorCode));

                byte[] data = ByteArrayLib.GetByteArrayFromByteArray(response, 16, response.Length - 16);

                if (data.Length >= 14)
                {
                    int endCode = data[12] * 256 + data[13];

                    if (endCode > 0) return new OperateResult<byte[]>(false, endCode, OmronHelper.GetEndCodeText(endCode));

                    //截取
                    return OperateResult.CreateSuccessResult();

                }
            }

            return OperateResult.CreateFailResult("Receive Error");
        }
    }
}
