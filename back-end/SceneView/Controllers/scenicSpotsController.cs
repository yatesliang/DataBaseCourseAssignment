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
    public class scenicSpotsController : Controller
    {
        private Entities db = new Entities();
        

        //CLASS: store info that will show in the pages later
        public class ScenicInfo
        {
            public string classname { get; set; }
            public string districtName { get; set; }
            public string searchContent { get; set; }
            IList<scenicSpot> scenicList { get; set; }

        }

        // GET: scenicSpots
    public ActionResult showDistrict(ScenicInfo info)
        {
            var scenic = db.scenicPos.Where(u => u.district == info.districtName);
            ViewBag.Data = scenic.ToArray();
            return View();
        }
       

 

       
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "scenicID,scenicName,scenicIntroduction")] scenicSpot scenicSpot)
        {
            if (ModelState.IsValid)
            {
                db.scenicSpot.Add(scenicSpot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.scenicID = new SelectList(db.scenicPos, "scenicID", "address", scenicSpot.scenicID);
            return View(scenicSpot);
        }

        
      
    }
}
