using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class CurriculumTemplateViewModel
    {
        public CurriculumTemplate CurriculumTemplate { get; set; }
        public List<Topic> AlleTopics { get; set; }

        public int[] TopicIds { get; set; }
    }
}