using System;
using System.Collections;
using System.Net.Configuration;
using System.Text;
using Zero.CommunicationLib.Base;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib.Library
{
    public class CIP : NetDeviceBase
    {
        /// <summary>
        /// 构造方法，欧姆龙PLC的大小端默认是CDAB
        /// </summary>
        /// <param name="dataFormat"></param>
        public CIP(DataFormat dataFormat = DataFormat.CDAB)
        {
            this.DataFormat = dataFormat;
        }


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

               //-------------------------------------------------------CommandSpecificData 指令指定数据 4byte

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
　　        0x04,0x00,//CommandSpecificData的长度 2byte
　　        0x6B,0x01,0x01,0x00,//会话句柄 4byte 由PLC生成
　　        0x00,0x00,0x00,0x00,//状态默认0 4byte
　　        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,//发送方描述默认0 8byte
　　        0x00,0x00,0x00,0x00,//选项默认0 4byte

           //-------------------------------------------------------CommandSpecificData 指令指定数据 4byte

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
        //报文由三部分组成 Header 24个字节 、CommandSpecificData 16个字节、以及CIP消息（由读取的标签生成）
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

        private byte[] CommandSpecificData = new byte[]
        {
            0x00,0x00,0x00,0x00,//接口句柄 CIP默认为0x00000000 4byte
　　        0x01,0x00,//超时默认0x0001 4byte
　　        0x02,0x00,//项数默认0x0002 4byte  
　　        0x00,0x00,//空地址项默认0x0000 2byte
　　        0x00,0x00,//长度默认0x0000 2byte
　　        0xb2,0x00,//未连接数据项默认为 0x00b2
　　        0x18,0x00,//后面数据包的长度 24个字节(总长度-Header的长度-CommandSpecificData的长度)
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
        private byte[] SessionHandle = new byte[] { 0x00, 0x00, 0x00, 0x00 };

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
        private byte[] DataType = new byte[] {0xB2, 0x00};

        /// <summary>
        /// 数据长度
        /// </summary>
        private byte[] DataLength = new byte[] { 0x00, 0x00 };

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
            Cip_Msg.Add((byte)TagStringToAscii.Length);//PLC标签长度 多少个字节
            ArrayList Var6 = new ArrayList(TagStringToAscii);
            Cip_Msg.AddRange(Var6);//添加标签 
            Cip_Msg.AddRange(getBytesFromTagType(tagType)); // 数据类型
            Cip_Msg.Add((byte)0x01); Cip_Msg.Add((byte)0x00); // 写入数量
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
            Header.Add((byte)array[0]); Header.Add((byte)array[1]); Header.Add((byte)array[2]); Header.Add((byte)array[3]);//添加发送方描述
            Header.Add((byte)array[4]); Header.Add((byte)array[5]); Header.Add((byte)array[6]); Header.Add((byte)array[7]);
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
            throw new NotImplementedException();
        }

        private byte[] getBytesFromTagType(string tagType)
        {
            throw new NotImplementedException();
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
            //CIP协议数据  =================================================
            ArrayList Cip_Msg = new ArrayList();
            Cip_Msg.Add((byte)0x4C);//服务标识
            Cip_Msg.Add((byte)((TagStringToAscii.Length + 2) / 2));//CIP长度多少字  从标识开始 大读取标签长度结束 
            Cip_Msg.Add((byte)0X91);//固定
            Cip_Msg.Add((byte)TagStringToAscii.Length);//PLC标签长度 多少个字节
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
            Header.Add((byte)array[0]); Header.Add((byte)array[1]); Header.Add((byte)array[2]); Header.Add((byte)array[3]);//添加发送方描述
            Header.Add((byte)array[4]); Header.Add((byte)array[5]); Header.Add((byte)array[6]); Header.Add((byte)array[7]);
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
            return base.Connect(iporhost, port);

        }

        // 注册请求

        // 注销请求

        // 写入单标签

        // 读取单标签

        // 读取多标签

        // 解析写入数据

        // 解析读取数据


    }
}
