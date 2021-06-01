using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.Extractor;
using NPOI.XSSF.UserModel;
using log4net;
namespace MESWebApi.Util
{
    public class ExcelHelper
    {
        ILog log;
        IWorkbook workbook;
        public ExcelHelper()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public void ReadExcel(string filePath)
        {
            
            string fileExt = Path.GetExtension(filePath);
            try
            {

                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (fileExt == ".xls")
                    {
                        workbook = new HSSFWorkbook(file);
                    }
                    else if (fileExt == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(file);
                    }
                    ISheet sheet = workbook.GetSheetAt(2);
                    int rowno = sheet.LastRowNum;
                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                       IRow row = sheet.GetRow(i);
                        for (int j = row.FirstCellNum; j <= row.LastCellNum; j++)
                        {
                            CellType celltype = row.GetCell(j).CellType;
                            switch (celltype)
                            {
                                case CellType.Unknown:
                                    break;
                                case CellType.Numeric:
                                    var nval = row.GetCell(j).NumericCellValue;
                                    break;
                                case CellType.String:
                                   var sval = row.GetCell(j).StringCellValue;
                                    break;
                                case CellType.Blank:
                                    break;
                                case CellType.Boolean:
                                    var bval = row.GetCell(j).BooleanCellValue;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
    }
}