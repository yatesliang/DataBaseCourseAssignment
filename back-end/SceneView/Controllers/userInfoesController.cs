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
    public class userInfoesController : Controller
    {
        private Entities db = new Entities();

        // GET: userInfoes
        public ActionResult Index()
        {
            var userInfo = db.userInfo.Include(u => u.user);
            return View(userInfo.ToList());
        }

        // GET: userInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInfo userInfo = db.userInfo.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // GET: userInfoes/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.user, "userID", "password");
            return View();
        }

        // POST: userInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,nickname,gender,headPortrait,introduction,phoneNumber")] userInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                db.userInfo.Add(userInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userID = new SelectList(db.user, "userID", "password", userInfo.userID);
            return View(userInfo);
        }

        // GET: userInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInfo userInfo = db.userInfo.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.user, "userID", "password", userInfo.userID);
            return View(userInfo);
        }

        // POST: userInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,nickname,gender,headPortrait,introduction,phoneNumber")] userInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(db.user, "userID", "password", userInfo.userID);
            return View(userInfo);
        }

        // GET: userInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInfo userInfo = db.userInfo.Find(id);
            if (userInfo == null)
            {
                return HttpNotFound();
            }
            return View(userInfo);
        }

        // POST: userInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userInfo userInfo = db.userInfo.Find(id);
            db.userInfo.Remove(userInfo);
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
