using System.Data.Entity;

namespace Model.DAL
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
    }
}