using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    /// <summary>
    /// Melsec软元件存储区：参考协议手册125页
    /// </summary>
    public  class MelsecStoreArea
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="areaBinaryCode"></param>
        /// <param name="areaASCIICode"></param>
        /// <param name="areaType"></param>
        /// <param name="fromBase"></param>
        public MelsecStoreArea(byte areaBinaryCode, string areaASCIICode,byte areaType, int fromBase )
        {
            AreaBinaryCode = areaBinaryCode;
            AreaASCIICode = areaASCIICode;   
            AreaType = areaType;
            FromBase = fromBase;
        }


        /// <summary>
        /// 软元件二进制代码
        /// </summary>
        public byte AreaBinaryCode { get; set; } = 0x00;

        /// <summary>
        /// 软元件ASCII代码
        /// </summary>
        public string AreaASCIICode { get; set; }

        /// <summary>
        /// 软元件进制
        /// </summary>
        public int FromBase { get; set; }

        /// <summary>
        /// 软元件类型：0表示字，1表示位
        /// </summary>
        public byte AreaType { get; set; } = 0x00;

        /// <summary>
        /// X输入继电器
        /// </summary>
        public readonly static MelsecStoreArea X = new MelsecStoreArea(0x9C,"X*",0x01,16);

        /// <summary>
        /// Y输出继电器
        /// </summary>
        public readonly static MelsecStoreArea Y = new MelsecStoreArea(0x9D, "Y*", 0x01, 16);

        /// <summary>
        /// X输入继电器（八进制）
        /// </summary>
        public readonly static MelsecStoreArea X8 = new MelsecStoreArea(0x9C, "X*", 0x01, 8);

        /// <summary>
        /// Y输出继电器（八进制）
        /// </summary>
        public readonly static MelsecStoreArea Y8 = new MelsecStoreArea(0x9D, "Y*", 0x01, 8);

        /// <summary>
        /// M内部继电器
        /// </summary>
        public readonly static MelsecStoreArea M = new MelsecStoreArea(0x90, "M*", 0x01, 10);

        /// <summary>
        /// L锁存继电器
        /// </summary>
        public readonly static MelsecStoreArea L = new MelsecStoreArea(0x92, "L*", 0x01, 10);

        /// <summary>
        /// F报警继电器
        /// </summary>
        public readonly static MelsecStoreArea F = new MelsecStoreArea(0x93, "F*", 0x01, 10);

        /// <summary>
        /// V边沿继电器
        /// </summary>
        public readonly static MelsecStoreArea V = new MelsecStoreArea(0x94, "V*", 0x01, 10);

        /// <summary>
        /// B链接继电器
        /// </summary>
        public readonly static MelsecStoreArea B = new MelsecStoreArea(0x95, "B*", 0x01, 10);

        /// <summary>
        /// D数据寄存器
        /// </summary>
        public readonly static MelsecStoreArea D = new MelsecStoreArea(0xA8, "D*", 0x00, 10);

        /// <summary>
        /// W链接寄存器
        /// </summary>
        public readonly static MelsecStoreArea W = new MelsecStoreArea(0xB4, "W*", 0x00, 16);

        /// <summary>
        /// 变址寄存器
        /// </summary>
        public readonly static MelsecStoreArea Z = new MelsecStoreArea(0xCC, "Z*", 0x00, 10);

        /// <summary>
        /// 定时器寄存器
        /// </summary>
        public readonly static MelsecStoreArea TN = new MelsecStoreArea(0xC2, "TN", 0x00, 10);

        /// <summary>
        /// 累计定时器寄存器
        /// </summary>
        public readonly static MelsecStoreArea SN = new MelsecStoreArea(0xC8, "SN", 0x00, 10);

        /// <summary>
        /// 计数器寄存器
        /// </summary>
        public readonly static MelsecStoreArea CN = new MelsecStoreArea(0xC5, "CN", 0x00, 10);
    }
}
