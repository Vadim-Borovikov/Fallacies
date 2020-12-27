using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fallacies.Data
{
    public class Fallacy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }

        public int GitId { get; set; }
        public string NameRu { get; set; }
        public string GitImagePathIcon { get; set; }
        public string GitImagePathBackground { get; set; }

        public string Description { get; set; }
        public string Examples { get; set; }

        public string Code { get; set; }
        public string SlackImage { get; set; }
    }
}