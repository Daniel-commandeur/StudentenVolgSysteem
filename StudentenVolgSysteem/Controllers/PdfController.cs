using PdfSharp.Drawing;
using PdfSharp.Pdf;
using StudentenVolgSysteem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace StudentenVolgSysteem.Controllers
{
    public class PdfController : Controller
    {
        MyDbContext db = new MyDbContext();

        public void MakePdfFromStudent(int? studentId)
        {
            if (studentId == null)
            {
                studentId = 1;
            }
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("verdana", 20, XFontStyle.Bold);

            gfx.DrawString(string.Format("Hello {0}, I can't let you do that.", db.Studenten.Find(studentId).FirstName),
                           font, XBrushes.Black,
                           new XRect(0, 0, page.Width, page.Height),
                           XStringFormats.Center);

            string filename = "Hello.pdf";
            document.Save(filename);
            Process.Start(filename);
        }

        [NonAction]
        private List<SelectListItem> GetCourses()
        {
            List<SelectListItem> listCourses = new List<SelectListItem>();
            //TODO: Make models and courses in the db and add a view for the admin to create, and edit
            listCourses.Add(new SelectListItem { Text = "Select", Value = "0" });
            listCourses.Add(new SelectListItem { Text = "Infra", Value = "1" });
            listCourses.Add(new SelectListItem { Text = "C#", Value = "2" });
            listCourses.Add(new SelectListItem { Text = "Data Science", Value = "3" });
            listCourses.Add(new SelectListItem { Text = "Cyber", Value = "4" });
            return listCourses;
        }

        [NonAction]
        public List<SelectListItem> GetStudents()
        {
            List<SelectListItem> listStudents = new List<SelectListItem>();
            listStudents.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in db.Studenten)
            {
                listStudents.Add(new SelectListItem { Text = item.WholeName, Value = item.StudentId.ToString() });
            }
            return listStudents;
        }

        [NonAction]
        public List<SelectListItem> GetNiveaus()
        {
            List<SelectListItem> listNiveaus = new List<SelectListItem>();
            listNiveaus.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in db.Niveaus)
            {
                listNiveaus.Add(new SelectListItem { Text = item.Niveau, Value = item.NiveauId.ToString() });
            }
            return listNiveaus;
        }

        [NonAction]
        public List<SelectListItem> GetCurricula(int studentId)
        {
            StudentModel student = db.Studenten.Find(studentId);
            List<SelectListItem> curricula = new List<SelectListItem>();
            curricula.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var curriculum in student.Curiculums)
            {
                curricula.Add(new SelectListItem { Text = curriculum.Name, Value = curriculum.CuriculumId.ToString() });
            }

            return curricula;
        }

        [HttpGet]
        public ActionResult CreatePDF(int? curriculumId)
        {
            if (curriculumId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }
            CuriculumModel curiculum = db.Curiculums.Include("StudentId").Where(m => m.CuriculumId == curriculumId).FirstOrDefault();
            if (curiculum == null)
            {
                return HttpNotFound();
            }

            PdfViewModel pdfViewModel = new PdfViewModel();

            pdfViewModel.Student = db.Studenten.Find(curiculum.StudentId.StudentId).WholeName;
            pdfViewModel.curriculumId = (int)curriculumId;
            pdfViewModel.NiveauList = GetNiveaus();

            return View(pdfViewModel);
        }

        [HttpPost]
        public ActionResult CreatePDF(PdfViewModel pdfViewModel)
        {
            pdfViewModel.NiveauList = GetNiveaus();
            if (ModelState.IsValid)
            {
                MakePdf(pdfViewModel);
            }
            return View(pdfViewModel);
        }

        private void MakePdf(PdfViewModel pdfvm)
        {
            CuriculumModel curriculum = db.Curiculums.Include("StudentId").Where(m => m.CuriculumId == pdfvm.curriculumId).FirstOrDefault();
            StudentModel student = curriculum.StudentId;

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //Fonts
            XFont verdana20Bold = new XFont("verdana", 20, XFontStyle.Bold);
            XFont arial20BoldItalic = new XFont("arial", 20, XFontStyle.BoldItalic);
            XFont LabelFont = new XFont("arial", 14, XFontStyle.Regular);

            //Title Header
            gfx.DrawString("Persoonlijk Curriculum", verdana20Bold, XBrushes.Black,
                           new XRect(0, 20, page.Width, 0), XStringFormats.Center);
            gfx.DrawString("ITvitae learning", arial20BoldItalic, XBrushes.Black,
                           new XRect(0, 41, page.Width, 0), XStringFormats.Center);
            //XImage ITvitaeLogo = XImage.FromFile(string.Empty);
            //gfx.DrawImage(ITvitaeLogo, new XRect(0, 0, 0, 0));

            //30-100-30 -100 -75-100-30 -100 -30
            //0 |30 |130|160|260|335|435|465|565|595
            //Student Info
            int col1x = 30;
            int col2x = 130;
            int col3x = 335;
            int col4x = 435;

            int colwidth = 100;
            int rowheight = 20;

            int row1 = 60;
            int row2 = 80;
            int row3 = 100;

            DrawLabel("Naam", col1x, row1, colwidth, rowheight, gfx);
            DrawLabel(student.WholeName, col2x, row1, colwidth, rowheight, gfx);

            DrawLabel("Geboortedatum", col1x, row2, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.DoB.ToShortDateString(), col2x, row2, colwidth, rowheight, gfx);

            DrawLabel("StartDatum", col1x, row3, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.StartDate.ToShortDateString(), col2x, row3, colwidth, rowheight, gfx);

            DrawLabel("Einddatum", col3x, row3, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.EndDate.ToShortDateString(), col4x, row3, colwidth, rowheight, gfx);

            DrawLabel("Niveau", col3x, row1, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.Niveau, col4x, row1, colwidth, rowheight, gfx);

            DrawLabel("Richting", col3x, row2, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.Course, col4x, row2, colwidth, rowheight, gfx);

            string filename = $"Hello_{DateTime.Now.Millisecond.ToString()}.pdf";
            document.Save(filename);
            Process.Start(filename);
        }

        private void DrawLabel(string text, int x, int y, int width, int height, XGraphics gfx)
        {
            XFont LabelFont = new XFont("arial", 14, XFontStyle.Regular);
            gfx.DrawString(text, LabelFont, XBrushes.Black, new XRect(x, y, width, height), XStringFormats.CenterLeft);
        }
    }
}