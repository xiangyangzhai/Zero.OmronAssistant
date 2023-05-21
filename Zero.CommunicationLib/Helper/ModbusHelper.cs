using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.StoreArea;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib.Helper
{
    public class ModbusHelper
    {

        /// <summary>
        /// Modbus��ַ��������
        /// </summary>
        /// <param name="address">Modbus��ַ</param>
        /// <param name="defaultDevAddress">Ĭ��վ��ַ</param>
        /// <param name="isShortAddress">�Ƿ�Ϊ�̵�ַģ��</param>
        /// <returns>���ؽ��</returns>
        /// <remarks>��һ��ֵ��ʾ�洢����Ϣ���洢����Ϣ�а���������</remarks>
        /// <remarks>�ڶ���ֵ��ʾվ��ַ</remarks>
        /// <remarks>������ֵ��ʾ��Ե�ַ��ֱ�Ӳ��뱨��</remarks>
        public static OperateResult<ModbusStoreArea, byte, ushort> ModbusAddressAnalysis(string address, byte defaultDevAddress, bool isShortAddress)
        {
            var result = new OperateResult<ModbusStoreArea, byte, ushort>()
            {
                IsSuccess = true
            };

            try
            {
                //�ж��Ƿ����վ��ַ

                string analysisAdd = string.Empty;

                if (address.Contains('.'))
                {
                    string[] values = address.Split('.');

                    if (values.Length == 2)
                    {
                        //��ȡ��վ��ַ
                        result.Content2 = Convert.ToByte(values[0]);

                        analysisAdd = values[1];
                    }
                    else
                    {
                        return OperateResult.CreateFailResult<ModbusStoreArea, byte, ushort>("������ַ��ʽ����ȷ:" + address);
                    }
                }
                else
                {
                    //��ȡ��վ��ַ
                    result.Content2 = defaultDevAddress;
                    analysisAdd = address;
                }

                //�������վ��ַ�Ѿ�ȷ��

                //ͬʱ���Ѿ�ȷ���ú��������ĵ�ַ   40001  10001

                //��ȫ��ַ,��Ҫ�����0��

                analysisAdd = analysisAdd.PadLeft(isShortAddress ? 5 : 6, '0');

                switch (analysisAdd[0].ToString())
                {
                    case "4":
                        result.Content1 = ModbusStoreArea.X4;
                        result.Content3 = Convert.ToUInt16(Convert.ToUInt32(analysisAdd.Substring(1)) - 1);
                        break;
                    case "3":
                        result.Content1 = ModbusStoreArea.X3;
                        result.Content3 = Convert.ToUInt16(Convert.ToUInt32(analysisAdd.Substring(1)) - 1);
                        break;
                    case "1":
                        result.Content1 = ModbusStoreArea.X1;
                        result.Content3 = Convert.ToUInt16(Convert.ToUInt32(analysisAdd.Substring(1)) - 1);
                        break;
                    case "0":
                        result.Content1 = ModbusStoreArea.X0;
                        result.Content3 = Convert.ToUInt16(Convert.ToUInt32(analysisAdd.Substring(1)) - 1);
                        break;
                    default:
                        result.IsSuccess = false;
                        result.Message = "������ַ��ʽ����ȷ��" + analysisAdd;
                        break;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;

        }

    }
}
