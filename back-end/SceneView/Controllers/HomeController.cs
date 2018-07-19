using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SceneView.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Login");
            }
        }

        public ActionResult Cacel()
        {
            Session.Clear();
            return Redirect("/Login");
        }
    }
}