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
    public class commentReplyMesController : Controller
    {
        private Entities db = new Entities();

        // GET: commentReplyMes
        public ActionResult Index()
        {
            var commentReplyMes = db.commentReplyMes.Include(c => c.message);
            return View(commentReplyMes.ToList());
        }

        // GET: commentReplyMes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentReplyMes commentReplyMes = db.commentReplyMes.Find(id);
            if (commentReplyMes == null)
            {
                return HttpNotFound();
            }
            return View(commentReplyMes);
        }

        // GET: commentReplyMes/Create
        public ActionResult Create()
        {
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID");
            return View();
        }

        // POST: commentReplyMes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "commentID,messageID")] commentReplyMes commentReplyMes)
        {
            if (ModelState.IsValid)
            {
                db.commentReplyMes.Add(commentReplyMes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", commentReplyMes.messageID);
            return View(commentReplyMes);
        }

        // GET: commentReplyMes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentReplyMes commentReplyMes = db.commentReplyMes.Find(id);
            if (commentReplyMes == null)
            {
                return HttpNotFound();
            }
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", commentReplyMes.messageID);
            return View(commentReplyMes);
        }

        // POST: commentReplyMes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentID,messageID")] commentReplyMes commentReplyMes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentReplyMes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", commentReplyMes.messageID);
            return View(commentReplyMes);
        }

        // GET: commentReplyMes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            commentReplyMes commentReplyMes = db.commentReplyMes.Find(id);
            if (commentReplyMes == null)
            {
                return HttpNotFound();
            }
            return View(commentReplyMes);
        }

        // POST: commentReplyMes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            commentReplyMes commentReplyMes = db.commentReplyMes.Find(id);
            db.commentReplyMes.Remove(commentReplyMes);
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
