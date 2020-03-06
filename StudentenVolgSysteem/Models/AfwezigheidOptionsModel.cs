using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    public class AfwezigheidOptionsModel : IDeletable
    {
        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        public bool IsDeleted { get; set; }
    }
}