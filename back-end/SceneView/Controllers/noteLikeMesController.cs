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
    public class noteLikeMesController : Controller
    {
        private Entities db = new Entities();

        // GET: noteLikeMes
        public ActionResult Index()
        {
            var noteLikeMes = db.noteLikeMes.Include(n => n.message);
            return View(noteLikeMes.ToList());
        }

        // GET: noteLikeMes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            noteLikeMes noteLikeMes = db.noteLikeMes.Find(id);
            if (noteLikeMes == null)
            {
                return HttpNotFound();
            }
            return View(noteLikeMes);
        }

        // GET: noteLikeMes/Create
        public ActionResult Create()
        {
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID");
            return View();
        }

        // POST: noteLikeMes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "noteID,messageID")] noteLikeMes noteLikeMes)
        {
            if (ModelState.IsValid)
            {
                db.noteLikeMes.Add(noteLikeMes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", noteLikeMes.messageID);
            return View(noteLikeMes);
        }

        // GET: noteLikeMes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            noteLikeMes noteLikeMes = db.noteLikeMes.Find(id);
            if (noteLikeMes == null)
            {
                return HttpNotFound();
            }
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", noteLikeMes.messageID);
            return View(noteLikeMes);
        }

        // POST: noteLikeMes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "noteID,messageID")] noteLikeMes noteLikeMes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(noteLikeMes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.messageID = new SelectList(db.message, "messageID", "messageID", noteLikeMes.messageID);
            return View(noteLikeMes);
        }

        // GET: noteLikeMes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            noteLikeMes noteLikeMes = db.noteLikeMes.Find(id);
            if (noteLikeMes == null)
            {
                return HttpNotFound();
            }
            return View(noteLikeMes);
        }

        // POST: noteLikeMes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            noteLikeMes noteLikeMes = db.noteLikeMes.Find(id);
            db.noteLikeMes.Remove(noteLikeMes);
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
