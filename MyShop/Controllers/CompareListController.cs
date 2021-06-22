using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Controllers
{
    public class CompareListController : Controller
    {
        UnitOfWork db = new UnitOfWork();
        // GET: CompareList
        [Route("Compare")]
        public ActionResult Index()
        {
            List<CompareItem> list = new List<CompareItem>();
            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItem>;
            }
            List<Feature> feature = new List<Feature>();
            List<ProductFeature> productFeature = new List<ProductFeature>();
            foreach(var item in list)
            {
                feature.AddRange(db.ProductFeatureRepository.Get(p => p.ProductID == item.ProductID).Select(p => p.Feature).ToList());
                productFeature.AddRange(db.ProductFeatureRepository.Get(p => p.ProductID == item.ProductID).ToList());
            }
            ViewBag.Feature = feature.Distinct().ToList();
            ViewBag.ProductFeature = productFeature.Distinct().ToList();
            return View(list);
        }
        public ActionResult CompareItem()
        {
            List<CompareItem> list = new List<CompareItem>();
            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItem>;
            }
            return PartialView(list);
        }
        public ActionResult AddToCompare(int id)
        {
            List<CompareItem> list = new List<CompareItem>();
            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItem>;
            }
            if (!list.Any(p=>p.ProductID==id))
            {
                var product = db.ProductRepository.Get(p=>p.ProductID==id).Select(p=>new {p.ImageName,p.Title}).Single();
                list.Add(new CompareItem() {
                ProductID=id,
                Title=product.Title,
                ImageName=product.ImageName
                });
            }
            Session["Compare"] = list;
            return PartialView("CompareItem", list);
        }
        public ActionResult DeleteCompare(int id)
        {
            List<CompareItem> list = new List<CompareItem>();
            if (Session["Compare"] != null)
            {
                list = Session["Compare"] as List<CompareItem>;
                var index = list.FindIndex(p => p.ProductID == id);
                list.RemoveAt(index);
            }
            Session["Compare"] = list;
            return PartialView("CompareItem", list);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}