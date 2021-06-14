using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new List<ImgUrlModel>();

            using (var context = new ProfileSampleEntities())
            {
                model = context.ImgUrls.Take(20).Select(x => new ImgUrlModel { Data = x.Data, Name = x.Name }).ToList();
            }

            //Unoptimized code
            //var context = new ProfileSampleEntities();
            //var model = new List<ImageModel>();

            //var sources = context.ImgSources.Take(20).Select(x => x.Id);
            //foreach (var id in sources)
            //{
            //    var item = context.ImgSources.Find(id);

            //    var obj = new ImageModel()
            //    {
            //        Name = item.Name,
            //        Data = item.Data
            //    };

            //    model.Add(obj);
            //}

            return View(model);
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg").Select(x => x.Replace(Server.MapPath("~"), ""));

            using (var context = new ProfileSampleEntities())
            {
                var i = 0;
                foreach (var file in files)
                {

                    i++;

                    var entity = new ImgUrl()
                    {
                        Id = i,
                        Name = Path.GetFileName(file),
                        Data = file,
                    };

                    context.ImgUrls.Add(entity);
                    context.SaveChanges();

                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}