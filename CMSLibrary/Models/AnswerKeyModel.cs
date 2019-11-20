using CMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class AnswerKeyModel
    {
        public GroupModel Group { get; set; } = new GroupModel();
        public string AnswersList { get; set; }
        public int QuestionCount { get; set; }

    }
}
