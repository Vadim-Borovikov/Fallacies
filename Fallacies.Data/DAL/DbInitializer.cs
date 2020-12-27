using System.Data.Entity;

namespace Fallacies.Data.DAL
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
    }
}