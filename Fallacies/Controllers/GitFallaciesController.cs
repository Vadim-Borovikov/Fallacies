﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fallacies.Data;

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
            List<GitFallacy> fallacies = GitDataFetcher.FetchData();
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

        private void SyncDbWith(IEnumerable<GitFallacy> fallacies)
        {
            _context.GitFallacies.RemoveRange(_context.GitFallacies);
            _context.GitFallacies.AddRange(fallacies);
            _context.SaveChanges();
        }
    }
}
