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
            position.province = province;
            position.city = city;
            position.district = district;
            Index(position);
        }

        //public ActionResult Index(Position position)
        //{
        //    string city = "上海市";
        //    string district = "嘉定区";
        //    ViewBag.locate = 1;

        //    // get spot and position
        //    var resultPos = db.scenicPos.Where(u => u.city == city && u.district == district).FirstOrDefault<scenicPos>();
        //    var resultSpot = db.scenicSpot.Where(u => u.scenicID.Equals(resultPos.scenicID)).ToList<scenicSpot>();
        //    ViewBag.pos = resultPos;
        //    ViewBag.spot = resultSpot;

        //    // get recommended images
        //    IList<string> imgSrcs = new List<string>();
        //    foreach(var spot in resultSpot)
        //    {
        //        var images = spot.image.ToList();
        //        foreach(var img in images)
        //        {
        //            string address = img.imageAddress;
        //            imgSrcs.Add(address);
        //        }
        //    }
        //    ViewBag.recommendedSrc = imgSrcs;

        //    // get notes
        //    IList<note> notes = new List<note>();
        //    IList<NoteView> noteViews = new List<NoteView>();
        //    foreach(var spot in resultSpot)
        //    {
        //        notes = (from n in db.note where n.scenicID == spot.scenicID select n).ToList();
        //        foreach (var note in notes)
        //        {
        //            var noteView = new NoteView();
        //            noteView.noteTitle = note.title;
        //            noteView.city = resultPos.city;
        //            noteViews.Add(noteView);
        //        }
        //    }
        //    ViewBag.notes = notes;
        //    ViewBag.noteViews = noteViews;

        //    // get comments
        //    IList<comment> comments = new List<comment>();
        //    IList<CommentView> commentViews = new List<CommentView>();
        //    foreach(var rspot in resultSpot)
        //    {
        //        var resultComments = db.comment.Where(c => c.scenicID == rspot.scenicID);
        //        comments = resultComments.ToList();
        //        foreach (var c in comments)
        //        {
        //            var cUser = db.user.Where(u => u.userID == c.userID).FirstOrDefault<user>();
        //            var cUserInfo = db.userInfo.Where(ui => ui.userID == cUser.userID).FirstOrDefault<userInfo>();
        //            var spot = db.scenicSpot.Where(s => s.scenicID == c.scenicID).FirstOrDefault<scenicSpot>();
        //            var commentView = new CommentView();
        //            commentView.city = resultPos.city;
        //            commentView.userName = cUserInfo.nickname;
        //            commentView.commentContent = c.commentContent;
        //            commentViews.Add(commentView);
        //        }
        //    }
        //    ViewBag.comments = comments;
        //    ViewBag.commentViews = commentViews;

        //    return View();
        //}

        //[HttpGet]
        public ActionResult Index(Position position)
        {
            string city = position.city;
            string district = position.district;
            if(city == null || district == null)
            {
                city = "上海市";
                district = "嘉定区";
                ViewBag.locate = 1;
            }
            else
            {
                ViewBag.locate = 0;
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

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}