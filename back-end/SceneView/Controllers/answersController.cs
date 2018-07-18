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
    public class answersController : Controller
    {
        private Entities db = new Entities();

        // GET: answers
        public ActionResult Index()
        {
            var answer = db.answer.Include(a => a.question);
            return View(answer.ToList());
        }

        // GET: answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            answer answer = db.answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: answers/Create
        public ActionResult Create()
        {
            ViewBag.questionID = new SelectList(db.question, "questionID", "questionContent");
            return View();
        }

        // POST: answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "answerID,questionID,answerContent")] answer answer)
        {
            if (ModelState.IsValid)
            {
                db.answer.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.questionID = new SelectList(db.question, "questionID", "questionContent", answer.questionID);
            return View(answer);
        }

        // GET: answers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            answer answer = db.answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.questionID = new SelectList(db.question, "questionID", "questionContent", answer.questionID);
            return View(answer);
        }

        // POST: answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "answerID,questionID,answerContent")] answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.questionID = new SelectList(db.question, "questionID", "questionContent", answer.questionID);
            return View(answer);
        }

        // GET: answers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            answer answer = db.answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            answer answer = db.answer.Find(id);
            db.answer.Remove(answer);
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
