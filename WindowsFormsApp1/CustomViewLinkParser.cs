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
        public CustomViewLinkParser(string rootLink = "http://localhost:50897")
        {
            this.rootLink = rootLink;

            exceptions = File.ReadAllLines("text.txt");

            exceptionsCount = exceptions.Length;
        }
        
        private string rootLink;

        private string[] exceptions;

        private int exceptionsCount;

        private void WriteToFile(string link)
        {
            using (FileStream fstream = new FileStream("text2.txt", FileMode.Append, FileAccess.Write, FileShare.None))
            using (var write = new StreamWriter(fstream))
            {
                write.WriteLine(link);
            }
        }

        public void Parse1(string startParseLink)
        {
            var web = new HtmlWeb();
            var currentLink = startParseLink;
            HtmlAgilityPack.HtmlDocument doc = web.Load(rootLink + currentLink);

            var nodes = doc.DocumentNode.CssSelect("a").ToList();

            int counter = 0;
            string link;

            WriteToFile("\r\n!!!" + rootLink + currentLink + "!!!");
            foreach (var node in nodes)
            {
                //Console.WriteLine(node.InnerHtml);

                link = node.GetAttributeValue("href");

                if ((link.Length > 0) && link[0] != '#' && (link.IndexOf("datamaskingwiki") < 0) && (link.IndexOf("mailto") < 0) 
                    && (link.IndexOf("http") < 0) && (link.IndexOf("https") < 0) && (link.IndexOf("tel") < 0))
                {
                    counter = 0;

                    foreach (string tempLink in exceptions)
                    {
                        if (tempLink == link)
                        {
                            break;
                        }
                        counter++;
                    }

                    if (counter == exceptionsCount)
                    {
                        WriteToFile(link);
                        //Console.WriteLine("Unique link, which was written to the file, = " + link);
                    }
                }

            }

            MessageBox.Show("Parse is finished");
        }
    }
}
