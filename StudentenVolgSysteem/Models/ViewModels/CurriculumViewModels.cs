using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class CurriculumViewModels
    {
        public Curriculum Curriculum { get; set; }
        public List<Topic> AlleTopics { get; set; }
        public int StudentId { get; set; }
    }
}