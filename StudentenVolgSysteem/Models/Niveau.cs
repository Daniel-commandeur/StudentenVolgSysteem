using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Niveaus")]
    public class Niveau
    {
        [Key]
        public int NiveauId { get; set; }
        public string Naam { get; set; }

        // just added
        public bool IsDeleted { get; set; }
    }
}