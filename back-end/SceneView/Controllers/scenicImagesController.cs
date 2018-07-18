using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SceneView.Models;

namespace SceneView.Controllers
{
    public class scenicImagesController : Controller
    {
        private Entities db = new Entities();

        // GET: scenicImages
        public ActionResult Index()
        {
            var scenicImage = db.scenicImage.Include(s => s.scenicSpot);
            return View(scenicImage.ToList());
        }

        // GET: scenicImages/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicImage scenicImage = db.scenicImage.Find(id);
            if (scenicImage == null)
            {
                return HttpNotFound();
            }
            return View(scenicImage);
        }

        // GET: scenicImages/Create
        public ActionResult Create()
        {
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName");
            return View();
        }

        // POST: scenicImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "scenicID,imageAddress")] scenicImage scenicImage)
        {
            if (ModelState.IsValid)
            {
                db.scenicImage.Add(scenicImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", scenicImage.scenicID);
            return View(scenicImage);
        }

        // GET: scenicImages/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicImage scenicImage = db.scenicImage.Find(id);
            if (scenicImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", scenicImage.scenicID);
            return View(scenicImage);
        }

        // POST: scenicImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "scenicID,imageAddress")] scenicImage scenicImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scenicImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", scenicImage.scenicID);
            return View(scenicImage);
        }

        // GET: scenicImages/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicImage scenicImage = db.scenicImage.Find(id);
            if (scenicImage == null)
            {
                return HttpNotFound();
            }
            return View(scenicImage);
        }

        // POST: scenicImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            scenicImage scenicImage = db.scenicImage.Find(id);
            db.scenicImage.Remove(scenicImage);
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
