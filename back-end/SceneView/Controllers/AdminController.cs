using SceneView.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SceneView.Controllers
{
    public class AdminController : Controller
    {
        // 每页展示8个项目
        private int itemEachPage = 8;
        private Entities db = new Entities();
        public enum Gender
        {
            Male,
            Female
        }
        public void ValidSession()
        {
            if (Session["admin"] == null)
            {
                Redirect("/Login/Admin");
            }
        }
        [HttpGet]
        public ActionResult Index()
        {
            ValidSession();
            ViewBag.type = 0;
            AdminData adminData = new AdminData();
            var result = db.userInfo.OrderBy(u=>u.userID).ToList();
            var current = Request.QueryString["page"];
            var search = Request.QueryString["search"];
            if (search != null)
            {
                adminData.search = search;
                result = result.Where(r => r.nickname.Contains(search)).ToList();
            }
            adminData.totalPage = result.Count() / itemEachPage + 1;
            // 默认页面1
            if (current == null)
            {
                adminData.currentPage = 1;
            }
            else
            {
                adminData.currentPage = int.Parse(current);
            }
            // 分页操作，即取子列表
            if (adminData.currentPage >= adminData.totalPage)
            {
                int remain = result.Count() % itemEachPage;
                adminData.userInfos = result.Skip(result.Count - remain).Take(remain).ToList();
            }
            else
            {
                adminData.userInfos = result.Skip((adminData.currentPage - 1) * itemEachPage).Take(itemEachPage).ToList();
            }
            return View(adminData);
        }
        [HttpPost]
        public ActionResult Index(AdminData adminData)
        {
            ValidSession();
            var url = "/Admin/Index?search=" + adminData.userInfo.nickname + "&page=1";
            return Redirect(url);
        }
        [HttpPost]
        public ActionResult EditUser(AdminData adminData)
        {
            ValidSession();
            var result = db.userInfo.Where(u => u.userID == adminData.userInfo.userID).FirstOrDefault<userInfo>();
            if (result != null)
            {
                // 修改用户信息
                result.nickname = adminData.userInfo.nickname;
                result.gender = adminData.userInfo.gender;
                result.phoneNumber = adminData.userInfo.phoneNumber;
                result.introduction = adminData.userInfo.introduction;
                result.SQAnswer = adminData.userInfo.SQAnswer;
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
                return Redirect("/Admin");
            }
            return Redirect("/Error");
        }
        [HttpPost]
        public ActionResult DeleteUser(AdminData adminData)
        {
            ValidSession();
            var result = db.user.Where(u => u.userID == adminData.userInfo.userID).FirstOrDefault<user>();
            if (result != null)
            {
                db.user.Remove(result);
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
            return Redirect("/Admin");
        }
        [HttpGet]
        public ActionResult Scene()
        {
            ValidSession();
            ViewBag.type = 1;
            AdminData adminData = new AdminData();
            var result = db.scenicSpot.OrderBy(s => s.scenicID).ToList();
            var current = Request.QueryString["page"];
            var search = Request.QueryString["search"];
            if(search != null)
            {
                adminData.search = search;
                result = result.Where(r => r.scenicName.Contains(search)).ToList();
            }
            adminData.totalPage = result.Count() / itemEachPage + 1;
            // 默认页面1
            if (current == null)
            {
                adminData.currentPage = 1;
            }
            else
            {
                adminData.currentPage = int.Parse(current);
            }
            // 分页操作，即取子列表
            if (adminData.currentPage >= adminData.totalPage)
            {
                int remain = result.Count() % itemEachPage;
                adminData.scenicSpots = result.Skip(result.Count - remain).Take(remain).ToList();
            }
            else
            {
                adminData.scenicSpots = result.Skip((adminData.currentPage - 1) * itemEachPage).Take(itemEachPage).ToList();
            }
            return View(adminData);
        }
        [HttpPost]
        public ActionResult Scene(AdminData adminData)
        {
            ValidSession();
            var url = "/Admin/Scene?search=" + adminData.scenicSpot.scenicName + "&page=1";
            return Redirect(url);
        }
        [HttpPost]
        public ActionResult EditScene(AdminData adminData)
        {
            ValidSession();
            var result = db.scenicSpot.Where(s => s.scenicID == adminData.scenicSpot.scenicID).FirstOrDefault();
            if (result != null)
            {
                result.scenicName = adminData.scenicSpot.scenicName;
                result.scenicPos.latitude = adminData.scenicSpot.scenicPos.latitude;
                result.scenicPos.longitude = adminData.scenicSpot.scenicPos.longitude;
                result.scenicPos.city = adminData.scenicSpot.scenicPos.city;
                result.scenicPos.district = adminData.scenicSpot.scenicPos.district;
                result.scenicPos.address = adminData.scenicSpot.scenicPos.address;
                result.scenicIntroduction = adminData.scenicSpot.scenicIntroduction;
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
            return Redirect("/Admin/Scene");
        }
        [HttpPost]
        public ActionResult DeleteScene(AdminData adminData)
        {
            ValidSession();
            var result = db.scenicSpot.Where(s => s.scenicID == adminData.scenicSpot.scenicID).FirstOrDefault();
            if (result != null)
            {
                db.scenicSpot.Remove(result);
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
            return Redirect("/Admin/Scene");
        }
        [HttpPost]
        public ActionResult AddScene(AdminData adminData)
        {
            ValidSession();
            var result = db.scenicSpot.Where(s => s.scenicName == adminData.scenicSpot.scenicName);
            if (result == null)
            {
                var spot = new scenicSpot();
                var pos = new scenicPos();
                int id = db.scenicSpot.Count() == 0 ? 1 : (db.scenicSpot.Select(s => s.scenicID).Max() + 1);
                // 添加景点
                spot.scenicID = short.Parse(id.ToString());
                spot.scenicIntroduction = adminData.scenicSpot.scenicIntroduction;
                spot.scenicName = adminData.scenicSpot.scenicName;
                db.scenicSpot.Add(spot);
                // 添加景点对应位置
                pos.scenicID = short.Parse(id.ToString());
                pos.latitude = adminData.scenicSpot.scenicPos.latitude;
                pos.longitude = adminData.scenicSpot.scenicPos.longitude;
                pos.city = adminData.scenicSpot.scenicPos.city;
                pos.district = adminData.scenicSpot.scenicPos.district;
                pos.address = adminData.scenicSpot.scenicPos.address;
                db.scenicPos.Add(pos);
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
            return Redirect("/Admin/Scene");
        }
        public ActionResult Cancel()
        {
            Session["admin"] = null;
            return Redirect("/Login/Admin");
        }
        public class AdminData
        {
            public List<userInfo> userInfos { get; set; }
            public userInfo userInfo { get; set; }
            public List<scenicSpot> scenicSpots { get; set; }
            public scenicSpot scenicSpot { get; set; }
            public int currentPage = 1;
            public int totalPage = 1;
            public string search;
            public string admin;
            public AdminData()
            {
                userInfos = new List<userInfo>();
                userInfo = new userInfo();
                scenicSpots = new List<scenicSpot>();
                scenicSpot = new scenicSpot();
            }
        }
    }
}