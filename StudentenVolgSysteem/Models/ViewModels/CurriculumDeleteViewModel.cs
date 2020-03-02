using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class CurriculumDeleteViewModel
    {
        public Curriculum curriculum { get; set; }
        public List<CurriculumTopic> topics { get; set; }
    }
}