using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.DataConvertLib
{
    /// <summary>
    /// Byteת����
    /// </summary>
    public class ByteLib
    {
        /// <summary>
        /// ���ֽ��е�ĳ��λ��ֵ
        /// </summary>
        /// <param name="value">ԭʼ�ֽ�</param>
        /// <param name="offset">λ</param>
        /// <param name="bitValue">д����ֵ</param>
        /// <returns>�����ֽ�</returns>
        public static byte SetbitValue(byte value, int offset, bool bitValue)
        {
            return bitValue ? (byte)(value | (byte)Math.Pow(2, offset)) : (byte)(value & ~(byte)Math.Pow(2, offset));
        }

        /// <summary>
        /// ���ֽ������н�ȡĳ���ֽ�
        /// </summary>
        /// <param name="value">�ֽ�����</param>
        /// <param name="start">��ʼ����</param>
        /// <returns>�����ֽ�</returns>
        public static byte GetByteFromByteArray(byte[] value, int start)
        {
            if (start > value.Length - 1) throw new ArgumentException("�ֽ����鳤�Ȳ�����ʼ����̫��");

            return value[start];
        }

        /// <summary>
        /// ����������ת�����ֽ�����
        /// </summary>
        /// <param name="value">��������</param>
        /// <returns>�ֽ�����</returns>
        public static byte GetByteFromBoolArray(bool[] value)
        {
            if (value.Length != 8) throw new ArgumentNullException("������鳤���Ƿ�Ϊ8");

            byte result = 0;

            //������ǰ�ֽڵ�ÿ��λ��ֵ
            for (int i = 0; i < 8; i++)
            {
                result = SetbitValue(result, i, value[i]);
            }
            return result;
        }
    }
}