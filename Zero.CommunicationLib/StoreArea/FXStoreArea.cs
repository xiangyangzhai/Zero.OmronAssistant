using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    /// <summary>
    /// FX系列PLC的存储区
    /// </summary>
    public  class FXStoreArea
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="areaType"></param>
        /// <param name="fromBase"></param>
        public FXStoreArea(byte areaType, int fromBase)
        {
            AreaType = areaType;
            FromBase = fromBase;
        }

        /// <summary>
        /// 存储区类型：0表示字，1表示位
        /// </summary>
        public byte AreaType { get; set; } = 0x00;

        /// <summary>
        /// 存储区进制
        /// </summary>
        public int FromBase { get; set; } = 10;

        /// <summary>
        /// X输入继电器
        /// </summary>
        public readonly static FXStoreArea X = new FXStoreArea(0x01, 8);

        /// <summary>
        /// Y输出继电器
        /// </summary>
        public readonly static FXStoreArea Y = new FXStoreArea(0x01, 8);

        /// <summary>
        /// M内部继电器
        /// </summary>
        public readonly static FXStoreArea M = new FXStoreArea(0x01, 10);

        /// <summary>
        /// S内部继电器
        /// </summary>
        public readonly static FXStoreArea S = new FXStoreArea(0x01, 10);

        /// <summary>
        /// D内部继电器
        /// </summary>
        public readonly static FXStoreArea D = new FXStoreArea(0x00, 10);

        /// <summary>
        /// TN定时器当前值
        /// </summary>
        public readonly static FXStoreArea TN = new FXStoreArea(0x00, 10);

       /// <summary>
        /// TS定时器触点
        /// </summary>
        public readonly static FXStoreArea TS = new FXStoreArea(0x01, 10);

        /// <summary>
        /// CN计数器当前值
        /// </summary>
        public readonly static FXStoreArea CN = new FXStoreArea(0x00, 10);


        /// <summary>
        /// CS计数器触点
        /// </summary>
        public readonly static FXStoreArea CS = new FXStoreArea(0x01, 10);
    }
}
