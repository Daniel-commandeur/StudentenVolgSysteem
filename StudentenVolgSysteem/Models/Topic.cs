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

        public int[] CertificeringenIds { get; set; }
        //Certificering	
        public virtual ICollection<Certificering> Certificeringen { get; set; }
        
        public int[] VoorkennisIds { get; set; }
        //Benodigde voorkennis	      
        public virtual ICollection<Topic> Voorkennis { get; set; }
        //Inhoud	
        public string Inhoud { get; set; }

        public int[] BenodigdheidIds { get; set; }
        //Benodigdheden	
        public virtual ICollection<Benodigdheid> Benodigdheden { get; set; }
        public int[] PercipioLinkIds { get; set; }
        //Percipio 
        public virtual ICollection<PercipioLink> PercipioLinks { get; set; }
        //Tags
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