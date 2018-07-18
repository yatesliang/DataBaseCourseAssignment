using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SceneView.Models;

namespace SceneView.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            using (var context = new Entities())
            {
                var Query = from a in context.admin where a.adminID == 1 select a;
                var resultAdmin = Query.FirstOrDefault<admin>();
            }
            return View();
        }
    }
}