using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSLibrary.Models
{
    public class ActiveTermModel
    {
        public int Id { get; set; }
        public YearModel Year { get; set; }
        public TermModel Term { get; set; }
    }
}
