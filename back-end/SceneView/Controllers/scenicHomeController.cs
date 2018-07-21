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
        public ActionResult Index(ScenicInfo info)
        {
            info.districtName = "松江区";
            var scenic = db.scenicPos.Where(u => u.district == info.districtName);
            var scenicArr = scenic.ToArray();
            IList<ScenicInfo> scenicInfos = new List<ScenicInfo>();
            ScenicInfo temp = new ScenicInfo();
            foreach(var item in scenicArr)
            {
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



    }
}
