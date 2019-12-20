using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class CurriculumViewModel
    {
        public Curriculum Curriculum { get; set; }
        public List<Topic> AlleTopics { get; set; }
        public List<Topic> CurriculumTopics { get; set; }
        public List<Student> AlleStudenten { get; set; }
        public int[] TopicIds { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public List<Voortgang> voortgang { get; set; }
    }
}