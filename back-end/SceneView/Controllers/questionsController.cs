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
    public class questionsController : Controller
    {
        private Entities db = new Entities();

        // GET: questions
        public ActionResult Index()
        {
            var question = db.question.Include(q => q.scenicSpot);
            return View(question.ToList());
        }

        // GET: questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            question question = db.question.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: questions/Create
        public ActionResult Create()
        {
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName");
            return View();
        }

        // POST: questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "questionID,scenicID,questionContent")] question question)
        {
            if (ModelState.IsValid)
            {
                db.question.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", question.scenicID);
            return View(question);
        }

        // GET: questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            question question = db.question.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", question.scenicID);
            return View(question);
        }

        // POST: questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "questionID,scenicID,questionContent")] question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", question.scenicID);
            return View(question);
        }

        // GET: questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            question question = db.question.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            question question = db.question.Find(id);
            db.question.Remove(question);
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
