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
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Category.ToList().Where(d=>d.Id!=1));
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Pname = null;
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Category category = db.Category.Find(id);
            //if (category == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(category);
            int cid;
            if (id != null)
            {
                cid = (int)id;
            } else
            {
                cid = 1;
            }
            
            if (cid == 1)
            {
                ViewBag.CategoryName = "All ads";
                var query =
                                    from ad in db.Ad
                                    join photo in db.Photo on ad.Id equals photo.AdId
                                    where photo.position == 1
                                    orderby ad.Name ascending
                                    select new AdExtends
                                    {
                                        Id = ad.Id,
                                        Name = ad.Name,
                                        Description = ad.Description,
                                        Price = ad.Price,
                                        Url = photo.Url
                                    };
                return View(query);
            }
            else
            {
                var Cname = db.Category.FirstOrDefault(p => p.Id == cid);
                ViewBag.CategoryName = Cname.Name;
                int idC = (int)id;
                var query =
                        from ad in db.Ad
                        join photo in db.Photo on ad.Id equals photo.AdId
                        join category in db.Category on ad.CategoryId equals category.Id
                        where photo.position == 1 && category.Id==idC
                        orderby ad.Name ascending
                        select new AdExtends
                        {
                            Id = ad.Id,
                            Name = ad.Name,
                            Description = ad.Description,
                            Price = ad.Price,
                            Url = photo.Url,
                            CName = category.Name,
                        };
                return View(query);
            } 
        }

        [HttpPost]
        public ActionResult Details(int CategoryId, string pname,string sortid)
        {
            ViewBag.Pname = pname;
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name");
            if (CategoryId == 1)
            {
                ViewBag.CategoryName = "All ads";
                if (sortid == "1") {
                    ViewBag.sortid = "1";
                    var query =
                                    from ad in db.Ad
                                    join photo in db.Photo on ad.Id equals photo.AdId
                                    where photo.position == 1
                                    orderby ad.Name ascending
                                    select new AdExtends
                                    {
                                        Id = ad.Id,
                                        Name = ad.Name,
                                        Description = ad.Description,
                                        Price = ad.Price,
                                        Url = photo.Url
                                    };
                return View(query);
                } else if(sortid == "2")
                {
                    ViewBag.sortid = "2";
                    var query =
                                        from ad in db.Ad
                                        join photo in db.Photo on ad.Id equals photo.AdId
                                        where photo.position == 1 
                                        orderby ad.Name descending
                                        select new AdExtends
                                        {
                                            Id = ad.Id,
                                            Name = ad.Name,
                                            Description = ad.Description,
                                            Price = ad.Price,
                                            Url = photo.Url
                                        };
                    return View(query);
                } else if (sortid == "3")
                {
                    ViewBag.sortid = "3";
                    var query =
                                        from ad in db.Ad
                                        join photo in db.Photo on ad.Id equals photo.AdId
                                        where photo.position == 1
                                        orderby ad.Price ascending
                                        select new AdExtends
                                        {
                                            Id = ad.Id,
                                            Name = ad.Name,
                                            Description = ad.Description,
                                            Price = ad.Price,
                                            Url = photo.Url
                                        };
                    return View(query);
                } else
                {
                    ViewBag.sortid = "4";
                    var query =
                                        from ad in db.Ad
                                        join photo in db.Photo on ad.Id equals photo.AdId
                                        where photo.position == 1
                                        orderby ad.Price descending
                                        select new AdExtends
                                        {
                                            Id = ad.Id,
                                            Name = ad.Name,
                                            Description = ad.Description,
                                            Price = ad.Price,
                                            Url = photo.Url
                                        };
                    return View(query);
                }
            }
            else
            {
                var Cname = db.Category.FirstOrDefault(p => p.Id == CategoryId);
                ViewBag.CategoryName = Cname.Name;
                if (sortid == "1")
                {
                    ViewBag.sortid = "1";
                    var query =
                            from ad in db.Ad
                            join photo in db.Photo on ad.Id equals photo.AdId
                            join category in db.Category on ad.CategoryId equals category.Id
                            where photo.position == 1 && category.Id == CategoryId
                            orderby ad.Name ascending
                            select new AdExtends
                            {
                                Id = ad.Id,
                                Name = ad.Name,
                                Description = ad.Description,
                                Price = ad.Price,
                                Url = photo.Url,
                                CName = category.Name,
                            };
                    return View(query);
                } else if (sortid == "2")
                {
                    ViewBag.sortid = "2";
                    var query =
                            from ad in db.Ad
                            join photo in db.Photo on ad.Id equals photo.AdId
                            join category in db.Category on ad.CategoryId equals category.Id
                            where photo.position == 1 && category.Id == CategoryId
                            orderby ad.Name descending
                            select new AdExtends
                            {
                                Id = ad.Id,
                                Name = ad.Name,
                                Description = ad.Description,
                                Price = ad.Price,
                                Url = photo.Url,
                                CName = category.Name,
                            };
                    return View(query);
                } else if (sortid == "3")
                {
                    ViewBag.sortid = "3";
                    var query =
                            from ad in db.Ad
                            join photo in db.Photo on ad.Id equals photo.AdId
                            join category in db.Category on ad.CategoryId equals category.Id
                            where photo.position == 1 && category.Id == CategoryId
                            orderby ad.Price ascending
                            select new AdExtends
                            {
                                Id = ad.Id,
                                Name = ad.Name,
                                Description = ad.Description,
                                Price = ad.Price,
                                Url = photo.Url,
                                CName = category.Name,
                            };
                    return View(query);
                } else
                {
                    ViewBag.sortid = "4";
                    var query =
                            from ad in db.Ad
                            join photo in db.Photo on ad.Id equals photo.AdId
                            join category in db.Category on ad.CategoryId equals category.Id
                            where photo.position == 1 && category.Id == CategoryId
                            orderby ad.Price descending
                            select new AdExtends
                            {
                                Id = ad.Id,
                                Name = ad.Name,
                                Description = ad.Description,
                                Price = ad.Price,
                                Url = photo.Url,
                                CName = category.Name,
                            };
                    return View(query);
                }
            }
        }


            // GET: Categories/Create
            public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Category.Find(id);
            db.Category.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
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
