using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PennyTest2.PdfHelper
{
    public class ITextFooterEvents : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate footerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        string pdfFormNo;

        #region Fields
        private string _header;

        public ITextFooterEvents(string formNo)
        {
            this.pdfFormNo = formNo;
        }
        #endregion

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            String text = $"PAGE {writer.PageNumber} of ";
            String formNo = $"FORM NO : {pdfFormNo}";

            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                float len = bf.GetWidthPoint(text, 12);
                float len2 = bf.GetWidthPoint(formNo, 12);
                cb.SetTextMatrix(document.PageSize.Width / 2 - len, document.PageSize.GetBottom(20));
                cb.ShowText(text);
                cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(20));
                cb.ShowText(formNo);
                cb.EndText();
                cb.AddTemplate(footerTemplate, document.PageSize.Width / 2, document.PageSize.GetBottom(20));
            }

            //Move the pointer and draw line to separate footer section from rest of page
            //cb.MoveTo(40, document.PageSize.GetBottom(50));
            //cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            //cb.Stroke();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bf, 12);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText(writer.PageNumber.ToString());
            footerTemplate.EndText();
        }
    }
}