using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class DeelnemerAfwezigheidListViewModel
    {
        public int Id { get; set; }
        public IEnumerable<SelectListItem> OptionsSelectList { get; set; }
        [Display(Name ="Opties")]
        public string Option { get; set; }
        public Student Student { get; set; }
        public DateTime Date { get; set; }
    }
}