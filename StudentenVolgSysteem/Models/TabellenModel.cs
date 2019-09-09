using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("DefaultConnection") { }
        public DbSet<WerkvormModel> Werkvormen { get; set; }
        public DbSet<NiveauModel> Niveaus { get; set; }
        public DbSet<TijdsDuurModel> TijdsDuren { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<CertificeringenInfraModel> CertificeringenInfras { get; set; }
    } 

    public class WerkvormModel
    {
        [Key]
        public int WerkvormId { get; set; }
        public string Werkvorm { get; set; }
    }

    public class NiveauModel
    {
        [Key]
        public int NiveauId { get; set; }
        public string niveau { get; set; }
    }

    public class TijdsDuurModel
    {
        [Key]
        public int TijdsDuurId { get; set; }
        public string Eenheid { get; set; }
    }

    public class TagModel
    {
        [Key]
        public int TagId { get; set; }
        public string naam { get; set; }
    }

    public class CertificeringenInfraModel
    {
        [Key]
        public int CertificeringenInfraId { get; set; }
        public string Certificering { get; set; }
    }

}