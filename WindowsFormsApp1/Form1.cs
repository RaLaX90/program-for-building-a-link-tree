using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var txtHTML = GetPage(@"https://joblab.ru/search.php?r=vac&srregion=100&maxThread=100&submit=1");
            //var doc = new HtmlAgilityPack.HtmlDocument();   // Создание документа
            //doc.LoadHtml(txtHTML);
            //label1.Text = doc.ParsedText.ToString();

            var Parser = new CustomViewLinkParser();

            Parser.Parse1(textBox1.Text);

        }
    }
}
