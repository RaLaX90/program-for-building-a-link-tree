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

        public void WriteToFile(string link)
        {
            using (FileStream fstream = new FileStream("text2.txt", FileMode.Open))
            {
                // преобразуем строку в байты
                link = "\r\n" + link;
                byte[] array = System.Text.Encoding.Default.GetBytes(link);
                // запись массива байтов в файл
                fstream.Seek(fstream.Length, SeekOrigin.Begin);
                fstream.Write(array, 0, array.Length);
                //Console.WriteLine("Текст записан в файл");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var txtHTML = GetPage(@"https://joblab.ru/search.php?r=vac&srregion=100&maxThread=100&submit=1");
            //var doc = new HtmlAgilityPack.HtmlDocument();   // Создание документа
            //doc.LoadHtml(txtHTML);
            //label1.Text = doc.ParsedText.ToString();

            //using (FileStream fstream = new FileStream("text.txt", FileMode.Open))
            //{
            //    // преобразуем строку в байты
            //    byte[] array = new byte[fstream.Length];
            //    // считываем данные
            //    fstream.Read(array, 0, array.Length);
            //    //fstream.Seek(array.Length / 2, SeekOrigin.Current);
            //    // декодируем байты в строку
            //    textFromFile[0] = System.Text.Encoding.Default.GetString(array);
            //    Console.WriteLine($"Текст из файла: {textFromFile}");
            //}

            List<string> textFromFile = new List<string>();

            string link = "";
            {


                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader("text.txt");
                while ((link = file.ReadLine()) != null)
                {
                    textFromFile.Add(link);
                }

                textFromFile.Capacity = textFromFile.Count;
                file.Close();
                link = "";
            }

            var web = new HtmlWeb();
            var rootLink = "http://localhost:50897/Products.aspx";
            HtmlDocument doc = web.Load(rootLink);

            var nodes = doc.DocumentNode.CssSelect("a").ToList();
            int counter = 0;

            foreach (var node in nodes)
            {
                //Console.WriteLine(node.InnerHtml);

                link = node.GetAttributeValue("href");
                
                counter = 0;

                foreach (string tempLink in textFromFile)
                {
                    if (tempLink == link)
                    {
                        break;
                    }
                    counter++;
                }

                if (counter == 51)
                {
                    WriteToFile(link);
                    //Console.WriteLine("Unique link, which was written to the file, = " + link);
                }
            }

            MessageBox.Show("Parse is finished");
        }
    }
}
