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
    public class commentsController : Controller
    {
        private Entities db = new Entities();

        // GET: comments
        public ActionResult Index()
        {
            var comment = db.comment.Include(c => c.scenicSpot).Include(c => c.user);
            return View(comment.ToList());
        }

        // GET: comments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: comments/Create
        public ActionResult Create()
        {
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName");
            ViewBag.userID = new SelectList(db.user, "userID", "password");
            return View();
        }

        // POST: comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "commentID,userID,scenicID,mark,commentContent,commentLike,commentTime")] comment comment)
        {
            if (ModelState.IsValid)
            {
                db.comment.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", comment.scenicID);
            ViewBag.userID = new SelectList(db.user, "userID", "password", comment.userID);
            return View(comment);
        }

        // GET: comments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", comment.scenicID);
            ViewBag.userID = new SelectList(db.user, "userID", "password", comment.userID);
            return View(comment);
        }

        // POST: comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentID,userID,scenicID,mark,commentContent,commentLike,commentTime")] comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.scenicID = new SelectList(db.scenicSpot, "scenicID", "scenicName", comment.scenicID);
            ViewBag.userID = new SelectList(db.user, "userID", "password", comment.userID);
            return View(comment);
        }

        // GET: comments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            comment comment = db.comment.Find(id);
            db.comment.Remove(comment);
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
