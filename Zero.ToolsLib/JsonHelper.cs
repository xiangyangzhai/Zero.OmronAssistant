using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Zero.ToolsLib
{
    public class JsonHelper
    {
        /// <summary>
        /// ʹ��Newtonsoft.json.dll�������л���Json�ַ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string EntityToJson<T>(T t)
        {
            try
            {
                return JsonConvert.SerializeObject(t);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// ʹ��Newtonsoft.json.dll ��Json�ַ��������л��ɶ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToEntity<T>(string json)
        {
            try
            {
                return (T)JsonConvert.DeserializeObject(json,typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// ʵ����ת����JSON�ַ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string EntityToJson2<T>(T obj)
        {
            //���л�
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            MemoryStream msObj = new MemoryStream();
            //�����л�֮���Json��ʽ����д������
            js.WriteObject(msObj, obj);
            msObj.Position = 0;
            //��0���λ�ÿ�ʼ��ȡ���е�����
            StreamReader sr = new StreamReader(msObj, Encoding.Default);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();
            return json;
        }


        /// <summary>
        /// JSON�ַ���ת����ʵ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToEntity2<T>(string json) where T : class
        {
            //�����л�
            using (var ms = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(T));
                T model = (T)deseralizer.ReadObject(ms);// //�����л�ReadObject
                return model;
            }
        }



    }
}
