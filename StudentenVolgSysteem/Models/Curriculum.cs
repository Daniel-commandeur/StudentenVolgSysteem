using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Curricula")]
    public class Curriculum
    {
        public Curriculum()
        {
            this.Topics = new HashSet<Topic>();
        }

        [Key]
        public int CurriculumId { get; set; }
        public Student Student { get; set; }
        public string Naam { get; set; }
        public string NotitieDocent { get; set; }
        public string NotitieStudent { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }
    }

    [NotMapped]
    public class CUCurriculum : Curriculum
    {
        public CUCurriculum()
        {

        }

        public CUCurriculum(Curriculum c)
        {
            this.CurriculumId = c.CurriculumId;
            this.Student = c.Student;
            this.Topics = c.Topics;
            this.Naam = c.Naam;
            this.NotitieDocent = c.NotitieDocent;
            this.NotitieStudent = c.NotitieStudent;
        }
        public List<Topic> AlleTopics { get; set; }
        public string[] alleTopicIds { get; set; }
        public int StudentId { get; set; }
    }
}