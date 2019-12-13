using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Werkvormen")]
    public class Werkvorm
    {
        [Key]
        public int WerkvormId { get; set; }
        public string Naam { get; set; }
    }
}