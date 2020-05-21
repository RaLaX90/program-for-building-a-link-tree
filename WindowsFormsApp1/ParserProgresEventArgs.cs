using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ParserProgresEventArgs : EventArgs
    {
        public ParserProgresEventArgs(string link)
        {
            this.link = link;
        }
        public string link { get; set; }
    }
}
