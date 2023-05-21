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
    public class NetDeviceBase : ReadWriteBase
    {
        //����һ��Socket����
        private Socket socket;

        /// <summary>
        /// ���ӳ�ʱʱ��
        /// </summary>
        public int ConnectTimeOut { get; set; } = 2000;

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
        /// ��������
        /// </summary>
        /// <param name="iporhost"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public virtual  bool Connect(string iporhost, int port)
        {
            //ʵ����Socket����
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //���ó�ʱʱ��
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, this.ReadTimeOut);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, this.WriteTimeOut);

            try
            {
                IAsyncResult asyncResult = socket.BeginConnect(iporhost, port, null, null);

                bool connectResult = asyncResult.AsyncWaitHandle.WaitOne(ConnectTimeOut, false);

                if (connectResult == false)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// �Ͽ�����
        /// </summary>
        public virtual void DisConnect()
        {
            if (socket != null)
            {
                socket.Close();            
            }       
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
                //���ͱ���
                socket.Send(request,  request.Length,SocketFlags.None);

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
                            int count = socket.Receive(buffer,  SocketFlags.None);

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
                        byte[] headData = ReceiveMessage(message.HeadDataLength);

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

                    byte[] contentData = ReceiveMessage(contentLength);

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
        private byte[] ReceiveMessage(int length)
        {
            byte[] buffer = new byte[length];

            //�Ѿ���ȡ�ĳ���
            int numBytesRead = 0;

            while (numBytesRead < length)
            {
                //��SocketBufferSize�Աȣ�ȡ��Сֵ
                int count = Math.Min(length - numBytesRead, SocketBufferSize);

                int readCount = socket.Receive(buffer, numBytesRead, count,SocketFlags.None);

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
