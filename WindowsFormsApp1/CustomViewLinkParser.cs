using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class CustomViewLinkParser
    {
        public CustomViewLinkParser(string _rootLink = "http://localhost:50897/")
        {
            this._rootLink = _rootLink;
        }

        private readonly string _rootLink;

        public delegate void ParserProgresEventHandler(ParserProgresEventArgs Args, int level);

        public event ParserProgresEventHandler PageAnalyzed;

        private Section FindSection(HtmlNode child, Page page)
        {
            //HtmlNode node = child;

            //while (true)
            //{
            //    if (node.ParentNode.Name == "div")
            //    {
            //        return child.ParentNode.GetAttributeValue("class");
            //    }
            //    else
            //    {
            //        node = node.ParentNode;
            //    }
            //}

            // TODO: need to upgrade
            if (page.Sections.Length == 0) //temporary solutuion
            {
                page.AddSection("defaultSection");
            }

            return page.Sections[0];

        }

        private string GetLinkTitle(HtmlNode node)
        {
            if (!string.IsNullOrWhiteSpace(node.GetAttributeValue("title"))) // !
            {
                return node.GetAttributeValue("title");
            }
            else
            {
                return node.InnerText;
            }
        }

        private LinkType GetLinkType(HtmlNode node, Page page)
        {
            // TODO: need to upgrade
            return LinkType.Internal;
        }

        private void AddLinkToPage(HtmlNode node, Page page)
        {
            var section = FindSection(node, page);
            var linkTitle = GetLinkTitle(node);
            var linkType = GetLinkType(node, page);
            var linkURL = node.GetAttributeValue("href");

            section.AddLink(linkTitle, linkType, linkURL);
        }

        private void AddVideoToPage(HtmlNode video, Page page)
        {
            string videoTitle = "";
            string videoURL = video.FirstChild.GetAttributeValue("src");
            page.AddVideo(videoTitle, videoURL);
        }

        private string GetLinkWithoutAnchor(string link) => (link.IndexOf("#") > -1) ? link.Remove(link.IndexOf("#")) : link;

        private bool IsExternalLink(string link)
        {
            Uri uri = new Uri(link, UriKind.RelativeOrAbsolute);

            if (!uri.IsAbsoluteUri || uri.Host.ToLower() == "localhost")
            {
                return false;
            }

            return true;
        }

        private bool IsLinkValid(string link)
        {
            var linkWithoutAnchor = GetLinkWithoutAnchor(link);

            if ((linkWithoutAnchor.Length > 0) && (linkWithoutAnchor.IndexOf("mailto") < 0) && (linkWithoutAnchor.IndexOf("tel") < 0)
                && (linkWithoutAnchor.IndexOf("pdf") < 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetFullURL(string URL)
        {
            Uri uri = new Uri(URL, UriKind.RelativeOrAbsolute);

            if (uri.IsAbsoluteUri)
            {
                return URL;
            }
            else
            {
                return _rootLink + URL;
            }
        }

        private void OnPageAnalyzed(Page page)
        {
            if (PageAnalyzed != null)
            {
                ParserProgresEventArgs args = new ParserProgresEventArgs();
                args.Page = page;
                PageAnalyzed(args, 1);
            }
        }

        private Page ParsePage(string parseLink)
        {
            var web = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc = web.Load(GetFullURL(parseLink));

            // TODO: create a dynamic collection of object type on the page
            bool hasForm = Convert.ToBoolean(doc.DocumentNode.Descendants("form").Count());
            bool hasInteractive = Convert.ToBoolean(doc.DocumentNode.CssSelect("#cpMainContent_customActionPlaceholder").ToList().Count());
            string title = doc.DocumentNode.CssSelect("head title").ToList()[0].InnerText.Replace(Environment.NewLine, "").Replace("\t", "");
            
            var nodes = doc.DocumentNode.CssSelect("a").ToList();

            //var linkWithoutRootLink = parseLink.Remove(parseLink.LastIndexOf("/")); //////////////////////////////////////////////
            var page = new Page(title, parseLink, hasForm, hasInteractive);

            bool hasVideo = Convert.ToBoolean(doc.DocumentNode.CssSelect("#cpMainContent_YoutubeVid").ToList().Count());
            if (hasVideo)
            {
                var spanBeforeIframe1 = doc.DocumentNode.CssSelect("#cpMainContent_YoutubeVid").ToList()[0];
                var lul = doc.DocumentNode.CssSelect(".fright").ToList();
                var spanBeforeIframe2 = doc.DocumentNode.CssSelect("#cpMainContent_YoutubeVid2").ToList()[0];
               
                AddVideoToPage(spanBeforeIframe1, page);
                AddVideoToPage(spanBeforeIframe2, page);
            }

            foreach (var node in nodes)
            {
                AddLinkToPage(node, page);
            }

            return page;
        }

        public List<Page> BuildMap(string parseLink/*, CancellationToken token*/)
        {
            List<Page> pages = new List<Page>();
            HashSet<string> visited = new HashSet<string>();
            Queue<string> analysisQueue = new Queue<string>();

            Page page;

            if (!IsLinkValid(parseLink))
            {
                throw new Exception("Unindexable link");
            }

            string pureLink = GetLinkWithoutAnchor(parseLink);

            visited.Add(pureLink);
            analysisQueue.Enqueue(pureLink);

            while (analysisQueue.Count > 0)
            {
                //if (token.IsCancellationRequested)
                //{
                //    MessageBox.Show("Operation aborted by closing window");
                //    token.ThrowIfCancellationRequested();
                //}

                page = ParsePage(analysisQueue.Dequeue());
                pages.Add(page);

                OnPageAnalyzed(page);

                foreach (var link in page.AllLinks)
                {
                    if (!IsLinkValid(link.URL))
                    {
                        continue;
                    }

                    if (IsExternalLink(link.URL) || link.URL.Contains("datamaskingwiki"))
                    {
                        continue;
                    }

                    pureLink = GetLinkWithoutAnchor(link.URL);

                    if (!visited.Contains(pureLink))
                    {
                        visited.Add(pureLink);
                        analysisQueue.Enqueue(pureLink);
                    }
                }
            }

            return pages;
        }
    }
}




