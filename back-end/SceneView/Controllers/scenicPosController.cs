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
    public class scenicPosController : Controller
    {
        private Entities db = new Entities();

        // GET: scenicPos
        public ActionResult Index()
        {
            var scenicPos = db.scenicPos.Include(s => s.scenicSpot);
            return View(scenicPos.ToList());
        }

        // GET: scenicPos/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicPos scenicPos = db.scenicPos.Find(id);
            if (scenicPos == null)
            {
                return HttpNotFound();
            }
            return View(scenicPos);
        }

        // GET: scenicPos/Create
        public ActionResult Create()
        {
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName");
            return View();
        }

        // POST: scenicPos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "scenicID,longitue,latitude,address")] scenicPos scenicPos)
        {
            if (ModelState.IsValid)
            {
                db.scenicPos.Add(scenicPos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", scenicPos.scenicID);
            return View(scenicPos);
        }

        // GET: scenicPos/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicPos scenicPos = db.scenicPos.Find(id);
            if (scenicPos == null)
            {
                return HttpNotFound();
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", scenicPos.scenicID);
            return View(scenicPos);
        }

        // POST: scenicPos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "scenicID,longitue,latitude,address")] scenicPos scenicPos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scenicPos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", scenicPos.scenicID);
            return View(scenicPos);
        }

        // GET: scenicPos/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scenicPos scenicPos = db.scenicPos.Find(id);
            if (scenicPos == null)
            {
                return HttpNotFound();
            }
            return View(scenicPos);
        }

        // POST: scenicPos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            scenicPos scenicPos = db.scenicPos.Find(id);
            db.scenicPos.Remove(scenicPos);
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
