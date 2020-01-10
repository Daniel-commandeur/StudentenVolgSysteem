using StudentenVolgSysteem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;
using StudentenVolgSysteem.DAL;
using StudentenVolgSysteem.Documents;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator, Docent")]
    public class PdfController : Controller
    {
        SVSContext db = new SVSContext();

        [NonAction]
        private List<SelectListItem> GetCourses()
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public List<SelectListItem> GetStudents()
        {
            List<SelectListItem> listStudents = new List<SelectListItem>();
            listStudents.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in db.Studenten)
            {
                listStudents.Add(new SelectListItem { Text = item.VolledigeNaam, Value = item.StudentId.ToString() });
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
                listNiveaus.Add(new SelectListItem { Text = item.Naam, Value = item.NiveauId.ToString() });
            }
            return listNiveaus;
        }

        [NonAction]
        public List<SelectListItem> GetCurricula(int studentId)
        {
            Student student = db.Studenten.Find(studentId);
            List<SelectListItem> curricula = new List<SelectListItem>();
            curricula.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var curriculum in student.Curricula)
            {
                curricula.Add(new SelectListItem { Text = curriculum.Naam, Value = curriculum.CurriculumId.ToString() });
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
            Curriculum curiculum = db.Curricula.Include("Student").Where(m => m.CurriculumId == curriculumId).FirstOrDefault();
            if (curiculum == null)
            {
                return HttpNotFound();
            }

            PdfViewModel pdfViewModel = new PdfViewModel();

            pdfViewModel.Student = db.Studenten.Find(curiculum.Student.StudentId).VolledigeNaam;
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
                Curriculum curriculum = db.Curricula.Include("Student").Include("Topics.Topic").Where(m => m.CurriculumId == pdfViewModel.curriculumId).FirstOrDefault();
                PDF.Create(pdfViewModel, curriculum);
                //MakePdf(pdfViewModel);
            }
            return View(pdfViewModel);
        }
    }
}