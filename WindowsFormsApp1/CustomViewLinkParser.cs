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

            exceptions = File.ReadAllLines("text.txt");

            exceptionsCount = exceptions.Length;

        }

        private string rootLink;

        //1
        private string[] exceptions;

        private int exceptionsCount;

        //2
        private string[] visited;

        private int visitedCount;

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

        //public void Parse1(string startParseLink)
        //{
        //    var web = new HtmlWeb();
        //    var currentLink = startParseLink;
        //    HtmlAgilityPack.HtmlDocument doc = web.Load(rootLink + currentLink);

        //    var nodes = doc.DocumentNode.CssSelect("a").ToList();

        //    int counter = 0;
        //    string link;

        //    WriteToFile("\r\n!!!" + rootLink + currentLink + "!!!", "text2.txt");
        //    foreach (var node in nodes)
        //    {
        //        //Console.WriteLine(node.InnerHtml);

        //        link = node.GetAttributeValue("href");

        //        if ((link.Length > 0) && link[0] != '#' && (link.IndexOf("datamaskingwiki") < 0) && (link.IndexOf("mailto") < 0)
        //            && (link.IndexOf("http") < 0) && (link.IndexOf("https") < 0) && (link.IndexOf("tel") < 0))
        //        {
        //            counter = 0;

        //            foreach (string tempLink in exceptions)
        //            {
        //                if (tempLink == link)
        //                {
        //                    break;
        //                }
        //                counter++;
        //            }

        //            if (counter == exceptionsCount)
        //            {
        //                WriteToFile(link,"text2.txt");
        //                //Console.WriteLine("Unique link, which was written to the file, = " + link);
        //            }
        //        }

        //    }

        //    MessageBox.Show("Parse is finished");
        //}

        public void refreshVisitedCount()
        {
            visited = File.ReadAllLines("Visited.txt");

            visitedCount = visited.Length;
        }

        public void Parse2(string ParseLink, int level = 0)
        {
            var web = new HtmlWeb();
            var currentLink = ParseLink;
            HtmlAgilityPack.HtmlDocument doc = web.Load(rootLink + currentLink);

            var nodes = doc.DocumentNode.CssSelect("a").ToList();

            int counter = 0;
            string link, linkWithoutAnchor;

            WriteToFile("!!! " + rootLink + currentLink + " !!!", "Visited.txt", 0); //can be optimazed without it
            WriteToFile(currentLink, "Visited.txt", 0);
            WriteToFile("!!! " + rootLink + currentLink + " !!!", "Marked.txt", level); //can be optimazed without it

            foreach (var node in nodes)
            {
                //Console.WriteLine(node.InnerHtml);

                link = node.GetAttributeValue("href");

                WriteToFile(link, "Marked.txt", level + 1);
                
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
                        this.Parse2(linkWithoutAnchor, level + 1);
                    }
                }
            }

            WriteToFile("??? " + rootLink + currentLink + " ???", "Marked.txt", level); //can be optimazed without it 
            WriteToFile("??? " + rootLink + currentLink + " ???", "Visited.txt", 0); //can be optimazed without it
        }
    }
}
