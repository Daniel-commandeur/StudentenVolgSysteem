using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class TopicViewModels
    { 
        public Topic Topic { get; set; }
        public IEnumerable<Niveau> Niveaus { get; set; }
        public IEnumerable<Tijdsduur> Tijdsduren { get; set; }
        public IEnumerable<Werkvorm> Werkvormen { get; set; }
        public IEnumerable<Certificering> Certificeringen { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Topic> Voorkennis { get; set; }
        public IEnumerable<Benodigdheid> Benodigheden { get; set; }
        public IEnumerable<PercipioLink> PercipioLinks { get; set; }
    }
}