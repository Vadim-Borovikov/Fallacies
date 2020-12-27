using System.Web.Mvc;

namespace Fallacies.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Git()
        {
            return RedirectToAction("Index", "GitFallacies");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}