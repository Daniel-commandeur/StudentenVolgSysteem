using StudentenVolgSysteem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
                listStudents.Add(new SelectListItem { Text = item.VolledigeNaam, Value = item.Id.ToString() });
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
                listNiveaus.Add(new SelectListItem { Text = item.Naam, Value = item.Id.ToString() });
            }
            return listNiveaus;
        }

        [NonAction]
        public SelectListItem GetCurriculum(int studentId)
        {
            Student student = db.Studenten.Find(studentId);
            SelectListItem curriculum = new SelectListItem
            {
                Text = student.Curriculum.Naam,
                Value = student.Curriculum.Id.ToString()
            };

            return curriculum;
        }

        [HttpGet]
        public ActionResult CreatePDF(int? studentId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.GetFromDatabase<Student>(studentId);

            Curriculum curriculum = student.Curriculum;
                //db.Curricula.Include("Student").Where(m => m.Id == curriculumId).FirstOrDefault();
            if (curriculum == null)
            {
                return HttpNotFound();
            }

            PdfViewModel pdfViewModel = new PdfViewModel();

            pdfViewModel.StudentId = student.Id;
            pdfViewModel.StudentNaam = student.VolledigeNaam;
            pdfViewModel.CurriculumId = curriculum.Id;
            pdfViewModel.NiveauList = GetNiveaus();
            pdfViewModel.Course = student.CurriculumTemplate.Naam;
            //pdfViewModel.Niveau = student.Curriculum.

            return View(pdfViewModel);
        }

        [HttpPost]
        public ActionResult CreatePDF(PdfViewModel pdfViewModel)
        {
            pdfViewModel.NiveauList = GetNiveaus();
            if (ModelState.IsValid)
            {
                Student student = db.Studenten.Include(a => a.Curriculum)
                                        .Include(a => a.Curriculum.Topics.Select(c => c.Topic.Duur))
                                        .Include(a => a.Curriculum.Topics.Select(c => c.Topic.Benodigdheden))
                                        .Include(a => a.Curriculum.Topics.Select(c => c.Topic.Werkvorm))
                                        .Include(a => a.Curriculum.Topics.Select(c => c.Topic.Niveau))
                                        .Where(a => a.Id == pdfViewModel.StudentId)
                                        .FirstOrDefault();
                //Student student = db.Studenten.Include("")
                //Curriculum curriculum = db.Curricula.Include("Student").Include("Topics.Topic").Where(m => m.Id == pdfViewModel.CurriculumId).FirstOrDefault();
                PDF.Create(pdfViewModel, student);
            }
            return View(pdfViewModel);
        }
    }
    }