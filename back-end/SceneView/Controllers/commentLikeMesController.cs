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
    public class commentLikeMesController : Controller
    {
        private Entities db = new Entities();

        // GET: commentLikeMes
        public ActionResult Index()
        {
            var commentLikeMes = db.commentLikeMes.Include(c => c.message);
            return View(commentLikeMes.ToList());
        }

        // GET: commentLikeMes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentLikeMes commentLikeMes = db.commentLikeMes.Find(id);
            if (commentLikeMes == null)
            {
                return HttpNotFound();
            }
            return View(commentLikeMes);
        }

        // GET: commentLikeMes/Create
        public ActionResult Create()
        {
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID");
            return View();
        }

        // POST: commentLikeMes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "commentID,messageID")] commentLikeMes commentLikeMes)
        {
            if (ModelState.IsValid)
            {
                db.commentLikeMes.Add(commentLikeMes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", commentLikeMes.messageID);
            return View(commentLikeMes);
        }

        // GET: commentLikeMes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentLikeMes commentLikeMes = db.commentLikeMes.Find(id);
            if (commentLikeMes == null)
            {
                return HttpNotFound();
            }
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", commentLikeMes.messageID);
            return View(commentLikeMes);
        }

        // POST: commentLikeMes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentID,messageID")] commentLikeMes commentLikeMes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentLikeMes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", commentLikeMes.messageID);
            return View(commentLikeMes);
        }

        // GET: commentLikeMes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentLikeMes commentLikeMes = db.commentLikeMes.Find(id);
            if (commentLikeMes == null)
            {
                return HttpNotFound();
            }
            return View(commentLikeMes);
        }

        // POST: commentLikeMes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            commentLikeMes commentLikeMes = db.commentLikeMes.Find(id);
            db.commentLikeMes.Remove(commentLikeMes);
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
