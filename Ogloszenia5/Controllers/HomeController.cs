using Ogloszenia5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ogloszenia5.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("elo1");
            ViewBag.Id = new SelectList(db.Category, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Index([Bind(Include = "Id")] Category category)
        {
            int id = (int)category.GetType().GetProperty("Id").GetValue(category);
            System.Diagnostics.Debug.WriteLine(id);
            return RedirectToAction("Details", "Categories", new { id });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}