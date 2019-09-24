using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using StudentenVolgSysteem.Models;

namespace StudentenVolgSysteem.Controllers
{
    public class PdfController : Controller
    {
        MyDbContext db = new MyDbContext();

        public void MakePdfFromStudent(int? studentId)
        {
            if(studentId == null)
            {
                studentId = 1;
            }
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("verdana", 20, XFontStyle.Bold);

            gfx.DrawString(string.Format("Hello {0}, I can't let you do that.", db.Studenten.Find(studentId).FirstName ),
                           font, XBrushes.Black,
                           new XRect(0, 0, page.Width, page.Height),
                           XStringFormats.Center);

            string filename = "Hello.pdf";
            document.Save(filename);
            Process.Start(filename);
        }

        public void MakePdfFromCurriculum(int curriculumId)
        {

        }

        public void MakeCurriculumSegment(int curriculumId)
        {

        }
    }
}