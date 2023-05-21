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
    public class SiemensHelper
    {

        /// <summary>
        /// ������PLC��ַ����
        /// </summary>
        /// <param name="address">PLC��ַ  M0 I100  DB1.0</param>
        /// <returns>��һ�������Ǵ洢�����ڶ�����������ʼ�ֽ�</returns>
        public static OperateResult<SiemensStoreArea, int> SiemensAddressAnalysis(string address)
        {
            var result = new OperateResult<SiemensStoreArea, int>()
            {
                IsSuccess = true
            };

            try
            {
                switch (address[0].ToString().ToUpper())
                {
                    case "I":
                        {
                            result.Content1 = SiemensStoreArea.I;
                            result.Content2 = Convert.ToInt32(address.Substring(1));
                            break;
                        }
                    case "Q":
                        {
                            result.Content1 = SiemensStoreArea.Q;
                            result.Content2 = Convert.ToInt32(address.Substring(1));
                            break;
                        }
                    case "M":
                        {
                            result.Content1 = SiemensStoreArea.M;
                            result.Content2 = Convert.ToInt32(address.Substring(1));
                            break;
                        }
                    case "C":
                        {
                            result.Content1 = SiemensStoreArea.C;
                            result.Content2 = Convert.ToInt32(address.Substring(1));
                            break;
                        }
                    case "T":
                        {
                            result.Content1 = SiemensStoreArea.T;
                            result.Content2 = Convert.ToInt32(address.Substring(1));
                            break;
                        }
                    case "D":
                        {
                            if (address.Substring(0, 2).ToUpper() == "DB")
                            {
                                result.Content1 = SiemensStoreArea.DB;
                                if (address.Substring(2).Contains('.'))
                                {
                                    result.Content1.DBNO = Convert.ToInt32(address.Substring(2).Split('.')[0]);
                                    result.Content2 = Convert.ToInt32(address.Substring(2).Split('.')[1]);
                                }
                                else
                                {
                                    result.IsSuccess = false;
                                    result.Message = "����Ч��ַ";
                                }
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Message = "����Ч��ַ";
                            }
                            break;
                        }
                    default:
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

        /// <summary>
        /// �����Ӳ�����ַ����
        /// </summary>
        /// <param name="address">PLC��ַ  M0.3  I10.2  DB1.0.0</param>
        /// <returns></returns>
        public static OperateResult<string, int> SiemensBoolAddressAnalysis(string address)
        {
            if (address.Contains('.'))
            {
                string[] values = address.Split('.');

                string realAddress = string.Empty;
                int bitOffset = 0;

                if (values.Length == 2)
                {
                    realAddress = values[0];
                    bitOffset = Convert.ToInt32(values[1]);
                }
                else if (values.Length == 3)
                {
                    realAddress = values[0] + "." + values[1];
                    bitOffset = Convert.ToInt32(values[2]);
                }
                else
                {
                    return OperateResult.CreateFailResult<string, int>("������ַ��ʽ����ȷ��" + address);
                }

                return OperateResult.CreateSuccessResult(realAddress, bitOffset);
            }
            else
            {
                return OperateResult.CreateFailResult<string,int>("������ַ��ʽ����ȷ��" + address);
            }

        }


    }
}
