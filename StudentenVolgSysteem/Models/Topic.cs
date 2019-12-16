using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using StudentenVolgSysteem.DAL;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Topics")]
    public class Topic : IDeletable
    {
        public Topic()
        {
            this.Tags = new HashSet<Tag>();
            this.Voorkennis = new HashSet<Topic>();
            this.Certificeringen = new HashSet<Certificering>();
            this.Curricula = new HashSet<Curriculum>();
            this.Benodigdheden = new HashSet<Benodigdheid>();
            this.PercipioLinks = new HashSet<PercipioLink>();
        }

        [Key]
        public int TopicId { get; set; }
        //Code
        public string Code { get; set; }
        //Niveau 	
        public Niveau Niveau { get; set; }
        //Topic	
        public string Naam { get; set; }
        //Duur	
        public Tijdsduur Duur { get; set; }
        //Werkvorm(en)	
        public Werkvorm Werkvorm { get; set; }
        //Leerdoel(en)	
        public string Leerdoel { get; set; }
        public bool IsDeleted { get; set; }
        //Certificering	
        public virtual ICollection<Certificering> Certificeringen { get; set; }
        //Benodigde voorkennis	
        public virtual ICollection<Topic> Voorkennis { get; set; }
        //Inhoud	
        public string Inhoud { get; set; }
        //Benodigdheden	
        public virtual ICollection<Benodigdheid> Benodigdheden { get; set; }
        //Percipio 
        public virtual ICollection<PercipioLink> PercipioLinks { get; set; }
        //Tags
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Curriculum> Curricula { get; set; }

        [NotMapped]
        public string NaamCode
        {
            get
            {
                string returnString;
                if (string.IsNullOrEmpty(Naam))
                {
                    returnString = Code;
                }
                else
                {
                    returnString = Naam;
                }
                return returnString;
            }
        }

        public void CopyCUTopicToTopic(CUTopic cuTopic, SVSContext db)
        {
            //Make changes to the topicModel here, we have to get all the models from side-tables from the dbcontext by reference.
            //Otherwise EF will think these are new objects
            this.Code = cuTopic.Code;
            this.Niveau = db.Niveaus.FirstOrDefault(n => n.NiveauId.ToString() == cuTopic.NiveauId);
            this.Naam = cuTopic.Naam;
            this.Duur = db.Tijdsduren.FirstOrDefault(d => d.TijdsduurId.ToString() == cuTopic.TijdsduurId);
            this.Werkvorm = db.Werkvormen.FirstOrDefault(w => w.WerkvormId.ToString() == cuTopic.WerkvormId);
            this.Leerdoel = cuTopic.Leerdoel;
            this.Inhoud = cuTopic.Inhoud;

            //Certs
            this.Certificeringen.Clear();
            try
            {
                foreach (var certId in cuTopic.CertificeringIds)
                {
                    this.Certificeringen.Add(db.Certificeringen.Where(c => c.CertificeringId.ToString() == certId).FirstOrDefault());
                }
            }
            catch (NullReferenceException e)
            {

            }

            //Voorkennis
            this.Voorkennis.Clear();
            try
            {
                foreach (var voorkennisId in cuTopic.VoorkennisIds)
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
                foreach (var benodigdheidsId in cuTopic.BenodigdhedenIds)
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
                foreach (var percipioId in cuTopic.PercipioLinkIds)
                {
                    this.PercipioLinks.Add(db.PercipioLinks.Where(p => p.PercipioLinkId.ToString() == percipioId).FirstOrDefault());
                }
            }
            catch (NullReferenceException e)
            {

            }

            //Tags
            this.Tags.Clear();
            try
            {
                foreach (var tagId in cuTopic.TagIds)
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
    /// Model for creating and updating Topics
    /// </summary>
    [NotMapped]
    public class CUTopic : Topic
    {
        public CUTopic()
        {

        }
        public CUTopic(Topic topic)
        {
            using (SVSContext db = new SVSContext())
            {
                TopicId = topic.TopicId;
                Code = topic.Code;
                Naam = topic.Naam;
                Inhoud = topic.Inhoud;
                Leerdoel = topic.Leerdoel;

                Benodigdheden = topic.Benodigdheden;
                CUBenodigdheden = db.Benodigdheden.ToList();

                Certificeringen = topic.Certificeringen;
                CUCertificeringen = db.Certificeringen.ToList();

                Duur = topic.Duur;
                CUTijdsduren = db.Tijdsduren.ToList();

                Niveau = topic.Niveau;
                CUNiveaus = db.Niveaus.ToList();

                PercipioLinks = topic.PercipioLinks;
                CUPercipioLinks = db.PercipioLinks.ToList();

                Tags = topic.Tags;
                CUTags = db.Tags.ToList();

                Voorkennis = topic.Voorkennis;
                CUVoorkennis = db.Topics.ToList();

                Werkvorm = topic.Werkvorm;
                CUWerkvormen = db.Werkvormen.ToList();
            }
        }

        public IEnumerable<Niveau> CUNiveaus { get; set; }
        public string NiveauId { get; set; }
        public IEnumerable<Tijdsduur> CUTijdsduren { get; set; }
        public string TijdsduurId { get; set; }
        public IEnumerable<Werkvorm> CUWerkvormen { get; set; }
        public string WerkvormId { get; set; }
        public IEnumerable<Certificering> CUCertificeringen { get; set; }
        public string[] CertificeringIds { get; set; }
        public IEnumerable<Tag> CUTags { get; set; }
        public string[] TagIds { get; set; }
        public IEnumerable<Topic> CUVoorkennis { get; set; }
        public string[] VoorkennisIds { get; set; }
        public IEnumerable<Benodigdheid> CUBenodigdheden { get; set; }
        public string[] BenodigdhedenIds { get; set; }
        public IEnumerable<PercipioLink> CUPercipioLinks { get; set; }
        public string[] PercipioLinkIds { get; set; }

        public Topic GetAsTopic()
        {
            return this as Topic;
        }
    }
}