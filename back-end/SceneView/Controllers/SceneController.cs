using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SceneView.Models;
namespace SceneView.Controllers
{
    public class SceneController : Controller
    {
        private Entities db = new Entities();
        // GET: Scene
        // public ActionResult Index()
        // {

        //     return View();
        // }
        //[HttpPost]
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return View("~/Views/Login/Index.cshtml");
            }
            else
            {
                var id = Session["user"].ToString();
                var usern = db.userInfo.Where(u => u.userID == id).FirstOrDefault<userInfo>().nickname;
                var user = db.user.Where(u => u.userID == id).FirstOrDefault<user>();
                var spotname = Request.QueryString["sn"];
                spotname = spotname == "" || spotname == null ? "上海迪士尼度假区" : spotname;
                Spot scene = new Spot();
                scene.usern = usern;
                scene.user = user;
                var scenicSpot = db.scenicSpot.Where(u => u.scenicName == spotname).FirstOrDefault<scenicSpot>();
                scene.spot = scenicSpot;
                if (scenicSpot == null)
                {

                    ViewBag.flag = -1;
                    return View();
                }
                else
                {
                    return View(scene);
                }
                
               
            }
        }
        [HttpGet]
        public ActionResult UpdateComment()
        {
            var spotname = Request.QueryString["sn"];
            spotname = spotname == "" || spotname == null ? "东方明珠广播电视塔" : spotname;
            var scenicSpot = db.scenicSpot.Where(u => u.scenicName == spotname).FirstOrDefault<scenicSpot>();
            var c = new Comment();
            List<SceneView.Models.comment> commentList = db.comment.Where(u => u.scenicID == scenicSpot.scenicID).OrderByDescending(u => u.commentTime).ToList();
            
            List<Comment> comlist = new List<Comment>();
            foreach (var cn in commentList)
            {
                Comment comm = new Comment();
                comm.userID = cn.userID;
                comm.commentID = cn.commentID;
                comm.username = db.userInfo.Where(u => u.userID == cn.userID).FirstOrDefault().nickname;
                comm.commentContent = cn.commentContent;
                comm.commentLike = cn.commentLike;
                comm.commentTime = cn.commentTime;
                comlist.Add(comm);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(comlist);

            }
            return View(comlist);

        }
        [HttpGet]
        public ActionResult Blog()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult  UpdateComment(Spot r)
       {   
                var comment = new comment();
                comment.commentContent = r.commentContent;
                var id = Session["user"].ToString();
                comment.userID = id;
                var spotname = Request.QueryString["sn"];
                spotname = spotname == "" || spotname == null ? "东方明珠广播电视塔" : spotname;
                var scenicSpot = db.scenicSpot.Where(u => u.scenicName == spotname).FirstOrDefault<scenicSpot>();
                comment.scenicID = scenicSpot.scenicID;
                comment.commentLike = 0;
                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;
                comment.commentTime = currentTime;
                var com = db.comment;
                comment.commentID = com.Count() == 0 ? 1 : (com.Select(u => u.commentID).Max() + 1);
                db.comment.Add(comment);
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        ex.Entries.Single().Reload();
                    }
                } while (saveFailed);
            var c = new Comment();
            
            List<SceneView.Models.comment> commentList = db.comment.Where(u=>u.scenicID==scenicSpot.scenicID).OrderByDescending(u => u.commentTime).ToList();
            
            List<Comment> comlist=new List<Comment>();  
            foreach (var cn in commentList)
            {
                Comment comm = new Comment();
                comm.userID = cn.userID;
                comm.commentID = cn.commentID;
                comm.username = db.userInfo.Where(u => u.userID == cn.userID).FirstOrDefault().nickname;
                comm.commentContent = cn.commentContent;
                comm.commentLike = cn.commentLike;
                comm.commentTime = cn.commentTime;
                comlist.Add(comm);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(comlist);

            }
            return PartialView(comlist);


        }
        public ActionResult SingleNote()
        {
            SceneView.Models.note tempnote = new SceneView.Models.note();
            var noteI= Request.QueryString["nn"];
            noteI = noteI == ""||noteI==null ? "1" : noteI;
            var noteID = int.Parse(noteI);
            
            var singlenote = db.note.Where(u => u.noteID == noteID).FirstOrDefault();
            var singl = db.note.Where(u => u.scenicSpot.scenicID == singlenote.scenicID).ToList();
            foreach(var s in singl)
            {
                if (s.noteID == noteID)
                {
                    tempnote = s;
                    break;
                }
            }
            singl.Remove(tempnote);
            noteList n = new noteList();
            n.singlenote = singlenote;
            n.noteL = singl;
            return View(n);
        }
        [HttpPost]
        public void UpdateLike(string ln,string ni)
        {
            decimal likenum = int.Parse(ln);
            int noteID = int.Parse(ni);
            var note = db.note.Where(u => u.noteID == noteID).FirstOrDefault();
            note.noteLike = likenum;
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.Single().Reload();
                }
            }while (saveFailed) ;
        }

        public ActionResult UpdateBlog()
        {

            var noteList = db.note.ToList();
            var list = noteList.Skip(0).Take(6).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult UpdateBlog(int pageIndex)
        {
            var noteList = db.note.ToList();
            var list = noteList.Skip(pageIndex * 6).Take(6).ToList();
            return PartialView(list);
        }
        public ActionResult Strategy()
        {
            var strategyI = Request.QueryString["si"];
            strategyI = strategyI == "" || strategyI == null ? "1" : strategyI;
            var strategyID = int.Parse(strategyI);
            var strategy = db.strategy.Where(s => s.strategyID == strategyID).FirstOrDefault<strategy>();
            return View(strategy);
        }

        [HttpGet]
        public ActionResult UpdateVisitor()
        {
            var num = 5654;
            return View(num);
        }
        [HttpPost]
        public ActionResult UpdateVisitor(int index)
        {
            var num = 6666;
            if (Request.IsAjaxRequest())
            {
                return View(num);

            }
            return View(num);
        }

        public class Spot
        {
            public string usern { get; set; }
            public SceneView.Models.user user { get; set; }
            public SceneView.Models.scenicSpot spot { get; set; }
            public string commentContent { get; set; }
        }
        public class Comment
        {
            public string userID { get; set; }
            public string username { get; set; }
            public long commentID { get; set; }
            public string commentContent { get; set;}
            public decimal commentLike { get; set; }
            public System.DateTime commentTime { get; set; }
            
        }
        public class noteList
        {
            public SceneView.Models.note singlenote { get; set; }
            public List<SceneView.Models.note> noteL { get; set; }       
        }
        
    }
}