using System;
using System.Collections;
using System.ComponentModel;
using System.Net.Configuration;
using System.Text;
using Zero.CommunicationLib.Base;
using Zero.Models;
using Zero.DataConvertLib;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Zero.CommunicationLib.Library
{
    public class CIP : NetDeviceBase
    {
        /// <summary>
        /// 构造方法，欧姆龙PLC的大小端默认是CDAB
        /// </summary>
        /// <param name="DataFormat"></param>
        //public CIP(DataFormat DataFormat = DataFormat.DCBA)
        //{
        //    this.DataFormat = DataFormat;
        //}

        public bool Connected { get; set; } = false;

        /// <summary>
        /// 注册请求报文
        /// </summary>
        private byte[] Registercmd = new byte[28]
        {

　　        //--------------------------------------------------------Header 24byte-------------------------------------
　　        0x65,0x00,//命令 2byte
　　        0x04,0x00,//Header后面数据的长度 2byte
　　        0x00,0x00,0x00,0x00,//会话句柄 4byte
　　        0x00,0x00,0x00,0x00,//状态默认0 4byte
　　        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,//发送方描述默认0 8byte
　　        0x00,0x00,0x00,0x00,//选项默认0 4byte

               //-------------------------------------------------------CommandSpecificresponse 指令指定数据 4byte

　　        0x01,0x00,//协议版本 2byte

　　        0x00,0x00,//选项标记 2byte
        };

        /// <summary>
        /// 注册请求的应答报文
        /// </summary>
        private byte[] RefRegistercmd = new byte[28]
        {

　　        //--------------------------------------------------------Header 24byte-------------------------------------
　　        0x65,0x00,//命令 2byte
　　        0x04,0x00,//CommandSpecificresponse的长度 2byte
　　        0x6B,0x01,0x01,0x00,//会话句柄 4byte 由PLC生成
　　        0x00,0x00,0x00,0x00,//状态默认0 4byte
　　        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,//发送方描述默认0 8byte
　　        0x00,0x00,0x00,0x00,//选项默认0 4byte

           //-------------------------------------------------------CommandSpecificresponse 指令指定数据 4byte

　　        0x01,0x00,//协议版本 2byte

　　        0x00,0x00,//选项标记 2byte
        };


        private byte[] UnRegistercmd = new byte[]
        {
            0x66, 0x00, //命令
            0x00, 0x00, //长度
            0x00, 0x00, 0x00, 0x00, //要注销的会话句柄
            0x00, 0x00, 0x00, 0x00, //状态 默认0
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //发送内容 默认0
            0x00, 0x00, 0x00, 0x00, //选项 默认0
        };

        /// <summary>
        /// 从应答报文提取的会话ID，后续读写PLC的报文中，需要包含PLC返回的会话ID
        /// </summary>


        //读数据服务请求报文
        //报文由三部分组成 Header 24个字节 、CommandSpecificresponse 16个字节、以及CIP消息（由读取的标签生成）
        //实例，读取单个标签名为 TAG1的报文总长度为64个字节

        /// <summary>
        /// 封装头
        /// </summary>
        private byte[] Header = new byte[]
        {
            0x6F,0x00,//命令 2byte
　　        0x28,0x00,//长度 2byte（总长度-Header的长度）=40 
　　        0x6B,0x01,0x01,0x00,//会话句柄 4byte
　　        0x00,0x00,0x00,0x00,//状态默认0 4byte
　　        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,//发送方描述默认0 8byte
　　        0x00,0x00,0x00,0x00,//选项默认0 4byte
        };

        private byte[] CommandSpecificresponse = new byte[]
        {
            0x00,0x00,0x00,0x00,//接口句柄 CIP默认为0x00000000 4byte
　　        0x01,0x00,//超时默认0x0001 4byte
　　        0x02,0x00,//项数默认0x0002 4byte  
　　        0x00,0x00,//空地址项默认0x0000 2byte
　　        0x00,0x00,//长度默认0x0000 2byte
　　        0xb2,0x00,//未连接数据项默认为 0x00b2
　　        0x18,0x00,//后面数据包的长度 24个字节(总长度-Header的长度-CommandSpecificresponse的长度)
        };

        private byte[] CipMessage = new byte[]
        {
            0x52,0x02,　　   //服务默认0x52  请求路径大小 默认2
            0x20,0x06,0x24,0x01,//请求路径 默认0x01240622 4byte
　　        0x0A,0xF0, //超时默认0xF00A 4byte
            0x0A,0x00, //Cip指令长度  服务标识到服务命令指定数据的长度
　　        0x4C,//服务标识固定为0x4C 1byte  
　　        0x03,// CIP长度多少字
　　        0x91,//扩展符号 默认为 0x91  固定
　　        0x04,//PLC标签长度 多少个字节
　　        0x54,0x41,0x47,0x31,//标签名 ：TAG1转换成ASCII字节 当标签名的长度为奇数时，需要在末尾补0  比如TAG转换成ASCII为0x54,0x41,0x47，需要在末尾补0 变成 0x54,0x41,0x47，0
            0x01,0x00,//服务命令指定数据　默认为0x0001　
　          0x01,0x00,0x01,0x00//最后一位是PLC的槽号
        };


 

        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 标签值
        /// </summary>
        public string TagValue { get; set; }

        /// <summary>
        /// 标签值类型
        /// </summary>
        public string TagType { get; set; }

        #region 报文字段
        /// <summary>
        /// 命令码
        /// </summary>
        private byte[] Command = new byte[] { 0x6F, 0x00 };

        /// <summary>
        /// 报文头之后的消息长度
        /// </summary>
        private byte[] Length = new byte[] { 0x04, 0x00 };

        /// <summary>
        /// 会话句柄
        /// </summary>
        public byte[] SessionHandle = new byte[] { 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 状态码
        /// </summary>
        private byte[] Status = new byte[] { 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 发送方描述
        /// </summary>
        private byte[] SenderContext = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 选项
        /// </summary>
        private byte[] Options = new byte[] { 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 协议版本
        /// </summary>
        private byte[] ProtocolVersion = new byte[] { 0x01, 0x00 };

        /// <summary>
        /// 选项标记
        /// </summary>
        private byte[] OptionID = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 接口句柄,0 0 0 0 代表CIP
        /// </summary>
        private byte[] InterfaceHandle = new byte[] { 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        /// 超时
        /// </summary>
        private byte[] Timeout = new byte[] { 0x01, 0x00 };

        /// <summary>
        /// 项数
        /// </summary>
        private byte[] ItemCount = new byte[] {0x02, 0x00};

        /// <summary>
        /// 空地址项
        /// </summary>
        private byte[] AddressType= new byte[] {0x00, 0x00};

        /// <summary>
        /// 空地址项长度
        /// </summary>
        private byte[] AddressLength = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 数据类型
        /// </summary>
        private byte[] responseType = new byte[] {0xB2, 0x00};

        /// <summary>
        /// 数据长度
        /// </summary>
        private byte[] responseLength = new byte[] { 0x00, 0x00 };

        /// <summary>
        /// 命令
        /// </summary>
        private byte ControlCode = 0x52;

        /// <summary>
        /// 请求路径长度
        /// </summary>
        private byte RouterLength = 0x02;

        /// <summary>
        /// 默认请求路径
        /// </summary>
        private byte[] DefaultRoutePath = new byte[] { 0x20, 0x06, 0x24, 0x01 };

        /// <summary>
        /// 默认超时
        /// </summary>
        private byte[] DefaultTimeout = new byte[] { 0x0A, 0xF0, 0x0A, 0x00 };

        /// <summary>
        /// 服务标识
        /// </summary>
        private byte ServiceID = 0x4C;

        /// <summary>
        /// //CIP长度多少字  从服务标识开始 到读取标签长度结束
        /// </summary>
        private byte CIPLength = 0x03;

        /// <summary>
        /// 固定值
        /// </summary>
        private byte Fixed = 0x91;

        /// <summary>
        /// 标签长度，单位字节
        /// </summary>
        private byte TagLength = 0x00;

        /// <summary>
        /// 读取长度
        /// </summary>
        private byte[] ReadLength = new byte[] {0x01, 0x00};

        /// <summary>
        /// 槽号
        /// </summary>
        private byte[] Slot = new byte[] { 0x01, 0x00, 0x01, 0x00 };

        #endregion

        // 构建写入单标签报文
        private OperateResult<byte[]> BuildWriteSingleTagFrame(string tagName, string tagType, string tagValue)
        {
            byte[] TagStringToAscii;

            byte[] tagNametoBytes_S = Encoding.Default.GetBytes(tagName);
            if (tagNametoBytes_S.Length % 2 == 0)
            {
                TagStringToAscii = new byte[tagNametoBytes_S.Length];
                for (int i = 0; i < tagNametoBytes_S.Length; i++)
                {
                    TagStringToAscii[i] = tagNametoBytes_S[i];
                }
            }
            else
            {
                TagStringToAscii = new byte[tagNametoBytes_S.Length + 1];
                for (int i = 0; i < tagNametoBytes_S.Length; i++)
                {
                    TagStringToAscii[i] = tagNametoBytes_S[i];
                }
                TagStringToAscii[tagNametoBytes_S.Length] = 0x00;
            }

            //CIP协议数据  =================================================
            ArrayList Cip_Msg = new ArrayList();
            Cip_Msg.Add((byte)0x4D);//服务标识
            Cip_Msg.Add((byte)((TagStringToAscii.Length + 2) / 2));//CIP长度多少字  从标识开始 大读取标签长度结束 
            Cip_Msg.Add((byte)0X91);//固定
            Cip_Msg.Add((byte)tagNametoBytes_S.Length);//PLC标签长度 多少个字节
            ArrayList Var6 = new ArrayList(TagStringToAscii);
            Cip_Msg.AddRange(Var6);//添加标签 
            Cip_Msg.AddRange(getBytesFromTagType(tagType)); // 数据类型
            if (tagType == "STRING")
            {
                Cip_Msg.Add((byte)tagValue.Length); Cip_Msg.Add((byte)0x00); // 写入数量
            }
            else
            {
                Cip_Msg.Add((byte)0x01); Cip_Msg.Add((byte)0x00); // 写入数量
            }
            Cip_Msg.AddRange(getBytesFromTagValue(tagType,tagValue));  // 写入的值
            ArrayList Slot = new ArrayList { (byte)0x01, (byte)0x00, (byte)0x01, (byte)0x00 }; //槽号
            ArrayList Cip_Msg0 = new ArrayList();
            Cip_Msg0.Add((byte)0x52);//命令
            Cip_Msg0.Add((byte)0X02);//请求路径长度
            Cip_Msg0.Add((byte)0X20); Cip_Msg0.Add((byte)0X06); Cip_Msg0.Add((byte)0X24); Cip_Msg0.Add((byte)0X01);//默认请求路径
            Cip_Msg0.Add((byte)0x0a); Cip_Msg0.Add((byte)0xF0);//默认超时 245760ms  
            Cip_Msg0.Add((byte)(Cip_Msg.Count & 0XFF)); Cip_Msg0.Add((byte)(Cip_Msg.Count >> 8));//后面报文长度 不包含PLC槽号 
            Cip_Msg0.AddRange(Cip_Msg);//添加CIP报文 

            ArrayList C_S_D = new ArrayList();
            C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00);//接口句柄 CIP
            C_S_D.Add((byte)0X01); C_S_D.Add((byte)0X00);//超时时间 默认 
            C_S_D.Add((byte)0X02); C_S_D.Add((byte)0X00);//项数 默认 
            C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00);//空地址项 默认 
            C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00);//空地址长度  默认
            C_S_D.Add((byte)0XB2); C_S_D.Add((byte)0X00);//未连接项 默认
            C_S_D.Add((byte)((Cip_Msg0.Count + Slot.Count) & 0XFF)); C_S_D.Add((byte)((Cip_Msg0.Count + Slot.Count) >> 8));//CIP报文包的长度 包括槽号  

            ArrayList Header = new ArrayList();
            Header.Add((byte)0x6F); Header.Add((byte)0x00);//命令码
            Header.Add((byte)((Cip_Msg0.Count + C_S_D.Count + Slot.Count) & 0XFF)); Header.Add((byte)((Cip_Msg0.Count + C_S_D.Count + Slot.Count) >> 8));//后面报文包的长度 是指特定命令数据 和 CIP报文长度  和PLC槽号）

            ArrayList SesHanle = new ArrayList(SessionHandle);
            Header.AddRange(SesHanle);//添加会话句柄 
            Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00);//添加状态 
            //发送方描述更改为标签标志 用以区分报文是指哪个标签 
            byte[] array = System.Text.Encoding.ASCII.GetBytes(tagName);
            //Header.Add((byte)array[0]); Header.Add((byte)array[1]); Header.Add((byte)array[2]); Header.Add((byte)array[3]);//添加发送方描述
            //Header.Add((byte)array[4]); Header.Add((byte)array[5]); Header.Add((byte)array[6]); Header.Add((byte)array[7]);
            Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00);//添加发送方描述
            Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00);
            Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00);//选项默认
            ArrayList EtherNet_IP_CIP_MSG = new ArrayList();//单标签读取完整报文 
            EtherNet_IP_CIP_MSG.AddRange(Header);//添加Header 封装头
            EtherNet_IP_CIP_MSG.AddRange(C_S_D);//添加特定命令数据
            EtherNet_IP_CIP_MSG.AddRange(Cip_Msg0);//添加CIP报文
            EtherNet_IP_CIP_MSG.AddRange(Slot);//添加槽号
            byte[] EtherNet_IP_CIP_MSG0 = (byte[])EtherNet_IP_CIP_MSG.ToArray(typeof(byte));
            return OperateResult.CreateSuccessResult(EtherNet_IP_CIP_MSG0);
        }

        private byte[] getBytesFromTagValue(string tagType, string tagValue)
        {
            // 将 tagValue 转换为 tagType对应的C#数据类型
            CIP_Data_Type CIP_Data_Type;
            bool success = Enum.TryParse(tagType, out CIP_Data_Type);
            if (success)
            {
                switch (CIP_Data_Type)
                {
                    case CIP_Data_Type.BOOL:
                        
                        return BitConverter.GetBytes((short)((tagValue != null && string.Equals(tagValue, "TRUE", StringComparison.OrdinalIgnoreCase)) ? 1 : 0));
                    case CIP_Data_Type.BYTE:
                    case CIP_Data_Type.USINT:
                        byte outByte;
                        Byte.TryParse(tagValue, out outByte);
                        byte[] result = new byte[] { outByte };
                        return result; 
                    case CIP_Data_Type.INT:
                        short outShort;
                        short.TryParse(tagValue, out outShort);
                        return BitConverter.GetBytes(outShort);
                    case CIP_Data_Type.UINT:
                    case CIP_Data_Type.WORD:
                        ushort outUShort;
                        ushort.TryParse(tagValue, out outUShort);
                        return BitConverter.GetBytes(outUShort);
                    case CIP_Data_Type.DINT:
                    
                        int outInt;
                        int.TryParse(tagValue, out outInt);
                        return BitConverter.GetBytes(outInt);
                    case CIP_Data_Type.DWORD:
                    case CIP_Data_Type.UDINT:
                        uint outUInt;
                        uint.TryParse(tagValue, out outUInt);
                        return BitConverter.GetBytes(outUInt);
                    case CIP_Data_Type.REAL:
                        float outFloat;
                        float.TryParse(tagValue, out outFloat);
                        return BitConverter.GetBytes(outFloat);
                    case CIP_Data_Type.LREAL:
                        double outDouble;
                        double.TryParse(tagValue, out outDouble);
                        return BitConverter.GetBytes(outDouble);
                    case CIP_Data_Type.STRING:
                        List<byte> result_bytes = new List<byte>();
                        foreach(var item in tagValue)
                        {
                            byte[] res = getBytesFromTagValue("BYTE", item.ToString());
                            result_bytes.AddRange(res);
                        }
                        return result_bytes.ToArray();

                    case CIP_Data_Type.TIME:
                    case CIP_Data_Type.ULINT:
                        ulong outULong;
                        ulong.TryParse(tagValue, out outULong);
                        return BitConverter.GetBytes(outULong);
                    default:
                        return new byte[] { 0x01, 0x00 };
                }
            }
            else
            {
                return new byte[] { 0x01, 0x00 };
            }

        }

        private byte[] getBytesFromTagType(string tagType)
        {
            CIP_Data_Type CIP_Data_Type;
            bool success = Enum.TryParse(tagType, out CIP_Data_Type);
            if (success)
            {
                switch (CIP_Data_Type)
                {
                    case CIP_Data_Type.BOOL:
                        return new byte[] { 0xC1, 0x00 };
                    case CIP_Data_Type.SINT:
                        return new byte[] { 0xC2, 0x00 };
                    case CIP_Data_Type.INT:
                        return new byte[] { 0xC3, 0x00 };
                    case CIP_Data_Type.DINT:
                        return new byte[] { 0xC4, 0x00 };
                    case CIP_Data_Type.USINT:
                        return new byte[] { 0xC6, 0x00 };
                    case CIP_Data_Type.UINT:
                        return new byte[] { 0xC7, 0x00 };
                    case CIP_Data_Type.UDINT:
                        return new byte[] { 0xC8, 0x00 };
                    case CIP_Data_Type.ULINT:
                        return new byte[] {0xC9, 0x00 };
                    case CIP_Data_Type.REAL:
                        return new byte[] { 0xCA, 0x00 };
                    //case CIP_Data_Type.BIT_STRING:
                    //    return new byte[] { 0xD3, 0x00 };
                    case CIP_Data_Type.BYTE:
                        return new byte[] { 0xD1, 0x00 };
                    case CIP_Data_Type.WORD:
                        return new byte[] { 0xD2, 0x00 };
                    case CIP_Data_Type.DWORD:
                        return new byte[] { 0xD3, 0x00 };
                    case CIP_Data_Type.STRING:
                        return new byte[] { 0xD0, 0x00 };
                    //case CIP_Data_Type.TIME:
                    //    return new byte[] { 0xD2, 0x00 };
                    case CIP_Data_Type.LINT:
                        return new byte[] { 0xC5, 0x00 };
                    case CIP_Data_Type.LWORD:
                        return new byte[] { 0xD7, 0x00 };
                    case CIP_Data_Type.LREAL:
                        return new byte[] { 0xCB, 0x00 };
                    case CIP_Data_Type.SHORT_STRING:
                        return new byte[] { 0xD1, 0x00 };
                    case CIP_Data_Type.STRUCT:
                        return new byte[] { 0xCC, 0x00 };
                    case CIP_Data_Type.TIME:
                        return new byte[] {0x09, 0x00 };
                    default:
                        return new byte[] { 0x00, 0x00 };
                }
            }
            else
            {
                return new byte[] { 0x00, 0x00 };
            }
        }

        // 构建读取单标签报文
        private OperateResult<byte[]> BuildReadSingleTagFrame(string tagName, string tagType)
        {
            byte[] TagStringToAscii;

            byte[] tagNametoBytes_S = Encoding.Default.GetBytes(tagName);
            if (tagNametoBytes_S.Length % 2 == 0)
            {
                TagStringToAscii = new byte[tagNametoBytes_S.Length];
                for (int i = 0; i < tagNametoBytes_S.Length; i++)
                {
                    TagStringToAscii[i] = tagNametoBytes_S[i];
                }
            }
            else
            {
                TagStringToAscii = new byte[tagNametoBytes_S.Length + 1];
                for (int i = 0; i < tagNametoBytes_S.Length; i++)
                {
                    TagStringToAscii[i] = tagNametoBytes_S[i];
                }
                TagStringToAscii[tagNametoBytes_S.Length] = 0x00;
            }
            //CIP协议数据  =========================
            //========================
            ArrayList Cip_Msg = new ArrayList();
            Cip_Msg.Add((byte)0x4C);//服务标识
            Cip_Msg.Add((byte)((TagStringToAscii.Length + 2) / 2));//CIP长度多少字  从标识开始 大读取标签长度结束 
            Cip_Msg.Add((byte)0X91);//固定
            Cip_Msg.Add((byte)tagNametoBytes_S.Length);//PLC标签长度 多少个字节
            ArrayList Var6 = new ArrayList(TagStringToAscii);
            Cip_Msg.AddRange(Var6);//添加标签 
            Cip_Msg.Add((byte)0x01); Cip_Msg.Add((byte)0x00); // 读取长度
            ArrayList Slot = new ArrayList { (byte)0x01, (byte)0x00, (byte)0x01, (byte)0x00 }; //槽号
            ArrayList Cip_Msg0 = new ArrayList();
            Cip_Msg0.Add((byte)0x52);//命令
            Cip_Msg0.Add((byte)0X02);//请求路径长度
            Cip_Msg0.Add((byte)0X20); Cip_Msg0.Add((byte)0X06); Cip_Msg0.Add((byte)0X24); Cip_Msg0.Add((byte)0X01);//默认请求路径
            Cip_Msg0.Add((byte)0x0a); Cip_Msg0.Add((byte)0xF0);//默认超时 
            Cip_Msg0.Add((byte)(Cip_Msg.Count & 0XFF)); Cip_Msg0.Add((byte)(Cip_Msg.Count >> 8));//后面报文长度 不包含PLC槽号 
            Cip_Msg0.AddRange(Cip_Msg);//添加CIP报文 

            ArrayList C_S_D = new ArrayList();
            C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00);//接口句柄 CIP
            C_S_D.Add((byte)0X01); C_S_D.Add((byte)0X00);//超时时间 默认 
            C_S_D.Add((byte)0X02); C_S_D.Add((byte)0X00);//项数 默认 
            C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00);//空地址项 默认 
            C_S_D.Add((byte)0X00); C_S_D.Add((byte)0X00);//空地址长度  默认
            C_S_D.Add((byte)0XB2); C_S_D.Add((byte)0X00);//未连接项 默认
            C_S_D.Add((byte)((Cip_Msg0.Count + Slot.Count) & 0XFF)); C_S_D.Add((byte)((Cip_Msg0.Count + Slot.Count) >> 8));//CIP报文包的长度 包括槽号  

            ArrayList Header = new ArrayList();
            Header.Add((byte)0x6F); Header.Add((byte)0x00);//命令码
            Header.Add((byte)((Cip_Msg0.Count + C_S_D.Count + Slot.Count) & 0XFF)); Header.Add((byte)((Cip_Msg0.Count + C_S_D.Count + Slot.Count) >> 8));//后面报文包的长度 是指特定命令数据 和 CIP报文长度  和PLC槽号）

            ArrayList SesHanle = new ArrayList(SessionHandle);
            Header.AddRange(SesHanle);//添加会话句柄 
            Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00);//添加状态 
            //发送方描述更改为标签标志 用以区分报文是指哪个标签 
            byte[] array = System.Text.Encoding.ASCII.GetBytes(tagName);
            //Header.Add((byte)array[0]); Header.Add((byte)array[1]); Header.Add((byte)array[2]); Header.Add((byte)array[3]);//添加发送方描述
            //Header.Add((byte)array[4]); Header.Add((byte)array[5]); Header.Add((byte)array[6]); Header.Add((byte)array[7]);
            Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00);//添加发送方描述
            Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00);
            Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00); Header.Add((byte)0x00);//选项默认
            ArrayList EtherNet_IP_CIP_MSG = new ArrayList();//单标签读取完整报文 
            EtherNet_IP_CIP_MSG.AddRange(Header);//添加Header 封装头
            EtherNet_IP_CIP_MSG.AddRange(C_S_D);//添加特定命令数据
            EtherNet_IP_CIP_MSG.AddRange(Cip_Msg0);//添加CIP报文
            EtherNet_IP_CIP_MSG.AddRange(Slot);//添加槽号
            byte[] EtherNet_IP_CIP_MSG0 = (byte[])EtherNet_IP_CIP_MSG.ToArray(typeof(byte));
            return OperateResult.CreateSuccessResult( EtherNet_IP_CIP_MSG0);
        }

        private byte[] getTagNameToBytes(string tagName)
        {
            byte[] tagNameToBytes_S = Encoding.Default.GetBytes(tagName);

            if (tagNameToBytes_S.Length % 2 == 0)
            {

                return tagNameToBytes_S;
            }
            else
            {
                tagNameToBytes_S[tagNameToBytes_S.Length] = 0x00;
                return tagNameToBytes_S;
            }
            
        }




        // 构建读取多标签报文
        private OperateResult<byte[]> BuildReadTagsFrame(string tagName, string tagType, string tagValue)
        {
            throw new NotImplementedException();
        }

        // 连接
        public override bool Connect(string iporhost, int port)
        {
            if( base.Connect(iporhost, port))
            {
                Connected = true;
                
            }
            else
            {
                Connected = false;
            }
            return Connected;
        }

        public override void DisConnect()
        {
            base.DisConnect();
        }

        // 注册请求
        public OperateResult<byte[]> Regist()
        {
            // 发送报文 接受报文
            byte[] response = null;
            var receive = SendAndReceive(Registercmd, ref response);
            if (receive.IsSuccess)
            {
                SessionHandle = ByteArrayLib.Get4BytesFromByteArray(response, 4,DataFormat.DCBA);
                return OperateResult.CreateSuccessResult(SessionHandle);
            }
            return OperateResult.CreateFailResult<byte[]>(receive.Message);

        }

        // 注销请求
        public void Unregist()
        {
            byte[] response = null;
            // 发送报文
            SendAndReceive(UnRegistercmd, ref response);
        }
        
        // 写入单标签
        public OperateResult<string> WriteSingleTag(string tagName, string tagType, string tagValue)
        {
            // 发送报文 接受报文
            byte[] response = null;
            var receive = SendAndReceive(BuildWriteSingleTagFrame(tagName, tagType, tagValue).Content, ref response);
            //获取 response 中的最后两个字节
            CIP_State_Code csc = GetStateCode(response,2);
            if (receive.IsSuccess)
            {
                if (csc==CIP_State_Code.成功)
                {
                    return OperateResult.CreateSuccessResult("写入成功");
                }
            }
            
            return OperateResult.CreateFailResult<string>("写入失败");
        }

        private static CIP_State_Code GetStateCode(byte[] response,int index_reverse)
        {
            if (response is null)
            {
                return CIP_State_Code.不支持的封装协议修订;
            }
            int byte_index = response.Length - index_reverse;
            int var1 = ((short)response[byte_index] & 0x00FF) | (response[byte_index + 1] << 8);//错误码
            CIP_State_Code csc = (CIP_State_Code)var1;
            return csc;
        }

        // 读取单标签
        public OperateResult<string> ReadSingleTag(string tagName, string tagType)
        {
            // 发送报文 接受报文
            byte[] response = null;
            var receive = SendAndReceive(BuildReadSingleTagFrame(tagName, tagType).Content, ref response);

            //解析报文
            if (receive.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(AnalysisReadSingleTagFrame(response));
            }
            //获取 response 中的最后两个字节
            CIP_State_Code csc = GetStateCode(response,6);
            return OperateResult.CreateFailResult<string>(csc.ToString());
        }

        // 解析读取数据
        private string AnalysisReadSingleTagFrame(byte[] response)
        {
            int byte_index = 0;
            int var1 = ((short)response[0] & 0x00FF) | (response[1] << 8);//命令码
            int var2 = ((short)response[2] & 0x00FF) | (response[3] << 8);//去除Hearder后的长度 
            if (var2 + 24 != response.Length)//24是Hearder封装头的长度   封装头长度+后面报文长度 应该等于总长度 否则报文错误     
            {
                return string.Empty;
            }
            if (response[4] != SessionHandle[0] || response[5] != SessionHandle[1] || response[6] != SessionHandle[2] || response[7] != SessionHandle[3])
            {
                //如果会话句柄不一致 返回
                return string.Empty;
            }
            if (response[8] != 0x00 || response[9] != 0x00 || response[10] != 0x00 || response[11] != 0x00)
            {
                //会话状态不正确 
                return string.Empty;
            }

            string var3 = "";//发送方描述  转换成ASCII  方便后面比对 
            for (int i = 0; i < 8; i++)
            {
                var3 += ((char)response[i + 12]).ToString();
                byte_index = i + 12;
            }

            //选项 4字节 
            byte_index = byte_index + 4;
            //接口句柄 4字节 
            byte_index = byte_index + 4;
            //超时 2字节 
            byte_index = byte_index + 2;
            //项数 2字节
            byte_index = byte_index + 2;
            //连接的地址项 2字节 
            byte_index = byte_index + 2;
            //连接地址项长度 2字节 
            byte_index = byte_index + 2;
            //未连接数据项 2字节 
            byte_index = byte_index + 2;
            //连接长度 2字节
            byte_index = byte_index + 2;
            for (int i = byte_index + 1; i < response.Length; i++)
            {
                //服务标识 
                if (response[i] == 0xCC)
                {
                    //填充字节  1字节 
                    int index_inner = i + 1;
                    if (response[index_inner] != 0x00 || response[index_inner + 1] != 0x00)
                    {
                        return string.Empty;
                    }
                    //状态 2字节
                    index_inner = index_inner + 2;
                    //数据类型 2字节
                    int tagType = ((short)response[index_inner + 1] & 0x00ff) | ((short)response[index_inner + 2] << 8);
                    index_inner = index_inner + 2;
                    //数据长度 2字节



                    switch (tagType)
                    {//判断数据类型 
                        case 0x00c1:
                            //布尔型数据
                            bool var6 = Convert.ToBoolean(((short)response[index_inner + 1] & 0x00ff) | ((short)response[index_inner + 2] << 8));
                            
                            index_inner = index_inner + 2;
                            i = index_inner;
                            return var6.ToString();
                        case 0x00d2:
                        case 0x00c7:
                            // word
                            // uint
                            ushort var17 = (ushort)(((ushort)response[index_inner + 1] & 0x00ff) | ((ushort)response[index_inner + 2] << 8));
                            index_inner = index_inner + 2;
                            i = index_inner;
                            return var17.ToString();
                        case 0x00c3:
                            //整形
                            int var8 = ((short)response[index_inner + 1] & 0x00ff) | ((short)response[index_inner + 2] << 8);
                            
                            index_inner = index_inner + 2;
                            i = index_inner;
                            return var8.ToString();
                        case 0x00c4:
                            //long型 
                            int var10 = ((short)response[index_inner + 1] & 0x00ff) | ((short)response[index_inner + 2] << 8);
                            int var11 = ((short)response[index_inner + 3] & 0x00ff) | ((short)response[index_inner + 4] << 8);
                            int var12 = (var10 & 0x0000ffff) | (var11 << 16);
                                    
                            index_inner = index_inner + 4;
                            i = index_inner;
                            return var12.ToString();
                        case 0x00C5:
                            //LINT
                            long var25 = BitConverter.ToInt64(ByteArrayLib.Get8BytesFromByteArray(response, index_inner + 1, DataFormat.DCBA), 0);
                            index_inner = index_inner + 8;
                            i = index_inner;
                            return var25.ToString();
                        case 0x00d3:
                        case 0x00c8:
                            //dword
                            //udint
                            int var20 = ((short)response[index_inner + 1] & 0x00ff) | ((short)response[index_inner + 2] << 8);
                            int var21 = ((short)response[index_inner + 3] & 0x00ff) | ((short)response[index_inner + 4] << 8);
                            int var22 = (var20 & 0x0000ffff) | (var21 << 16);

                            index_inner = index_inner + 4;
                            i = index_inner;
                            return var22.ToString();
                        case 0x00c9:
                            //ulint
                            ulong var23 = BitConverter.ToUInt64(ByteArrayLib.Get8BytesFromByteArray(response, index_inner + 1, DataFormat.DCBA), 0);
                            index_inner = index_inner + 4;
                            i = index_inner;
                            return var23.ToString();
                        case 0x00CA:
                            //实型real
                            int var13 = ((short)response[index_inner + 1] & 0x00ff) | ((short)response[index_inner + 2] << 8);
                            int var14 = ((short)response[index_inner + 3] & 0x00ff) | ((short)response[index_inner + 4] << 8);
                            int var15 = (var13 & 0x0000ffff) | (var14 << 16);
                            float var16 = BitConverter.ToSingle(BitConverter.GetBytes(var15), 0);
                            
                            index_inner = index_inner + 4;
                            i = index_inner;
                            return var16.ToString();
                        case 0x00CB:
                            //双精度实型lreal
                            ;
                            double var24 = BitConverter.ToDouble(ByteArrayLib.Get8BytesFromByteArray(response, index_inner + 1, DataFormat.DCBA), 0);
                            
                            index_inner = index_inner + 8;
                            i = index_inner;
                            return var24.ToString();
                        case 0x00d1:
                        case 0x00c6:
                            //Byte型数据
                            //usint
                            byte var7 = response[index_inner + 1];

                            index_inner = index_inner + 1;
                            i = index_inner;
                            return var7.ToString();
                        case 0x0009:
                            // time
                            ulong var30 = BitConverter.ToUInt64 (ByteArrayLib.Get8BytesFromByteArray(response, index_inner + 1, DataFormat.DCBA), 0);

                            index_inner = index_inner + 8;
                            i = index_inner;
                            return var30.ToString();
                        case 0x00d0:
                            // string
                            int stringlength= BitConverter.ToInt16(ByteArrayLib.Get2BytesFromByteArray(response,index_inner+1,DataFormat.DCBA),0);
                            index_inner = index_inner + 2;

                            return Encoding.Default.GetString(response, index_inner+1, stringlength);
                        default:
                            return "读取失败";
                    }
                }

            }

            return "读取失败";
        }
        // 读取多标签

        // 解析写入数据

        // 解析数组数据类型
        public static OperateResult<string,int,int> ParseTagType(string tagType)
        {
            var match = Regex.Match(tagType, @"(?<datatype>\w+)\[(?<start>\d+)\.\.(?<end>\d+)\]");
            if (!match.Success)
            {
                throw new FormatException("Invalid input format.");
            }

            return new OperateResult<string, int, int>
            {
                Content1 = match.Groups["datatype"].Value,
                Content2 = int.Parse(match.Groups["start"].Value),
                Content3 = int.Parse(match.Groups["end"].Value),
            };
        }


    }
}
