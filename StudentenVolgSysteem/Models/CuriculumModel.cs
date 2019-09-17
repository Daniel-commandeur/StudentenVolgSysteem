using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table (name: "Curiculums")]
    public class CuriculumModel
    {
        public CuriculumModel()
        {
            this.Topics = new HashSet<TopicModel>();
        }

        [Key]
        public int CuriculumId { get; set; }
        public StudentModel StudentId { get; set; }

        public virtual ICollection<TopicModel> Topics { get; set; }
    }
}