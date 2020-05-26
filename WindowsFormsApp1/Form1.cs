using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Html;
using ScrapySharp.Html.Forms;
using ScrapySharp.Network;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Parser = new CustomViewLinkParser();
            Parser.PageAnalyzed += Parser_ParserProgres;
        }

        private void ParsingComplete(List<Page> pages/*, CancellationToken token*/)
        {
            // TODO: need to upgrade
            MessageBox.Show("Parse is complete");
        }

        private void Parser_ParserProgres(ParserProgresEventArgs Args, int level)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(Args.Page.URL);

            foreach (var url in Args.Page.AllLinks)
            {
                stringBuilder.AppendLine($"~ {url.URL}");
            }
            Action action = () => textBox2.Text += stringBuilder;
            BeginInvoke(action);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //var txtHTML = GetPage(@"https://joblab.ru/search.php?r=vac&srregion=100&maxThread=100&submit=1");
            //var doc = new HtmlAgilityPack.HtmlDocument();   // Создание документа
            //doc.LoadHtml(txtHTML);
            //label1.Text = doc.ParsedText.ToString();

            //Parser.Parse1(textBox1.Text);
        }

        private CustomViewLinkParser Parser;
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private void Button2_Click(object sender, EventArgs e)
        {
            CancellationToken token = cancelTokenSource.Token;

            var url = textBox1.Text;

            Task.Run(() =>
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                var result = Parser.BuildMap(url/*, token*/);

                stopwatch.Stop();

                var time = stopwatch.Elapsed;

                MessageBox.Show($"Parse is finished from {time}");

                return result;
            }, token).ContinueWith(t => ParsingComplete(t.Result/*, token*/));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //cancelTokenSource.Cancel();
        }
    }
}
