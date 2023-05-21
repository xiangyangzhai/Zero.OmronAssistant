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
    public class OmronHelper
    {

        /// <summary>
        /// 欧姆龙PLC的地址解析
        /// </summary>
        /// <param name="address">PLC地址</param>
        /// <param name="isBit">是否为位读取写入</param>
        /// <returns>第一个参数是存储区类型，第二个参数是地址解析的三个字节</returns>
        public static OperateResult<OmronFinsStoreArea, byte[]> OmronFinsAnalysisAddress(string address, bool isBit)
        {
            var result = new OperateResult<OmronFinsStoreArea, byte[]>();

            try
            {
                switch (address[0].ToString().ToUpper())
                {
                    case "D":
                        {
                            result.Content1 = OmronFinsStoreArea.DM;
                            result.Content2 = GetContent2ByAddress(address, false, isBit);
                        }
                        break;
                    case "C":
                        {
                            result.Content1 = OmronFinsStoreArea.CIO;
                            result.Content2 = GetContent2ByAddress(address, false, isBit);
                        }
                        break;
                    case "W":
                        {
                            result.Content1 = OmronFinsStoreArea.WR;
                            result.Content2 = GetContent2ByAddress(address, false, isBit);
                        }
                        break;
                    case "H":
                        {
                            result.Content1 = OmronFinsStoreArea.HR;
                            result.Content2 = GetContent2ByAddress(address, false, isBit);
                        }
                        break;
                    case "A":
                        {
                            result.Content1 = OmronFinsStoreArea.AR;
                            result.Content2 = GetContent2ByAddress(address, false, isBit);
                        }
                        break;
                    case "E":
                        {
                            string[] spilts = address.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                            switch (spilts[0].ToUpper())
                            {
                                case "E0":
                                    result.Content1 = OmronFinsStoreArea.E0;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E1":
                                    result.Content1 = OmronFinsStoreArea.E1;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E2":
                                    result.Content1 = OmronFinsStoreArea.E2;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E3":
                                    result.Content1 = OmronFinsStoreArea.E3;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E4":
                                    result.Content1 = OmronFinsStoreArea.E4;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E5":
                                    result.Content1 = OmronFinsStoreArea.E5;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E6":
                                    result.Content1 = OmronFinsStoreArea.E6;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E7":
                                    result.Content1 = OmronFinsStoreArea.E7;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E8":
                                    result.Content1 = OmronFinsStoreArea.E8;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E9":
                                    result.Content1 = OmronFinsStoreArea.E9;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E10":
                                    result.Content1 = OmronFinsStoreArea.E10;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E11":
                                    result.Content1 = OmronFinsStoreArea.E11;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E12":
                                    result.Content1 = OmronFinsStoreArea.E12;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E13":
                                    result.Content1 = OmronFinsStoreArea.E13;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E14":
                                    result.Content1 = OmronFinsStoreArea.E14;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E15":
                                    result.Content1 = OmronFinsStoreArea.E15;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E16":
                                    result.Content1 = OmronFinsStoreArea.E16;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E17":
                                    result.Content1 = OmronFinsStoreArea.E17;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E18":
                                    result.Content1 = OmronFinsStoreArea.E18;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E19":
                                    result.Content1 = OmronFinsStoreArea.E19;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E20":
                                    result.Content1 = OmronFinsStoreArea.E20;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E21":
                                    result.Content1 = OmronFinsStoreArea.E21;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E22":
                                    result.Content1 = OmronFinsStoreArea.E22;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E23":
                                    result.Content1 = OmronFinsStoreArea.E23;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                case "E24":
                                    result.Content1 = OmronFinsStoreArea.E24;
                                    result.Content2 = GetContent2ByAddress(address, true, isBit);
                                    break;
                                default:
                                    break;
                            }

                        }
                        break;
                    default:
                        throw new Exception("不支持的存储区");
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }

            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 解析PLC地址得到三个字节，用于报文拼接
        /// </summary>
        /// <param name="address"></param>
        /// <param name="isEArea"></param>
        /// <param name="isBit"></param>
        /// <returns></returns>
        private static byte[] GetContent2ByAddress(string address, bool isEArea, bool isBit)
        {
            byte[] result = new byte[3];

            if (isEArea)
            {
                //E0.0.0
                if (isBit)
                {
                    string[] add = address.Substring(1).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                    if (add.Length == 3)
                    {
                        ushort start = Convert.ToUInt16(add[1]);
                        result[0] = BitConverter.GetBytes(start)[1];
                        result[1] = BitConverter.GetBytes(start)[0];
                        result[2] = Convert.ToByte(add[2]);
                    }
                }
                //E0.0
                else
                {
                    string[] add = address.Substring(1).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                    if (add.Length == 2)
                    {
                        ushort start = Convert.ToUInt16(add[1]);
                        result[0] = BitConverter.GetBytes(start)[1];
                        result[1] = BitConverter.GetBytes(start)[0];
                    }
                }
            }
            else
            {
                //D0.0
                if (isBit)
                {
                    string[] add = address.Substring(1).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                    if (add.Length == 2)
                    {
                        ushort start = Convert.ToUInt16(add[0]);
                        result[0] = BitConverter.GetBytes(start)[1];
                        result[1] = BitConverter.GetBytes(start)[0];
                        result[2] = Convert.ToByte(add[1]);
                    }
                }
                //D0
                else
                {
                    ushort start = Convert.ToUInt16(address.Substring(1));
                    result[0] = BitConverter.GetBytes(start)[1];
                    result[1] = BitConverter.GetBytes(start)[0];
                }
            }

            return result;
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="error">错误代号</param>
        /// <returns>文本消息</returns>
        public static string GetErrorCodeText(int error)
        {
            switch (error)
            {
                case 1: return "消息头不是FINS";
                case 2: return "数据长度太长";
                case 3: return "命令不支持";
                case 20: return "超过连接上限";
                case 21: return "节点已经处于连接中";
                case 22: return "节点还未配置到PLC中";
                case 23: return "网络节点超过正常范围";
                case 24: return "网络节点已经被使用";
                case 25: return "所有的网络节点已经被使用";
                default: return "未知错误";
            }
        }

        /// <summary>
        /// 获取结束信息
        /// </summary>
        /// <param name="error">结束代号</param>
        /// <returns>文本消息</returns>
        public static string GetEndCodeText(int error)
        {
            switch (error)
            {
                case 0x0001: return "Service was canceled.";
                case 0x0101: return "Local node is not participating in the network.";
                case 0x0102: return "Token does not arrive.";
                case 0x0103: return "Send was not possible during the specified number of retries.";
                case 0x0104: return "Cannot send because maximum number of event frames exceeded.";
                case 0x0105: return "Node address setting error occurred.";
                case 0x0106: return "The same node address has been set twice in the same network.";
                case 0x0201: return "The destination node is not in the network.";
                case 0x0202: return "There is no Unit with the specified unit address.";
                case 0x0203: return "The third node does not exist.";
                case 0x0204: return "The destination node is busy.";
                case 0x0205: return "The message was destroyed by noise";
                case 0x0301: return "An error occurred in the communications controller.";
                case 0x0302: return "A CPU error occurred in the destination CPU Unit.";
                case 0x0303: return "A response was not returned because an error occurred in the Board.";
                case 0x0304: return "The unit number was set incorrectly";
                case 0x0401: return "The Unit/Board does not support the specified command code.";
                case 0x0402: return "The command cannot be executed because the model or version is incorrect";
                case 0x0501: return "The destination network or node address is not set in the routing tables.";
                case 0x0502: return "Relaying is not possible because there are no routing tables";
                case 0x0503: return "There is an error in the routing tables.";
                case 0x0504: return "An attempt was made to send to a network that was over 3 networks away";
                // Command format error
                case 0x1001: return "The command is longer than the maximum permissible length.";
                case 0x1002: return "The command is shorter than the minimum permissible length.";
                case 0x1003: return "The designated number of elements differs from the number of write data items.";
                case 0x1004: return "An incorrect format was used.";
                case 0x1005: return "Either the relay table in the local node or the local network table in the relay node is incorrect.";
                // Parameter error
                case 0x1101: return "The specified word does not exist in the memory area or there is no EM Area.";
                case 0x1102: return "The access size specification is incorrect or an odd word address is specified.";
                case 0x1103: return "The start address in command process is beyond the accessible area";
                case 0x1104: return "The end address in command process is beyond the accessible area.";
                case 0x1106: return "FFFF hex was not specified.";
                case 0x1109: return "A large–small relationship in the elements in the command data is incorrect.";
                case 0x110B: return "The response format is longer than the maximum permissible length.";
                case 0x110C: return "There is an error in one of the parameter settings.";
                // Read Not Possible
                case 0x2002: return "The program area is protected.";
                case 0x2003: return "A table has not been registered.";
                case 0x2004: return "The search data does not exist.";
                case 0x2005: return "A non-existing program number has been specified.";
                case 0x2006: return "The file does not exist at the specified file device.";
                case 0x2007: return "A data being compared is not the same.";
                // Write not possible
                case 0x2101: return "The specified area is read-only.";
                case 0x2102: return "The program area is protected.";
                case 0x2103: return "The file cannot be created because the limit has been exceeded.";
                case 0x2105: return "A non-existing program number has been specified.";
                case 0x2106: return "The file does not exist at the specified file device.";
                case 0x2107: return "A file with the same name already exists in the specified file device.";
                case 0x2108: return "The change cannot be made because doing so would create a problem.";
                // Not executable in current mode
                case 0x2201:
                case 0x2202:
                case 0x2208: return "The mode is incorrect.";
                case 0x2203: return "The PLC is in PROGRAM mode.";
                case 0x2204: return "The PLC is in DEBUG mode.";
                case 0x2205: return "The PLC is in MONITOR mode.";
                case 0x2206: return "The PLC is in RUN mode.";
                case 0x2207: return "The specified node is not the polling node.";
                //  No such device
                case 0x2301: return "The specified memory does not exist as a file device.";
                case 0x2302: return "There is no file memory.";
                case 0x2303: return "There is no clock.";
                case 0x2401: return "The data link tables have not been registered or they contain an error.";
                default: return "UnKnownError";
            }
        }
    }
}
