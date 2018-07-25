using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using SceneView.Models;

namespace SceneView.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        private string province;
        private string city;
        private string district;


        public class Position
        {
            public string province { get; set; }
            public string city { get; set; }
            public string district { get; set; }
        }

        public class CommentView
        {
            public string imageSrc { get; set; }
            public string userName { get; set; }
            public string commentContent { get; set; }
            public string city { get; set; }

        }

        public class NoteView
        {
            public string noteTitle { get; set; }
            public string imageSrc { get; set; }
            public string city { get; set; }
        }
        public void getPosition(string province, string city, string district)
        {
            var position = new Position();
            ViewBag.locate = 0;
            Session["province"] = province;
            Session["city"] = city;
            Session["district"] = district;
            Redirect("~/Home");
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return Redirect("/Login");
            }
            else
            {
                var userID = Session["user"].ToString();
                user us = db.user.Where(cu => cu.userID == userID).FirstOrDefault<user>();
                if (us == null)
                {
                    return Redirect("/Error");
                }
                else
                {
                    if (Session["province"] == null || Session["province"].ToString() == ""
               || Session["city"] == null || Session["city"].ToString() == ""
               || Session["district"] == null || Session["district"].ToString() == "")
                    {
                        Session["needLocation"] = 1;
                        this.province = "上海市";
                        this.city = "上海市";
                        this.district = "浦东新区";
                    }
                    else
                    {
                        Session["needLocation"] = 0;
                        this.province = Session["province"].ToString();
                        this.city = Session["city"].ToString();
                        this.district = Session["district"].ToString();
                    }

                    // get spot and position
                    var resultPos = db.scenicPos.Where(u => u.city == city && u.district == district).FirstOrDefault<scenicPos>();
                    var resultSpot = db.scenicSpot.Where(u => u.scenicID.Equals(resultPos.scenicID)).ToList<scenicSpot>();
                    ViewBag.pos = resultPos;
                    ViewBag.spot = resultSpot;

                    // get recommended images
                    IList<string> imgSrcs = new List<string>();
                    foreach (var spot in resultSpot)
                    {
                        var images = spot.image.ToList();
                        foreach (var img in images)
                        {
                            string address = img.imageAddress;
                            imgSrcs.Add(address);
                        }
                    }
                    ViewBag.recommendedSrc = imgSrcs;

                    // get notes
                    IList<note> notes = new List<note>();
                    IList<NoteView> noteViews = new List<NoteView>();
                    foreach (var spot in resultSpot)
                    {
                        notes = (from n in db.note where n.scenicID == spot.scenicID select n).ToList();
                        foreach (var note in notes)
                        {
                            var noteView = new NoteView();
                            noteView.noteTitle = note.title;
                            noteView.city = resultPos.city;
                            noteViews.Add(noteView);
                        }
                    }
                    ViewBag.notes = notes;
                    ViewBag.noteViews = noteViews;

                    // get comments
                    IList<comment> comments = new List<comment>();
                    IList<CommentView> commentViews = new List<CommentView>();
                    foreach (var rspot in resultSpot)
                    {
                        var resultComments = db.comment.Where(c => c.scenicID == rspot.scenicID);
                        comments = resultComments.ToList();
                        foreach (var c in comments)
                        {
                            var cUser = db.user.Where(u => u.userID == c.userID).FirstOrDefault<user>();
                            var cUserInfo = db.userInfo.Where(ui => ui.userID == cUser.userID).FirstOrDefault<userInfo>();
                            var spot = db.scenicSpot.Where(s => s.scenicID == c.scenicID).FirstOrDefault<scenicSpot>();
                            var commentView = new CommentView();
                            commentView.city = resultPos.city;
                            commentView.userName = cUserInfo.nickname;
                            commentView.commentContent = c.commentContent;
                            commentViews.Add(commentView);
                        }
                    }
                    ViewBag.comments = comments;
                    ViewBag.commentViews = commentViews;

                    return View(us);
                }

            }
        }
        [HttpPost]
        public ActionResult Search(user info)
        {
            var searchContent = info.comment;
            return Redirect("~/ScenicHome/Index?search=" + searchContent);
        }






    }
}

//       