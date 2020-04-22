using System;
using System.Collections.Generic;
using System.Windows;

namespace CMSLibrary.Models
{
    public class ExamModel
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string FilePath { get; set; }
        public AssignmentModel Assignment { get; set; } = new AssignmentModel();
        public ExamTypeModel ExamType { get; set; }
        public List<ExamGroupModel> ExamGroups { get; set; } = new List<ExamGroupModel>();
        public UserModel User { get; set; }
        public Visibility AddExcelFile
        {
            get
            {
                if (FilePath == "" || FilePath == null)
                {
                    return Visibility.Visible;
                }
                else return Visibility.Collapsed;
            }
        }
        public string DateString
        {
            get
            {
                return Date.ToShortDateString();
            }
            set { }
        }
    }
}
