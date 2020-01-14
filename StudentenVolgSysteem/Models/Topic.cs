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
            this.Curricula = new HashSet<CurriculumTopic>();
            this.Benodigdheden = new HashSet<Benodigdheid>();
            this.PercipioLinks = new HashSet<PercipioLink>();
        }

        // COMMENT: 
        // Required Status cannot be set yet, because TopicDataInfra.csv contains empty fields.
        // This leads to a problem with loading the csv file.
        [Key]
        public int TopicId { get; set; }
        //[Required]
        public string Code { get; set; }
        //[Required]
        public Niveau Niveau { get; set; }
        //[Required]
        public string Naam { get; set; }
        //[Required]
        public Tijdsduur Duur { get; set; }
        //[Required]
        public Werkvorm Werkvorm { get; set; }
        //[Required]
        public string Leerdoel { get; set; }

        public bool IsDeleted { get; set; }

        public int[] CertificeringenIds { get; set; }
        public virtual ICollection<Certificering> Certificeringen { get; set; }
        
        public int[] VoorkennisIds { get; set; }  
        public virtual ICollection<Topic> Voorkennis { get; set; }
        
        public string Inhoud { get; set; }

        public int[] BenodigdheidIds { get; set; }
        public virtual ICollection<Benodigdheid> Benodigdheden { get; set; }
    
        public int[] PercipioLinkIds { get; set; }
        public virtual ICollection<PercipioLink> PercipioLinks { get; set; }
        

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<CurriculumTopic> Curricula { get; set; }

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
    } 
}