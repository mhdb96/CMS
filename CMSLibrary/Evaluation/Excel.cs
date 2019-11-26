using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace CMSLibrary.Evaluation
{
    public class Excel
    {
        //string path = "";
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        public Excel()
        {

        }
        /*
        public Excel (string path, int sheet)
        {
            this.path = path;
            wb = excel.Workbooks.Open(path);
            ws = excel.Worksheets[sheet];
        }*/

        public void CreateNewFile()
        {
            wb=excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            ws = wb.Worksheets[1];
        }
        public void CreateNewSheet()
        {
            wb.Worksheets.Add(After:ws);
            ws = wb.Worksheets[2];
        }

        public decimal ReadCell(int i, int j)
        {
            i++;
            j++;
            if(ws.Cells[i, j].Value2 != null)
            {
                return Convert.ToDecimal(ws.Cells[i, j].Value2);
            }
            else
            {
                return -1;
            }

        }
        public void WriteToCell(int i, int j, string s)
        {
            i++;
            j++;
            ws.Cells[i, j].Value2 = s;
        
        }
        public void SelectWorkSheet(int sheetNumber)
        {
            ws = wb.Worksheets[sheetNumber];

        }
        public void Save()
        {
            wb.Save();
        }
        public void SaveAs(string path)
        {
            wb.SaveAs(path);
        }
        public void Close()
        {
            wb.Close();
        }

    }
}
