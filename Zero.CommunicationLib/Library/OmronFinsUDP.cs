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
    public  class OmronFinsUDP : UDPDeviceBase
    {
        /// <summary>
        /// 构造方法，欧姆龙PLC的大小端默认是CDAB
        /// </summary>
        /// <param name="dataFormat"></param>
        public OmronFinsUDP(string ip, int port, DataFormat dataFormat = DataFormat.CDAB)
        {
            base.Init(ip, port);
            this.DataFormat = dataFormat;
        }


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
        public byte SA1 { get; set; } = 0x01;

        /// <summary>
        /// 源单元号
        /// </summary>
        public byte SA2 { get; set; } = 0x00;

        /// <summary>
        /// 服务ID
        /// </summary>
        public byte SID { get; set; } = 0x00;


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

            var receive = SendAndReceive(command.Content, ref response, null);

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

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<bool[]>(command);

            //发送报文  接收报文
            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response, null);

            if (receive.IsSuccess)
            {
                //验证报文 解析报文
                var data = AnalysisReadResponseMessage(response);

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

            var receive = SendAndReceive(command.Content, ref response, null);

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
            var command = BuildWriteMessageFrame(address, value.Select(c => c ? (byte)0x01 : (byte)0x00).ToArray(), true);

            if (command.IsSuccess == false) return command;

            //发送报文  接收报文
            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response, null);

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
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length, bool isBit)
        {
            var result = OmronHelper.OmronFinsAnalysisAddress(address, isBit);

            //如果地址解析失败
            if (result.IsSuccess == false) return OperateResult.CreateFailResult<byte[]>(result);

            ByteArray SendCommand = new ByteArray();

            //ICF RSV GCT
            SendCommand.Add(new byte[] { ICF, RSV, GCT });

            //DNA DA1 DA2
            SendCommand.Add(new byte[] { DNA, DA1, DA2 });

            //SNA SA1 SA2 SID
            SendCommand.Add(new byte[] { SNA, SA1, SA2, SID });

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

            //ICF RSV GCT
            SendCommand.Add(new byte[] { ICF, RSV, GCT });

            //DNA DA1 DA2
            SendCommand.Add(new byte[] { DNA, DA1, DA2 });

            //SNA SA1 SA2 SID
            SendCommand.Add(new byte[] { SNA, SA1, SA2, SID });

            //MRC SRC
            SendCommand.Add(new byte[] { 0x01, 0x02 });

            //AREA
            SendCommand.Add(isBit ? result.Content1.BitCode : result.Content1.WordCode);

            //Address
            SendCommand.Add(result.Content2);

            //Length
            SendCommand.Add(isBit ? new byte[] { (byte)(value.Length / 256), (byte)(value.Length % 256) } : new byte[] { (byte)(value.Length / 2 / 256), (byte)(value.Length / 2 % 256) });

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
            byte[] data = response;

            if (data.Length >= 14)
            {
                int endCode = data[12] * 256 + data[13];

                if (endCode > 0) return new OperateResult<byte[]>(false, endCode, OmronHelper.GetEndCodeText(endCode));

                //截取
                return OperateResult.CreateSuccessResult(ByteArrayLib.GetByteArrayFromByteArray(data, 14, data.Length - 14));

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

            byte[] data = response;

            if (data.Length >= 14)
            {
                int endCode = data[12] * 256 + data[13];

                if (endCode > 0) return new OperateResult<byte[]>(false, endCode, OmronHelper.GetEndCodeText(endCode));

                //截取
                return OperateResult.CreateSuccessResult();

            }

            return OperateResult.CreateFailResult("Receive Error");
        }
    }
}
