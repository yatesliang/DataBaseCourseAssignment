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
    public class scenicSpotsController : Controller
    {
        private Entities db = new Entities();

        // GET: scenicSpots
        public ActionResult Index()
        {
            var scenicSpot = db.scenicSpot.Include(s => s.scenicImage).Include(s => s.scenicPos);
            return View(scenicSpot.ToList());
        }

        // GET: scenicSpots/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicSpot scenicSpot = db.scenicSpot.Find(id);
            if (scenicSpot == null)
            {
                return HttpNotFound();
            }
            return View(scenicSpot);
        }

        // GET: scenicSpots/Create
        public ActionResult Create()
        {
            ViewBag.scenicID = new SelectList(db.scenicImage, "scenicID", "imageAddress");
            ViewBag.scenicID = new SelectList(db.scenicPos, "scenicID", "address");
            return View();
        }

        // POST: scenicSpots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "scenicID,scenicName,scenicIntroduction")] scenicSpot scenicSpot)
        {
            if (ModelState.IsValid)
            {
                db.scenicSpot.Add(scenicSpot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.scenicID = new SelectList(db.scenicImage, "scenicID", "imageAddress", scenicSpot.scenicID);
            ViewBag.scenicID = new SelectList(db.scenicPos, "scenicID", "address", scenicSpot.scenicID);
            return View(scenicSpot);
        }

        // GET: scenicSpots/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicSpot scenicSpot = db.scenicSpot.Find(id);
            if (scenicSpot == null)
            {
                return HttpNotFound();
            }
            ViewBag.scenicID = new SelectList(db.scenicImage, "scenicID", "imageAddress", scenicSpot.scenicID);
            ViewBag.scenicID = new SelectList(db.scenicPos, "scenicID", "address", scenicSpot.scenicID);
            return View(scenicSpot);
        }

        // POST: scenicSpots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "scenicID,scenicName,scenicIntroduction")] scenicSpot scenicSpot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scenicSpot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.scenicID = new SelectList(db.scenicImage, "scenicID", "imageAddress", scenicSpot.scenicID);
            ViewBag.scenicID = new SelectList(db.scenicPos, "scenicID", "address", scenicSpot.scenicID);
            return View(scenicSpot);
        }

        // GET: scenicSpots/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicSpot scenicSpot = db.scenicSpot.Find(id);
            if (scenicSpot == null)
            {
                return HttpNotFound();
            }
            return View(scenicSpot);
        }

        // POST: scenicSpots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            scenicSpot scenicSpot = db.scenicSpot.Find(id);
            db.scenicSpot.Remove(scenicSpot);
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
