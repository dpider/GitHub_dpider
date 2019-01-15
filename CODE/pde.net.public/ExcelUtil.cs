using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace pde.pub
{
    public static class ExcelUtil
    {
        #region 读取CSV数据表 
        /// <summary>
        /// 打开CSV 文件
        /// </summary>
        /// <param name="fileName">文件全名</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="firstRow">开始行</param>
        /// <param name="firstColumn">开始列</param>
        /// <param name="getRows">获取多少行</param>
        /// <param name="getColumns">获取多少列</param>
        /// <param name="haveTitleRow">是有标题行</param>
        /// <returns>DataTable</returns>
        public static DataTable OpenCSV(string fullFileName, string filter = "", int firstRow = 0, int firstColumn = 0, int getRows = 0, int getColumns = 0, bool haveTitleRow = true)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                //记录每次读取的一行记录
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine;
                //标示列数
                int columnCount = 0;
                //是否已建立了表的字段
                bool bCreateTableColumns = false;
                //第几行
                int iRow = 1;

                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    strLine = strLine.Trim();
                    aryLine = SplitCVSRowStr(strLine);
                    if (bCreateTableColumns == false)
                    {
                        bCreateTableColumns = true;
                        columnCount = aryLine.Length;
                        //创建列
                        for (int i = firstColumn; i < (getColumns == 0 ? columnCount : firstColumn + getColumns); i++)
                        {
                            DataColumn dc = new DataColumn(haveTitleRow == true ? aryLine[i] : "COL" + i.ToString());
                            dt.Columns.Add(dc);
                        }

                        bCreateTableColumns = true;

                        if (haveTitleRow == true)
                        {
                            continue;
                        }
                    }

                    //去除无用行
                    if (firstRow > iRow)
                    {
                        iRow++;
                        continue;
                    }

                    DataRow dr = dt.NewRow();
                    for (int j = firstColumn; j < (getColumns == 0 ? columnCount : firstColumn + getColumns); j++)
                    {
                        dr[j - firstColumn] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                    if (!filter.Equals(""))
                    {
                        if (!dt.Select(filter).Contains(dr))
                        {
                            dt.Rows.Remove(dr);
                            continue;
                        }
                    }

                    iRow++;

                    if (getRows > 0)
                    {
                        if ((iRow - firstRow) >= getRows)
                        {
                            break;
                        }
                    }
                }
                sr.Close();
            }
            return dt;
        }

        public static DataTable OpenCSVInfo(string fullFileName, out int rowCount, int firstColumn = 0, int getColumns = 0, bool haveTitleRow = true)
        {
            DataTable dt = new DataTable();
            rowCount = 0;
            using (FileStream fs = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                //记录每次读取的一行记录
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine;
                //标示列数
                int columnCount = 0;
                //是否已建立了表的字段
                bool bCreateTableColumns = false;

                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (bCreateTableColumns == false)
                    {
                        bCreateTableColumns = true;
                        strLine = strLine.Trim();
                        aryLine = SplitCVSRowStr(strLine);
                        columnCount = aryLine.Length;
                        //创建列
                        for (int i = firstColumn; i < (getColumns == 0 ? columnCount : firstColumn + getColumns); i++)
                        {
                            DataColumn dc = new DataColumn(haveTitleRow == true ? aryLine[i] : "COL" + i.ToString());
                            dt.Columns.Add(dc);
                        }

                        bCreateTableColumns = true;

                        if (haveTitleRow == true)
                        {
                            continue;
                        }
                    }
                    rowCount++;
                }
                sr.Close();
            }
            return dt;
        }

        private static string[] SplitCVSRowStr(string strdata)
        {
            strdata = strdata.Replace("\r", "");
            List<string> cells = new List<string>();
            string str = "";
            bool flag = false;
            for (int i = 0; i < strdata.Length; i++)
            {
                char ch = strdata[i];

                if (ch == ',')
                {
                    if (!flag)
                    {
                        cells.Add(str);
                        str = "";
                    }
                    else
                        str += ch;
                }
                else if (ch == '\"')
                {
                    if ((++i < strdata.Length) && strdata[i] == '\"')
                    {
                        str += strdata[i];
                    }
                    else
                    {
                        --i;
                        flag = flag ? false : true;
                    }
                }
                else
                {
                    str += ch;
                }
            }
            cells.Add(str);
            return cells.ToArray();
        }
        #endregion

        #region 读取Xls数据
        /// <summary>
        /// 获取excel的sheet列表
        /// </summary>
        /// <param name="filePath">Excel文件</param>
        /// <returns></returns>
        public static List<ISheet> getExcelSheets(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("Excel文件不存在！");
            }
            string fileExt = Path.GetExtension(filePath).ToLower();
            if ((!fileExt.Equals(".xlsx")) && (!fileExt.Equals(".xls")))
            {
                throw new Exception("Excel文件后缀名不正确！");
            }

            List<ISheet> sheets = new List<ISheet>();
            int sheetNumber = 0;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fileExt == ".xlsx")
                {
                    // 2007版本  
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    sheetNumber = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetNumber; i++)
                    {
                        ISheet sheet = workbook.GetSheet(workbook.GetSheetName(i));
                        if (sheet != null)
                        {
                            sheets.Add(sheet);
                        }
                    }
                }
                else if (fileExt == ".xls")
                {
                    // 2003版本  
                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                    sheetNumber = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetNumber; i++)
                    {
                        ISheet sheet = workbook.GetSheet(workbook.GetSheetName(i));
                        if (sheet != null)
                        {
                            sheets.Add(sheet);
                        }
                    }
                }
            }
            return sheets;
        }

        /// <summary>
        /// 获取excel的具体sheet
        /// </summary>
        /// <param name="filePath">Excel文件名</param>
        /// <param name="sheetName">sheet名称</param>
        /// <returns></returns>
        public static ISheet getExcelSheet(string filePath, string sheetName)
        {
            List<ISheet> sheets = getExcelSheets(filePath);
            foreach (ISheet sheet in sheets)
            {
                if (sheet.SheetName.Equals(sheetName))
                {
                    return sheet;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取excel的具体sheet
        /// </summary>
        /// <param name="filePath">Excel文件名</param>
        /// <param name="sheetIndex">sheet序号</param>
        /// <returns></returns>
        public static ISheet getExcelSheet(string filePath, int sheetIndex)
        {
            List<ISheet> sheets = getExcelSheets(filePath);
            for (int i = 0; i < sheets.Count; i++)
            {
                if (i == sheetIndex)
                {
                    return sheets[i];
                }
            }
            return null;
        }

        #region 获取sheet数据
        /// <summary>
        /// 获取sheet数据
        /// </summary>
        /// <param name="filePath">Excel文件</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="firstRow">起始行</param>
        /// <param name="getRows">获取记录数</param>
        /// <returns></returns>
        public static DataTable getSheetDataTable(string filePath, string sheetName, string filter = "", int firstRow = 0, int getRows = 0)
        {
            ISheet sheet = getExcelSheet(filePath, sheetName);
            if (sheet == null)
            {
                throw new Exception(string.Format("Excel文件Sheet[{0}]数据读取失败！", sheetName));
            }
            return getSheetDataTable(sheet, filter, firstRow, getRows);
        }
        #endregion

        #region 获取sheet数据
        /// <summary>
        /// 获取sheet数据
        /// </summary>
        /// <param name="filePath">Excel文件</param>
        /// <param name="sheetIndex">Sheet序号</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="firstRow">起始行</param>
        /// <param name="getRows">获取记录数</param>
        /// <returns></returns>
        public static DataTable getSheetDataTable(string filePath, int sheetIndex, string filter = "", int firstRow = 0, int getRows = 0)
        {
            ISheet sheet = getExcelSheet(filePath, sheetIndex);
            if (sheet == null)
            {
                throw new Exception(string.Format("Excel文件Sheet[{0}]数据读取失败！", sheetIndex));
            }
            return getSheetDataTable(sheet, filter, firstRow, getRows);
        }
        #endregion

        #region 获取sheet数据
        public static DataTable getSheetDataTable(ISheet sheet, string filter = "", int firstRow = 0, int getRows = 0)
        {
            DataTable dt = new DataTable();
            int startIndex = 0;// sheet.FirstRowNum;  
            int lastIndex = sheet.LastRowNum;
            if (lastIndex == 0)
            {
                throw new Exception(string.Format("Sheet[{0}]中没有数据！", sheet.SheetName));
            }

            //最大行数  
            int cellCount = 0;
            IRow maxRow = sheet.GetRow(0);
            IRow row;
            for (int i = startIndex; i <= lastIndex; i++)
            {
                row = sheet.GetRow(i);
                if ((row != null) && (cellCount < row.LastCellNum))
                {
                    cellCount = row.LastCellNum;
                    maxRow = row;
                }
            }

            //获取列名（第一行为列名）
            row = sheet.GetRow(startIndex);
            for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
            {
                if (row.GetCell(j) != null)
                {
                    dt.Columns.Add(getCellValue(row.GetCell(j)).ToString());
                }
            }
            startIndex++;

            int si = startIndex + firstRow;
            int iRows = 0;
            //数据填充  
            for (int i = si; i <= lastIndex; i++)
            {
                row = sheet.GetRow(i);
                DataRow drNew = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                    {
                        if (row.GetCell(j) != null)
                        {
                            drNew[j] = getCellValue(row.GetCell(j));
                        }
                    }
                }
                dt.Rows.Add(drNew);
                if (!filter.Equals(""))
                {
                    if (!dt.Select(filter).Contains(drNew))
                    {
                        dt.Rows.Remove(drNew);
                        continue;
                    }
                }
                iRows++;
                if (getRows == iRows) { break; }
            }
            dt.TableName = sheet.SheetName.Trim();
            return dt;
        }
        #endregion

        #region 获取sheet数据信息
        /// <summary>
        /// 获取sheet数据信息
        /// </summary>
        /// <param name="filePath">Excel文件</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="recordCount">返回数据数量</param> 
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static DataTable getSheetDataTableInfo(string filePath, string sheetName, out int recordCount, string filter = "")
        {
            ISheet sheet = getExcelSheet(filePath, sheetName);
            if (sheet == null)
            {
                throw new Exception(string.Format("Excel文件Sheet[{0}]数据读取失败！", sheetName));
            }
            return getSheetDataTableInfo(sheet, out recordCount, filter);
        }
        #endregion

        #region 获取sheet数据信息
        /// <summary>
        /// 获取sheet数据信息
        /// </summary>
        /// <param name="filePath">Excel文件</param>
        /// <param name="sheetIndex">Sheet序号</param>
        /// <param name="recordCount">返回数据数量</param> 
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static DataTable getSheetDataTableInfo(string filePath, int sheetIndex, out int recordCount, string filter = "")
        {
            ISheet sheet = getExcelSheet(filePath, sheetIndex);
            if (sheet == null)
            {
                throw new Exception(string.Format("Excel文件Sheet[{0}]数据读取失败！", sheetIndex));
            }
            return getSheetDataTableInfo(sheet, out recordCount, filter);
        }
        #endregion

        #region 获取sheet数据表信息
        public static DataTable getSheetDataTableInfo(ISheet sheet, out int recordCount, string filter = "")
        {
            recordCount = 0;
            DataTable dt = new DataTable();
            int startIndex = 0;// sheet.FirstRowNum;  
            int lastIndex = sheet.LastRowNum;
            if (lastIndex == 0)
            {
                throw new Exception(string.Format("Sheet[{0}]中没有数据！", sheet.SheetName));
            }

            //最大行数  
            int cellCount = 0;
            IRow maxRow = sheet.GetRow(0);
            IRow row;
            for (int i = startIndex; i <= lastIndex; i++)
            {
                row = sheet.GetRow(i);
                if ((row != null) && (cellCount < row.LastCellNum))
                {
                    cellCount = row.LastCellNum;
                    maxRow = row;
                }
            }

            //获取列名（第一行为列名）
            row = sheet.GetRow(startIndex);
            for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
            {
                if (row.GetCell(j) != null)
                {
                    dt.Columns.Add(getCellValue(row.GetCell(j)).ToString());
                }
            }
            startIndex++;

            //数据填充  
            for (int i = startIndex; i <= lastIndex; i++)
            {
                row = sheet.GetRow(i);
                DataRow drNew = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                    {
                        if (row.GetCell(j) != null)
                        {
                            drNew[j] = getCellValue(row.GetCell(j));
                        }
                    }
                }
                dt.Rows.Add(drNew);
                if (!filter.Equals(""))
                {
                    if (!dt.Select(filter).Contains(drNew))
                    {
                        dt.Rows.Remove(drNew);
                        continue;
                    }
                }
                dt.Rows.Remove(drNew);
                recordCount++;
            }
            dt.TableName = sheet.SheetName.Trim();
            return dt;
        }
        #endregion

        #region 获取单元格值
        private static object getCellValue(ICell cell)
        {
            object value = "";
            switch (cell.CellType)
            {
                case CellType.Blank:
                    value = "";
                    break;

                case CellType.Numeric:
                    short format = cell.CellStyle.DataFormat;
                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理   
                    if (format == 14 || format == 31 || format == 57 || format == 58)
                        value = cell.DateCellValue;
                    else
                        value = cell.NumericCellValue;
                    if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                        value = cell.NumericCellValue.ToString("#0.00");
                    break;

                case CellType.String:
                    value = cell.StringCellValue;
                    break;

                case CellType.Formula:
                    try
                    {
                        value = cell.NumericCellValue;
                        if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)
                            value = cell.NumericCellValue.ToString("#0.00");
                    }
                    catch
                    {
                        try
                        {
                            value = cell.StringCellValue;
                        }
                        catch { }
                    }
                    break;

                default:
                    value = cell.StringCellValue;
                    break;
            }

            return value;
        }
        #endregion
        #endregion

        #region 数据表导出到Excel
        public static void DataToExcel(DataTable data, string excelFile)
        {
            string fileExt = Path.GetExtension(excelFile).ToLower();
            if ((!fileExt.Equals(".xlsx")) && (!fileExt.Equals(".xls")))
            {
                throw new Exception("Excel文件后缀名不正确！");
            }
            string sheetName = data.TableName;
            if (sheetName.Equals(""))
            {
                sheetName = DateTime.Now.ToString("yyyyMMddHHmmsszzz");
            }

            ISheet sheet = null;
            if (fileExt.Equals(".xlsx"))
            {
                // 2007版本  
                XSSFWorkbook workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet(sheetName);
            }
            else if (fileExt == ".xls")
            {
                // 2003版本  
                HSSFWorkbook workbook = new HSSFWorkbook();
                sheet = workbook.CreateSheet(sheetName);
            }

            using (FileStream fs = new FileStream(excelFile, FileMode.Create, FileAccess.ReadWrite))
            {
                //写列首
                IRow row0 = sheet.CreateRow(0);
                int i = 0;
                foreach (DataColumn col in data.Columns)
                {
                    row0.CreateCell(i).SetCellValue(col.ColumnName);
                    i++;
                }

                //写数据
                for (int irow = 0; irow < data.Rows.Count; irow++)
                {
                    IRow row = sheet.CreateRow(irow + 1);
                    for (int icol = 0; icol < data.Columns.Count; icol++)
                    {
                        ICell cell = row.CreateCell(icol);
                        setCellValue(cell, data.Rows[irow][icol]);
                    }
                }

                sheet.Workbook.Write(fs);
            } 
        }


        #region 写入单元格值
        private static void setCellValue(ICell cell, object value)
        {
            if ((value is float) || (value is double))
            {
                cell.SetCellType(CellType.Numeric);
                cell.SetCellValue((int)value);
                return;
            }
            else if (value is DBNull)
            {
                cell.SetCellType(CellType.Blank); 
                return;
            }
            else
            {
                cell.SetCellType(CellType.String);
                cell.SetCellValue((string)value);
                return;
            }
        }
        #endregion
        #endregion
    }
}
