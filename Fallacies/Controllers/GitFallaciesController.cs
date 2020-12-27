using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Model;
using Model.Git;

namespace Fallacies.Controllers
{
    public class GitFallaciesController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: GitFallacies
        public ActionResult Index()
        {
            return View(_context.GitFallacies.ToList());
        }

        public ActionResult FetchDataFromGit()
        {
            List<Fallacy> fallacies = GitDataFetcher.FetchData();
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

        private void SyncDbWith(IEnumerable<Fallacy> fallacies)
        {
            _context.GitFallacies.RemoveRange(_context.GitFallacies);
            _context.GitFallacies.AddRange(fallacies);
            _context.SaveChanges();
        }
    }
}
