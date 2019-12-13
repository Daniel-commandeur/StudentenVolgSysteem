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

        public MyDbContext() : base(GetBase())
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<MyDbContext>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MyDbContext>());
        }

        private static string GetBase()
        {
            string ret = "name=TheMatrix2_0-Live";
#if (DEBUG)
            ret = "name=TheMatrix2_0";
#endif
            return (ret);
        }

        public DbSet<WerkvormModel> Werkvormen { get; set; }
        public DbSet<NiveauModel> Niveaus { get; set; }
        public DbSet<TijdsDuurModel> TijdsDuren { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<CertificeringenInfraModel> CertificeringenInfras { get; set; }
        public DbSet<TopicModel> Topics { get; set; }
        public DbSet<BenodigdheidModel> Benodigdheden { get; set; }
        public DbSet<PercipiolinkModel> PercipioLinks { get; set; }
        public DbSet<StudentModel> Studenten { get; set; }
        public DbSet<CuriculumModel> Curiculums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            modelBuilder.Entity<TopicModel>().HasMany(m => m.Voorkennis).WithMany();

        }
    }


    [Table(name: "Werkvormen")]
    public class WerkvormModel
    {
        [Key]
        public int WerkvormId { get; set; }
        public string Werkvorm { get; set; }
    }


    [Table(name: "Niveaus")]
    public class NiveauModel
    {
        [Key]
        public int NiveauId { get; set; }
        public string Niveau { get; set; }
    }


    [Table(name: "Tijdsduren")]
    public class TijdsDuurModel
    {
        [Key]
        public int TijdsDuurId { get; set; }
        public string Eenheid { get; set; }
    }


    [Table(name: "Tags")]
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


    [Table(name: "Certificeringen")]
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


    [Table(name: "Benodigdheden")]
    public class BenodigdheidModel
    {
        public BenodigdheidModel()
        {
            this.Topics = new HashSet<TopicModel>();
        }

        [Key]
        public int BenodigdheidId { get; set; }
        public string Content { get; set; }
        public virtual ICollection<TopicModel> Topics { get; set; }
    }

    [Table(name: "PercipioLinks")]
    public class PercipiolinkModel
    {
        public PercipiolinkModel()
        {
            this.Topics = new HashSet<TopicModel>();
        }

        [Key]
        public int PercipiolinkId { get; set; }
        public string Link { get; set; }
        public virtual ICollection<TopicModel> Topics { get; set; }
    }

    [Table(name: "Topics")]
    public class TopicModel
    {
        public TopicModel()
        {
            this.Tags = new HashSet<TagModel>();
            this.Voorkennis = new HashSet<TopicModel>();
            this.Certificeringen = new HashSet<CertificeringenInfraModel>();
            this.Curiculums = new HashSet<CuriculumModel>();
            this.Benodigdheden = new HashSet<BenodigdheidModel>();
            this.PercipioLinks = new HashSet<PercipiolinkModel>();
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
        public virtual ICollection<CuriculumModel> Curiculums { get; set; }

        [NotMapped]
        public string NameCode
        {
            get
            {
                string returnString;
                if (string.IsNullOrEmpty(Name))
                {
                    returnString = Code;
                }
                else
                {
                    returnString = Name;
                }
                return returnString;
            }
        }

        public void CopyCUTopicModelToTopicModel(CUTopicModel cuTopicModel, MyDbContext db)
        {
            //Make changes to the topicModel here, we have to get all the models from side-tables from the dbcontext by reference.
            //Otherwise EF will think these are new objects
            this.Code = cuTopicModel.Code;
            this.Niveau = db.Niveaus.FirstOrDefault(n => n.NiveauId.ToString() == cuTopicModel.NiveauId);
            this.Name = cuTopicModel.Name;
            this.Duur = db.TijdsDuren.FirstOrDefault(d => d.TijdsDuurId.ToString() == cuTopicModel.TijdsDuurId);
            this.Werkvorm = db.Werkvormen.FirstOrDefault(w => w.WerkvormId.ToString() == cuTopicModel.WerkvormId);
            this.Leerdoel = cuTopicModel.Leerdoel;
            this.Inhoud = cuTopicModel.Inhoud;

            //Certs
            this.Certificeringen.Clear();
            try
            {
                foreach (var certId in cuTopicModel.CertificeringIds)
                {
                    this.Certificeringen.Add(db.CertificeringenInfras.Where(c => c.CertificeringenInfraId.ToString() == certId).FirstOrDefault());
                }
            }
            catch (NullReferenceException e)
            {

            }

            //Voorkennis
            this.Voorkennis.Clear();
            try
            {
                foreach (var voorkennisId in cuTopicModel.VoorkennisIds)
                {
                    this.Voorkennis.Add(db.Topics.Where(t => t.TopicId.ToString() == voorkennisId).FirstOrDefault());
                }
            }
            catch (NullReferenceException e)
            {

            }

            //Benodigdheden
            this.Benodigdheden.Clear();
            try
            {
                foreach (var benodigdheidsId in cuTopicModel.BenodigdhedenIds)
                {
                    this.Benodigdheden.Add(db.Benodigdheden.Where(b => b.BenodigdheidId.ToString() == benodigdheidsId).FirstOrDefault());
                }
            }
            catch (NullReferenceException e)
            {

            }

            //Percipiolinks
            this.PercipioLinks.Clear();
            try
            {
                foreach (var percipioId in cuTopicModel.PercipiolinkIds)
                {
                    this.PercipioLinks.Add(db.PercipioLinks.Where(p => p.PercipiolinkId.ToString() == percipioId).FirstOrDefault());
                }
            }
            catch (NullReferenceException e)
            {

            }

            //Tags
            this.Tags.Clear();
            try
            {
                foreach (var tagId in cuTopicModel.TagIds)
                {
                    this.Tags.Add(db.Tags.Where(t => t.TagId.ToString() == tagId).FirstOrDefault());
                }
            }
            catch (NullReferenceException e)
            {

            }
        }
    }

    /// <summary>
    /// Model for Create and update Topics
    /// </summary>
    [NotMapped]
    public class CUTopicModel : TopicModel
    {
        public CUTopicModel()
        {

        }
        public CUTopicModel(TopicModel topicModel)
        {
            using (MyDbContext db = new MyDbContext())
            {
                TopicId = topicModel.TopicId;
                Code = topicModel.Code;
                Name = topicModel.Name;
                Inhoud = topicModel.Inhoud;
                Leerdoel = topicModel.Leerdoel;

                Benodigdheden = topicModel.Benodigdheden;
                CUBenodigdheden = db.Benodigdheden.ToList();

                Certificeringen = topicModel.Certificeringen;
                CUCertificeringenInfras = db.CertificeringenInfras.ToList();

                Duur = topicModel.Duur;//
                CUTijdsDuren = db.TijdsDuren.ToList();

                Niveau = topicModel.Niveau;//
                CUNiveaus = db.Niveaus.ToList();

                PercipioLinks = topicModel.PercipioLinks;//
                CUPercipiolinks = db.PercipioLinks.ToList();

                Tags = topicModel.Tags;
                CUTags = db.Tags.ToList();

                Voorkennis = topicModel.Voorkennis;
                VoorkennisTopics = db.Topics.ToList();

                Werkvorm = topicModel.Werkvorm;//
                CUwerkvormen = db.Werkvormen.ToList();
            }
        }

        public IEnumerable<NiveauModel> CUNiveaus { get; set; }
        public string NiveauId { get; set; }
        public IEnumerable<TijdsDuurModel> CUTijdsDuren { get; set; }
        public string TijdsDuurId { get; set; }
        public IEnumerable<WerkvormModel> CUwerkvormen { get; set; }
        public string WerkvormId { get; set; }
        public IEnumerable<CertificeringenInfraModel> CUCertificeringenInfras { get; set; }
        public string[] CertificeringIds { get; set; }
        public IEnumerable<TagModel> CUTags { get; set; }
        public string[] TagIds { get; set; }
        public IEnumerable<TopicModel> VoorkennisTopics { get; set; }
        public string[] VoorkennisIds { get; set; }
        public IEnumerable<BenodigdheidModel> CUBenodigdheden { get; set; }
        public string[] BenodigdhedenIds { get; set; }
        public IEnumerable<PercipiolinkModel> CUPercipiolinks { get; set; }
        public string[] PercipiolinkIds { get; set; }

        public TopicModel GetAsTopicModel()
        {
            return this as TopicModel;
        }
    }
}