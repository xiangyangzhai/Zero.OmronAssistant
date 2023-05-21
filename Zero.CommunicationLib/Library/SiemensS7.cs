using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Base;
using Zero.CommunicationLib.Helper;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib
{
    public class SiemensS7 : NetDeviceBase
    {
        public SiemensS7()
        {
            this.DataFormat = DataFormat.ABCD;
            this.AreaType = AreaType.Byte;
        }

        private Plc siemens;

        public OperateResult Connect(string ip, CpuType cpuType, short rack, short slot, int port = 102)
        {
            siemens = new Plc(cpuType, ip, port, rack, slot);

            try
            {
                siemens.Open();

                return OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult(ex.Message);
            }
        }

        public override void DisConnect()
        {
            siemens?.Close();
        }

        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            var result = SiemensHelper.SiemensAddressAnalysis(address);

            if (!result.IsSuccess) return OperateResult.CreateFailResult<byte[]>(result);

            try
            {
                byte[] data = siemens.ReadBytes(result.Content1.DataType, result.Content1.DBNO, result.Content2, length);

                return OperateResult.CreateSuccessResult(data);
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult<byte[]>(ex.Message);
            }
        }

        /// <summary>
        /// 读取布尔数组    DB1.0.0  M0.3   8
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            //解析布尔地址
            var boolAddress = SiemensHelper.SiemensBoolAddressAnalysis(address);

            if (!boolAddress.IsSuccess) return OperateResult.CreateFailResult<bool[]>(boolAddress);

            ushort byteLength = (ushort)IntLib.GetByteLengthFromBoolLength(boolAddress.Content2 + length);

            var result = ReadByteArray(boolAddress.Content1, byteLength);

            if (result.IsSuccess)
            {
                //解析
                return OperateResult.CreateSuccessResult(BitLib.GetBitArrayFromByteArray(result.Content, boolAddress.Content2, length));
            }
            else
            {
                return OperateResult.CreateFailResult<bool[]>(result);
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
            var result = SiemensHelper.SiemensAddressAnalysis(address);

            if (!result.IsSuccess) return OperateResult.CreateFailResult(result.Message);

            try
            {
                siemens.WriteBytes(result.Content1.DataType, result.Content1.DBNO, result.Content2, value);

                return OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult(ex.Message);
            }
        }

        /// <summary>
        /// 写入布尔数组   M0.3    10    M1.0
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override OperateResult WriteBoolArray(string address, bool[] value)
        {
            var boolAddress = SiemensHelper.SiemensBoolAddressAnalysis(address);

            if (!boolAddress.IsSuccess) return OperateResult.CreateFailResult(boolAddress.Message);

            var result = SiemensHelper.SiemensAddressAnalysis(boolAddress.Content1);

            if (!result.IsSuccess) return OperateResult.CreateFailResult(result.Message);

            try
            {
                for (int i = 0; i < value.Length; i++)
                {
                    siemens.WriteBit(result.Content1.DataType, result.Content1.DBNO, result.Content2 + (boolAddress.Content2 + i) / 8, (boolAddress.Content2 + i) % 8, value[i]);
                }
                return OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult(ex.Message);
            }
        }

    }
}
