using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Studenten")]
    public class Student : IDeletable
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Voornaam { get; set; }
        
        [Required]
        public string Achternaam { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime AanmeldDatum { get; set; }
        
        [NotMapped]
        [Display(Name = "Naam")]
        public string VolledigeNaam { get { return $"{Voornaam} {Achternaam}"; } }

        public virtual ICollection<Curriculum> Curricula { get; set; }
            
        public virtual ICollection<AfwezigheidModel> Afwezigheden { get; set; }

        public bool IsDeleted { get; set; }
    }
}