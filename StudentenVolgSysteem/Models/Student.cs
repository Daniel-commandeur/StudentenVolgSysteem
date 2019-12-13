using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Studenten")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }

        [NotMapped]
        public string VolledigeNaam { get { return $"{Voornaam} {Achternaam}"; } }

        public ICollection<Curriculum> Curricula { get; set; }
    }

    [NotMapped]
    public class CUStudent : Student
    {
        public CUStudent()
        {

        }

        public CUStudent(Student s)
        {
            this.Curricula = s.Curricula;
            this.StudentId = s.StudentId;
            this.Voornaam = s.Voornaam;
            this.Achternaam = s.Achternaam;
        }
    }
}