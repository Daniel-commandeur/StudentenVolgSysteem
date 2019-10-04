using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table (name: "Studenten")]
    public class StudentModel
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string WholeName { get { return $"{FirstName} {LastName}"; } }

        public ICollection<CuriculumModel> Curiculums { get; set; }
    }

    [NotMapped]
    public class CUStudentModel : StudentModel
    {
        public CUStudentModel()
        {

        }
        public CUStudentModel(StudentModel sm)
        {
            this.Curiculums = sm.Curiculums;
            this.StudentId = sm.StudentId;
            this.FirstName = sm.FirstName;
            this.LastName = sm.LastName;
        }


    }
}