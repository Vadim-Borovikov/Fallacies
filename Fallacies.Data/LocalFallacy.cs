using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fallacies.Data
{
    public class LocalFallacy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }

        public string NameRu { get; set; }
        public string Synopsys { get; set; }
        public string Description { get; set; }
        public string Examples { get; set; }
    }
}