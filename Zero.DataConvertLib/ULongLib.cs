using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.DataConvertLib
{
    /// <summary>
    /// ULongת����
    /// </summary>
    public class ULongLib
    {
        /// <summary>
        /// �ֽ������н�ȡת��64λ�޷�������
        /// </summary>
        /// <param name="value">�ֽ�����</param>
        /// <param name="start">��ʼ����</param>
        /// <param name="dataFormat">���ݸ�ʽ</param>
        /// <returns>����һ��ULong����</returns>
        public static ulong GetULongFromByteArray(byte[] value, int start = 0, DataFormat dataFormat = DataFormat.ABCD)
        {
            byte[] data = ByteArrayLib.Get8BytesFromByteArray(value, start, dataFormat);
            return BitConverter.ToUInt64(data, 0);
        }

        /// <summary>
        /// ���ֽ������н�ȡת��64λ�޷�����������
        /// </summary>
        /// <param name="value">�ֽ�����</param>
        /// <param name="dataFormat">���ݸ�ʽ</param>
        /// <returns>����Long����</returns>
        public static ulong[] GetULongArrayFromByteArray(byte[] value, DataFormat dataFormat = DataFormat.ABCD)
        {
            if (value == null) throw new ArgumentNullException("������鳤���Ƿ�Ϊ��");

            if (value.Length % 8 != 0) throw new ArgumentNullException("������鳤���Ƿ�Ϊ8�ı���");

            ulong[] values = new ulong[value.Length / 8];

            for (int i = 0; i < value.Length / 8; i++)
            {
                values[i] = GetULongFromByteArray(value, 8 * i, dataFormat);
            }
            return values;
        }

        /// <summary>
        /// ���ַ���תת��64λ�޷�����������
        /// </summary>
        /// <param name="value">�ֽ�����</param>
        /// <param name="spilt">�ָ���</param>
        /// <returns>����ULong����</returns>
        public static ulong[] GetULongArrayFromString(string value, string spilt = " ")
        {
            value = value.Trim();

            List<ulong> result = new List<ulong>();

            try
            {
                if (value.Contains(spilt))
                {
                    string[] str = value.Split(new string[] { spilt }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in str)
                    {
                        result.Add(Convert.ToUInt64(item.Trim()));
                    }
                }
                else
                {
                    result.Add(Convert.ToUInt64(value.Trim()));
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("����ת��ʧ�ܣ�" + ex.Message);
            }

        }
    }
}