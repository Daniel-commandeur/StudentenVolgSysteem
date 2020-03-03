using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class TopicViewModel
    {
        public Topic Topic { get; set; }


        public int[] BenodigdheidIds { get; set; }
        public int[] CertificeringenIds { get; set; }
        public int[] PercipioLinkIds { get; set; }
        public int[] VoorkennisIds { get; set; }
        // Lists to populate Create and Edit View dropdowns.
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


// PREVIOUSLY OUTCOMMENTED CODE

//public string Code { get; set; }
//public string Naam { get; set; }
//public string Leerdoel { get; set; }
//public string Inhoud { get; set; }

//public int TopicId { get; set; }
//public int Niveau { get; set; }
//public int Duur { get; set; }
//public int Werkvorm { get; set; }

//public List<int> Certificeringen { get; set; }
//public List<int> Voorkennis { get; set; }
//public List<int> Benodigdheden { get; set; }
//public List<int> PercipioLinks { get; set; }
//public List<int> Tags { get; set; }
////public List<int> Curricula { get; set; }

//[NotMapped]
//public string NaamCode
//{
//    get
//    {
//        string returnString;
//        if (string.IsNullOrEmpty(Naam))
//        {
//            returnString = Code;
//        }
//        else
//        {
//            returnString = Naam;
//        }
//        return returnString;
//    }
//}