using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib
{
    /// <summary>
    /// 存储区最小单位
    /// </summary>
    public enum AreaType
    {
        /// <summary>
        /// 字节
        /// </summary>
        Byte = 1,
        /// <summary>
        /// 字
        /// </summary>
        Word = 2,
    }


    public enum StringType
    {
        /// <summary>
        /// 十进制字符串
        /// </summary>
        DecString,
        /// <summary>
        /// 十六进制字符串
        /// </summary>
        HexString,
        /// <summary>
        /// ASCII字符串
        /// </summary>
        ASCIIString,
        /// <summary>
        /// BitConvert字符串
        /// </summary>
        BitConvertString,
        /// <summary>
        /// 西门子字符串
        /// </summary>
        SiemensString

    }

    /// <summary>
    /// 功能码
    /// </summary>
    public enum FunctionCode
    {
        /// <summary>
        /// 读取输出线圈
        /// </summary>
        ReadOutputStatus = 0x01,

        /// <summary>
        /// 读取输入线圈
        /// </summary>
        ReadInputStatus = 0x02,

        /// <summary>
        /// 读取输出寄存器
        /// </summary>
        ReadOutputRegister = 0x03,

        /// <summary>
        /// 读取输入寄存器
        /// </summary>
        ReadInputRegister = 0x04,

        /// <summary>
        /// 写入单个线圈
        /// </summary>
        ForceCoil = 0x05,

        /// <summary>
        /// 写入单个寄存器
        /// </summary>
        PreSetRegister = 0x06,

        /// <summary>
        /// 写入多个线圈
        /// </summary>
        ForceMultiCoils = 0x0F,

        /// <summary>
        /// 写入多个寄存器
        /// </summary>
        PreSetMultiRegisters = 0x10,


    }
    public enum CIP_State_Code
    {
        成功 = 0x0000,
        发件人发出无效或不受支持的封装命令 = 0x0001,
        接收器中的内存资源不足以处理命令_这不是一个应用程序错误相反只有在封装层无法获得所需内存资源的情况下才会导致此问题 = 0x00002,
        封装消息的数据部分中的数据形成不良或不正确 = 0x0003,
        向目标发送封装消息时_始发者使用了无效的会话句柄 = 0x0064,
        目标收到一个无效长度的信息 = 0x0065,
        不支持的封装协议修订 = 0x0069

    }

    public enum CIP_Data_Type
    {
        TIME = 0x09,
        BOOL = 0xc1,
        SINT = 0xc2,
        INT = 0xc3,
        DINT = 0xc4,
        LINT = 0xc5,
        USINT = 0xc6,
        UINT = 0xc7,
        UDINT = 0xc8,
        ULINT = 0xc9,
        REAL = 0xca,
        LREAL = 0xcb,
        ST = 0xcc,
        DATE = 0xcd,
        TIME_OF_DAY = 0xce,
        DATE_AND_TIME = 0xcf,
        STRING = 0xd0,
        BYTE = 0xd1,
        WORD = 0xd2,
        DWORD = 0xd3,
        LWORD = 0xd4,
        STRING2 = 0xd5,
        FTIME = 0xd6,
        LTIME = 0xd7,
        ITIME = 0xd8,
        STRINGN = 0xd9,
        SHORT_STRING = 0xda,
        EPATH = 0xdc,
        ENGUNIT = 0xdd,
        STRINGI = 0xde,
        STRUCT = 0xdf,
        ARRAY = 0xe0,
        _STRUCT = 0xe1,
        _ARRAY = 0xe2,
        ABBREV_STRUCT = 0xe5,
        ABBREV_ARRAY = 0xe6,
        SLC_SHORT_STRING = 0xe8,
        SLC_STRING = 0xe9,
        SLC_INT = 0xea,
        SLC_FLOAT = 0xeb,
        SLC_N_REGISTER = 0xec,
        SLC_STATUS = 0xed,
        SLC_TIMER = 0xee,
        SLC_COUNTER = 0xef,
        SLC_CONTROL = 0xf0,
        SLC_AUX = 0xf1,
        SLC_PID = 0xf2,
        SLC_SHORT = 0xf3,
        SLC_INT64 = 0xf4,
        SLC_STRINGN = 0xf5,
        SLC_FLOAT32 = 0xf6,
        SLC_FLOAT64 = 0xf7,
        SLC_BYTE = 0xf8,
        SLC_WORD = 0xf9,
        SLC_DWORD = 0xfa,
        // 补充完整
        BOOL_ARRAY = 0x2d,
        SINT_ARRAY = 0x2e,
        INT_ARRAY = 0x2f,
        DINT_ARRAY = 0x30,
        LINT_ARRAY = 0x31,
        USINT_ARRAY = 0x32,
        UINT_ARRAY = 0x33,
        UDINT_ARRAY = 0x34,
        ULINT_ARRAY = 0x35,
        REAL_ARRAY = 0x36,
        LREAL_ARRAY = 0x37,
        ST_ARRAY = 0x38,
        DATE_ARRAY = 0x39,
        TIME_OF_DAY_ARRAY = 0x3a,
        DATE_AND_TIME_ARRAY = 0x3b,
        STRING_ARRAY = 0x3c,
        BYTE_ARRAY = 0x3d,
        WORD_ARRAY = 0x3e,
        DWORD_ARRAY = 0x3f,
        LWORD_ARRAY = 0x40,
        STRING2_ARRAY = 0x41,
        FTIME_ARRAY = 0x42,
        LTIME_ARRAY = 0x43,
        ITIME_ARRAY = 0x44,
        STRINGN_ARRAY = 0x45,
        SHORT_STRING_ARRAY = 0x46,
        TIME_ARRAY = 0x47,
        EPATH_ARRAY = 0x48,
        ENGUNIT_ARRAY = 0x49,
        STRINGI_ARRAY = 0x4a,
        STRUCT_ARRAY = 0x4b,
        ARRAY_ARRAY = 0x4c,
        _STRUCT_ARRAY = 0x4d,
        _ARRAY_ARRAY = 0x4e,
        ABBREV_STRUCT_ARRAY = 0x51,
        ABBREV_ARRAY_ARRAY = 0x52,
        SLC_SHORT_STRING_ARRAY = 0x54,
        SLC_STRING_ARRAY = 0x55,
        SLC_INT_ARRAY = 0x56,
        SLC_FLOAT_ARRAY = 0x57,
        SLC_N_REGISTER_ARRAY = 0x58,
        SLC_STATUS_ARRAY = 0x59,
        SLC_TIMER_ARRAY = 0x5a,
        SLC_COUNTER_ARRAY = 0x5b,
        SLC_CONTROL_ARRAY = 0x5c,
        SLC_AUX_ARRAY = 0x5d,
        SLC_PID_ARRAY = 0x5e,
        // 继续补充
        SLC_SHORT_ARRAY = 0x60,
        SLC_INT64_ARRAY = 0x61,
        SLC_STRINGN_ARRAY = 0x62,
        SLC_FLOAT32_ARRAY = 0x63,
        SLC_FLOAT64_ARRAY = 0x64,
        SLC_BYTE_ARRAY = 0x65,
        SLC_WORD_ARRAY = 0x66,
        SLC_DWORD_ARRAY = 0x67,
        SLC_LWORD_ARRAY = 0x68,
        // 补充完整
        BOOL_ARRAY_ARRAY = 0x95,
        SINT_ARRAY_ARRAY = 0x96,
        INT_ARRAY_ARRAY = 0x97,
        DINT_ARRAY_ARRAY = 0x98,
        LINT_ARRAY_ARRAY = 0x99,
        USINT_ARRAY_ARRAY = 0x9a,
        UINT_ARRAY_ARRAY = 0x9b,
        UDINT_ARRAY_ARRAY = 0x9c,
        ULINT_ARRAY_ARRAY = 0x9d,
        REAL_ARRAY_ARRAY = 0x9e,
        LREAL_ARRAY_ARRAY = 0x9f,
        ST_ARRAY_ARRAY = 0xa0,
        DATE_ARRAY_ARRAY = 0xa1,
        TIME_OF_DAY_ARRAY_ARRAY = 0xa2,
        DATE_AND_TIME_ARRAY_ARRAY = 0xa3,
        STRING_ARRAY_ARRAY = 0xa4,
        BYTE_ARRAY_ARRAY = 0xa5,
        WORD_ARRAY_ARRAY = 0xa6,
        DWORD_ARRAY_ARRAY = 0xa7,
        LWORD_ARRAY_ARRAY = 0xa8,
        STRING2_ARRAY_ARRAY = 0xa9,
        FTIME_ARRAY_ARRAY = 0xaa,
        LTIME_ARRAY_ARRAY = 0xab,
        ITIME_ARRAY_ARRAY = 0xac,
        STRINGN_ARRAY_ARRAY = 0xad,
        SHORT_STRING_ARRAY_ARRAY = 0xae,
        TIME_ARRAY_ARRAY = 0xaf,
        EPATH_ARRAY_ARRAY = 0xb0,
        ENGUNIT_ARRAY_ARRAY = 0xb1,
        STRINGI_ARRAY_ARRAY = 0xb2,
        STRUCT_ARRAY_ARRAY = 0xb3,
        ARRAY_ARRAY_ARRAY = 0xb4,
        _STRUCT_ARRAY_ARRAY = 0xb5,
        _ARRAY_ARRAY_ARRAY = 0xb6,
        ABBREV_STRUCT_ARRAY_ARRAY = 0xb9,
        
    }
}
