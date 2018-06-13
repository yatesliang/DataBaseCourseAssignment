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
    public class strategyImagesController : Controller
    {
        private Entities db = new Entities();

        // GET: strategyImages
        public ActionResult Index()
        {
            var strategyImage = db.strategyImage.Include(s => s.strategy);
            return View(strategyImage.ToList());
        }

        // GET: strategyImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            strategyImage strategyImage = db.strategyImage.Find(id);
            if (strategyImage == null)
            {
                return HttpNotFound();
            }
            return View(strategyImage);
        }

        // GET: strategyImages/Create
        public ActionResult Create()
        {
            ViewBag.strategyID = new SelectList(db.strategy, "strategyID", "title");
            return View();
        }

        // POST: strategyImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "imageID,strategyID,imageAddress")] strategyImage strategyImage)
        {
            if (ModelState.IsValid)
            {
                db.strategyImage.Add(strategyImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.strategyID = new SelectList(db.strategy, "strategyID", "title", strategyImage.strategyID);
            return View(strategyImage);
        }

        // GET: strategyImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            strategyImage strategyImage = db.strategyImage.Find(id);
            if (strategyImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.strategyID = new SelectList(db.strategy, "strategyID", "title", strategyImage.strategyID);
            return View(strategyImage);
        }

        // POST: strategyImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "imageID,strategyID,imageAddress")] strategyImage strategyImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(strategyImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.strategyID = new SelectList(db.strategy, "strategyID", "title", strategyImage.strategyID);
            return View(strategyImage);
        }

        // GET: strategyImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            strategyImage strategyImage = db.strategyImage.Find(id);
            if (strategyImage == null)
            {
                return HttpNotFound();
            }
            return View(strategyImage);
        }

        // POST: strategyImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            strategyImage strategyImage = db.strategyImage.Find(id);
            db.strategyImage.Remove(strategyImage);
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
