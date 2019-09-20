﻿using System;
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
        private List<TopicModel> allTopics;
        public List<TopicModel> AllTopics
        {
            get {
                return allTopics;
            }
            set {
                allTopics = value;
                allTopicIds = new string[allTopics.Count];
                for (int i = 0; i<allTopics.Count; i++)
                {
                    allTopicIds[i] = allTopics[i].TopicId.ToString();
                }
            }
        }
        public string[] allTopicIds { get; set; }
    }


}