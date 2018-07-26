using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SceneView.Models;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
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
                Spot scene = new Spot();
                scene.viewed = false;
                scene.want = false;
                var user = db.user.Where(u => u.userID == id).FirstOrDefault<user>();
                var spotname = Request.QueryString["sn"];
                spotname = spotname == "" || spotname == null ? "东方明珠广播电视塔" : spotname;
                var scenicSpot = db.scenicSpot.Where(u => u.scenicName == spotname).FirstOrDefault<scenicSpot>();
                scene.spot = scenicSpot;
                scene.usern = usern;
                scene.user = user;
                var viewedList = user.scenicSpot1.ToList();
                foreach (var v in viewedList)
                {
                    if (v.scenicID == scenicSpot.scenicID)
                    {
                        scene.viewed = true;
                        break;
                    }
                }
                var wantList = user.scenicSpot.ToList();
                foreach (var w in wantList)
                {
                    if (w.scenicID == scenicSpot.scenicID)
                    {
                        scene.want = true;
                        break;
                    }
                }

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
        [HttpPost]
        public ActionResult UpdateCommentStart(string spotname)
        {
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
                return PartialView(commentList);

            }
            return View(commentList);


        }
        [HttpGet]
        public ActionResult Blog()
        {
            var page = Request.QueryString["page"];
            page = page == "" || page == null ? "0" : page;
            var pageIndex = int.Parse(page);
            var noteList = db.note.ToList();
            var list = noteList.Skip(pageIndex * 6).Take(6).ToList();
            var p = new page();
            p.list = list;
            p.currentpage = pageIndex;
            p.pagetotal = noteList.Count() / 6 + 1;
            return View(p);
        }
        [HttpPost]
        public ActionResult UpdateComment(string content, string spotname, string mark)
        {
            var comment = new comment();
            comment.commentContent = content;
            var id = Session["user"].ToString();
            var user = db.user.Where(u => u.userID == id).FirstOrDefault<user>();
            comment.user = user;
            spotname = spotname == "" || spotname == null ? "上海杜莎夫人蜡像馆" : spotname;
            var scenicSpot = db.scenicSpot.Where(u => u.scenicName == spotname).FirstOrDefault<scenicSpot>();
            comment.scenicID = scenicSpot.scenicID;
            comment.commentLike = 0;
            comment.mark = int.Parse(mark);
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
                return PartialView(commentList);

            }
            return View(commentList);


        }
        public ActionResult SingleNote()
        {
            SceneView.Models.note tempnote = new SceneView.Models.note();
            var noteI = Request.QueryString["nn"];
            noteI = noteI == "" || noteI == null ? "1" : noteI;
            var noteID = int.Parse(noteI);

            var singlenote = db.note.Where(u => u.noteID == noteID).FirstOrDefault();
            var singl = db.note.Where(u => u.scenicSpot.scenicID == singlenote.scenicID).ToList();
            foreach (var s in singl)
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
        [HttpGet]
        public ActionResult UpdateVisitor()
        {
           
            var num = 5654;
            return View(num);
        }
        [HttpPost]
        public ActionResult UpdateVisitor(string Index,string sceneName)
        {
            //ScriptRuntime pyRumTime = Python.CreateRuntime();
            //dynamic obj = pyRumTime.UseFile("getNumOfSpot.py");
            //var num = obj.outputTheNumOf(sceneName);
            var num = 6666;
            if (Request.IsAjaxRequest())
            {
                return View(num);
            }
            return View(num);

        }
        public ActionResult Strategy()
        {
            var strategyI = Request.QueryString["si"];
            strategyI = strategyI == "" || strategyI == null ? "1" : strategyI;
            var strategyID = int.Parse(strategyI);
            var strategy = db.strategy.Where(s => s.strategyID == strategyID).FirstOrDefault<strategy>();
            return View(strategy);
        }

        [HttpPost]
        public void UpdateViewedList(string userid, short scenicid)
        {
            var scene = db.scenicSpot.Where(u => u.scenicID == scenicid).FirstOrDefault<scenicSpot>();
            var user = db.user.Where(u => u.userID == userid).FirstOrDefault<user>();
            var sceniclist = user.scenicSpot1.ToList();
            var flag = 0;
            SceneView.Models.scenicSpot temp = new SceneView.Models.scenicSpot();
            foreach (var s in sceniclist)
            {
                if (s.scenicID == scenicid)
                {
                    flag = 1;
                    temp = s;
                    break;
                }
            }
            if (flag == 0)
            {
                user.scenicSpot1.Add(scene);
            }
            else
            {
                user.scenicSpot1.Remove(temp);
            }
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
            var user1 = db.user.Where(u => u.userID == userid).FirstOrDefault<user>();
            var sceniclist1 = user.scenicSpot1.ToList();
        }
        [HttpPost]
        public void UpdateWantList(string userid, short scenicid)
        {
            var scene = db.scenicSpot.Where(u => u.scenicID == scenicid).FirstOrDefault<scenicSpot>();
            var user = db.user.Where(u => u.userID == userid).FirstOrDefault<user>();
            var sceniclist = user.scenicSpot.ToList();
            var flag = 0;
            SceneView.Models.scenicSpot temp = new SceneView.Models.scenicSpot();
            foreach (var s in sceniclist)
            {
                if (s.scenicID == scenicid)
                {
                    flag = 1;
                    temp = s;
                    break;
                }
            }
            if (flag == 0)
            {
                user.scenicSpot.Add(scene);
            }
            else
            {
                user.scenicSpot.Remove(temp);
            }
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
            var user1 = db.user.Where(u => u.userID == userid).FirstOrDefault<user>();
            var sceniclist1 = user.scenicSpot.ToList();
        }
        public class Spot
        {
            public string usern { get; set; }
            public SceneView.Models.user user { get; set; }
            public SceneView.Models.scenicSpot spot { get; set; }
            public string commentContent { get; set; }
            public bool viewed { get; set; }
            public bool want { get; set; }
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
        public class page
        {
            public List<SceneView.Models.note> list { get; set; }
            public int pagetotal { get; set; }
            public int currentpage { get; set; }
        }
        public class noteList
        {
            public SceneView.Models.note singlenote { get; set; }
            public List<SceneView.Models.note> noteL { get; set; }       
        }
        
    }
}