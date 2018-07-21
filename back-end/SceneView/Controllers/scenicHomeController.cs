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

        public class ScenicInfo
        {
            public string imageAddress { get; set; }
            public string districtName { get; set; }
            public string scenicName { get; set; }
            public string address { get; set; }
            public double rate { get; set; }
        }

        // GET: scenicSpots
        [HttpGet]
        public ActionResult Index()
        {
            var districtName = Request.QueryString["dn"];
            districtName = districtName == "" || districtName == null ? "黄浦区" : districtName;

          
            //var scenic = db.scenicPos.Where(r => r.district == info.districtName ).SingleOrDefault();
            var scenic = (from c in db.scenicPos where c.district == districtName select c).Distinct();
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
         

    }
}
