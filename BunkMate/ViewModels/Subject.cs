using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunkMate.ViewModels
{
    public class Subject
    {
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public int MaxBunks { get; set; }
        public string BunkCounterDigit1 { get; set; }
        public string BunkCounterDigit2 { get; set; }
        public int BunkCounter { get; set; }

        public Subject()
        {
            BunkCounterDigit1 = "0";
            BunkCounterDigit1 = "0";
            BunkCounter = 0;
        }
    }
}
