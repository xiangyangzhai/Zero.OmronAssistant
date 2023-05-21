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
        /// ���췽����ŷķ��PLC�Ĵ�С��Ĭ����CDAB
        /// </summary>
        /// <param name="dataFormat"></param>
        public OmronFinsUDP(string ip, int port, DataFormat dataFormat = DataFormat.CDAB)
        {
            base.Init(ip, port);
            this.DataFormat = dataFormat;
        }


        /// <summary>
        /// ��ʽΪ 1?00000?������ Bit0��0 ��ʾ��Ҫ�ظ���1 ��ʾ����Ҫ�ظ���
        /// Bit7��0 ��ʾ���1 ��ʾ��Ӧ
        /// </summary>
        private byte ICF = 0x80;

        /// <summary>
        /// ����ֵ
        /// </summary>
        private byte RSV = 0x00;

        /// <summary>
        /// �������Ϣ 
        /// </summary>
        private byte GCT = 0x02;

        /// <summary>
        /// Ŀ�������
        /// </summary>
        public byte DNA { get; set; } = 0x00;

        /// <summary>
        /// Ŀ��ڵ��
        /// </summary>
        public byte DA1 { get; set; } = 0x00;

        /// <summary>
        /// Ŀ�굥Ԫ��
        /// </summary>
        public byte DA2 { get; set; } = 0x00;

        /// <summary>
        /// Դ�����
        /// </summary>
        public byte SNA { get; set; } = 0x00;

        /// <summary>
        /// Դ�ڵ��
        /// </summary>
        public byte SA1 { get; set; } = 0x01;

        /// <summary>
        /// Դ��Ԫ��
        /// </summary>
        public byte SA2 { get; set; } = 0x00;

        /// <summary>
        /// ����ID
        /// </summary>
        public byte SID { get; set; } = 0x00;


        /// <summary>
        /// ��ȡ�ֽ�����ķ���
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            //ƴ�ӱ���
            var command = BuildReadMessageFrame(address, length, false);

            if (command.IsSuccess == false) return command;

            //���ͱ���  ���ձ���
            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response, null);

            if (receive.IsSuccess)
            {
                //��֤���� ��������
                return AnalysisReadResponseMessage(response);
            }
            else
            {
                return OperateResult.CreateFailResult<byte[]>(receive);
            }
        }


        /// <summary>
        /// ��ȡ��������ķ���
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            //ƴ�ӱ���
            var command = BuildReadMessageFrame(address, length, true);

            if (command.IsSuccess == false) return OperateResult.CreateFailResult<bool[]>(command);

            //���ͱ���  ���ձ���
            byte[] response = null;

            var receive = SendAndReceive(command.Content, ref response, null);

            if (receive.IsSuccess)
            {
                //��֤���� ��������
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
        /// д���ֽ�����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteByteArray(string address, byte[] value)
        {
            //ƴ�ӱ���
            var command = BuildWriteMessageFrame(address, value, false);

            if (command.IsSuccess == false) return command;

            //���ͱ���  ���ձ���
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
        /// д�벼������ķ���
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteBoolArray(string address, bool[] value)
        {
            //ƴ�ӱ���
            var command = BuildWriteMessageFrame(address, value.Select(c => c ? (byte)0x01 : (byte)0x00).ToArray(), true);

            if (command.IsSuccess == false) return command;

            //���ͱ���  ���ձ���
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
        /// ��ȡ��ȡ����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildReadMessageFrame(string address, ushort length, bool isBit)
        {
            var result = OmronHelper.OmronFinsAnalysisAddress(address, isBit);

            //�����ַ����ʧ��
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
        /// ��ȡд�뱨��
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private OperateResult<byte[]> BuildWriteMessageFrame(string address, byte[] value, bool isBit)
        {
            var result = OmronHelper.OmronFinsAnalysisAddress(address, isBit);

            //�����ַ����ʧ��
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
        /// ��ȡ���ݵĽ�������
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

                //��ȡ
                return OperateResult.CreateSuccessResult(ByteArrayLib.GetByteArrayFromByteArray(data, 14, data.Length - 14));

            }

            return OperateResult.CreateFailResult<byte[]>("Receive Error");
        }

        /// <summary>
        /// ��ȡ���ݵĽ�������
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

                //��ȡ
                return OperateResult.CreateSuccessResult();

            }

            return OperateResult.CreateFailResult("Receive Error");
        }
    }
}
