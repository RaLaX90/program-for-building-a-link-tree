using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public enum LinkType
    {
        None = 0,
        DropDownMenuTab,
        DropDownMenuItem,
        Contents,
        RequestAQuote,
        RequestAWebinar,
        RequestADownloadFile,
        OpenFileWithoutRequest,
        InstantFileDownload,
        External,
        Internal,
        Mail
    }

    public class Link
    {
        public Link(string LinkTitle, LinkType LinkType, string LinkURL)
        {
            this.Title = LinkTitle;

            this.Type = LinkType;

            this.URL = LinkURL;
        }

        public string Title { get; set; }

        public LinkType Type { get; set; }

        public string URL { get; set; }
    }
}
