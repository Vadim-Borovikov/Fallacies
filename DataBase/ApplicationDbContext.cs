using System.Data.Entity;
using Model.Git;

namespace Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Fallacy> GitFallacies { get; set; }

        public ApplicationDbContext() : base("DefaultConnection") { }
    }
}