﻿using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Benodigdheden")]
    public class Benodigdheid : IDeletable
    {
        public Benodigdheid()
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