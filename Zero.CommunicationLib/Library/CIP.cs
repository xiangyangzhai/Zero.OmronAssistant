using System;
using System.Net.Configuration;
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

        #region 注册请求
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

        /// <summary>
        /// 从应答报文提取的会话ID，后续读写PLC的报文中，需要包含PLC返回的会话ID
        /// </summary>
        //public byte[] SessionHandle = new byte[4] { 0x6B, 0x01, 0x01, 0x00 };

        #endregion

        #region 单标签读取
        //读数据服务请求报文
        //报文由三部分组成 Header 24个字节 、CommandSpecificData 16个字节、以及EIP消息（由读取的标签生成）
        //实例，读取单个标签名为 TAG1的报文总长度为64个字节
        /// <summary>
        /// 封装头
        /// </summary>
        private byte[] Header = new byte[24]
        {
            0x6F,0x00,//命令 2byte
　　        0x28,0x00,//长度 2byte（总长度-Header的长度）=40 
　　        0x6B,0x01,0x01,0x00,//会话句柄 4byte
　　        0x00,0x00,0x00,0x00,//状态默认0 4byte
　　        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,//发送方描述默认0 8byte
　　        0x00,0x00,0x00,0x00,//选项默认0 4byte
        };

        private byte[] CommandSpecificData = new byte[16]
        {
            0x00,0x00,0x00,0x00,//接口句柄 CIP默认为0x00000000 4byte
　　        0x01,0x00,//超时默认0x0001 4byte
　　        0x02,0x00,//项数默认0x0002 4byte  
　　        0x00,0x00,//空地址项默认0x0000 2byte
　　        0x00,0x00,//长度默认0x0000 2byte
　　        0xb2,0x00,//未连接数据项默认为 0x00b2
　　        0x18,0x00,//后面数据包的长度 24个字节(总长度-Header的长度-CommandSpecificData的长度)
        };

        private byte[] CipMessage = new byte[24]
        {
            0x52,0x02,　　   //服务默认0x52  请求路径大小 默认2
            0x20,06,0x24,0x01,//请求路径 默认0x01240622 4byte
　　        0x0A,0xF0,0x0A,0x00,//超时默认0xF00A 4byte
　　        0x4C,//服务标识固定为0x4C 1byte  
　　        0x03,// CIP长度多少字
　　        0x91,//扩展符号 默认为 0x91  固定
　　        0x04,//PLC标签长度 多少个字节
　　        0x54,0x41,0x47,0x31,//标签名 ：TAG1转换成ASCII字节 当标签名的长度为奇数时，需要在末尾补0  比如TAG转换成ASCII为0x54,0x41,0x47，需要在末尾补0 变成 0x54,0x41,0x47，0
            0x01,0x00,//服务命令指定数据　默认为0x0001　
　          0x01,0x00,0x01,0x00//最后一位是PLC的槽号
        };



        #endregion

        #region 读取多标签

        #endregion

        //todo:1. 规范报文字段命名
        //todo:2. 添加报文字段
        //todo:3. 

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


        private byte CIPLength = 0x03;

        // 构建注册请求报文
        private OperateResult<byte[]> BuildRegisteSessionFrame()
        {
            ByteArray SendCommand = new ByteArray();

            //CommandCode
            SendCommand.Add(new byte[] { 0x65, 0x00 });

            //MessageLength
            SendCommand.Add(Length);

            SendCommand.Add(SessionHandle);

            SendCommand.Add(Status);

            SendCommand.Add(SenderContext);

            SendCommand.Add(Options);

            SendCommand.Add(ProtocolVersion);

            SendCommand.Add(OptionID);

            return OperateResult.CreateSuccessResult(SendCommand.array);
        }

        // 构建注销请求报文
        private OperateResult<byte[]> BuildUnRegisteSessionFrame()
        {
            ByteArray SendCommand = new ByteArray();

            //CommandCode
            SendCommand.Add(new byte[] { 0x66, 0x00 });

            //MessageLength
            SendCommand.Add(new byte[] { 0x00, 0x00 });

            SendCommand.Add(SessionHandle);

            //StateCode
            SendCommand.Add(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            //SendContext
            SendCommand.Add(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

            //Option
            SendCommand.Add(new byte[] { 0x00, 0x00, 0x00, 0x00 });

            return OperateResult.CreateSuccessResult(SendCommand.array);
        }

        // 构建写入单标签报文
        private OperateResult<byte[]> BuildWriteSingleTagFrame(string tagName, string tagType, string tagValue)
        {
            throw new NotImplementedException();
        }

        // 构建读取单标签报文
        private OperateResult<byte[]> BuildReadSingleTagFrame(string tagName, string tagType)
        {
            throw new NotImplementedException();
        }

        // 构建读取多标签报文
        private OperateResult<byte[]> BuildReadTagsFrame(string tagName, string tagType, string tagValue)
        {
            throw new NotImplementedException();
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
