using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Curricula")]
    public class Curriculum : IDeletable
    {
        public Curriculum()
        {
            this.Topics = new HashSet<CurriculumTopic>();
        }

        [Key]
        public int Id { get; set; }

        public virtual Student Student { get; set; }

        //[Required]
        public string Naam { get; set; }
        public string NotitieDocent { get; set; }
        public string NotitieStudent { get; set; }
        
        [Required]
        [Display(Name = "Onderwerpen")]
        public virtual ICollection<CurriculumTopic> Topics { get; set; }

        public bool IsDeleted { get; set; }
    }
}