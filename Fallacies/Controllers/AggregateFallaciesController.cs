using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fallacies.Data;

namespace Fallacies.Controllers
{
    public class AggregateFallaciesController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(_context.Fallacies.OrderBy(f => f.GitId).ThenBy(f => f.Name).ToList());
        }

        public ActionResult Icons()
        {
            return View(_context.Fallacies.OrderBy(f => f.GitId).ToList());
        }

        public ActionResult Emoji()
        {
            return View(_context.Fallacies.ToList());
        }

        public ActionResult AggregateData()
        {
            List<Fallacy> fallacies =
                DataAggregator.Aggregate(_context.LocalFallacies, _context.SlackFallacies, _context.GitFallacies);
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
            _context.Fallacies.RemoveRange(_context.Fallacies);
            _context.Fallacies.AddRange(fallacies);
            _context.SaveChanges();
        }
    }
}
