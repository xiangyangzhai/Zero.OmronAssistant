using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using Zero.DataConvertLib;
using Zero.CommunicationLib.Interface;
using System.Threading;
using System.IO;
using Zero.Models;

namespace Zero.CommunicationLib.Base
{
    public class SerialDeviceBase : ReadWriteBase
    {

        /// <summary>
        /// ����ͨ�Ŷ���
        /// </summary>
        private SerialPort serialPort { get; set; } = new SerialPort();

        /// <summary>
        /// ��ȡ��ʱʱ��
        /// </summary>
        public int ReadTimeOut { get; set; } = 2000;

        /// <summary>
        /// д�볬ʱʱ��
        /// </summary>
        public int WriteTimeOut { get; set; } = 2000;

        /// <summary>
        /// DtrEnable
        /// </summary>
        public bool DtrEnable { get; set; } = false;

        /// <summary>
        /// RtsEnable
        /// </summary>
        public bool RtsEnable { get; set; } = false;

        /// <summary>
        /// ������
        /// </summary>
        public SimpleHybirdLock simpleHybirdLock { get; set; } = new SimpleHybirdLock();

        /// <summary>
        /// ��ʱʱ��
        /// </summary>
        public int ReceiveTimeOut { get; set; } = 2000;

        /// <summary>
        /// ��ʱʱ��
        /// </summary>
        public int SleepTime { get; set; } = 20;

        /// <summary>
        /// �����ģ��洢��
        /// </summary>
        public byte[] Request { get; set; }

        /// <summary>
        /// ���ر��ģ��洢��
        /// </summary>
        public byte[] Response { get; set; }

        /// <summary>
        /// ���ڻ�������С����
        /// </summary>
        private const int SerialBufferSize = 1024;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="dataBits"></param>
        /// <param name="parity"></param>
        /// <param name="stopBits"></param>
        /// <returns></returns>
        public bool Connect(string portName, int baudRate = 9600, int dataBits = 8, Parity parity = Parity.None, StopBits stopBits = StopBits.One)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }

            //���ô�������
            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.DataBits = dataBits;
            serialPort.Parity = parity;
            serialPort.StopBits = stopBits;

            serialPort.ReadTimeout = ReadTimeOut;
            serialPort.WriteTimeout = WriteTimeOut;
            serialPort.RtsEnable = RtsEnable;
            serialPort.DtrEnable = DtrEnable;

            try
            {
                serialPort.Open();
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
        public void DisConnect()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }


        /// <summary>
        /// ���Ͳ�����
        /// </summary>
        /// <param name="request">������</param>
        /// <param name="response">���ձ���</param>
        /// <param name="message">��Ϣ����</param>
        /// <returns>�������</returns>
        public OperateResult SendAndReceive(byte[] request, ref byte[] response, IMessage message = null)
        {
            //����
            simpleHybirdLock.Enter();

            //�����ĸ�ֵ
            Request = request;

            //����һ���ڴ�
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                //���ͱ���
                serialPort.Write(request, 0, request.Length);

                //����ʱ�����
                if (message == null)
                {
                    DateTime start = DateTime.Now;
                    while (true)
                    {
                        Thread.Sleep(SleepTime);

                        byte[] buffer = new byte[1024];
                        if (serialPort.BytesToRead > 0)
                        {
                            int count = serialPort.Read(buffer, 0, buffer.Length);
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
                //��SerialBufferSize�Աȣ�ȡ��Сֵ
                int count = Math.Min(length - numBytesRead, SerialBufferSize);

                int readCount = serialPort.Read(buffer, numBytesRead, count);

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
