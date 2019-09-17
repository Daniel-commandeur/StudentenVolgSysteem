using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentenVolgSysteem.Models
{
    [Table(name: "Curiculums")]
    public class CuriculumModel
    {
        public CuriculumModel()
        {
            this.Topics = new HashSet<TopicModel>();
        }

        public CuriculumModel(CUCuriculumModel cuc)
        {
            this.CuriculumId = cuc.CuriculumId;
            this.StudentId = cuc.StudentId;
            this.Topics = cuc.Topics;
        }

        [Key]
        public int CuriculumId { get; set; }
        public StudentModel StudentId { get; set; }

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
        }
        public List<TopicModel> allTopics { get; set; }
    }


}