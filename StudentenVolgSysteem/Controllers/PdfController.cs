using StudentenVolgSysteem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;
using StudentenVolgSysteem.Documents;



namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator, Docent")]
    public class PdfController : Controller
    {
        MyDbContext db = new MyDbContext();

        // GET: PDF/CreatePDF
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

        // POST: PDF/CreatePDF 
        [HttpPost]
        public ActionResult CreatePDF(PdfViewModel pdfViewModel)
        {
            pdfViewModel.NiveauList = GetNiveaus();
            if (ModelState.IsValid)
            {
                CuriculumModel curriculum = db.Curiculums.Include("StudentId").Where(m => m.CuriculumId == pdfViewModel.curriculumId).FirstOrDefault();

                PDF.Create(pdfViewModel, curriculum);
            }
            return View(pdfViewModel);
        }

        #region Helpers

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

        #endregion

    }

}