using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using StudentenVolgSysteem.Models;
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
        public ActionResult CreatePDF()
        {
            PdfViewModel pdfViewModel = new PdfViewModel();
            pdfViewModel.StudentList = GetStudents();
            pdfViewModel.NiveauList = GetNiveaus();
            pdfViewModel.Curricula = GetCurricula(1);
            //pdfViewModel.CourseList = GetCourses();

            return View(pdfViewModel);
        }

        [HttpPost]
        public ActionResult CreatePDF(PdfViewModel pdfViewModel)
        {
            pdfViewModel.StudentList = GetStudents();
            pdfViewModel.NiveauList = GetNiveaus();
            if (ModelState.IsValid)
            {
                MakePdf(pdfViewModel);
            }
            return View(pdfViewModel);
        }

        private void MakePdf(PdfViewModel pdfvm)
        {
            int studentId = 0;
            foreach (var item in pdfvm.StudentList)
            {
                if (item.Value == pdfvm.Student)
                {
                    studentId = Convert.ToInt32(item.Value);
                }
            }

            StudentModel student = db.Studenten.Find(studentId);

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("verdana", 20, XFontStyle.Bold);

            for (int i = 0; i < 100; i++)
            {
            gfx.DrawString(string.Format("Hello {0}, I can't let you do that.", student.WholeName),
                           font, XBrushes.Black,
                           new XRect(0, 20+ 21*i, page.Width, 0),
                           XStringFormats.Center);

            }

            string filename = $"Hello_{DateTime.Now.Millisecond.ToString()}.pdf";
            document.Save(filename);
            Process.Start(filename);
        }
    }
}