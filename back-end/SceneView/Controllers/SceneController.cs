using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            // Session 验证
            if (Session["user"] == null)
            {
                return Redirect("~/Login");
            }
            // 获取URL参数
            var scenicID = Request.QueryString["si"];
            int ID = scenicID == null || scenicID == "" ? 1 : int.Parse(scenicID);
            scenicSpot scenicSpot = db.scenicSpot.Where(s => s.scenicID.Equals(scenicID.ToString())).FirstOrDefault<scenicSpot>();
            if (scenicSpot == null)
            {
                scenicSpot = db.scenicSpot.FirstOrDefault<scenicSpot>();
            }
            return View(scenicSpot);
        }
    }
}