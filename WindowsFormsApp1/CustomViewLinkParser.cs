using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class CustomViewLinkParser
    {
        public CustomViewLinkParser(string rootLink = "http://localhost:50897/")
        {
            this._rootLink = rootLink;
        }

        private readonly string _rootLink;

        private string[] _visited;

        private int _visitedCount;

        public delegate void ParserProgresEventHandler(string text, int level);

        public event ParserProgresEventHandler ParserProgres;

        private List<Page> _pages = new List<Page>();

        private void WriteToFile(string link, string file, int level)
        {
            using (FileStream fstream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var write = new StreamWriter(fstream))
            {
                for (int i = 0; i < level; i++)
                {
                    write.Write("~");
                }
                write.WriteLine(link);
            }
        }

        public void RefreshVisitedCount()
        {
            _visited = File.ReadAllLines("Visited.txt");

            _visitedCount = _visited.Length;
        }

        public string GettingAClassAboutParentDiv(HtmlNode child)
        {
            HtmlNode node = child;

            while (true)
            {
                if (node.ParentNode.Name == "div")
                {
                    return child.ParentNode.GetAttributeValue("class");
                }
                else
                {
                    node = node.ParentNode;
                }
            }

        }

        public void ParsingAndArraying(HtmlNode node)
        {
            var parentClass = GettingAClassAboutParentDiv(node);
            var linkURL = node.GetAttributeValue("href");
            var sectionsCount = _pages[_pages.Count - 1].WhatContainsInSections().Count;
            var last = _pages.Last(); 
            if (Convert.ToBoolean(sectionsCount))
            {
                if (parentClass != _pages[_pages.Count - 1].WhatContainsInSections()[sectionsCount - 1].Title) // !
                {
                    _pages[_pages.Count - 1].AddSection(GettingAClassAboutParentDiv(node));
                }
            }
            else
            {
                _pages[_pages.Count - 1].AddSection(GettingAClassAboutParentDiv(node));
            }

            sectionsCount = _pages[_pages.Count - 1].WhatContainsInSections().Count;

            _pages[_pages.Count - 1].WhatContainsInSections()[sectionsCount - 1].AddLink(node.InnerHtml, LinkType.Internal, linkURL);
        }

        public void Parse(string parseLink, int level = 0)
        {
            var web = new HtmlWeb();
            var currentLink = parseLink; // can optimaze, if use directly
            HtmlAgilityPack.HtmlDocument doc = web.Load(_rootLink + currentLink);

            //TODO: create a dynamic collection of object type on the page
            bool hasForm = Convert.ToBoolean(doc.DocumentNode.Descendants("form").Count());
            bool hasInteractive = Convert.ToBoolean(doc.DocumentNode.CssSelect("#cpMainContent_customActionPlaceholder").ToList().Count());

            var nodes = doc.DocumentNode.CssSelect("a").ToList();
            
            //WriteToFile("!!! " + rootLink + currentLink + " !!!", "Visited.txt", 0); //can be optimazed without it
            WriteToFile(currentLink, "Visited.txt", 0);
            //WriteToFile("!!! " + rootLink + currentLink + " !!!", "Marked.txt", level); //can be optimazed without it
            ParserProgres("!!! " + _rootLink + currentLink + " !!!", 0); //can be optimazed without it 

            _pages.Add(new Page(currentLink, _rootLink + currentLink, hasForm, hasInteractive));
            
            foreach (var node in nodes)
            {
                //Console.WriteLine(node.InnerHtml);

                ParsingAndArraying(node);

                // WriteToFile(link, "Marked.txt", level + 1);
                ParserProgres(node.GetAttributeValue("href"), level + 1);
            }

            int counter = 0;
            string linkURL, linkWithoutAnchor;

            foreach (var node in nodes)
            {
                linkURL = node.GetAttributeValue("href"); // extra work

                if ((linkURL.Length > 0) && linkURL[0] != '#' && (linkURL.IndexOf("datamaskingwiki") < 0) && (linkURL.IndexOf("mailto") < 0)
                        && (linkURL.IndexOf("http") < 0) && (linkURL.IndexOf("tel") < 0) && (linkURL.IndexOf("pdf") < 0))
                {
                    counter = 0;

                    RefreshVisitedCount();

                    linkWithoutAnchor = (linkURL.IndexOf("#") > -1) ? linkURL.Remove(linkURL.IndexOf("#")) : linkURL;

                    foreach (string tempLink in _visited)
                    {
                        if (tempLink == linkWithoutAnchor)
                        {
                            break;
                        }
                        counter++;
                    }

                    if (counter == _visitedCount)
                    {
                        //this.Parse(linkWithoutAnchor, level + 1);
                    }
                }
            }
            //WriteToFile("??? " + rootLink + currentLink + " ???", "Marked.txt", level); //can be optimazed without it 
            //WriteToFile("??? " + rootLink + currentLink + " ???", "Visited.txt", 0); //can be optimazed without it
        }
    }
}




