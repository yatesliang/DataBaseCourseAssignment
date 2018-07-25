using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SceneView.Models;

namespace SceneView.Controllers
{
    public class scenicHomeController : Controller
    {
        private Entities db = new Entities();
        private bool isSearch = false;

        public class ScenicInfo
        {
            public string imageAddress { get; set; }
            public string districtName { get; set; }
            public string scenicName { get; set; }
            public string address { get; set; }
            public double rate { get; set; }
            public string searchContent { get; set; }
        }

        // GET: scenicSpots
        [HttpGet]
        public ActionResult Index()
        {
            var searchStr = Request.QueryString["search"];
            if(searchStr !="" && searchStr != null)
            {
                var scenicList = (from a in db.scenicPos
                                  join u in db.scenicSpot on a.scenicID equals u.scenicID
                                  where u.scenicName.Contains(searchStr) || a.district.Contains(searchStr)
                                  select new ScenicInfo()
                                  {
                                      scenicName = u.scenicName,
                                      address = a.address,
                                      imageAddress = u.image.FirstOrDefault().imageAddress,
                                      districtName = a.district,
                                      rate = 5                                                             // the rate not in the database?

                                  }).ToList();
                if (scenicList.Count() == 0)
                {
                    //Found nothing
                    ViewBag.flag = -1;

                }
                else
                {
                    ViewBag.Data = scenicList;
                    return View();
                }
                
            }
            var districtName = Request.QueryString["dn"];
            districtName = districtName == "" || districtName == null ? "浦东" : districtName;

          
            //var scenic = db.scenicPos.Where(r => r.district == info.districtName ).SingleOrDefault();
            var scenic = (from c in db.scenicPos where c.district.Contains(districtName) select c).Distinct();
            var scenicArr = scenic.ToList();
            IList<ScenicInfo> scenicInfos = new List<ScenicInfo>();
          
            foreach(var item in scenicArr)
            {
                ScenicInfo temp = new ScenicInfo();
                var scenicID = item.scenicSpot.scenicID;
                var image = item.scenicSpot.image.First<image>();
                temp.address = item.address;
                temp.districtName = item.district;
                temp.scenicName = item.scenicSpot.scenicName;
                temp.imageAddress = image.imageAddress;
                temp.rate = 5;
                scenicInfos.Add(temp);
                
            }
            ViewBag.Data = scenicInfos;

        
         
            return View(scenicInfos);
        }
        //Switch the district and show the scenic spots

        public ActionResult switchDistrict(string districName)
        {
            districName = "浦东新区";
            ScenicInfo temp = new ScenicInfo();
            temp.districtName = districName;
            return Index();
        }



        [HttpPost]
        public ActionResult Search(List<ScenicInfo> info)
        {
            var searchStr = info[0].searchContent;
 
            return Redirect("~/ScenicHome/Index?search="+searchStr);
            
        }
         

    }
}
