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
    public class MelsecHelper
    {
        /// <summary>
        /// 三菱PLC地址解析方法
        /// </summary>
        /// <param name="address">PLC地址</param>
        /// <param name="isFx5U">是否为Fx5U</param>
        /// <returns>返回解析的结果</returns>
        public static OperateResult<MelsecStoreArea, int> MelsecAddressAnalysis(string address, bool isFx5U = false)
        {
            var result = new OperateResult<MelsecStoreArea, int>();

            try
            {
                switch (address[0].ToString().ToUpper())
                {
                    case "X":
                        {
                            result.Content1 = MelsecStoreArea.X;
                            result.Content2 = isFx5U ? Convert.ToInt32(address.Substring(1), MelsecStoreArea.X8.FromBase) : Convert.ToInt32(address.Substring(1), MelsecStoreArea.X.FromBase);
                            break;
                        }
                    case "Y":
                        {
                            result.Content1 = MelsecStoreArea.Y;
                            result.Content2 = isFx5U ? Convert.ToInt32(address.Substring(1), MelsecStoreArea.Y8.FromBase) : Convert.ToInt32(address.Substring(1), MelsecStoreArea.Y.FromBase);
                            break;
                        }
                    case "M":
                        {
                            result.Content1 = MelsecStoreArea.M;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecStoreArea.M.FromBase);
                            break;
                        }
                    case "L":
                        {
                            result.Content1 = MelsecStoreArea.L;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecStoreArea.L.FromBase);
                            break;
                        }
                    case "F":
                        {
                            result.Content1 = MelsecStoreArea.F;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecStoreArea.F.FromBase);
                            break;
                        }
                    case "V":
                        {
                            result.Content1 = MelsecStoreArea.V;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecStoreArea.V.FromBase);
                            break;
                        }
                    case "B":
                        {
                            result.Content1 = MelsecStoreArea.B;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecStoreArea.B.FromBase);
                            break;
                        }


                    case "D":
                        {
                            result.Content1 = MelsecStoreArea.D;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecStoreArea.D.FromBase);
                            break;
                        }
                    case "W":
                        {
                            result.Content1 = MelsecStoreArea.W;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecStoreArea.W.FromBase);
                            break;
                        }
                    case "Z":
                        {
                            result.Content1 = MelsecStoreArea.Z;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecStoreArea.Z.FromBase);
                            break;
                        }

                    case "T":
                        {
                            if (address.Substring(0, 2).ToUpper() == "TN")
                            {
                                result.Content1 = MelsecStoreArea.TN;
                                result.Content2 = Convert.ToInt32(address.Substring(2), MelsecStoreArea.TN.FromBase);
                            }
                            break;
                        }
                    case "C":
                        {
                            if (address.Substring(0, 2).ToUpper() == "CN")
                            {
                                result.Content1 = MelsecStoreArea.CN;
                                result.Content2 = Convert.ToInt32(address.Substring(2), MelsecStoreArea.CN.FromBase);
                            }
                            break;
                        }
                    case "S":
                        {
                            if (address.Substring(0, 2).ToUpper() == "SN")
                            {
                                result.Content1 = MelsecStoreArea.SN;
                                result.Content2 = Convert.ToInt32(address.Substring(2), MelsecStoreArea.SN.FromBase);
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
                return result;
            }

            result.IsSuccess = true;
            return result;

        }

        /// <summary>
        /// 三菱FX PLC地址解析方法
        /// </summary>
        /// <param name="address">PLC地址</param>
        /// <returns>返回解析的结果</returns>
        public static OperateResult<FXStoreArea, ushort> MelsecFXAddressAnalysis(string address)
        {
            var result = new OperateResult<FXStoreArea, ushort>();

            try
            {
                switch (address[0].ToString().ToUpper())
                {
                    case "X":
                        {
                            result.Content1 = FXStoreArea.X;
                            result.Content2 = Convert.ToUInt16(address.Substring(1), FXStoreArea.X.FromBase);
                            break;
                        }
                    case "Y":
                        {
                            result.Content1 = FXStoreArea.Y;
                            result.Content2 = Convert.ToUInt16(address.Substring(1), FXStoreArea.Y.FromBase);
                            break;
                        }
                    case "M":
                        {
                            result.Content1 = FXStoreArea.M;
                            result.Content2 = Convert.ToUInt16(address.Substring(1), FXStoreArea.M.FromBase);
                            break;
                        }
                    case "S":
                        {
                            result.Content1 = FXStoreArea.S;
                            result.Content2 = Convert.ToUInt16(address.Substring(1), FXStoreArea.S.FromBase);
                            break;
                        }
                    case "D":
                        {
                            result.Content1 = FXStoreArea.D;
                            result.Content2 = Convert.ToUInt16(address.Substring(1), FXStoreArea.D.FromBase);
                            break;
                        }
                    case "T":
                        {
                            if (address.Substring(0, 2).ToUpper() == "TN")
                            {
                                result.Content1 = FXStoreArea.TN;
                                result.Content2 = Convert.ToUInt16(address.Substring(2), FXStoreArea.TN.FromBase);
                            }
                            else if (address.Substring(0, 2).ToUpper() == "TS")
                            {
                                result.Content1 = FXStoreArea.TS;
                                result.Content2 = Convert.ToUInt16(address.Substring(2), FXStoreArea.TS.FromBase);
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Message = "地址格式不正确";
                                return result;
                            }
                            break;
                        }
                    case "C":
                        {
                            if (address.Substring(0, 2).ToUpper() == "CN")
                            {
                                result.Content1 = FXStoreArea.CN;
                                result.Content2 = Convert.ToUInt16(address.Substring(2), FXStoreArea.CN.FromBase);
                            }
                            else if (address.Substring(0, 2).ToUpper() == "CS")
                            {
                                result.Content1 = FXStoreArea.CS;
                                result.Content2 = Convert.ToUInt16(address.Substring(2), FXStoreArea.CS.FromBase);
                            }
                            else
                            {
                                result.Message = "地址格式不正确";
                                return result;
                            }
                            break;
                        }
                    default:
                        {
                            result.Message = "地址格式不正确";
                            return result;
                        }
                }
            }
            catch (Exception ex)
            {
                result.Message =ex.Message ;
                return result;
            }

            result.IsSuccess = true;
            result.Message = "Success";
            return result;
        }

        /// <summary>
        /// 三菱A1E地址解析
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static OperateResult<MelsecA1EStoreArea, int> MelsecA1EAddressAnalysis(string address)
        {
            var result = new OperateResult<MelsecA1EStoreArea, int>();
            try
            {
                switch (address[0].ToString().ToUpper())
                {
                    case "X":
                        {
                            result.Content1 = MelsecA1EStoreArea.X;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecA1EStoreArea.X.FromBase);
                            break;
                        }
                    case "Y":
                        {
                            result.Content1 = MelsecA1EStoreArea.Y;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecA1EStoreArea.Y.FromBase);
                            break;
                        }
                    case "M":
                        {
                            result.Content1 = MelsecA1EStoreArea.M;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecA1EStoreArea.M.FromBase);
                            break;
                        }

                    case "D":
                        {
                            result.Content1 = MelsecA1EStoreArea.D;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecA1EStoreArea.D.FromBase);
                            break;
                        }
                    case "W":
                        {
                            result.Content1 = MelsecA1EStoreArea.W;
                            result.Content2 = Convert.ToInt32(address.Substring(1), MelsecA1EStoreArea.W.FromBase);
                            break;
                        }
                    default:
                        {
                            result.IsSuccess = false;
                            result.Message = "非有效地址";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }

            result.IsSuccess = true;
            result.Message = "Success";
            return result;
        }
    }
}
