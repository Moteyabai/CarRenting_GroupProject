using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using NotesFor.HtmlToOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace write_new_file
{
    public class Writing1
    {
        public static void ConvertHtmlToDocx(string htmlContent, string outputPath)
        {
            
            string b = outputPath;
            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new DocumentFormat.OpenXml.Wordprocessing.Document(new DocumentFormat.OpenXml.Wordprocessing.Body()).Save(mainPart);
                    }

                    HtmlConverter converter = new HtmlConverter(mainPart);
                }

                File.WriteAllBytes(outputPath, generatedDocument.ToArray());
            }
        }
    }
}
