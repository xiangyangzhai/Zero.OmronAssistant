using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.CommunicationLib.StoreArea
{
    public  class OmronFinsStoreArea
    {
        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="bitCode"></param>
        /// <param name="wordCode"></param>
        public OmronFinsStoreArea(byte bitCode, byte wordCode)
        {
            BitCode = bitCode;
            WordCode = wordCode;
        }


        /// <summary>
        /// λ����
        /// </summary>
        public byte BitCode { get; set; }

        /// <summary>
        /// �ִ���
        /// </summary>
        public byte WordCode { get; set; }

        /// <summary>
        /// DM�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea DM = new OmronFinsStoreArea(0x02,0x82);

        /// <summary>
        /// CIO�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea CIO = new OmronFinsStoreArea(0x30, 0xB0);

        /// <summary>
        /// WR�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea WR = new OmronFinsStoreArea(0x31, 0xB1);

        /// <summary>
        /// HR�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea HR = new OmronFinsStoreArea(0x32, 0xB2);

        /// <summary>
        /// AR�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea AR = new OmronFinsStoreArea(0x33, 0xB3);


        /// <summary>
        /// E0�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E0 = new OmronFinsStoreArea(0x20, 0xA0);

        /// <summary>
        /// E1�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E1 = new OmronFinsStoreArea(0x21, 0xA1);

        /// <summary>
        /// E2�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E2 = new OmronFinsStoreArea(0x22, 0xA2);

        /// <summary>
        /// E3�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E3 = new OmronFinsStoreArea(0x23, 0xA3);

        /// <summary>
        /// E4�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E4 = new OmronFinsStoreArea(0x24, 0xA4);

        /// <summary>
        /// E5�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E5 = new OmronFinsStoreArea(0x25, 0xA5);

        /// <summary>
        /// E6�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E6 = new OmronFinsStoreArea(0x26, 0xA6);

        /// <summary>
        /// E7�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E7 = new OmronFinsStoreArea(0x27, 0xA7);

        /// <summary>
        /// E8�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E8 = new OmronFinsStoreArea(0x28, 0xA8);

        /// <summary>
        /// E9�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E9 = new OmronFinsStoreArea(0x29, 0xA9);

        /// <summary>
        /// E10�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E10 = new OmronFinsStoreArea(0x2A, 0xAA);

        /// <summary>
        /// E11�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E11 = new OmronFinsStoreArea(0x2B, 0xAB);

        /// <summary>
        /// E12�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E12 = new OmronFinsStoreArea(0x2C, 0xAC);

        /// <summary>
        /// E13�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E13 = new OmronFinsStoreArea(0x2D, 0xAD);

        /// <summary>
        /// E14�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E14 = new OmronFinsStoreArea(0x2E, 0xAE);

        /// <summary>
        /// E15�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E15 = new OmronFinsStoreArea(0x2F, 0xAF);


        /// <summary>
        /// E16�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E16 = new OmronFinsStoreArea(0xE0, 0x60);

        /// <summary>
        /// E17�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E17 = new OmronFinsStoreArea(0xE1, 0x61);

        /// <summary>
        /// E18�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E18 = new OmronFinsStoreArea(0xE2, 0x62);

        /// <summary>
        /// E19�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E19 = new OmronFinsStoreArea(0xE3, 0x63);

        /// <summary>
        /// E20�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E20 = new OmronFinsStoreArea(0xE4, 0x64);

        /// <summary>
        /// E21�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E21 = new OmronFinsStoreArea(0xE5, 0x65);

        /// <summary>
        /// E22�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E22 = new OmronFinsStoreArea(0xE6, 0x66);

        /// <summary>
        /// E23�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E23 = new OmronFinsStoreArea(0xE7, 0x67);

        /// <summary>
        /// E24�洢��
        /// </summary>
        public readonly static OmronFinsStoreArea E24 = new OmronFinsStoreArea(0xE8, 0x68);



    }
}
