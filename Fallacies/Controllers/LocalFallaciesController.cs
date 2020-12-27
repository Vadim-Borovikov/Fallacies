using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fallacies.Data;

namespace Fallacies.Controllers
{
    public class LocalFallaciesController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: GitFallacies
        public ActionResult Index()
        {
            return View(_context.LocalFallacies.OrderByDescending(f => f.NameRu).ThenBy(f => f.Name).ToList());
        }

        public ActionResult FetchLocalData()
        {
            List<LocalFallacy> fallacies = LocalDataFetcher.FetchData();
            SyncDbWith(fallacies);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SyncDbWith(IEnumerable<LocalFallacy> fallacies)
        {
            _context.LocalFallacies.RemoveRange(_context.LocalFallacies);
            _context.LocalFallacies.AddRange(fallacies);
            _context.SaveChanges();
        }
    }
}
