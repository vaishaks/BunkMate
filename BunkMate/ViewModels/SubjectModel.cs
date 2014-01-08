using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunkMate.ViewModels
{
    public class SubjectModel
    {
        public List<Subject> Subjects { get; set; }
        public bool IsDataLoaded { get; private set; }

        public void LoadData()
        {
            // Load data

            IsDataLoaded = true;
        }
    }
}
