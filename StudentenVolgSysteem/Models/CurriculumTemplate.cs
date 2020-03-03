using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    public class CurriculumTemplate : IDeletable
    {

        public CurriculumTemplate()
        {
            this.Topics = new HashSet<Topic>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Naam { get; set; }
        [Required]
        public virtual ICollection<Topic> Topics { get; set; }

        public bool IsDeleted { get; set; }

    }
}