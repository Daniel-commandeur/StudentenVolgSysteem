using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "PercipioLinks")]
    public class PercipioLink
    {
        public PercipioLink()
        {
            this.Topics = new HashSet<Topic>();
        }

        [Key]
        public int PercipioLinkId { get; set; }
        public string Link { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}