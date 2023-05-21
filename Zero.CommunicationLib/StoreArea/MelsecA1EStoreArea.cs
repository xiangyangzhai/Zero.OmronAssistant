using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    /// <summary>
    /// Melsec软元件存储区，参考手册125页
    /// </summary>
    public class MelsecA1EStoreArea
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="areaBinaryCode">二进制代码</param>
        /// <param name="areaASCIICode">ASCII代码</param>
        /// <param name="areaType">类型</param>
        /// <param name="fromBase">进制</param>
        public MelsecA1EStoreArea(byte[] areaBinaryCode, string areaASCIICode, byte areaType, int fromBase)
        {
            AreaBinaryCode = areaBinaryCode;
            AreaASCIICode = areaASCIICode;
            AreaType = areaType;
            FromBase = fromBase;
        }

        /// <summary>
        /// 二进制代码
        /// </summary>
        public byte[] AreaBinaryCode { get; set; } = { 0x00, 0x00 };

        /// <summary>
        /// ASCII代码
        /// </summary>
        public string AreaASCIICode { get; set; }

        /// <summary>
        /// 类型：位/字，0代表按字，1代表按位
        /// </summary>
        public byte AreaType { get; set; } = 0x00;

        /// <summary>
        /// 进制
        /// </summary>
        public int FromBase { get; set; }

        /// <summary>
        /// X输入继电器
        /// </summary>
        public readonly static MelsecA1EStoreArea X = new MelsecA1EStoreArea(new byte[] { 0x58, 0x20 }, "X*", 0x01, 8);
        /// <summary>
        /// Y输出继电器
        /// </summary>
        public readonly static MelsecA1EStoreArea Y = new MelsecA1EStoreArea(new byte[] { 0x59, 0x20 }, "Y*", 0x01, 8);
        /// <summary>
        /// M内部继电器
        /// </summary>
        public readonly static MelsecA1EStoreArea M = new MelsecA1EStoreArea(new byte[] { 0x4D, 0x20 }, "M*", 0x01, 10);
        /// <summary>
        /// W链接继电器
        /// </summary>
        public readonly static MelsecA1EStoreArea W = new MelsecA1EStoreArea(new byte[] { 0x57, 0x20 }, "W*", 0x01, 16);
        /// <summary>
        /// D数据寄存器
        /// </summary>
        public readonly static MelsecA1EStoreArea D = new MelsecA1EStoreArea(new byte[] { 0x44, 0x20 }, "D*", 0x00, 10);


    }
}
