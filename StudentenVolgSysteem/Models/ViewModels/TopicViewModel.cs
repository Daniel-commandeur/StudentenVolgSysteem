using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models.ViewModels
{
    [NotMapped]
    public class TopicViewModel : Topic
    { 
        public IEnumerable<Niveau> AlleNiveaus { get; set; }
        public IEnumerable<Tijdsduur> AlleTijdsduren { get; set; }
        public IEnumerable<Werkvorm> AlleWerkvormen { get; set; }
        public IEnumerable<Certificering> AlleCertificeringen { get; set; }
        public IEnumerable<Tag> AlleTags { get; set; }
        public IEnumerable<Topic> AlleVoorkennis { get; set; }
        public IEnumerable<Benodigdheid> AlleBenodigdheden { get; set; }
        public IEnumerable<PercipioLink> AllePercipioLinks { get; set; }
    }
}