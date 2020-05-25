using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Page
    {
        private List<Section> _sections = new List<Section>();
        private List<Video> _videos = new List<Video>();

        public string Title { get; set; }

        public string URL { get; set; }

        public bool HaveForm { get; set; }

        public bool HaveInteractive { get; set; }

        public Section[] Sections => this._sections.ToArray(); //TODO: need to optimaze

        public Video[] Videos => this._videos.ToArray(); //TODO: need to optimaze

        public Link[] AllLinks => this._sections.SelectMany(x => x.Links).ToArray(); //TODO: need to optimaze

        public Page(string PageTitle, string PageURL, bool HaveForm, bool HaveInteractive)
        {
            this.Title = PageTitle;

            this.URL = PageURL;

            this.HaveForm = HaveForm;

            this.HaveInteractive = HaveInteractive;
        }

        public void AddSection(string SectionTitle)
        {
            _sections.Add(new Section(SectionTitle) { });
        }

        public void AddVideo(string VideoTitle, string VideoURL)
        {
            _videos.Add(new Video(VideoTitle, VideoURL) { });
        }
    }
}
