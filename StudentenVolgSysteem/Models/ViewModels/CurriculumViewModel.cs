using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class CurriculumViewModel
    {
        public CurriculumTemplate CurriculumTemplate { get; set; }

        public int CurriculumTemplateId { get; set; }
        public List<Niveau> Niveaus { get; set; }
        public Student Student { get; set; }

        public int StudentId { get; set; }
        public int NiveauId { get; set; }
        public List<CurriculumTemplate> CurriculumTemplates { get; set; }
        public List<Topic> AlleTopics { get; set; }
        public int[] TopicIds { get; set; }

        // What is this for? 0 references
        //public List<Topic> CurriculumTopics { get; set; }
        //public List<Voortgang> Voortgang { get; set; }
    }
}