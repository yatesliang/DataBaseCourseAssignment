using SceneView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.Entity.Infrastructure;
using Qiniu.Storage;
using Qiniu.Util;
using Qiniu.Http;

namespace SceneView.Controllers
{
    public class EditorController : Controller
    {
        public Entities db = new Entities();
       
        public void getNote(string content, string timeStamp, string noteTitle,string scenicName)
        {
            var note = new note();
                       
            content = content.Replace("&lt", "<");
            content = content.Replace("&gt", ">");

            //将noteContent写入本地文件
            //string filePath = "..\\Sources\\note\\" + timeStamp.ToString() + ".txt";
            string filePath = "C:\\Users\\admin\\Desktop\\DBCourse" + timeStamp.ToString() + ".html";
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(content);
            sw.Close();


            // 使用qiniu传递数据
            Config config = new Config();
            // 空间对应的机房
            config.Zone = Zone.ZONE_CN_East;
            // 是否使用https域名
            config.UseHttps = true;
            // 上传是否使用cdn加速
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;

            // 简单文件上传
            Mac mac = new Mac("LIjKmW98gcj0ahR_Fogx3AV8V1JsRtOQo6qT43Cu", "W_lPo12yjHkaVyfibquVSJH8ImLWXw-tIXeggNSo");
            // 上传文件名
            string key = timeStamp + ".html";
            // 本地文件路径
            filePath = "C:\\Users\\admin\\Desktop\\DBCourse" + timeStamp.ToString() + ".html";
            // 存储空间名
            string Bucket = "editor";
            // 设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            // 表单上传
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadFile(filePath, key, token, null);
            Console.WriteLine("form upload result: " + result.ToString());

            System.IO.File.Delete(filePath);

            // 查找scenic
            var spot = db.scenicSpot.Where(s => s.scenicName == scenicName).FirstOrDefault<scenicSpot>();
            var images = spot.image.ToArray<image>();
            int index = (1 + images.Length) / 2;
            if (index > 0)
            {
                note.image.Add(images[index]);
            }

            // noteID为自增长属性，note为空就编号为1
            var notes = db.note;
            note.noteID = notes.Count() == 0 ? 1 : notes.Select(n => n.noteID).Max() + 1;
            note.userID = Session["user"].ToString();
            note.noteContent = timeStamp + ".html";
            //int t = int.Parse(timeStamp);
            note.noteTime = new DateTime(long.Parse(timeStamp));
            note.scenicID = spot.scenicID;
            note.title = noteTitle;
            note.noteLike = 0;

            // 将数据写入数据库
            notes.Add(note);
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

        // GET: Editor
        public ActionResult Index()
        {
            ViewBag.scenicName = Request.QueryString["sn"];
            return View();
        }
    }
}