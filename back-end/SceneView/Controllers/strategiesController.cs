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
    public class strategiesController : Controller
    {
        private Entities db = new Entities();

        // GET: strategies
        public ActionResult Index()
        {
            var strategy = db.strategy.Include(s => s.scenicSpot);
            return View(strategy.ToList());
        }

        // GET: strategies/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            strategy strategy = db.strategy.Find(id);
            if (strategy == null)
            {
                return HttpNotFound();
            }
            return View(strategy);
        }

        // GET: strategies/Create
        public ActionResult Create()
        {
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName");
            return View();
        }

        // POST: strategies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "strategyID,scenicID,title,content")] strategy strategy)
        {
            if (ModelState.IsValid)
            {
                db.strategy.Add(strategy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", strategy.scenicID);
            return View(strategy);
        }

        // GET: strategies/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            strategy strategy = db.strategy.Find(id);
            if (strategy == null)
            {
                return HttpNotFound();
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", strategy.scenicID);
            return View(strategy);
        }

        // POST: strategies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "strategyID,scenicID,title,content")] strategy strategy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(strategy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", strategy.scenicID);
            return View(strategy);
        }

        // GET: strategies/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            strategy strategy = db.strategy.Find(id);
            if (strategy == null)
            {
                return HttpNotFound();
            }
            return View(strategy);
        }

        // POST: strategies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            strategy strategy = db.strategy.Find(id);
            db.strategy.Remove(strategy);
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
