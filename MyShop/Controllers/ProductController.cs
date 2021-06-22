using DataLayer;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Controllers
{
    public class ProductController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        // GET: Product
        public ActionResult ShowGroups()
        {
            var groups = db.ProductGroupsRepository.Get();
            return PartialView(groups.ToList());
        }
        public ActionResult LastProduct()
        {
            return PartialView(db.ProductRepository.Get().OrderByDescending(p => p.CreateDate).Take(12));
        }
        [Route("Products/{id}")]
        public ActionResult ShowProduct(int id)
        {
            var product = db.ProductRepository.GetById(id);
            ViewBag.ProductFeature = db.ProductFeatureRepository.Get().DistinctBy(f => f.FeatureID).Select(f => new ShowProductFeatureViewModel()
            {
                FeatureTitle = f.Feature.FeatureTitle,
                Values = db.ProductFeatureRepository.Get(fe => fe.FeatureID == f.FeatureID && fe.ProductID == product.ProductID).Select(fe => fe.Value).ToList()
            }).ToList();
            //ViewBag.ProductGroup = db.SelectGroupRepository.Get(p => p.ProductID == product.ProductID&&p.ProductGroups.ParentID!=null).Select(p=>new showProductGroupViewModel() {
            //GroupName=p.ProductGroups.GroupTitle
            //}).ToList();
            ViewBag.ProductGroup = db.SelectGroupRepository.Get(p => p.ProductID == product.ProductID).Select(p => p.ProductGroups).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        public ActionResult ShowComment(int id)
        {

            return PartialView(db.ProductCommentRepository.Get(c => c.ProductID == id));
        }
        public ActionResult CreateComment(int id)
        {
            return PartialView(new ProductComment() { ProductID = id });
        }
        [HttpPost]
        public ActionResult CreateComment(ProductComment productComment)
        {
            if (ModelState.IsValid)
            {
                productComment.CreateDate = DateTime.Now;
                db.ProductCommentRepository.Insert(productComment);
                db.ProductCommentRepository.Save();
                return PartialView("ShowComment", db.ProductCommentRepository.Get(c => c.ProductID == productComment.ProductID));
            }
            return PartialView(productComment);
        }
        [Route("Archive")]
        public ActionResult Archive(int pageId = 1, string title = "", int minPrice = 0, int maxPrice = 0, List<int> selectGroup = null)
        {
            ViewBag.Group = db.ProductGroupsRepository.Get().ToList();
            ViewBag.ProductTitle = title;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.selectedGroup = selectGroup;
            ViewBag.pageid = pageId;
            List<Products> list = new List<Products>();
            if (selectGroup != null && selectGroup.Any())
            {
                foreach (var group in selectGroup)
                {
                    list.AddRange(db.SelectGroupRepository.Get(p => p.GroupID == group).Select(p => p.Products).ToList());
                }
                list = list.Distinct().ToList();
            }
            else
            {
                list.AddRange(db.ProductRepository.Get().ToList());
            }
            if (title != null)
            {
                list = list.Where(p => p.Title.ToLower().Contains(title)).ToList();
            }
            if (minPrice > 0)
            {
                list = list.Where(p => p.Price >= minPrice).ToList();
            }
            if (maxPrice > 0)
            {
                list = list.Where(p => p.Price <= maxPrice).ToList();
            }
            int take = 9;
            int skip = (pageId - 1) * take;
            ViewBag.PageCount = list.Count() / take;
            return View(list.OrderByDescending(o => o.CreateDate).Skip(skip).Take(take).ToList());
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