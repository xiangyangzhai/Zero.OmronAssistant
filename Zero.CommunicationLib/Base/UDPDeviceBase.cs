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
        /// ��ȡ��ʱʱ��
        /// </summary>
        public int ReadTimeOut { get; set; } = 2000;

        /// <summary>
        /// д�볬ʱʱ��
        /// </summary>
        public int WriteTimeOut { get; set; } = 2000;

        /// <summary>
        /// ������
        /// </summary>
        public SimpleHybirdLock simpleHybirdLock { get; set; } = new SimpleHybirdLock();

        /// <summary>
        /// �����ģ��洢��
        /// </summary>
        public byte[] Request { get; set; }

        /// <summary>
        /// ���ر��ģ��洢��
        /// </summary>
        public byte[] Response { get; set; }

        /// <summary>
        /// ��ʱʱ��
        /// </summary>
        public int ReceiveTimeOut { get; set; } = 2000;

        /// <summary>
        /// ��ʱʱ��
        /// </summary>
        public int SleepTime { get; set; } = 5;

        /// <summary>
        /// ��̫����������С����
        /// </summary>
        private const int SocketBufferSize = 8192;

        /// <summary>
        /// IP��ַ
        /// </summary>
        private string ip = "127.0.0.1";

        /// <summary>
        /// �˿ں�
        /// </summary>
        public int port = 1000;

        /// <summary>
        /// ��ʼ��IP��ַ�Ͷ˿�
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Init(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        /// <summary>
        /// ���Ͳ�����
        /// </summary>
        /// <param name="request">�ֽ�����</param>
        /// <param name="response">���ر���</param>
        /// <param name="message">IMessage����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public OperateResult SendAndReceive(byte[] request, ref byte[] response, IMessage message = null)
        {
            simpleHybirdLock.Enter();

            //�����ĸ�ֵ
            Request = request;

            //����һ���ڴ�
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(this.ip), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //���ó�ʱʱ��
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, this.ReceiveTimeOut);

                //���ͱ���
                socket.SendTo(  request, request.Length, SocketFlags.None,endPoint);

                //�����IP��ַ�Ͷ˿ںž�����
                //EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

                EndPoint remote = new IPEndPoint(IPAddress.Parse(this.ip), port);

                //����ʱ�����
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
                            //�жϳ�ʱ
                            if ((DateTime.Now - start).TotalMilliseconds > this.ReceiveTimeOut)
                            {
                                memoryStream.Dispose();
                                return OperateResult.CreateFailResult("��ȡ��ʱ");
                            }
                            else if (memoryStream.Length > 0)
                            {
                                break;
                            }
                        }
                    }
                }
                //������Ϣ����
                else
                {
                    if (message.HeadDataLength > 0)
                    {
                        byte[] headData = ReceiveMessage(message.HeadDataLength,socket,remote);

                        if (message.CheckHeadData(headData) == false)
                        {
                            return OperateResult.CreateFailResult("��ͷ������֤��ͨ��");
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

                //��ȡ���ձ���
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
                //����
                simpleHybirdLock.Leave();
            }
        }

        /// <summary>
        /// �ӻ�������ȡָ���ĳ���
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] ReceiveMessage(int length,Socket socket,EndPoint remote)
        {
            byte[] buffer = new byte[length];

            //�Ѿ���ȡ�ĳ���
            int numBytesRead = 0;

            while (numBytesRead < length)
            {
                //��SocketBufferSize�Աȣ�ȡ��Сֵ
                int count = Math.Min(length - numBytesRead, SocketBufferSize);

                int readCount = socket.ReceiveFrom(buffer, numBytesRead, count, SocketFlags.None,ref remote);

                numBytesRead += readCount;

                if (readCount == 0)
                {
                    throw new Exception("�޷���ȡ������");
                }
            }
            return buffer;
        }
    }
}
