using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SceneView.Models;

namespace SceneView.Controllers
{
    public class LoginController : Controller
    {
        private Entities db = new Entities();

        public const int Success = 0;
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(result r)
        {
            if (ModelState.IsValid)
            {
                var md5 = new MD5TransferAndVerify();
                // 获取数据库查询结果
                var result = db.userInfo.Where(u => u.nickname == r.username).FirstOrDefault<userInfo>();
                // 用户不存在
                if (result == null)
                {
                    ViewBag.flag = -1;
                    return View();
                }
                else
                {   
                    if (result.user.password == md5.GetMD5Hash(r.password))
                        // 用户验证正确，则创建Session会话，跳转至主页
                    {
                        Session["user"] = result.user.userID;
                        return Redirect("~/Home");
                    }
                    else
                    {
                        // 否则验证失败，密码错误
                        ViewBag.flag = 0;
                        return View();
                    }
                }
            }
            return Redirect("Error");
        }
        public ActionResult Sign()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Sign(result r)
        {
            if (ModelState.IsValid)
            {
                var md5 = new MD5TransferAndVerify();
                // 获取数据库查询结果
               var result = db.userInfo.Where(u => u.nickname == r.username).FirstOrDefault<userInfo>();
                if (result == null)
                {
                    // 创建用户、用户信息实例
                    var user = new user();
                    var userInfo = new userInfo();
                    // 为其赋值并添加到db中
                    // userID为自增长属性，user为空时，第一个编号为1
                    var users = db.user;
                    //user.userID = users.Count() == 0 ? "1" : (int.Parse(users.Select(u => u.userID).Max()) + 1).ToString();
                    if (users.Count() == 0)
                    {
                        user.userID = "1";
                    }
                    else
                    {
                        var usersID = (from u in users select u.userID).ToList();
                        long max = 0;
                        foreach(var id in usersID)
                        {
                            if (long.Parse(id) > max)
                                max = long.Parse(id);
                        }
                        user.userID = (max + 1).ToString();
                    }
                    user.password = md5.GetMD5Hash(r.password);
                    userInfo.userID = user.userID;
                    userInfo.nickname = r.username;
                    userInfo.phoneNumber = r.phone;
                    userInfo.SQAnswer = r.answer;
                    db.user.Add(user);
                    db.userInfo.Add(userInfo);
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
                    // 完成更新
                }
                else
                {
                    // 用户已存在
                    ViewBag.flag = 1;
                    return RedirectToAction("Index");
                }
                return Redirect("~/Home");
            }
            return Redirect("Error");
        }
        public ActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Forget(result r)
        {
            if (ModelState.IsValid)
            {
                var md5 = new MD5TransferAndVerify();
                // 获取数据库查询结果
                var result = db.userInfo.Where(u => u.nickname == r.username).FirstOrDefault<userInfo>();
                if(result == null)
                    // 用户不存在
                {
                    ViewBag.fFlag = -1;
                    return View();
                }
                else
                {
                    if (!result.SQAnswer.Equals(r.answer))
                        // 验证问题错误
                    {
                        ViewBag.fFlag = 0;
                        return View();
                    }
                    else
                        // 验证正确，更换密码
                    {
                        result.user.password = md5.GetMD5Hash(r.password);
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
                        // 完成更新
                        return RedirectToAction("index");
                    }
                }
            }
            return Redirect("Error");
        }
        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Admin(result r)
        {
            if (ModelState.IsValid)
            {
                var md5 = new MD5TransferAndVerify();
                // 获取数据库查询结果
                var result = db.admin.Where(a => a.adminID == r.admin).FirstOrDefault();
                // 管理员不存在
                if (result == null)
                {
                    ViewBag.flag = -1;
                    return View();
                }
                else
                {
                    if (result.password == md5.GetMD5Hash(r.adminPas))
                    // 用户验证正确，则创建Session会话，跳转至主页
                    {
                        Session["admin"] = result.adminID;
                        return Redirect("/Admin");
                    }
                    else
                    {
                        // 否则验证失败，密码错误
                        ViewBag.flag = 0;
                        return View();
                    }
                }
            }
            return Redirect("Error");
        }
        public class result
        {
            public string username { get; set; }
            public string password { get; set; }
            public long phone { get; set; }
            public string answer { get; set; }
            public string admin { get; set; }
            public string adminPas { get; set; }
        }

    }
}