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
    public  class ExcelHelper
    {
        ILog log;
        IWorkbook workbook;
        public ExcelHelper()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public IWorkbook ReadExcel(string filePath)
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
                    return workbook;
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