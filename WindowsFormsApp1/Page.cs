using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Page
    {
        public Page(string PageTitle, string PageURL, bool HaveForm = false, bool HaveInteractive = false)
        {
            this.Title = PageTitle;

            this.URL = PageURL;

            this.HaveForm = HaveForm;

            this.HaveInteractive = HaveInteractive;
        }

        private List<Section> Sections = new List<Section>();
        private List<Video> Videos = new List<Video>();

        public string Title { get; set; }

        public string URL { get; set; }

        public bool HaveForm { get; set; }

        public bool HaveInteractive { get; set; }

        public void AddSection(string SectionTitle)
        {
            Sections.Add(new Section(SectionTitle) { });
        }

        public void AddVideo(string VideoTitle, string VideoURL)
        {
            Videos.Add(new Video(VideoTitle, VideoURL) { });
        }

        public List<Section> WhatContainsInSections() => this.Sections;

        public List<Video> WhatContainsInVideos() => this.Videos;
    }
}
