using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ogloszenia5.Models;

namespace Ogloszenia5.Controllers
{
    public class RatingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ratings
        public ActionResult Index()
        {
            var rating = db.Rating.Include(r => r.Ad);
            return View(rating.ToList());
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Rating.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Ratings/Create
        public ActionResult Create()
        {
            ViewBag.AdId = new SelectList(db.Ad, "Id", "Name");
            return View();
        }

        // POST: Ratings/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Mark,Comment,AdId")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Rating.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdId = new SelectList(db.Ad, "Id", "Name", rating.AdId);
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id,int? idA)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Rating.Find(id);
            ViewBag.userId=rating.ApplicationUserId;
            ViewBag.AdId = (int)idA;
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int idA,int id,int Mark, string Comment,string UserId)
        {
            if (ModelState.IsValid)
            {
                Rating rating = new Rating();
                rating.AdId = idA;
                rating.Comment = Comment;
                rating.Mark = Mark;
                rating.Id = id;
                rating.ApplicationUserId = UserId;
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Ads", new { id = idA });
            }
            return RedirectToAction("Details", "Ads", new { id = idA });
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id, int? idA)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Rating.Find(id);
            ViewBag.userId = rating.ApplicationUserId;
            ViewBag.AdId = (int)idA;
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,int idA)
        {
            Rating rating = db.Rating.Find(id);
            db.Rating.Remove(rating);
            db.SaveChanges();
            return RedirectToAction("Details","Ads",new { id = idA });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
