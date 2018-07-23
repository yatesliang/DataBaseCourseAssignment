using SceneView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SceneView.Controllers
{
    public class ProfileController : Controller
    {
        private Entities db = new Entities();
        // GET: Profile
        // 个人主页
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return Redirect("~/login");
            }
            else
            {
                // 获取用户信息
                var userID = Session["user"].ToString();
                user userSession = db.user.Where(u => u.userID == userID).FirstOrDefault<user>();
                if (userSession == null)
                {
                    return Redirect("~/Error");
                }
                else
                {
                    return View(ProfileData.getProfileData(db, userSession));
                }
            }
        }
        // 所有游记
        [HttpGet]
        public ActionResult Note()
        {
            if (Session["user"] == null)
            {
                return Redirect("~/login");
            }
            else
            {
                // 获取用户信息
                var userID = Session["user"].ToString();
                user userSession = db.user.Where(u => u.userID == userID).FirstOrDefault<user>();
                if (userSession == null)
                {
                    return Redirect("~/Error");
                }
                else
                {
                    return View(ProfileData.getProfileData(db, userSession));
                }
            }
        }
        // 单个游记页面
        [HttpGet]
        public ActionResult Detail()
        {
            if (Session["user"] == null)
            {
                return Redirect("~/login");
            }
            else
            {
                // 获取用户信息
                var userID = Session["user"].ToString();
                user userSession = db.user.Where(u => u.userID == userID).FirstOrDefault<user>();
                if (userSession == null)
                {
                    return Redirect("~/Error");
                }
                else
                {
                    if (Request.QueryString["ni"] == null)
                    {
                        return Redirect("/Profile/note");
                    }
                    else
                    {
                        int noteID = int.Parse(Request.QueryString["ni"]);
                        ProfileData profileData = ProfileData.getProfileData(db, userSession);
                        note note = db.note.Where(n => n.noteID == noteID).FirstOrDefault<note>();
                        if (note == null)
                        {
                            return Redirect("/Profile/note");
                        }
                        else
                        {
                            ViewBag.note = note;
                            return View(profileData);
                        }
                    }
                }
            }
        }
        // 编辑游记
        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["user"] == null)
            {
                return Redirect("~/login");
            }
            else
            {
                // 获取用户信息
                var userID = Session["user"].ToString();
                user userSession = db.user.Where(u => u.userID == userID).FirstOrDefault<user>();
                if (userSession == null)
                {
                    return Redirect("~/Error");
                }
                else
                {
                    if (Request.QueryString["ni"] == null)
                    {
                        return Redirect("/Profile/note");
                    }
                    else
                    {
                        int noteID = int.Parse(Request.QueryString["ni"]);
                        ProfileData profileData = ProfileData.getProfileData(db, userSession);
                        note note = db.note.Where(n => n.noteID == noteID).FirstOrDefault<note>();
                        if (note == null)
                        {
                            return Redirect("/Profile/note");
                        }
                        else
                        {
                            ViewBag.note = note;
                            return View(profileData);
                        }
                    }
                }
            }
        }
        // 查看消息
        [HttpGet]
        public ActionResult Message()
        {
            if (Session["user"] == null)
            {
                return Redirect("~/login");
            }
            else
            {
                // 获取用户信息
                var userID = Session["user"].ToString();
                user userSession = db.user.Where(u => u.userID == userID).FirstOrDefault<user>();
                if (userSession == null)
                {
                    return Redirect("~/Error");
                }
                else
                {
                    if (Request.QueryString["m"] == null)
                    {
                        return Redirect("/Profile/message?m=1");
                    }
                    else
                    {
                        int messageID = int.Parse(Request.QueryString["m"]);
                        ProfileData profileData = ProfileData.getProfileData(db, userSession);
                        if (messageID == 1)
                        {
                            ViewBag.message = 1;
                        }
                        else
                        {
                            ViewBag.message = 2;
                        }
                        return View(profileData);
                    }
                }
            }
        }
        public class ProfileData
        {
            public user userData { get; set; }
            public List<commentLikeMes> commentLikeMesData { get; set; }
            public List<noteLikeMes> noteLikeMesData { get; set; }
            public List<commentReplyMes> commentReplyMesData { get; set; }
            public List<comment> commentsData { get; set; }
            public static ProfileData getProfileData(Entities db, user userSession)
            {

                ProfileData profileData = new ProfileData();
                profileData.commentLikeMesData = new List<commentLikeMes>();
                profileData.commentReplyMesData = new List<commentReplyMes>();
                profileData.userData = userSession;
                // 获取该用户相关消息
                var comment = userSession.comment;
                foreach (var c in comment)
                {
                    foreach (var cr in c.commentLikeMes)
                    {
                        profileData.commentLikeMesData.Add(cr);
                    }
                    foreach (var cr in c.commentReplyMes)
                    {
                        profileData.commentReplyMesData.Add(cr);
                    }
                }
                profileData.noteLikeMesData = userSession.noteLikeMes.ToList();
                profileData.commentsData = db.comment.Where(c => c.userID == userSession.userID).ToList();
                if (profileData.commentsData == null)
                {
                    profileData.commentsData = new List<comment>();
                }
                
                return profileData;
            }
        }

    }
}