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
    class CustomViewLinkParser
    {
        public CustomViewLinkParser(string rootLink = "http://localhost:50897/")
        {
            this.rootLink = rootLink;
        }

        private string rootLink;

        private string[] visited;

        private int visitedCount;

        List<Page> Pages = new List<Page>();
        //private void WriteToFile(string link, string file, int level)
        //{
        //    using (FileStream fstream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.None))
        //    using (var write = new StreamWriter(fstream))
        //    {
        //        for (int i = 0; i < level; i++)
        //        {
        //            write.Write("~");
        //        }
        //        write.WriteLine(link);
        //    }
        //}

        private void WriteToTextBox(string text, TextBox textBox, int level)
        {
            for (int i = 0; i < level; i++)
            {
                textBox.Text += "~";
            }
            textBox.Text += text + "\r\n";
            textBox.Update();
        }

        public void refreshVisitedCount()
        {
            visited = File.ReadAllLines("Visited.txt");

            visitedCount = visited.Length;
        }

        public string GettingAClassAboutParentDiv(HtmlNode Child)
        {
            HtmlNode node = Child;

            while (true)
            {
                if (node.ParentNode.Name == "div")
                {
                    return Child.ParentNode.GetAttributeValue("class");
                }
                else
                {
                    node = node.ParentNode;
                }
            }

        }

        public void Parse2(string ParseLink, TextBox textBox, int level = 0)
        {
            var web = new HtmlWeb();
            var currentLink = ParseLink; // can optimaze, if use directly
            HtmlAgilityPack.HtmlDocument doc = web.Load(rootLink + currentLink);

            bool hasForms = Convert.ToBoolean(doc.DocumentNode.Descendants("form").Count());

            bool hasInteractive = Convert.ToBoolean(doc.DocumentNode.CssSelect("#cpMainContent_customActionPlaceholder").ToList().Count());
           
            var nodes = doc.DocumentNode.CssSelect("a").ToList();

            int counter = 0;
            string link, linkWithoutAnchor;

            Pages.Add(new Page(currentLink, rootLink + currentLink));
                
            //WriteToFile("!!! " + rootLink + currentLink + " !!!", "Visited.txt", 0); //can be optimazed without it
            //WriteToFile(currentLink, "Visited.txt", 0);
            //WriteToFile("!!! " + rootLink + currentLink + " !!!", "Marked.txt", level); //can be optimazed without it
            WriteToTextBox("!!! " + rootLink + currentLink + " !!!", textBox, level);

            foreach (var node in nodes)
            {
                //Console.WriteLine(node.InnerHtml);

                link = node.GetAttributeValue("href");

                GettingAClassAboutParentDiv(node);

                // WriteToFile(link, "Marked.txt", level + 1);
                WriteToTextBox(link, textBox, level + 1);
               
                if ((link.Length > 0) && link[0] != '#' && (link.IndexOf("datamaskingwiki") < 0) && (link.IndexOf("mailto") < 0)
                        && (link.IndexOf("http") < 0) && (link.IndexOf("tel") < 0) && (link.IndexOf("pdf") < 0))
                {
                    counter = 0;

                    refreshVisitedCount();

                    linkWithoutAnchor = (link.IndexOf("#") > -1) ? link.Remove(link.IndexOf("#")) : link;

                    foreach (string tempLink in visited)
                    {
                        if (tempLink == linkWithoutAnchor)
                        {
                            break;
                        }
                        counter++;
                    }

                    if (counter == visitedCount)
                    {
                        //this.Parse2(linkWithoutAnchor, level + 1);
                    }
                }
            }

            //WriteToFile("??? " + rootLink + currentLink + " ???", "Marked.txt", level); //can be optimazed without it 
            //WriteToFile("??? " + rootLink + currentLink + " ???", "Visited.txt", 0); //can be optimazed without it
        }
    }
}




