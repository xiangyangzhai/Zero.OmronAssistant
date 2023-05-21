using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

namespace Zero.ToolsLib
{
    public class ExcelHelper
    {
        #region 1.��DataTable/List������excel�ļ��У�֧��xls��xlsx��ʽ


        /// <summary>
        ///  DataTable������Excel�ļ�
        /// </summary>
        /// <param name="dtSource">����Դ</param>
        /// <param name="strHeaderText">����</param>
        /// <param name="strFileName">excel�ļ���</param>
        /// <param name="dir">datatable��excel������Ӧ�ֵ�</param>
        /// <param name="sheetRow">ÿ��sheet��ŵ�����</param>
        public static void DataTableToExcel(DataTable dtSource, string strHeaderText, string strFileName, Dictionary<string, string> dir, bool isNew, int sheetRow = 50000)
        {
            int currentSheetCount = GetSheetNumber(strFileName);//���е�ҳ��sheetnum
            if (sheetRow <= 0)
            {
                sheetRow = dtSource.Rows.Count;
            }
            string[] temp = strFileName.Split('.');
            string fileExtens = temp[temp.Length - 1];
            int sheetCount = (int)Math.Ceiling((double)dtSource.Rows.Count / sheetRow);//sheet��Ŀ
            if (temp[temp.Length - 1] == "xls" && dtSource.Columns.Count < 256 && sheetRow < 65536)
            {
                if (isNew)
                {
                    currentSheetCount = 0;
                }
                for (int i = currentSheetCount; i < currentSheetCount + sheetCount; i++)
                {
                    DataTable pageDataTable = dtSource.Clone();
                    int hasRowCount = dtSource.Rows.Count - sheetRow * (i - currentSheetCount) < sheetRow ? dtSource.Rows.Count - sheetRow * (i - currentSheetCount) : sheetRow;
                    for (int j = 0; j < hasRowCount; j++)
                    {
                        pageDataTable.ImportRow(dtSource.Rows[(i - currentSheetCount) * sheetRow + j]);
                    }

                    using (MemoryStream ms = ExportDataTable(strFileName, pageDataTable, strHeaderText, dir, i))
                    {
                        using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                        {

                            byte[] data = ms.ToArray();
                            fs.Write(data, 0, data.Length);
                            fs.Flush();
                        }
                    }
                }
            }
            else
            {
                if (temp[temp.Length - 1] == "xls")
                    strFileName = strFileName + "x";
                if (isNew)
                {
                    currentSheetCount = 0;
                }
                for (int i = currentSheetCount; i < currentSheetCount + sheetCount; i++)
                {
                    DataTable pageDataTable = dtSource.Clone();
                    int hasRowCount = dtSource.Rows.Count - sheetRow * (i - currentSheetCount) < sheetRow ? dtSource.Rows.Count - sheetRow * (i - currentSheetCount) : sheetRow;
                    for (int j = 0; j < hasRowCount; j++)
                    {
                        pageDataTable.ImportRow(dtSource.Rows[(i - currentSheetCount) * sheetRow + j]);
                    }
                    FileStream readfs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Read);
                    MemoryStream readfsm = new MemoryStream();
                    readfs.CopyTo(readfsm);
                    readfs.Close();
                    using (FileStream writefs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                    {

                        ExportDataTable(pageDataTable, strHeaderText, writefs, readfsm, dir, i);
                    }
                    readfsm.Close();
                }
            }
        }

        /// <summary>
        /// ��List���ϵ�����Excel�ļ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="strHeaderText"></param>
        /// <param name="strFileName"></param>
        /// <param name="dir"></param>
        /// <param name="isNew"></param>
        /// <param name="sheetRow"></param>
        public static void ListToExcel<T>(List<T> list, string strHeaderText, string strFileName, Dictionary<string, string> dir, bool isNew, int sheetRow = 50000)
        {
            DataTable dataTable = ListToDataTable<T>(list);

            DataTableToExcel(dataTable, strHeaderText, strFileName, dir, isNew, sheetRow);
        }


        /// <summary>
        /// ����Ϊxls�ļ��ڲ�����
        /// </summary>
        /// <param name="strFileName">excel�ļ���</param>
        /// <param name="dtSource">datatabeԴ����</param>
        /// <param name="strHeaderText">����</param>
        /// <param name="sheetnum">sheet�ı��</param>
        /// <returns></returns>
        private static MemoryStream ExportDataTable(string strFileName, DataTable dtSource, string strHeaderText, Dictionary<string, string> dir, int sheetnum)
        {
            //������������sheet
            IWorkbook workbook = new HSSFWorkbook();
            using (Stream writefile = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (writefile.Length > 0 && sheetnum > 0)
                {
                    workbook = WorkbookFactory.Create(writefile);
                }
            }

            ISheet sheet = null;
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region �½�������ͷ�������ͷ����ʽ
                if (rowIndex == 0)
                {
                    string sheetName = strHeaderText + (sheetnum == 0 ? "" : sheetnum.ToString());
                    if (workbook.GetSheetIndex(sheetName) >= 0)
                    {
                        workbook.RemoveSheetAt(workbook.GetSheetIndex(sheetName));
                    }
                    sheet = workbook.CreateSheet(sheetName);
                    #region ��ͷ����ʽ
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.IsBold = true;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;

                        rowIndex = 1;
                    }
                    #endregion

                    #region ��ͷ����ʽ

                    if (rowIndex == 1)
                    {
                        IRow headerRow = sheet.CreateRow(1);//�ڶ�����������
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.IsBold = true;
                        headStyle.SetFont(font);
                        //д���б���
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(dir[column.ColumnName]);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //�����п�
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256 * 2);
                        }
                        rowIndex = 2;
                    }
                    #endregion
                }
                #endregion

                #region �������

                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String": //�ַ�������
                            double result;
                            if (isNumeric(drValue, out result))
                            {
                                //�����ַ���
                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            else
                            {
                                newCell.SetCellValue(drValue);
                                break;
                            }

                        case "System.DateTime": //��������
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //��ʽ����ʾ
                            break;
                        case "System.Boolean": //������
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //����
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //������
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //��ֵ����
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue(drValue.ToString());
                            break;
                    }

                }
                #endregion
                rowIndex++;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }

        }

        /// <summary>
        /// ����Ϊxlsx�ļ��ڲ�����
        /// </summary>
        /// <param name="dtSource">datatable����Դ</param>
        /// <param name="strHeaderText">����</param>
        /// <param name="fs">�ļ���</param>
        /// <param name="readfs">�ڴ���</param>
        /// <param name="sheetnum">sheet����</param>
        private static void ExportDataTable(DataTable dtSource, string strHeaderText, FileStream fs, MemoryStream readfs, Dictionary<string, string> dir, int sheetnum)
        {

            IWorkbook workbook = new XSSFWorkbook();
            if (readfs.Length > 0 && sheetnum > 0)
            {
                workbook = WorkbookFactory.Create(readfs);
            }
            ISheet sheet = null;
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //ȡ���п�
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region �½�������ͷ�������ͷ����ʽ

                if (rowIndex == 0)
                {
                    #region ��ͷ����ʽ
                    {
                        string sheetName = strHeaderText + (sheetnum == 0 ? "" : sheetnum.ToString());
                        if (workbook.GetSheetIndex(sheetName) >= 0)
                        {
                            workbook.RemoveSheetAt(workbook.GetSheetIndex(sheetName));
                        }
                        sheet = workbook.CreateSheet(sheetName);
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.IsBold = true;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                    }
                    #endregion

                    #region ��ͷ����ʽ
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.IsBold = true;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(dir[column.ColumnName]);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            //�����п�
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256 * 2);
                        }
                    }

                    #endregion

                    rowIndex = 2;
                }
                #endregion

                #region �������
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String": //�ַ�������
                            double result;
                            if (isNumeric(drValue, out result))
                            {

                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            else
                            {
                                newCell.SetCellValue(drValue);
                                break;
                            }
                        case "System.DateTime": //��������
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //��ʽ����ʾ
                            break;
                        case "System.Boolean": //������
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //����
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //������
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //��ֵ����
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue(drValue.ToString());
                            break;
                    }
                }
                #endregion
                rowIndex++;
            }
            workbook.Write(fs);
            fs.Close();
        }

        private static DataTable ListToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();

            var dt = new DataTable();

            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());

            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();

                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }

                    object[] array = tempList.ToArray();

                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        /// <summary>
        /// �ж������Ƿ�������
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool isNumeric(String message, out double result)
        {
            Regex rex = new Regex(@"^[-]?\d+[.]?\d*$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = double.Parse(message);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// ��ȡexcel�ļ���sheet��Ŀ
        /// </summary>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        private static int GetSheetNumber(string outputFile)
        {
            int number = 0;
            using (FileStream readfile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (readfile.Length > 0)
                {
                    IWorkbook wb = WorkbookFactory.Create(readfile);
                    number = wb.NumberOfSheets;
                }
            }
            return number;
        }

        #endregion


        #region  2.��excel�ļ��н����ݵ�����DataTable/List

        /// <summary>
        /// ��ȡExcel�ļ��ض�����sheet�����ݵ�DataTable
        /// </summary>
        /// <param name="strFileName">excel�ļ�·��</param>
        /// <param name="sheet">��Ҫ������sheet</param>
        /// <param name="HeaderRowIndex">��ͷ�����кţ�-1��ʾû����ͷ</param>
        /// <param name="dir">excel������DataTable�����Ķ�Ӧ�ֵ�</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string strFileName, Dictionary<string, string> dir, string SheetName, int HeaderRowIndex = 1)
        {
            DataTable table = new DataTable();
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (file.Length > 0)
                {
                    IWorkbook wb = WorkbookFactory.Create(file);
                    ISheet isheet = wb.GetSheet(SheetName);
                    table = ImportExcel(isheet, HeaderRowIndex, dir);
                    isheet = null;
                }
            }
            return table;
        }

        /// <summary>
        /// ��ȡExcel�ļ�ĳһ����sheet�����ݵ�DataTable
        /// </summary>
        /// <param name="strFileName">excel�ļ�·��</param>
        /// <param name="sheet">��Ҫ������sheet���</param>
        /// <param name="HeaderRowIndex">��ͷ�����кţ�-1��ʾû����ͷ</param>
        /// <param name="dir">excel������DataTable�����Ķ�Ӧ�ֵ�</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string strFileName, Dictionary<string, string> dir, int SheetIndex = 0, int HeaderRowIndex = 1)
        {
            DataTable table = new DataTable();
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (file.Length > 0)
                {
                    IWorkbook wb = WorkbookFactory.Create(file);
                    ISheet isheet = wb.GetSheetAt(SheetIndex);
                    table = ImportExcel(isheet, HeaderRowIndex, dir);
                    isheet = null;
                }
            }
            return table;

        }

        /// <summary>
        /// ��ȡExcel�ļ��ض�����sheet�����ݵ�List����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFileName"></param>
        /// <param name="dir"></param>
        /// <param name="SheetName"></param>
        /// <param name="HeaderRowIndex"></param>
        /// <returns></returns>
        public static List<T> ExcelToList<T>(string strFileName, Dictionary<string, string> dir, string SheetName, int HeaderRowIndex = 1)
        {
            DataTable dataTable = ExcelToDataTable(strFileName, dir, SheetName, HeaderRowIndex);

            return DataTableToList<T>(dataTable);
        }


        /// <summary>
        /// ��ȡExcel�ļ�ĳһ����sheet�����ݵ�List����
        /// </summary>
        /// <param name="strFileName">excel�ļ�·��</param>
        /// <param name="sheet">��Ҫ������sheet���</param>
        /// <param name="HeaderRowIndex">��ͷ�����кţ�-1��ʾû����ͷ</param>
        /// <param name="dir">excel������DataTable�����Ķ�Ӧ�ֵ�</param>
        /// <returns></returns>
        public static List<T> ExcelToList<T>(string strFileName, Dictionary<string, string> dir, int SheetIndex, int HeaderRowIndex = 1)
        {
            DataTable dataTable = ExcelToDataTable(strFileName, dir, SheetIndex, HeaderRowIndex);

            return DataTableToList<T>(dataTable);
        }

        /// <summary>
        /// ���ƶ�sheet�е����ݵ�����datatable��
        /// </summary>
        /// <param name="sheet">��Ҫ������sheet</param>
        /// <param name="HeaderRowIndex">��ͷ�����кţ�-1��ʾû����ͷ</param>
        /// <param name="dir">excel������DataTable�����Ķ�Ӧ�ֵ�</param>
        /// <returns></returns>
        private static DataTable ImportExcel(ISheet sheet, int HeaderRowIndex, Dictionary<string, string> dir)
        {
            DataTable table = new DataTable();
            IRow headerRow;
            int cellCount;
            try
            {
                //û�б�ͷ���߲���Ҫ��ͷ��excel�е���ţ�1,2,3..����ΪDataTable������
                if (HeaderRowIndex < 0)
                {
                    headerRow = sheet.GetRow(0);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        DataColumn column = new DataColumn(Convert.ToString(i));
                        table.Columns.Add(column);
                    }
                }
                //�б�ͷ��ʹ�ñ�ͷ��ΪDataTable������
                else
                {
                    headerRow = sheet.GetRow(HeaderRowIndex);
                    cellCount = headerRow.LastCellNum;
                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        //���excelĳһ�����������ڣ��Ը��е������ΪDatatable�����������DataTable�а������������Ϊ�����У���ô����Ϊ�ظ�����+���
                        if (headerRow.GetCell(i) == null)
                        {
                            if (table.Columns.IndexOf(Convert.ToString(i)) > 0)
                            {
                                DataColumn column = new DataColumn(Convert.ToString("�ظ�����" + i));
                                table.Columns.Add(column);
                            }
                            else
                            {
                                DataColumn column = new DataColumn(Convert.ToString(i));
                                table.Columns.Add(column);
                            }

                        }
                        //excel�е�ĳһ��������Ϊ�գ������ظ��ˣ���Ӧ��Datatable����Ϊ���ظ�����+��š�
                        else if (table.Columns.IndexOf(headerRow.GetCell(i).ToString()) > 0)
                        {
                            DataColumn column = new DataColumn(Convert.ToString("�ظ�����" + i));
                            table.Columns.Add(column);
                        }
                        else
                        //������������������Ҳ��ظ�����excel�е�������Ϊdatatable�ж�Ӧ������
                        {
                            string colName = dir.Where(s => s.Value == headerRow.GetCell(i).ToString()).First().Key;
                            DataColumn column = new DataColumn(colName);
                            table.Columns.Add(column);
                        }
                    }
                }
                int rowCount = sheet.LastRowNum;
                for (int i = (HeaderRowIndex + 1); i <= sheet.LastRowNum; i++)//excel�б���
                {
                    try
                    {
                        IRow row;
                        if (sheet.GetRow(i) == null)//���excel�п��У������ȱʧ����
                        {
                            row = sheet.CreateRow(i);
                        }
                        else
                        {
                            row = sheet.GetRow(i);
                        }

                        DataRow dataRow = table.NewRow();

                        for (int j = row.FirstCellNum; j <= cellCount; j++)//excel�б���
                        {
                            try
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    {
                                        case CellType.String://�ַ���
                                            string str = row.GetCell(j).StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[j] = str.ToString();
                                            }
                                            else
                                            {
                                                dataRow[j] = default(string);
                                            }
                                            break;
                                        case CellType.Numeric://����
                                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))//ʱ�������
                                            {
                                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                            }
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                            break;
                                        case CellType.Formula://��ʽ
                                            switch (row.GetCell(j).CachedFormulaResultType)
                                            {
                                                case CellType.String:
                                                    string strFORMULA = row.GetCell(j).StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[j] = strFORMULA.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow[j] = null;
                                                    }
                                                    break;
                                                case CellType.Numeric:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                                    break;
                                                case CellType.Error:
                                                    dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                                    break;
                                                default:
                                                    dataRow[j] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        table.Rows.Add(dataRow);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table;
        }


        private static List<T> DataTableToList<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        private static List<T> ConvertTo<T>(IList<DataRow> rows)
        {
            List<T> list = null;
            if (rows != null)
            {

                list = new List<T>();
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }

            }
            return list;

        }

        private static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);

            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)

                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);

                    try
                    {
                        object value = row[column.ColumnName];

                        SetObjectPropertyValue(obj, column.ColumnName, value.ToString());
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// ͨ��������������ֵ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="obj">����</param>
        /// <param name="propertyName">��������</param>
        /// <param name="value">ֵ</param>
        /// <returns>�Ƿ�ɹ�</returns>
        private static bool SetObjectPropertyValue<T>(T obj, string propertyName, string value)
        {
            try
            {
                Type type = typeof(T);

                object t = Convert.ChangeType(value, type.GetProperty(propertyName).PropertyType);

                type.GetProperty(propertyName).SetValue(obj, t, null);

                return true;

            }
            catch (Exception )
            {
                return false;
            }
        }

        #endregion

        #region Excel���뵼����DataGridView

        /// <summary>
        /// DataGridView������Excel
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dataGridView"></param>
        /// <param name="isAppend"></param>
        /// <returns></returns>
        public static bool DataGridViewToExcel(string filePath, DataGridView dataGridView, bool isAppend = false)
        {
            if (isAppend)
            {
                return DataGridViewToExcelAdd(filePath, dataGridView);
            }
            else
            {
                return DataGridViewToExcelNew(filePath, dataGridView);
            }
        }

        //------------�������������ؼ�������Excel�ļ�(�½�/�滻)��------------    

        //filePathҪ�����Ŀ��Excel�ļ�·����
        //datagGridViewҪ������Excel�ı��ؼ�
        //------------------------------------------------------------------------
        private static bool DataGridViewToExcelNew(string filePath, DataGridView dataGridView)
        {
            bool result = true;

            FileStream fs = null;//����һ���µ��ļ���
            HSSFWorkbook workbook = null;//����һ���µ�Excel�ļ�
            ISheet sheet = null;//ΪExcel����һ�Ź�����

            //�����������������뵱ǰExcel��������
            int rowCount = dataGridView.RowCount;//��¼����е�����
            int colCount = dataGridView.ColumnCount;//��¼����е�����

            //�ж��ļ����Ƿ����
            if (CheckAndCreatPath(DecomposePathAndName(filePath, DecomposePathEnum.PathOnly)) == "error")
            {
                result = false;
                return result;
            }

            //����������
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                workbook = new HSSFWorkbook();
                sheet = workbook.CreateSheet("Sheet1");
                IRow row = sheet.CreateRow(0);
                for (int j = 0; j < colCount; j++)  //��ѭ��
                {
                    if (dataGridView.Columns[j].Visible && dataGridView.Rows[0].Cells[j].Value != null)
                    {
                        ICell cell = row.CreateCell(j);//������
                        cell.SetCellValue(dataGridView.Columns[j].HeaderText.ToString());//���ĵ�Ԫ��ֵ                  
                    }
                }
            }
            catch
            {
                result = false;
                return result;
            }

            for (int i = 0; i < rowCount; i++)      //��ѭ��
            {
                //��ֹ��������Excel����
                if (i >= 65536)
                {
                    result = false;
                    break;
                }
                IRow row = sheet.CreateRow(1 + i);  //������
                for (int j = 0; j < colCount; j++)  //��ѭ��
                {
                    if (dataGridView.Columns[j].Visible && dataGridView.Rows[i].Cells[j].Value != null)
                    {
                        ICell cell = row.CreateCell(j);//������
                        cell.SetCellValue(dataGridView.Rows[i].Cells[j].Value.ToString());//���ĵ�Ԫ��ֵ                  
                    }
                }
            }
            try
            {
                workbook.Write(fs);
            }
            catch
            {
                result = false;
                return result;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                workbook = null;
            }
            return result;
        }

        //------------�������������ؼ�������Excel�ļ�(���/�½�)��------------    
        //filePathҪ�����Ŀ��Excel�ļ�·����
        //datagGridViewҪ������Excel�ı��ؼ�
        //------------------------------------------------
        private static bool DataGridViewToExcelAdd(string filePath, DataGridView dataGridView)
        {
            bool result = true;

            FileStream fs = null;//����һ���µ��ļ���
            HSSFWorkbook workbook = null;//����һ���µ�Excel�ļ�
            ISheet sheet = null;//ΪExcel����һ�Ź�����

            //�����������������뵱ǰExcel��������
            int rowCount = dataGridView.RowCount;//��¼����е�����
            int colCount = dataGridView.ColumnCount;//��¼����е�����
            int numCount = 0;//Excell���һ�����

            //�ж��ļ����Ƿ����
            if (CheckAndCreatPath(DecomposePathAndName(filePath, DecomposePathEnum.PathOnly)) == "error")
            {
                result = false;
                return result;
            }
            //�ж��ļ��Ƿ����
            if (!File.Exists(filePath))
            {
                try
                {
                    fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    workbook = new HSSFWorkbook();
                    sheet = workbook.CreateSheet("Sheet1");
                    IRow row = sheet.CreateRow(0);
                    for (int j = 0; j < colCount; j++)  //��ѭ��
                    {
                        if (dataGridView.Columns[j].Visible && dataGridView.Rows[0].Cells[j].Value != null)
                        {
                            ICell cell = row.CreateCell(j);//������
                            cell.SetCellValue(dataGridView.Columns[j].HeaderText.ToString());//���ĵ�Ԫ��ֵ                  
                        }
                    }
                    workbook.Write(fs);
                }
                catch
                {
                    result = false;
                    return result;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                        fs = null;
                    }
                    workbook = null;
                }
            }
            //����ָ���ļ��Ĺ�����
            try
            {
                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                workbook = new HSSFWorkbook(fs);//.xls
                sheet = workbook.GetSheetAt(0);
                if (sheet == null)
                {
                    result = false;
                    return result;
                }
                numCount = sheet.LastRowNum + 1;
            }
            catch
            {
                result = false;
                return result;
            }

            for (int i = 0; i < rowCount; i++)      //��ѭ��
            {
                //��ֹ��������Excel����
                if (numCount + i >= 65536)
                {
                    result = false;
                    break;
                }
                IRow row = sheet.CreateRow(numCount + i);  //������
                for (int j = 0; j < colCount; j++)  //��ѭ��
                {
                    if (dataGridView.Columns[j].Visible && dataGridView.Rows[i].Cells[j].Value != null)
                    {
                        ICell cell = row.CreateCell(j);//������
                        cell.SetCellValue(dataGridView.Rows[i].Cells[j].Value.ToString());//���ĵ�Ԫ��ֵ                  
                    }
                }
            }
            try
            {
                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                workbook.Write(fs);
            }
            catch
            {
                result = false;
                return result;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                workbook = null;
            }
            return result;
        }


        //------------����������Excel�ļ���ȡ���ݵ����ؼ���------------    
        //filePathΪExcel�ļ�·����
        //datagGridViewҪ��ʾ���ݵı��ؼ�
        //------------------------------------------------
        public static bool ExcelToDataGridView(string filePath, DataGridView dataGridView, bool hastitle = false)
        {
            bool result = true;

            FileStream fs = null;//����һ���µ��ļ���
            HSSFWorkbook workbook = null;//����һ���µ�Excel�ļ�
            ISheet sheet = null;//ΪExcel����һ�Ź�����

            //��������������
            int rowCount = 0;//��¼Excel�е�����
            int colCount = 0;//��¼Excel�е�����

            //�ж��ļ��Ƿ����
            if (!File.Exists(filePath))
            {
                result = false;
                return result;
            }
            //����ָ���ļ��Ĺ�����
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                workbook = new HSSFWorkbook(fs);//.xls
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                sheet = workbook.GetSheetAt(0);
                if (sheet == null)
                {
                    result = false;
                    return result;
                }
                rowCount = sheet.LastRowNum;
                colCount = sheet.GetRow(0).LastCellNum;
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();
                for (int j = 0; j < colCount; j++)  //��ѭ��
                {
                    ICell cell = sheet.GetRow(0).GetCell(j);//��ȡ��
                    dataGridView.Columns.Add(j.ToString() + cell.ToString(), cell.ToString());
                }

                for (int i = 1; i <= rowCount; i++)      //��ѭ��
                {
                    IRow row = sheet.GetRow(i);  //��ȡ��
                    int index = dataGridView.Rows.Add();
                    colCount = row.LastCellNum;
                    for (int j = 0; j < colCount; j++)  //��ѭ��
                    {
                        ICell cell = row.GetCell(j);//��ȡ��
                        dataGridView.Rows[index].Cells[j].Value = cell.ToString();
                    }
                }
            }
            catch (Exception )
            {
                result = false;
                return result;
            }
            return result;
        }


        //�ֽ�·����ö��
        public enum DecomposePathEnum
        {
            PathOnly = 0,//������·��
            NameAndExtension = 1,//�����ļ���+��չ��
            NameOnly = 2,//�������ļ���
            ExtensionOnly = 3,//��������չ��(��.)

        }

        //------------�����������ļ�·���ֽ⡿------------  

        //filePath�ļ�·��
        //DecomposePathEnum��������
        //------------------------------------------------
        private static string DecomposePathAndName(string filePath, DecomposePathEnum decomposePathEnum)
        {
            string result = "";
            switch (decomposePathEnum)
            {
                case DecomposePathEnum.PathOnly://������·��
                    result = filePath.Substring(0, filePath.LastIndexOf("\\"));
                    break;
                case DecomposePathEnum.NameAndExtension://�����ļ���+��չ��
                    result = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                    break;
                case DecomposePathEnum.NameOnly://�������ļ���
                    result = filePath.Substring(filePath.LastIndexOf("\\") + 1, filePath.LastIndexOf(".") - filePath.LastIndexOf("\\") - 1);
                    break;
                case DecomposePathEnum.ExtensionOnly://��������չ��(��.)
                    result = filePath.Substring(filePath.LastIndexOf("."));
                    break;
                default://
                    result = "";
                    break;
            }
            return result;
        }


        //------------���������ж��ļ�·���Ƿ���ڣ��������򴴽���------------  

        //filePath�ļ���·��
        //DecomposePathEnum��������
        //---------------------------------------------------------------------
        private static string CheckAndCreatPath(string path)
        {
            if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                if (path.LastIndexOf("\\") <= 0)
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                        return path;
                    }
                    catch
                    {
                        return "error";
                    }
                }
                else
                {
                    if (CheckAndCreatPath(DecomposePathAndName(path, DecomposePathEnum.PathOnly)) == "error")
                    {
                        return "error";
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                        return path;
                    }
                }
            }
        }


        #endregion

    }
}
