using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DbSet<TopicModel> Topics { get; set; }
        public DbSet<BenodigdheidModel> Benodigdheden { get; set; }
        public DbSet<PercipiolinkModel> PercipioLinks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            modelBuilder.Entity<TopicModel>().HasMany(m => m.Voorkennis).WithMany();
        }
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
        public string Niveau { get; set; }
    }

    public class TijdsDuurModel
    {
        [Key]
        public int TijdsDuurId { get; set; }
        public string Eenheid { get; set; }
    }

    public class TagModel
    {
        public TagModel()
        {
            this.Topics = new HashSet<TopicModel>();
        }

        [Key]
        public int TagId { get; set; }
        public string Naam { get; set; }
        public virtual ICollection<TopicModel> Topics { get; set; }
    }

    public class CertificeringenInfraModel
    {
        public CertificeringenInfraModel()
        {
            this.Topics = new HashSet<TopicModel>();
        }

        [Key]
        public int CertificeringenInfraId { get; set; }
        public string Certificering { get; set; }
        public virtual ICollection<TopicModel> Topics { get; set; }
    }

    public class BenodigdheidModel
    {
        [Key]
        public int BenodigdheidId { get; set; }
        public string Content { get; set; }
        public TopicModel Topic { get; set; }
    }

    public class PercipiolinkModel
    {
        [Key]
        public int PercipiolinkId { get; set; }
        public string Link { get; set; }
        public TopicModel Topic { get; set; }
    }

    //public class VoorkennisModel
    //{
    //    [Key, Column(Order = 1)]
    //    public TopicModel NeededId { get; set; }

    //    [Key,Column(Order =2)]
    //    public TopicModel ForId { get; set; }
    //}

    public class TopicModel
    {
        public TopicModel()
        {
            this.Tags = new HashSet<TagModel>();
            this.Voorkennis = new HashSet<TopicModel>();
            this.Certificeringen = new HashSet<CertificeringenInfraModel>();
        }

        [Key]
        public int TopicId { get; set; }
        //Code
        public string Code { get; set; }
        //Niveau 	
        public NiveauModel Niveau { get; set; }
        //Topic	
        public string Name { get; set; }
        //Duur	
        public TijdsDuurModel Duur { get; set; }
        //Werkvorm(en)	
        public WerkvormModel Werkvorm { get; set; }
        //Leerdoel(en)	
        public string Leerdoel { get; set; }
        //Certificering	
        public virtual ICollection<CertificeringenInfraModel> Certificeringen { get; set; }
        //Benodigde voorkennis	
        public virtual ICollection<TopicModel> Voorkennis { get; set; }
        //Inhoud	
        public string Inhoud { get; set; }
        //Benodigdheden	
        public virtual ICollection<BenodigdheidModel> Benodigdheden { get; set; }
        //Percipio 
        public virtual ICollection<PercipiolinkModel> PercipioLinks { get; set; }
        //Tags
        public virtual ICollection<TagModel> Tags { get; set; }
    }

    /// <summary>
    /// Model for Create and update Topics
    /// </summary>
    [NotMapped]
    public class CUTopicModel : TopicModel
    {
        public IEnumerable<NiveauModel> CUNiveaus { get; set; }
        public IEnumerable<TijdsDuurModel> CUTijdsDuren { get; set; }
        public IEnumerable<WerkvormModel> CUwerkvormen { get; set; }
        public IEnumerable<CertificeringenInfraModel> CUCertificeringenInfras { get; set; }
        public IEnumerable<TagModel> CUTags { get; set; }
    }
}