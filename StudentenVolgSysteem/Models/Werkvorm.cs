using StudentenVolgSysteem.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Werkvormen")]
    public class Werkvorm : IDeletable
    {
        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        public bool IsDeleted { get; set; }
    }
}