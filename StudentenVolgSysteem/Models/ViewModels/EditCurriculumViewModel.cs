using System.Collections.Generic;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class EditCurriculumViewModel
    {
        public Curriculum Curriculum { get; set; }
        public List<Topic> AlleTopics { get; set; }
        public int[] TopicIds { get; set; }

    }
}