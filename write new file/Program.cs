using DocumentFormat.OpenXml.Packaging;
using NotesFor.HtmlToOpenXml;
using System;
using System.IO;

namespace write_new_file
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fileName = "HopDongThueXe2311.docx";
            string a = "<h1>aaa</h1>";
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            Writing1.ConvertHtmlToDocx(fileName,outputPath);
            Console.WriteLine("haha");
        }
    }
}
