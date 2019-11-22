﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
