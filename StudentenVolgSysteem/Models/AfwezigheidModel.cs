using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    public class AfwezigheidModel : IDeletable
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public AfwezigheidOptionsModel OptionsModel { get; set; }
        public Student Deelnemer { get; set; }
        public bool IsDeleted { get; set ; }
    }
}