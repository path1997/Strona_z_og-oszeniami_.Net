using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Ogloszenia5.Models;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;

namespace Ogloszenia5.Controllers
{
    public class AdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Ads
        public ActionResult Index()
        {
            var ad = db.Ad.Include(a => a.Category);
            return View(ad.ToList());
        }
        public ActionResult MyAds()
        {
            string usid = User.Identity.GetUserId();
            var querya =
                    from ad in db.Ad
                    join user in db.Users on ad.ApplicationUserId equals user.Id
                    join photo in db.Photo on ad.Id equals photo.AdId
                    where  usid== ad.ApplicationUserId && photo.position == 1
                    select new AdExtends
                    {
                        Id = ad.Id,
                        Name = ad.Name,
                        Description = ad.Description,
                        Price = ad.Price,
                        Url = photo.Url
                    };
            return View(querya);
        }
        public ActionResult AllAds()
        {
            string usid = User.Identity.GetUserId();
            var querya =
                    from ad in db.Ad
                    join photo in db.Photo on ad.Id equals photo.AdId
                    where photo.position == 1
                    select new AdExtends
                    {
                        Id = ad.Id,
                        Name = ad.Name,
                        Description = ad.Description,
                        Price = ad.Price,
                        Url = photo.Url
                    };
            return View(querya);
        }

        // GET: Ads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new Ad_VIew();
            
            var querya =
                    from ad in db.Ad
                    join user in db.Users on ad.ApplicationUserId equals user.Id
                    where ad.Id == id
                    select new AdExtends
                    {
                        Id = ad.Id,
                        Name = ad.Name,
                        Description = ad.Description,
                        Price = ad.Price,
                        Email=user.Email,
                        PhoneNumber=user.PhoneNumber,
                        UserName=user.UserName
                    };
            var queryp =
                    from photo in db.Photo
                    where photo.AdId == id
                    select new AdExtends
                    {
                        Url = photo.Url,
                        Position = photo.position,
                    };
            model.AdExtends = querya;
            ViewBag.id = id;
            var queryr =
                   from rating in db.Rating
                   join user in db.Users on rating.ApplicationUserId equals user.Id
                   where rating.AdId == id
                   select new AdExtends
                   {
                       Mark = rating.Mark,
                       Comment = rating.Comment,
                       UserName = user.UserName,
                       IdR=rating.Id
                 };
            model.Rating = queryr;
            model.Photo = queryp;
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Details(int? id,string comment,int mark)
        {
            Rating r = new Rating();
            r.AdId = (int)id;
            r.ApplicationUserId = User.Identity.GetUserId();
            r.Mark = mark;
            r.Comment = comment;
            db.Rating.Add(r);
            db.SaveChanges();
            return RedirectToAction("Details", new { id =id });
        }
        // GET: Ads/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category.Where(d=>d.Id!=1), "Id", "Name");
            return View();
        }

        // POST: Ads/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3,[Bind(Include = "Id,Name,Description,Price,CategoryId,IdentityUser_Id")] Ad ad)
        {
            int cid=ad.CategoryId;
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("siema");
                System.Diagnostics.Debug.WriteLine(User.Identity.GetUserName());
                System.Diagnostics.Debug.WriteLine(User.Identity.GetUserId());
                //System.Diagnostics.Debug.WriteLine(ad.);
                // ad
                //ad.IdentityUser = User.Identity.GetUserName();
                //IdentityUser xyz = new IdentityUser();
                ad.ApplicationUserId = User.Identity.GetUserId();
                //var currentUser = UserManager.FindByName(User.Identity.GetUserName());
                //String UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                //ad.IdentityUser = User.Identity.GetUserId();
                db.Ad.Add(ad);
                if (file1 != null)
                {
                    string pic = System.IO.Path.GetFileName(file1.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/images"), pic);
                    // file is uploaded
                    file1.SaveAs(path);

                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file1.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    Photo p = new Photo();
                    p.AdId = ad.Id;
                    p.position = 1;
                    p.Url = pic;
                    db.Photo.Add(p);

                }
                if (file2 != null)
                {
                    string pic = System.IO.Path.GetFileName(file2.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/images"), pic);
                    // file is uploaded
                    file2.SaveAs(path);

                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file2.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    Photo p = new Photo();
                    p.AdId = ad.Id;
                    p.position = 2;
                    p.Url = pic;
                    db.Photo.Add(p);

                }
                if (file3 != null)
                {
                    string pic = System.IO.Path.GetFileName(file3.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/images"), pic);
                    // file is uploaded
                    file3.SaveAs(path);

                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file3.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    Photo p = new Photo();
                    p.AdId = ad.Id;
                    p.position = 3;
                    p.Url = pic;
                    db.Photo.Add(p);

                }
                db.SaveChanges();
                return RedirectToAction("Details","Categories",new { id = cid });
            }

            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", ad.CategoryId);
            return View(ad);
        }

        // GET: Ads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ads = db.Ad.Find(id);
            if (ads == null)
             {
                return HttpNotFound();
            }
            ViewBag.userId = User.Identity.GetUserId();
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", ads.CategoryId);
            var query =
                   from ad in db.Ad
                   join photo in db.Photo on ad.Id equals photo.AdId
                   join user in db.Users on ad.ApplicationUserId equals user.Id
                   where ad.Id == id
                   select new AdExtends
                   {
                       Id = ad.Id,
                       Name = ad.Name,
                       Description = ad.Description,
                       Price = ad.Price,
                       Url = photo.Url,
                       Position = photo.position,
                       Email = user.Email,
                       PhoneNumber = user.PhoneNumber,
                       UserName = user.UserName,
                       ApplicationUserId=user.Id
                   };
            return View(query);
        }

        // POST: Ads/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, bool? foto1c, bool? foto2c, bool? foto3c, [Bind(Include = "Id,Name,Description,Price,CategoryId,ApplicationUserId")] Ad ad)
        {
            int cid = ad.CategoryId;
            if (foto1c == true)
            {
                var photoa = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position==1);
                string fullPath = Request.MapPath("~/images/" + photoa.Url);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                db.Photo.Remove(photoa);

            } else
            {
                var photoa = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position == 1);
                if (file1 != null)
                {
                    string pic = System.IO.Path.GetFileName(file1.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/images"), pic);
                    // file is uploaded
                    file1.SaveAs(path);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file1.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    if (photoa == null)
                    {
                        Photo x = new Photo();
                        x.AdId = ad.Id;
                        x.position = 1;
                        x.Url = pic;
                        db.Photo.Add(x);
                    }
                    else
                    {
                        photoa.Url = pic;
                        db.Entry(photoa).State = EntityState.Modified;
                    }

                }
            }
            if (foto2c == true)
            {
                var photoa = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position == 2);
                string fullPath = Request.MapPath("~/images/" + photoa.Url);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                db.Photo.Remove(photoa);

            } else
            {
                var photoa = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position == 2);
                if (file2 != null)
                {
                    string pic = System.IO.Path.GetFileName(file2.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/images"), pic);
                    // file is uploaded
                    file2.SaveAs(path);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file2.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    if (photoa == null)
                    {
                        Photo x = new Photo();
                        x.AdId = ad.Id;
                        x.position = 2;
                        x.Url = pic;
                        db.Photo.Add(x);
                    }
                    else
                    {
                        photoa.Url = pic;
                        db.Entry(photoa).State = EntityState.Modified;
                    }
                }
            }
            if (foto3c == true)
            {
                var photoa = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position == 3);
                string fullPath = Request.MapPath("~/images/" + photoa.Url);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                db.Photo.Remove(photoa);

            } else
            {
                var photoa = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position == 3);
                if (file3 != null)
                {
                    string pic = System.IO.Path.GetFileName(file3.FileName);
                    
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/images"), pic);
                    // file is uploaded
                    file3.SaveAs(path);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file3.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    if (photoa == null)
                    {
                        Photo x = new Photo();
                        x.AdId = ad.Id;
                        x.position = 3;
                        x.Url = pic;
                        db.Photo.Add(x);
                    }
                    else
                    {
                        photoa.Url = pic;
                        db.Entry(photoa).State = EntityState.Modified;
                    }
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Categories", new { id = cid });
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Name", ad.CategoryId);
            return View(ad);
        }

        // GET: Ads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ad.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // POST: Ads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ad ad = db.Ad.Find(id);
            int cid=ad.CategoryId;
            var photo1 = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position == 1);
            var photo2 = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position == 2);
            var photo3 = db.Photo.FirstOrDefault(p => p.AdId == ad.Id && p.position == 3);
            if (photo1 != null)
            {
                string fullPath = Request.MapPath("~/images/" + photo1.Url);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                db.Photo.Remove(photo1);
            }
            if (photo2 != null)
            {
                string fullPath = Request.MapPath("~/images/" + photo2.Url);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                db.Photo.Remove(photo2);
            }
            if (photo3 != null)
            {
                string fullPath = Request.MapPath("~/images/" + photo3.Url);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                db.Photo.Remove(photo3);
            }
         
            db.Ad.Remove(ad);
            db.SaveChanges();
            return RedirectToAction("Details","Categories",new { id = cid });
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
