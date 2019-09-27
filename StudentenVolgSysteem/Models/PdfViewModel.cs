using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace StudentenVolgSysteem.Models
{
    public class PdfViewModel
    {
        [Required(ErrorMessage = "Selecteer student")]
        public string Student { get; set; }
        public List<SelectListItem> StudentList { get; set; }

        [Required(ErrorMessage = "Voer geboortedatum in")]
        public DateTime DoB { get; set; }

        [Required(ErrorMessage = "Voer startdatum in")]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Selecteer Niveau")]
        public string Niveau { get; set; }
        public List<SelectListItem> NiveauList { get; set; }

        [Required(ErrorMessage = "Voer de richting in")]
        public string Course { get; set; }
        //public List<SelectListItem> CourseList { get; set; }

        [Required(ErrorMessage = "Selecteer curriculum")]
        public string Curriculum { get; set; }
        public List<SelectListItem> Curricula { get; set; }

        public bool Leerdoel { get; set; }
        public bool Certificeringen { get; set; }
    }
}