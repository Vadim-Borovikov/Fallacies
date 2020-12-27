using System.Data.Entity;

namespace Fallacies.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LocalFallacy> LocalFallacies { get; set; }
        public DbSet<SlackFallacy> SlackFallacies { get; set; }
        public DbSet<GitFallacy> GitFallacies { get; set; }
        public DbSet<Fallacy> Fallacies { get; set; }

        public ApplicationDbContext() : base("DefaultConnection") { }
    }
}