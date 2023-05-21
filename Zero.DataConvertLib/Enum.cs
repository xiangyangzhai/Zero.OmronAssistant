using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.DataConvertLib
{

    /// <summary>
    /// �ֽ���
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// ����˳������
        /// </summary>
        ABCD = 0,
        /// <summary>
        /// ���յ��ַ�ת
        /// </summary>
        BADC = 1,
        /// <summary>
        /// ����˫�ַ�ת
        /// </summary>
        CDAB = 2,
        /// <summary>
        /// ���յ�������
        /// </summary>
        DCBA = 3,
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// ����
        /// </summary>
        Bool,
        /// <summary>
        /// �ֽ�
        /// </summary>
        Byte,
        /// <summary>
        /// �ֽ�
        /// </summary>
        SByte,
        /// <summary>
        /// �з���16λ����
        /// </summary>
        Short,
        /// <summary>
        /// �޷���16λ����
        /// </summary>
        UShort,
        /// <summary>
        /// �з���32λ����
        /// </summary>
        Int,
        /// <summary>
        /// �޷���32λ����
        /// </summary>
        UInt,
        /// <summary>
        /// 32λ������
        /// </summary>
        Float,
        /// <summary>
        /// 64λ������
        /// </summary>
        Double,
        /// <summary>
        /// �з���64λ����
        /// </summary>
        Long,
        /// <summary>
        /// �޷���64λ����
        /// </summary>
        ULong,
        /// <summary>
        /// �ַ���
        /// </summary>
        String,
        /// <summary>
        /// ���ı��ַ���
        /// </summary>
        WString,
        /// <summary>
        /// �ṹ��
        /// </summary>
        Struct,
        /// <summary>
        /// ��������
        /// </summary>
        BoolArray,
        /// <summary>
        /// �ֽ�����
        /// </summary>
        ByteArray,
        /// <summary>
        /// �ֽ�����
        /// </summary>
        SByteArray,
        /// <summary>
        /// �з���16λ��������
        /// </summary>
        ShortArray,
        /// <summary>
        /// �޷���16λ��������
        /// </summary>
        UShortArray,
        /// <summary>
        /// �з���32λ��������
        /// </summary>
        IntArray,
        /// <summary>
        /// �޷���32λ��������
        /// </summary>
        UIntArray,
        /// <summary>
        /// 32λ����������
        /// </summary>
        FloatArray,
        /// <summary>
        /// 64λ����������
        /// </summary>
        DoubleArray,
        /// <summary>
        /// 64λ�з�����������
        /// </summary>
        LongArray,
        /// <summary>
        /// 64λ�޷�����������
        /// </summary>
        ULongArray,
        /// <summary>
        /// �ַ�������
        /// </summary>
        StringArray,
        /// <summary>
        /// ���ı��ַ�������
        /// </summary>
        WStringArray,
    }
}
