using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    public class AfwezigheidModel : IDeletable
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [ForeignKey("OptionsModel")]
        public int OptionsId { get; set; }
        public AfwezigheidOptionsModel OptionsModel { get; set; }

        [ForeignKey("Deelnemer")]
        public int DeelnemerId { get; set; }
        public Student Deelnemer { get; set; }
        public bool IsDeleted { get; set ; }
    }
}