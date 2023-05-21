using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Interface;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib.Base
{
    public class ReadWriteBase : IReadWrite
    {

        #region ����
        /// <summary>
        /// �洢����С��λ
        /// </summary>
        public AreaType AreaType { get; set; } = AreaType.Word;

        /// <summary>
        /// �ֽڴ�С��˳��
        /// </summary>
        public DataFormat DataFormat { get; set; } = DataFormat.ABCD;

        #endregion

        #region ��д�ӿڷ�������Ҫ��д��

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual OperateResult<bool[]> ReadBoolArray(string address, ushort length)
        {
            return OperateResult.CreateFailResult<bool[]>(new OperateResult());
        }

        /// <summary>
        /// ��ȡ�ֽ�����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual OperateResult<byte[]> ReadByteArray(string address, ushort length)
        {
            return OperateResult.CreateFailResult<byte[]>(new OperateResult());
        }

        /// <summary>
        /// д�벼������
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual OperateResult WriteBoolArray(string address, bool[] value)
        {
            return OperateResult.CreateFailResult();
        }

        /// <summary>
        /// д���ֽ�����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual OperateResult WriteByteArray(string address, byte[] value)
        {
            return OperateResult.CreateFailResult();
        }

        #endregion

        #region ��ȡ����

        #region ReadCommon
        /// <summary>
        ///  ͨ�ö�ȡ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual OperateResult<T> ReadCommon<T>(string address, ushort length = 1)
        {
            string dataType = typeof(T).Name;

            OperateResult<T> result = new OperateResult<T>();

            switch (dataType)
            {
                case "Boolean":
                    result = OperateResult.CopyOperateResult<T, bool>(ReadBool(address));
                    break;
                case "Boolean[]":
                    result = OperateResult.CopyOperateResult<T, bool[]>(ReadBoolArray(address, length));
                    break;
                case "Byte":
                    result = OperateResult.CopyOperateResult<T, byte>(ReadByte(address));
                    break;
                case "Byte[]":
                    result = OperateResult.CopyOperateResult<T, byte[]>(ReadByteArray(address, length));
                    break;
                case "Int16":
                    result = OperateResult.CopyOperateResult<T, short>(ReadShort(address));
                    break;
                case "Int16[]":
                    result = OperateResult.CopyOperateResult<T, short[]>(ReadShortArray(address, length));
                    break;
                case "UInt16":
                    result = OperateResult.CopyOperateResult<T, ushort>(ReadUShort(address));
                    break;
                case "UInt16[]":
                    result = OperateResult.CopyOperateResult<T, ushort[]>(ReadUShortArray(address, length));
                    break;
                case "Int32":
                    result = OperateResult.CopyOperateResult<T, int>(ReadInt(address));
                    break;
                case "Int32[]":
                    result = OperateResult.CopyOperateResult<T, int[]>(ReadIntArray(address, length));
                    break;
                case "UInt32":
                    result = OperateResult.CopyOperateResult<T, uint>(ReadUInt(address));
                    break;
                case "UInt32[]":
                    result = OperateResult.CopyOperateResult<T, uint[]>(ReadUIntArray(address, length));
                    break;
                case "Int64":
                    result = OperateResult.CopyOperateResult<T, long>(ReadLong(address));
                    break;
                case "Int64[]":
                    result = OperateResult.CopyOperateResult<T, long[]>(ReadLongArray(address, length));
                    break;
                case "UInt64":
                    result = OperateResult.CopyOperateResult<T, ulong>(ReadULong(address));
                    break;
                case "UInt64[]":
                    result = OperateResult.CopyOperateResult<T, ulong[]>(ReadULongArray(address, length));
                    break;
                case "Single":
                    result = OperateResult.CopyOperateResult<T, float>(ReadFloat(address));
                    break;
                case "Single[]":
                    result = OperateResult.CopyOperateResult<T, float[]>(ReadFloatArray(address, length));
                    break;
                case "Double":
                    result = OperateResult.CopyOperateResult<T, double>(ReadDouble(address));
                    break;
                case "Double[]":
                    result = OperateResult.CopyOperateResult<T, double[]>(ReadDoubleArray(address, length));
                    break;
                case "String":
                    result = OperateResult.CopyOperateResult<T, string>(ReadString(address, length));
                    break;
                default:
                    break;
            }

            return result;
        }
        #endregion

        #region ��ȡ��������

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<bool> ReadBool(string address)
        {
            var result = ReadBoolArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<bool>(result);
            }
        }

        #endregion

        #region ��ȡ�����ֽ�

        /// <summary>
        /// ��ȡ�����ֽ�
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<byte> ReadByte(string address)
        {
            var result = ReadByteArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<byte>(result);
            }
        }

        #endregion

        #region ��ȡInt16

        /// <summary>
        /// ��ȡShort����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        public virtual OperateResult<short[]> ReadShortArray(string address, ushort length)
        {
            var result = ReadByteArray(address, (ushort)(length * 2 / (int)AreaType));

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(ShortLib.GetShortArrayFromByteArray(result.Content, DataFormat));
            }
            else
            {
                return OperateResult.CreateFailResult<short[]>(result);
            }
        }

        /// <summary>
        /// ��ȡ����Short
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<short> ReadShort(string address)
        {
            var result = ReadShortArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<short>(result);
            }
        }

        #endregion

        #region ��ȡUInt16

        /// <summary>
        /// ��ȡUShort����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        public virtual OperateResult<ushort[]> ReadUShortArray(string address, ushort length)
        {
            var result = ReadByteArray(address, (ushort)(length * 2 / (int)AreaType));

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(UShortLib.GetUShortArrayFromByteArray(result.Content, DataFormat));
            }
            else
            {
                return OperateResult.CreateFailResult<ushort[]>(result);
            }
        }

        /// <summary>
        /// ��ȡ����UShort
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<ushort> ReadUShort(string address)
        {
            var result = ReadUShortArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<ushort>(result);
            }
        }

        #endregion

        #region ��ȡInt32

        /// <summary>
        /// ��ȡInt����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        public virtual OperateResult<int[]> ReadIntArray(string address, ushort length)
        {
            var result = ReadByteArray(address, (ushort)(length * 4 / (int)AreaType));

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(IntLib.GetIntArrayFromByteArray(result.Content, DataFormat));
            }
            else
            {
                return OperateResult.CreateFailResult<int[]>(result);
            }
        }

        /// <summary>
        /// ��ȡ����Int
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<int> ReadInt(string address)
        {
            var result = ReadIntArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<int>(result);
            }
        }

        #endregion

        #region ��ȡUInt32

        /// <summary>
        /// ��ȡUInt����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        public virtual OperateResult<uint[]> ReadUIntArray(string address, ushort length)
        {
            var result = ReadByteArray(address, (ushort)(length * 4 / (int)AreaType));

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(UIntLib.GetUIntArrayFromByteArray(result.Content, DataFormat));
            }
            else
            {
                return OperateResult.CreateFailResult<uint[]>(result);
            }
        }

        /// <summary>
        /// ��ȡ����UInt
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<uint> ReadUInt(string address)
        {
            var result = ReadUIntArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<uint>(result);
            }
        }

        #endregion

        #region ��ȡInt64

        /// <summary>
        /// ��ȡLong����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        public virtual OperateResult<long[]> ReadLongArray(string address, ushort length)
        {
            var result = ReadByteArray(address, (ushort)(length * 8 / (int)AreaType));

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(LongLib.GetLongArrayFromByteArray(result.Content, DataFormat));
            }
            else
            {
                return OperateResult.CreateFailResult<long[]>(result);
            }
        }

        /// <summary>
        /// ��ȡ����Long
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<long> ReadLong(string address)
        {
            var result = ReadLongArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<long>(result);
            }
        }

        #endregion

        #region ��ȡUInt64

        /// <summary>
        /// ��ȡULong����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        public virtual OperateResult<ulong[]> ReadULongArray(string address, ushort length)
        {
            var result = ReadByteArray(address, (ushort)(length * 8 / (int)AreaType));

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(ULongLib.GetULongArrayFromByteArray(result.Content, DataFormat));
            }
            else
            {
                return OperateResult.CreateFailResult<ulong[]>(result);
            }
        }

        /// <summary>
        /// ��ȡ����ULong
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<ulong> ReadULong(string address)
        {
            var result = ReadULongArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<ulong>(result);
            }
        }

        #endregion

        #region ��ȡFloat

        /// <summary>
        /// ��ȡFloat����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        public virtual OperateResult<float[]> ReadFloatArray(string address, ushort length)
        {
            var result = ReadByteArray(address, (ushort)(length * 4 / (int)AreaType));

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(FloatLib.GetFloatArrayFromByteArray(result.Content, DataFormat));
            }
            else
            {
                return OperateResult.CreateFailResult<float[]>(result);
            }
        }

        /// <summary>
        /// ��ȡ����Float
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<float> ReadFloat(string address)
        {
            var result = ReadFloatArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<float>(result);
            }
        }

        #endregion

        #region ��ȡDouble

        /// <summary>
        /// ��ȡDouble����
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>

        public virtual OperateResult<double[]> ReadDoubleArray(string address, ushort length)
        {
            var result = ReadByteArray(address, (ushort)(length * 8 / (int)AreaType));

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(DoubleLib.GetDoubleArrayFromByteArray(result.Content, DataFormat));
            }
            else
            {
                return OperateResult.CreateFailResult<double[]>(result);
            }
        }

        /// <summary>
        /// ��ȡ����Double
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public virtual OperateResult<double> ReadDouble(string address)
        {
            var result = ReadDoubleArray(address, 1);

            if (result.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(result.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<double>(result);
            }
        }

        #endregion

        #region ��ȡString

        /// <summary>
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="stringType">�ַ�������</param>
        /// <returns></returns>
        public virtual OperateResult<string> ReadString(string address, ushort length, StringType stringType = StringType.ASCIIString)
        {
            var result = ReadByteArray(address, (ushort)(length / (int)AreaType));

            if (result.IsSuccess)
            {
                switch (stringType)
                {
                    case StringType.DecString:
                        return OperateResult.CreateSuccessResult(StringLib.GetStringFromValueArray(result.Content, 0, result.Content.Length));
                    case StringType.HexString:
                        return OperateResult.CreateSuccessResult(StringLib.GetHexStringFromByteArray(result.Content, 0, result.Content.Length));
                    case StringType.ASCIIString:
                        return OperateResult.CreateSuccessResult(StringLib.GetStringFromByteArrayByEncoding(result.Content, 0, result.Content.Length, Encoding.ASCII));
                    case StringType.BitConvertString:
                        return OperateResult.CreateSuccessResult(StringLib.GetStringFromByteArrayByBitConvert(result.Content, 0, result.Content.Length));
                    case StringType.SiemensString:
                        return OperateResult.CreateSuccessResult(StringLib.GetSiemensStringFromByteArray(result.Content, 0, result.Content.Length));
                    default:
                        return OperateResult.CreateSuccessResult(StringLib.GetStringFromByteArrayByEncoding(result.Content, 0, result.Content.Length, Encoding.ASCII));
                }
            }
            else
            {
                return OperateResult.CreateFailResult<string>(result);
            }
        }
        #endregion

        #endregion

        #region д�뷽��


        #region WriteCommon
        public virtual OperateResult WriteCommon(string address, object value)
        {
            try
            {
                switch (value.GetType().Name)
                {
                    case "Boolean":
                        return WriteBool(address, (bool)value);
                    case "Byte":
                        return WriteByte(address, (byte)value);
                    case "Int16":
                        return WriteShort(address, (short)value);
                    case "UInt16":
                        return WriteUShort(address, (ushort)value);
                    case "Int32":
                        return WriteInt(address, (int)value);
                    case "UInt32":
                        return WriteUInt(address, (uint)value);
                    case "Int64":
                        return WriteLong(address, (long)value);
                    case "UInt64":
                        return WriteULong(address, (ulong)value);
                    case "Single":
                        return WriteFloat(address, (float)value);
                    case "Double":
                        return WriteDouble(address, (double)value);
                    case "Boolean[]":
                        return WriteBoolArray(address, (bool[])value);
                    case "Byte[]":
                        return WriteByteArray(address, (byte[])value);
                    case "Int16[]":
                        return WriteShortArray(address, (short[])value);
                    case "UInt16[]":
                        return WriteUShortArray(address, (ushort[])value);
                    case "Int32[]":
                        return WriteIntArray(address, (int[])value);
                    case "UInt32[]":
                        return WriteUIntArray(address, (uint[])value);
                    case "Int64[]":
                        return WriteLongArray(address, (long[])value);
                    case "UInt64[]":
                        return WriteULongArray(address, (ulong[])value);
                    case "Single[]":
                        return WriteFloatArray(address, (float[])value);
                    case "Double[]":
                        return WriteDoubleArray(address, (double[])value);
                    case "String":
                        return WriteString(address, value.ToString());
                    default:
                        return new OperateResult(false, "��֧�ֵ���������");
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(false, ex.Message);
            }
        }

        #endregion

        #region д�뵥������

        public virtual OperateResult WriteBool(string address, bool value, bool isRegBool = false)
        {
            if (isRegBool)
            {
                // 01.1
                int index = address.LastIndexOf('.');

                string add = address.Substring(0, index);
                int offset = Convert.ToInt32(address.Substring(index + 1));

                var result = ReadUShort(add);

                if (result.IsSuccess)
                {
                    ushort setValue = UShortLib.SetBitValueFromUShort(result.Content, offset, value);

                    return WriteUShort(add, setValue);
                }
                else
                {
                    return new OperateResult(false, "��ȡ�Ĵ������ʧ��:" + result.Message);
                }
            }
            else
            {
                return WriteBoolArray(address, new bool[] { value });
            }
        }

        #endregion

        #region д�뵥���ֽ�

        public virtual OperateResult WriteByte(string address, byte value)
        {
            return WriteByteArray(address, new byte[] { value });
        }

        #endregion

        #region д��Int16

        public virtual OperateResult WriteShortArray(string address, short[] value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromShortArray(value, this.DataFormat));
        }

        public virtual OperateResult WriteShort(string address, short value)
        {
            return WriteShortArray(address, new short[] { value });
        }

        #endregion

        #region д��UInt16

        public virtual OperateResult WriteUShortArray(string address, ushort[] value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromUShortArray(value, this.DataFormat));
        }

        public virtual OperateResult WriteUShort(string address, ushort value)
        {
            return WriteUShortArray(address, new ushort[] { value });
        }

        #endregion

        #region д��Int32

        public virtual OperateResult WriteIntArray(string address, int[] value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromIntArray(value, this.DataFormat));
        }

        public virtual OperateResult WriteInt(string address, int value)
        {
            return WriteIntArray(address, new int[] { value });
        }

        #endregion

        #region д��UInt32

        public virtual OperateResult WriteUIntArray(string address, uint[] value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromUIntArray(value, this.DataFormat));
        }

        public virtual OperateResult WriteUInt(string address, uint value)
        {
            return WriteUIntArray(address, new uint[] { value });
        }

        #endregion

        #region д��Int64

        public virtual OperateResult WriteLongArray(string address, long[] value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromLongArray(value, this.DataFormat));
        }

        public virtual OperateResult WriteLong(string address, long value)
        {
            return WriteLongArray(address, new long[] { value });
        }

        #endregion

        #region д��UInt64

        public virtual OperateResult WriteULongArray(string address, ulong[] value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromULongArray(value, this.DataFormat));
        }

        public virtual OperateResult WriteULong(string address, ulong value)
        {
            return WriteULongArray(address, new ulong[] { value });
        }

        #endregion

        #region д��Float
        public virtual OperateResult WriteFloatArray(string address, float[] value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromFloatArray(value, this.DataFormat));
        }

        public virtual OperateResult WriteFloat(string address, float value)
        {
            return WriteFloatArray(address, new float[] { value });
        }
        #endregion

        #region д��Double
        public virtual OperateResult WriteDoubleArray(string address, double[] value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromDoubleArray(value, this.DataFormat));
        }

        public virtual OperateResult WriteDouble(string address, double value)
        {
            return WriteDoubleArray(address, new double[] { value });
        }
        #endregion

        #region д��String

        public virtual OperateResult WriteString(string address, string value, Encoding encoding)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromString(value, encoding));
        }
        public virtual OperateResult WriteString(string address, string value)
        {
            return WriteByteArray(address, ByteArrayLib.GetByteArrayFromString(value, Encoding.ASCII));
        }

        #endregion

        #endregion


    }
}
