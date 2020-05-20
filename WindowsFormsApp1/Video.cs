using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Video
    {
        public Video(string VideoTitle, string VideoURL)
        {
            this.Title = VideoTitle;
            this.URL = VideoURL;
        }

        public string Title { get; set; }

        public string URL { get; set; }
    }
}
