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
                ProfileData profileData = new ProfileData();
                profileData.userData = userSession;
                if (userSession == null)
                {
                    return Redirect("~/Error");
                }
                else
                {
                    // 获取该用户相关消息
                    profileData.commentLikeMesData = db.commentLikeMes.Where(m => m.receiverID == userSession.userID).ToList();
                    if (profileData.commentLikeMesData == null)
                    {
                        profileData.commentLikeMesData = new List<commentLikeMes>();
                    }
                    profileData.commentReplyMesData = db.commentReplyMes.Where(m => m.receiverID == userSession.userID).ToList();
                    if (profileData.commentReplyMesData == null)
                    {
                        profileData.commentReplyMesData = new List<commentReplyMes>();
                    }
                    profileData.noteLikeMesData = db.noteLikeMes.Where(m => m.receiverID == userSession.userID).ToList();
                    if (profileData.noteLikeMesData == null)
                    {
                        profileData.noteLikeMesData = new List<noteLikeMes>();
                    }
                    profileData.commentsData = db.comment.Where(c => c.userID == userSession.userID).ToList();
                    if (profileData.commentsData == null)
                    {
                        profileData.commentsData = new List<comment>();
                    }
                    return View(profileData);
                }
            }
        }

        public class ProfileData
        {
            public user userData { get; set; }
            public List<commentLikeMes> commentLikeMesData { get; set; }
            public List<commentReplyMes> commentReplyMesData { get; set; }
            public List<noteLikeMes> noteLikeMesData { get; set; }
            public List<comment> commentsData { get; set; }
           
        }
    }
}