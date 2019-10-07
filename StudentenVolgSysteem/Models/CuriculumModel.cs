using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{

    //TODO: Notitievelden voor studenten en docenten bij het curriculum


    [Table(name: "Curiculums")]
    public class CuriculumModel
    {
        public CuriculumModel()
        {
            this.Topics = new HashSet<TopicModel>();
        }

        [Key]
        public int CuriculumId { get; set; }
        public StudentModel StudentId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TopicModel> Topics { get; set; }
    }

    [NotMapped]
    public class CUCuriculumModel : CuriculumModel
    {
        public CUCuriculumModel()
        {

        }

        public CUCuriculumModel(CuriculumModel cm)
        {
            this.CuriculumId = cm.CuriculumId;
            this.StudentId = cm.StudentId;
            this.Topics = cm.Topics;
            this.Name = cm.Name;
        }
        public List<TopicModel> AllTopics { get; set; }
        public string[] allTopicIds { get; set; }
        public int StudentIdInt { get; set; }
    }


}