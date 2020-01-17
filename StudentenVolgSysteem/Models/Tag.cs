using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Tags")]
    public class Tag : IDeletable
    {
        public Tag()
        {
            this.Topics = new HashSet<Topic>();
        }

        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public bool IsDeleted { get; set; }
    }
}