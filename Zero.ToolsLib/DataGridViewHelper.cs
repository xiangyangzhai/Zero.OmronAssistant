using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zero.ToolsLib
{
    public class DataGridViewHelper
    {
        /// <summary>
        /// ��DataGridView����к�
        /// </summary>
        /// <param name="dgv">dgv�ؼ�</param>
        /// <param name="e">dgv����</param>
        public static void DgvRowPostPaint(DataGridView dgv, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //����к� 
                SolidBrush solidBrush = new SolidBrush(dgv.RowHeadersDefaultCellStyle.ForeColor);
                string lineNo = (e.RowIndex + 1).ToString();
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(lineNo, e.InheritedRowStyle.Font, solidBrush, new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgv.RowHeadersWidth, dgv.RowTemplate.Height), sf);
            }
            catch (Exception ex)
            {
                MessageBox.Show("����к�ʱ�������󣬴�����Ϣ��" + ex.Message, "����ʧ��");
            }
        }

        /// <summary>
        /// ��DataGridView���Ʊ߿�
        /// </summary>
        /// <param name="dgv">dgv�ؼ�</param>
        /// <param name="e">dgv����</param>
        public static void DgvRowPaint(DataGridView dgv, PaintEventArgs e, Color borderColor)
        {
            e.Graphics.DrawRectangle(new Pen(borderColor), new Rectangle(0, 0, dgv.Width - 1, dgv.Height - 1));
        }

        /// <summary>
        /// ��ż��ɫ
        /// </summary>        
        public static void DgvStyle(DataGridView dgv, Color defaultBackColor, Color alternatingBackColor, Color gridColor)
        {
            //�����еı���ɫ
            dgv.AlternatingRowsDefaultCellStyle.BackColor = alternatingBackColor;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = alternatingBackColor;

            //Ĭ�ϵ�����ʽ
            dgv.RowsDefaultCellStyle.BackColor = defaultBackColor;
            dgv.RowsDefaultCellStyle.SelectionBackColor = defaultBackColor;


            dgv.RowHeadersDefaultCellStyle.BackColor = defaultBackColor;
            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = defaultBackColor;

            //����������ɫ
            dgv.GridColor = gridColor;
        }
    }
}
