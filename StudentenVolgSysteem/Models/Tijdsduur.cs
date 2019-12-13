using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Tijdsduren")]
    public class Tijdsduur
    {
        [Key]
        public int TijdsduurId { get; set; }
        public string Eenheid { get; set; }
    }
}