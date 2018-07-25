﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SceneView.Models;

namespace SceneView.Controllers
{
    public class ProfileController : Controller
    {
        private Entities db = new Entities();
        // GET: Profile
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
                    ProfileData profileData = ProfileData.getProfileData(db, userSession);
                    if (Request.QueryString["note"] != null && Request.QueryString["note"] != "")
                    {
                        profileData.userData.note = profileData.userData.note.Where(n=>n.title.Contains(Request.QueryString["note"])).ToList();
                    }
                    return View(ProfileData.getProfileData(db, userSession));
                }
            }
        }
        [HttpPost]
        public ActionResult Index(ProfileData profileData)
        {
            var url = "/profile/index?note=" + profileData.note.title;
            return Redirect(url);
        }
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
                    ProfileData profileData = ProfileData.getProfileData(db, userSession);
                    return View(ProfileData.getProfileData(db, userSession));
                }
            }
        }
        [HttpPost]
        public ActionResult DeleteMessage(ProfileData profileData)
        {
            var type = profileData.message.type;
            var messageID = int.Parse(profileData.message.messageID);
            if (type == "1")
            {
                var result = db.commentReplyMes.Where(c => c.messageID == messageID).FirstOrDefault();
                if (result != null)
                {
                    db.commentReplyMes.Remove(result);
                }
            }
            else if(type == "2") {
                var result = db.commentLikeMes.Where(c => c.messageID == messageID).FirstOrDefault();
                if (result != null)
                {
                    db.commentLikeMes.Remove(result);
                }
            }
            else if (type == "3")
            {
                var result = db.noteLikeMes.Where(c => c.messageID == messageID).FirstOrDefault();
                if (result != null)
                {
                    db.noteLikeMes.Remove(result);
                }
            }
            // 循环检测db是否被占用，防止并发冲突
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
            return Redirect("/profile/message");
        }
        [HttpGet]
        public ActionResult TimeLine()
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
                    ProfileData profileData = ProfileData.getProfileData(db, userSession);
                    return View(ProfileData.getProfileData(db, userSession));
                }
            }
        }
        [HttpPost]
        public ActionResult Gone(ProfileData profileData)
        {
            var userID = profileData.viewed.userID;
            var scenicID = profileData.viewed.scenicID;
            var result = db.user.Where(u => u.userID == userID).FirstOrDefault();
            if (result != null)
            {
                var scene = result.scenicSpot.Where(s => s.scenicID == int.Parse(scenicID)).FirstOrDefault();
                if (scene != null)
                {
                    result.scenicSpot.Remove(scene);
                    if (!result.scenicSpot1.Contains(scene))
                    {
                        result.scenicSpot1.Add(scene);
                    }
                    // 循环检测db是否被占用，防止并发冲突
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
                }
            }
            return Redirect("/profile/timeline");
        }
        [HttpGet]
        public ActionResult Setting()
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
                    ProfileData profileData = ProfileData.getProfileData(db, userSession);
                    return View(ProfileData.getProfileData(db, userSession));
                }
            }
        }
        [HttpPost]
        public ActionResult Setting(ProfileData profileData)
        {
            var result = db.user.Where(u => u.userID == Session["user"].ToString()).FirstOrDefault();
            if (result != null)
            {
                result.userInfo.nickname = profileData.userData.userInfo.nickname;
                result.userInfo.gender = profileData.userData.userInfo.gender;
                result.userInfo.phoneNumber = profileData.userData.userInfo.phoneNumber;
                result.userInfo.introduction = profileData.userData.userInfo.introduction;
                // 循环检测db是否被占用，防止并发冲突
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
            }
            return Redirect("/Profile");
        }

        public class ProfileData
        {
            public user userData { get; set; }
            public List<commentLikeMes> commentLikeMesData { get; set; }
            public List<noteLikeMes> noteLikeMesData { get; set; }
            public List<commentReplyMes> commentReplyMesData { get; set; }
            public List<comment> commentsData { get; set; }
            public scenicSpot scenicSpot { get; set; }
            public note note { get; set; }
            public Message message { get; set; }
            public ViewedScene viewed { get; set; }
            public class Message
            {
                public string messageID { get; set; }
                public string type { get; set; }
            }
            public class ViewedScene
            {
                public string userID { get; set; }
                public string scenicID { get; set; }
            }
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
                profileData.commentLikeMesData = profileData.commentLikeMesData.OrderBy(n => n.time).ToList();
                profileData.commentReplyMesData = profileData.commentReplyMesData.OrderBy(n => n.commentReply.commentTime).ToList();
                profileData.noteLikeMesData = db.noteLikeMes.Where(n => n.note.userID == userSession.userID).OrderBy(n=>n.time).ToList();
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