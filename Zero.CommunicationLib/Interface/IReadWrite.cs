using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib.Interface
{
    /// <summary>
    /// ��д�Ľӿ�
    /// </summary>
    public interface IReadWrite
    {
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="address">�豸��ַ</param>
        /// <param name="length">����</param>
        /// <returns>���ز������������Ϊbool[]</returns>
        OperateResult<bool[]> ReadBoolArray(string address, ushort length);

        /// <summary>
        /// ��ȡ�ֽ�����
        /// </summary>
        /// <param name="address">�豸��ַ</param>
        /// <param name="length">����</param>
        /// <returns>���ز������������Ϊbyte[]</returns>
        OperateResult<byte[]> ReadByteArray(string address, ushort length);


        /// <summary>
        /// д�벼������
        /// </summary>
        /// <param name="address">�豸��ַ</param>
        /// <param name="value">д�벼����������</param>
        /// <returns>���ز���ֵ</returns>
        OperateResult WriteBoolArray(string address, bool[] value);

        /// <summary>
        /// д���ֽ�����
        /// </summary>
        /// <param name="address">�豸��ַ</param>
        /// <param name="value">д���ֽ���������</param>
        /// <returns>���ز���ֵ</returns>
        OperateResult WriteByteArray(string address, byte[] value);


    }
}
