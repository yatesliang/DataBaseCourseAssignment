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
        private List<scenicSpot> resultSpot;
        private List<scenicPos> resultPos;
        private static int commentCount;

        public HomeController()
        {
            commentCount = 0;
        }

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
                    this.resultPos = db.scenicPos.Where(u => u.city == city && u.district == district).ToList<scenicPos>();
                    this.resultSpot = new List<scenicSpot>();
                    ViewBag.pos = resultPos;

                    foreach(var pos in resultPos)
                    {
                        var spot = db.scenicSpot.Where(s => s.scenicID == pos.scenicID).FirstOrDefault<scenicSpot>();
                        resultSpot.Add(spot);
                    }
                    ViewBag.spot = resultSpot.ToArray();

                    // get recommended images
                    IList<string> imgSrcs = new List<string>();
                    foreach (var spot in resultSpot)
                    {
                        var images = spot.image.ToList();
                        foreach (var img in images)
                        {
                            string address = img.imageAddress;
                            imgSrcs.Add(address);
                            break;
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
                            noteView.city = this.city;
                            noteViews.Add(noteView);
                        }
                    }
                    ViewBag.notes = notes;
                    ViewBag.noteViews = noteViews;

                    return View(us);
                }

            }
        }
        [HttpPost]
        public ActionResult Search(user info)
        {
            var searchContent = info.userInfo.nickname;
            return Redirect("~/ScenicHome/Index?search=" + searchContent);
        }

        public ActionResult commentPart()
        {
            commentCount += 5;

            // get user
            var userID = Session["user"].ToString();
            user us = db.user.Where(cu => cu.userID == userID).FirstOrDefault<user>();
            if (us == null)
            {
                return Redirect("/Error");
            }
            // get province, city and district
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
            this.resultPos = db.scenicPos.Where(u => u.city == city && u.district == district).ToList<scenicPos>();
            this.resultSpot = new List<scenicSpot>();
            ViewBag.pos = resultPos;

            foreach (var pos in resultPos)
            {
                var spot = db.scenicSpot.Where(s => s.scenicID == pos.scenicID).FirstOrDefault<scenicSpot>();
                resultSpot.Add(spot);
            }
            ViewBag.spot = resultSpot.ToArray();


            // get comments
            IList<comment> comments = new List<comment>();
            IList<CommentView> commentViews = new List<CommentView>();
            foreach (var rspot in this.resultSpot)
            {
                var resultComments = db.comment.Where(c => c.scenicID == rspot.scenicID);
                comments = resultComments.ToList();
                foreach (var c in comments)
                {
                    var cUser = db.user.Where(u => u.userID == c.userID).FirstOrDefault<user>();
                    var cUserInfo = db.userInfo.Where(ui => ui.userID == cUser.userID).FirstOrDefault<userInfo>();
                    var commentView = new CommentView();
                    commentView.city = this.city;
                    commentView.userName = cUserInfo.nickname;
                    commentView.commentContent = c.commentContent;
                    commentViews.Add(commentView);
                }
            }
            ViewBag.comments = comments;
            ViewBag.commentViews = commentViews;
            ViewBag.commentCount = commentCount;

            return PartialView();
        }

    }
}

//       