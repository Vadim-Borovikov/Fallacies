﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Slack
{
    public class Fallacy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string ImagePathIcon { get; set; }
        public string ImagePathBackground { get; set; }
        public string Description { get; set; }
        public string Examples { get; set; }
    }
}