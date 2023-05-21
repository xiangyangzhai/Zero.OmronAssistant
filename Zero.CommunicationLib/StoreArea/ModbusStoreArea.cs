using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    public  class ModbusStoreArea
    {
        public ModbusStoreArea(FunctionCode readFunctionCode,FunctionCode writeFunctionCode)
        {
            this.ReadFunctionCode = readFunctionCode;
            this.WriteFunctionCode = writeFunctionCode;
        }


        /// <summary>
        /// ��ȡ������
        /// </summary>
        public FunctionCode ReadFunctionCode { get; set; }

        /// <summary>
        /// д�빦����
        /// </summary>
        public FunctionCode WriteFunctionCode  { get; set; }

        /// <summary>
        /// 0�������Ȧ
        /// </summary>
        public readonly static ModbusStoreArea X0 = new ModbusStoreArea(FunctionCode.ReadOutputStatus, FunctionCode.ForceMultiCoils);

        /// <summary>
        /// 1�������Ȧ
        /// </summary>
        public readonly static ModbusStoreArea X1 = new ModbusStoreArea(FunctionCode.ReadInputStatus, 0x00);

        /// <summary>
        /// 3������Ĵ���
        /// </summary>
        public readonly static ModbusStoreArea X3 = new ModbusStoreArea(FunctionCode.ReadInputRegister, 0x00);

        /// <summary>
        /// 4������Ĵ���
        /// </summary>
        public readonly static ModbusStoreArea X4 = new ModbusStoreArea(FunctionCode.ReadOutputRegister, FunctionCode.PreSetMultiRegisters);

    }
}
