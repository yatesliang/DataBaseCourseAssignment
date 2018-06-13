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
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Login()
        {
            bool valid = ModelState.IsValid;
            if (valid)
            {
                var adminResult = admin.Login(admin);
                if (adminResult != null)
                {
                    return RedirectToAction("Index", "admins");
                }
            }
            return View("Error");
        }
        // GET: Login
        public ActionResult Index()
        {
//            using (var context = new Entities())
//            {
//                // - 不可行(找不到表名)
////                var resultAdmin = context.admin.SqlQuery("select password from admin where adminID = 1").FirstOrDefault<admin>();
//                // - 可行
//                var resultAdmin = context.admin.Where(s => s.adminID == 1).FirstOrDefault<admin>();
//                // - 可行
////                var Query = from a in context.admin where a.adminID == 1 select a;
////                var resultAdmin = Query.FirstOrDefault<admin>();
//                string password = "tanrui105";
//                MD5TransferAndVerify md5 = new MD5TransferAndVerify();
//                string md5ps = md5.GetMD5Hash(password);
//                Boolean md5verify = md5.Verify(resultAdmin.password, password);
//            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(admin admin)
        {
            bool valid = ModelState.IsValid;
            if (valid)
            {
                var adminResult = admin.Login(admin);
                if (adminResult != null)
                {
                    return RedirectToAction("Index", "admins");
                }
            }
            return View("Error");
        }

    }
}