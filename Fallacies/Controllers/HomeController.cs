using System.Web.Mvc;

namespace Fallacies.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Local();
        }

        public ActionResult Local()
        {
            return RedirectToAction("Index", "LocalFallacies");
        }

        public ActionResult Slack()
        {
            return RedirectToAction("Index", "SlackFallacies");
        }

        public ActionResult Git()
        {
            return RedirectToAction("Index", "GitFallacies");
        }

        public ActionResult Aggregate()
        {
            return RedirectToAction("Index", "AggregateFallacies");
        }

        public ActionResult Icons()
        {
            return RedirectToAction("Icons", "AggregateFallacies");
        }

        public ActionResult Emoji()
        {
            return RedirectToAction("Emoji", "AggregateFallacies");
        }
    }
}