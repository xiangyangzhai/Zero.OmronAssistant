using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.CommunicationLib.Base;
using Zero.DataConvertLib;

namespace Zero.CommunicationLib.Library
{
    public class CIP: NetDeviceBase
    {
        /// <summary>
        /// 构造方法，欧姆龙PLC的大小端默认是CDAB
        /// </summary>
        /// <param name="dataFormat"></param>
        public CIP(DataFormat dataFormat=DataFormat.CDAB) 
        {
            this.DataFormat = dataFormat;
        }
        //注册会话ID
        private byte[] Registercmd = new byte[28]
        {

　　        //--------------------------------------------------------Header 24byte-------------------------------------
　　        0x6F,0x00,//命令 2byte
　　        0x04,0x00,//Header后面数据的长度 2byte
　　        0x00,0x00,0x00,0x00,//会话句柄 4byte
　　        0x00,0x00,0x00,0x00,//状态默认0 4byte
　　        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,//发送方描述默认0 8byte
　　        0x00,0x00,0x00,0x00,//选项默认0 4byte

               //-------------------------------------------------------CommandSpecificData 指令指定数据 4byte

　　        0x01,0x00,//协议版本 2byte

　　        0x00,0x00,//选项标记 2byte
        };

        //提取会话ID-注册请求的应答报文
        private byte[] RefRegistercmd = new byte[28]
        {

　　        //--------------------------------------------------------Header 24byte-------------------------------------
　　        0x6F,0x00,//命令 2byte
　　        0x04,0x00,//CommandSpecificData的长度 2byte
　　        0x6B,0x01,0x01,0x00,//会话句柄 4byte 由PLC生成
　　        0x00,0x00,0x00,0x00,//状态默认0 4byte
　　        0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,//发送方描述默认0 8byte
　　        0x00,0x00,0x00,0x00,//选项默认0 4byte

           //-------------------------------------------------------CommandSpecificData 指令指定数据 4byte

　　        0x01,0x00,//协议版本 2byte

　　        0x00,0x00,//选项标记 2byte
        };

        public byte[] SessionHandle = new byte[4] { 0x6B, 0x01, 0x01, 0x00 };//从应答报文提取的会话ID，后续读写PLC的报文中，需要包含PLC返回的会话ID

        //读数据服务请求报文
        //报文由三部分组成 Header 24个字节 、CommandSpecificData 16个字节、以及CIP消息（由读取的标签生成）
        //实例，读取单个标签名为 TAG1的报文总长度为64个字节
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
            0x22,06,0x24,0x01,//请求路径 默认0x01240622 4byte
　　        0x0A,0xF0,//超时默认0xF00A 4byte
　　        0x0A,0x00,//Cip指令长度  服务标识到服务命令指定数据的长度 
　　        0x4C,//服务标识固定为0x4C 1byte  
　　        0x03,// 节点长度 2byte  规律为 (标签名的长度+1/2)+1
　　        0x91,//扩展符号 默认为 0x91
　　        0x04,//标签名的长度
　　        0x54,0x41,0x47,0x31,//标签名 ：TAG1转换成ASCII字节 当标签名的长度为奇数时，需要在末尾补0  比如TAG转换成ASCII为0x54,0x41,0x47，需要在末尾补0 变成 0x54,0x41,0x47，0
            0x01,0x00,//服务命令指定数据　默认为0x0001　
　          0x01,0x00,0x01,0x00//最后一位是PLC的槽号
        };

        
    }
}
