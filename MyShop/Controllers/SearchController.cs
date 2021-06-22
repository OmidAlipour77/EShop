using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Controllers
{
    public class SearchController : Controller
    {
        UnitOfWork db = new UnitOfWork();
        // GET: Search
        public ActionResult Index(string q)
        {
            List<Products> list = new List<Products>();
            list.AddRange(db.TagsRepository.Get(t => t.Tag == q).Select(t => t.Products));
            list.AddRange(db.ProductRepository.Get(p => p.Title.Contains(q) || p.ShortDescription.Contains(q) || p.Text.Contains(q)));
            ViewBag.Search = q;
            return View(list.Distinct());
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