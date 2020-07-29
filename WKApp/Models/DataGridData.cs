using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WKApp.Models
{
    public class DataGridData
    {
        public double ItemWidth { get; set; } = 180;
        public double ItemFont { get; set; } = 50;
        public string Title { get; set; }
        public string MainReading { get; set; }
        public string Meaning { get; set; }
        public string Level;
        public double score;
    }
}
