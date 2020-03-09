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
        public IEnumerable<SelectListItem> OptionsSelectList { get; set; }
        public List<AfwezigheidModel> Afwezigheid { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public List<string> Dates { get; set; }
    }
}