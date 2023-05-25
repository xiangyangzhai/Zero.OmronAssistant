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
}
