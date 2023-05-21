using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zero.CommunicationLib.Interface;
using Zero.DataConvertLib;
using Zero.Models;

namespace Zero.CommunicationLib.Base
{
    public class UDPDeviceBase:ReadWriteBase
    {
        /// <summary>
        /// 读取超时时间
        /// </summary>
        public int ReadTimeOut { get; set; } = 2000;

        /// <summary>
        /// 写入超时时间
        /// </summary>
        public int WriteTimeOut { get; set; } = 2000;

        /// <summary>
        /// 锁对象
        /// </summary>
        public SimpleHybirdLock simpleHybirdLock { get; set; } = new SimpleHybirdLock();

        /// <summary>
        /// 请求报文（存储）
        /// </summary>
        public byte[] Request { get; set; }

        /// <summary>
        /// 返回报文（存储）
        /// </summary>
        public byte[] Response { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int ReceiveTimeOut { get; set; } = 2000;

        /// <summary>
        /// 延时时间
        /// </summary>
        public int SleepTime { get; set; } = 5;

        /// <summary>
        /// 以太网缓冲区大小设置
        /// </summary>
        private const int SocketBufferSize = 8192;

        /// <summary>
        /// IP地址
        /// </summary>
        private string ip = "127.0.0.1";

        /// <summary>
        /// 端口号
        /// </summary>
        public int port = 1000;

        /// <summary>
        /// 初始化IP地址和端口
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Init(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        /// <summary>
        /// 发送并接收
        /// </summary>
        /// <param name="request">字节数组</param>
        /// <param name="response">返回报文</param>
        /// <param name="message">IMessage对象</param>
        /// <returns>是否成功</returns>
        public OperateResult SendAndReceive(byte[] request, ref byte[] response, IMessage message = null)
        {
            simpleHybirdLock.Enter();

            //请求报文赋值
            Request = request;

            //定义一个内存
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(this.ip), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //设置超时时间
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, this.ReceiveTimeOut);

                //发送报文
                socket.SendTo(  request, request.Length, SocketFlags.None,endPoint);

                //任意的IP地址和端口号均接收
                //EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

                EndPoint remote = new IPEndPoint(IPAddress.Parse(this.ip), port);

                //按照时间接收
                if (message == null)
                {
                    DateTime start = DateTime.Now;

                    byte[] buffer = new byte[1024];

                    while (true)
                    {
                        Thread.Sleep(SleepTime);

                        if (socket.Available > 0)
                        {
                            int count = socket.ReceiveFrom(buffer, ref remote);

                            memoryStream.Write(buffer, 0, count);
                        }
                        else
                        {
                            //判断超时
                            if ((DateTime.Now - start).TotalMilliseconds > this.ReceiveTimeOut)
                            {
                                memoryStream.Dispose();
                                return OperateResult.CreateFailResult("读取超时");
                            }
                            else if (memoryStream.Length > 0)
                            {
                                break;
                            }
                        }
                    }
                }
                //按照消息接收
                else
                {
                    if (message.HeadDataLength > 0)
                    {
                        byte[] headData = ReceiveMessage(message.HeadDataLength,socket,remote);

                        if (message.CheckHeadData(headData) == false)
                        {
                            return OperateResult.CreateFailResult("包头报文验证不通过");
                        }
                        else
                        {
                            memoryStream.Write(headData, 0, headData.Length);
                        }
                    }

                    int contentLength = message.GetContentLength();

                    byte[] contentData = ReceiveMessage(contentLength,socket,endPoint);

                    memoryStream.Write(contentData, 0, contentData.Length);
                }

                //获取接收报文
                response = memoryStream.ToArray();

                Response = response;

                memoryStream.Dispose();

                return OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult(ex.Message);
            }
            finally
            {
                //解锁
                simpleHybirdLock.Leave();
            }
        }

        /// <summary>
        /// 从缓冲区读取指定的长度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] ReceiveMessage(int length,Socket socket,EndPoint remote)
        {
            byte[] buffer = new byte[length];

            //已经读取的长度
            int numBytesRead = 0;

            while (numBytesRead < length)
            {
                //跟SocketBufferSize对比，取较小值
                int count = Math.Min(length - numBytesRead, SocketBufferSize);

                int readCount = socket.ReceiveFrom(buffer, numBytesRead, count, SocketFlags.None,ref remote);

                numBytesRead += readCount;

                if (readCount == 0)
                {
                    throw new Exception("无法读取到数据");
                }
            }
            return buffer;
        }
    }
}
