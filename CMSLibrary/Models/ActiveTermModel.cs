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
        public string Name
        {
            get
            {
                return $"{Year.Name} {Term.Name}"; 
            }
        }
        public YearModel Year { get; set; } = new YearModel();
        public TermModel Term { get; set; } = new TermModel();

    }
}
