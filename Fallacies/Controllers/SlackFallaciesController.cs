using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fallacies.Data;

namespace Fallacies.Controllers
{
    public class SlackFallaciesController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: GitFallacies
        public ActionResult Index()
        {
            return View(_context.SlackFallacies.ToList());
        }

        public ActionResult FetchDataFromSlack()
        {
            List<SlackFallacy> fallacies = SlackDataFetcher.FetchData();
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

        private void SyncDbWith(IEnumerable<SlackFallacy> fallacies)
        {
            _context.SlackFallacies.RemoveRange(_context.SlackFallacies);
            _context.SlackFallacies.AddRange(fallacies);
            _context.SaveChanges();
        }
    }
}
