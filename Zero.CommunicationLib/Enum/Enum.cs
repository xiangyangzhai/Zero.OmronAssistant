using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib
{
    /// <summary>
    /// �洢����С��λ
    /// </summary>
    public enum AreaType
    {
        /// <summary>
        /// �ֽ�
        /// </summary>
        Byte = 1,
        /// <summary>
        /// ��
        /// </summary>
        Word = 2,
    }


    public enum StringType
    {
        /// <summary>
        /// ʮ�����ַ���
        /// </summary>
        DecString,
        /// <summary>
        /// ʮ�������ַ���
        /// </summary>
        HexString,
        /// <summary>
        /// ASCII�ַ���
        /// </summary>
        ASCIIString,
        /// <summary>
        /// BitConvert�ַ���
        /// </summary>
        BitConvertString,
        /// <summary>
        /// �������ַ���
        /// </summary>
        SiemensString

    }

    /// <summary>
    /// ������
    /// </summary>
    public enum FunctionCode
    {
        /// <summary>
        /// ��ȡ�����Ȧ
        /// </summary>
        ReadOutputStatus = 0x01,

        /// <summary>
        /// ��ȡ������Ȧ
        /// </summary>
        ReadInputStatus = 0x02,

        /// <summary>
        /// ��ȡ����Ĵ���
        /// </summary>
        ReadOutputRegister = 0x03,

        /// <summary>
        /// ��ȡ����Ĵ���
        /// </summary>
        ReadInputRegister = 0x04,

        /// <summary>
        /// д�뵥����Ȧ
        /// </summary>
        ForceCoil = 0x05,

        /// <summary>
        /// д�뵥���Ĵ���
        /// </summary>
        PreSetRegister = 0x06,

        /// <summary>
        /// д������Ȧ
        /// </summary>
        ForceMultiCoils = 0x0F,

        /// <summary>
        /// д�����Ĵ���
        /// </summary>
        PreSetMultiRegisters = 0x10,


    }
    public enum CIP_State_Code
    {
        �ɹ� = 0x0000,
        �����˷�����Ч����֧�ֵķ�װ���� = 0x0001,
        �������е��ڴ���Դ�����Դ�������_�ⲻ��һ��Ӧ�ó�������෴ֻ���ڷ�װ���޷���������ڴ���Դ������²Żᵼ�´����� = 0x00002,
        ��װ��Ϣ�����ݲ����е������γɲ�������ȷ = 0x0003,
        ��Ŀ�귢�ͷ�װ��Ϣʱ_ʼ����ʹ������Ч�ĻỰ��� = 0x0064,
        Ŀ���յ�һ����Ч���ȵ���Ϣ = 0x0065,
        ��֧�ֵķ�װЭ���޶� = 0x0069

    }
}
