using StudentenVolgSysteem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    public class CurriculumTopic
    {
        [Key, Column(Order= 1)]
        public int CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }

        [Key, Column(Order = 2)]
        public int TopicId { get; set; }
        public Topic Topic { get; set; }

        public Voortgang TopicVoortgang { get; set; }

        public DateTime? StartDatum { get; set; }
        public DateTime? EindDatum { get; set; }
        public bool Akkoord { get; set; }
    }

    public enum Voortgang
    {
        Nog_niet_begonnen,
        Gestart,
        Klaar
    }
}