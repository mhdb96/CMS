using Aspose.Cells;

namespace CMSLibrary.Evaluation
{
    public class Excel
    {
        Workbook wb = new Workbook();
        string[][,] DataArray = { new string [1000,105],
        new string [500,5],
        new string [500,7]};


        public Excel(int studentsCount, int questionsCount, int outcomesCount)
        {
            string[][,] DataArray =
            {
                new string [studentsCount + 30, 105],
                new string [questionsCount + 20, 5],
                new string [outcomesCount + 20, 7]
            };
            wb.Worksheets.Clear();
            wb.Worksheets.Add("Students Statistics");
            wb.Worksheets.Add("Questions Statistics");
            wb.Worksheets.Add("Outcomes Statistics");
        }

        public void WriteToCell(int row, int col, string content, int sheetIndex)
        {
            DataArray[sheetIndex][row, col] = content;
        }

        public void WriteFile()
        {
            for (int i = 0; i < 3; i++)
            {
                wb.Worksheets[i].Cells.ImportTwoDimensionArray(DataArray[i], 0, 0, true);
                wb.Worksheets[i].AutoFitColumns();
            }
        }

        public void SaveAs(string path)
        {
            WriteFile();
            wb.Save(path);
        }

    }
}
