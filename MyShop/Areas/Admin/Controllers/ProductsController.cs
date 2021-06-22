using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyShop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        // GET: Admin/Products
        public ActionResult Index()
        {
            return View(db.ProductRepository.Get());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.ProductRepository.GetById(id.Value);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.showGroup = db.ProductGroupsRepository.Get().ToList();
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Title,ShortDescription,Text,Price,CreateDate,ImageName")] Products products, List<int> selectGroup, HttpPostedFileBase imgUp, string Tags)
        {
            if (ModelState.IsValid)
            {
                products.CreateDate = DateTime.Now;
                products.ImageName = "image.jpg";
                if (imgUp != null && imgUp.IsImage())
                {
                    products.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/Product/" + products.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/Product/" + products.ImageName), Server.MapPath("/Images/Product/thumb/" + products.ImageName));
                }
                if (selectGroup == null)
                {
                    ViewBag.ErrorSelectGroup = true;
                    ViewBag.showGroup = db.ProductGroupsRepository.Get().ToList();

                    return View(products);
                }
                foreach (int item in selectGroup)
                {
                    db.SelectGroupRepository.Insert(new ProductSelectGroup()
                    {
                        ProductID = products.ProductID,
                        GroupID = item
                    });
                }
                if (!string.IsNullOrEmpty(Tags))
                {
                    string[] tag = Tags.Split(',');
                    foreach (string t in tag)
                    {
                        db.TagsRepository.Insert(new ProductTags()
                        {
                            ProductID = products.ProductID,
                            Tag = t
                        });
                    }

                }
                db.ProductRepository.Insert(products);
                db.ProductRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.showGroup = db.ProductGroupsRepository.Get().ToList();
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.ProductRepository.GetById(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.selectGroup = products.ProductSelectGroup.ToList();
            ViewBag.Tags = string.Join(",", products.ProductTags.Select(t => t.Tag).ToList());
            ViewBag.showGroup = db.ProductGroupsRepository.Get().ToList();
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Title,ShortDescription,Text,Price,CreateDate,ImageName")] Products products, HttpPostedFileBase imgUp, List<int> selectGroup, string Tags)
        {
            if (ModelState.IsValid)
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    if (products.ImageName != "image.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/Product/" + products.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/Product/thumb/" + products.ImageName));
                    }
                    products.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/Product/" + products.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/Product/" + products.ImageName), Server.MapPath("/Images/Product/thumb/" + products.ImageName));

                }
                db.TagsRepository.Get(t => t.ProductID == products.ProductID).ToList().ForEach(t => db.TagsRepository.Delete(t));
                if (!string.IsNullOrEmpty(Tags))
                {
                    string[] tag = Tags.Split(',');
                    foreach (var t in tag)
                    {
                        db.TagsRepository.Insert(new ProductTags()
                        {
                            ProductID = products.ProductID,
                            Tag = t
                        });
                    }
                }
                db.SelectGroupRepository.Get(g => g.ProductID == products.ProductID).ToList().ForEach(g => db.SelectGroupRepository.Delete(g));
                foreach (int item in selectGroup)
                {
                    db.SelectGroupRepository.Insert(new ProductSelectGroup()
                    {
                        ProductID = products.ProductID,
                        GroupID = item
                    });
                }
                db.ProductRepository.Update(products);
                db.ProductRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.showGroup = selectGroup;
            ViewBag.showGroup = db.ProductGroupsRepository.Get().ToList();
            ViewBag.Tags = Tags;
            return View(products);
        }


        #region Gallery
        public ActionResult Gallery(int id)
        {
            ViewBag.Gallery = db.GalleryRepository.Get(g => g.ProductID == id).ToList();
            return View(new Product_Galleries()
            {
                ProductID = id
            });
        }
        [HttpPost]
        public ActionResult Gallery(Product_Galleries gallery, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    gallery.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/Product/Gallery/" + gallery.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/Product/Gallery/" + gallery.ImageName), Server.MapPath("/Images/Product/Gallery/thumb/" + gallery.ImageName));
                    db.GalleryRepository.Insert(gallery);
                    db.GalleryRepository.Save();
                }

            }
            return RedirectToAction("Gallery", new { id = gallery.ProductID });
        }

        public ActionResult DeleteGallery(int id)
        {
            var gallery = db.GalleryRepository.GetById(id);
            System.IO.File.Delete(Server.MapPath("/Images/Product/Gallery/" + gallery.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/Product/Gallery/thumb/" + gallery.ImageName));
            db.GalleryRepository.Delete(gallery);
            db.GalleryRepository.Save();
            return RedirectToAction("Gallery", new { id = gallery.ProductID });
        }
        #endregion

        #region ProductFeature
        public ActionResult ProductFeature(int id)
        {
            ViewBag.Feature = db.ProductFeatureRepository.Get(f => f.ProductID == id).ToList();
            ViewBag.FeatureID = new SelectList(db.FeatureRepository.Get(), "FeatureID", "FeatureTitle");
            return View(new ProductFeature()
            {
                ProductID = id
            });
        }
        [HttpPost]
        public ActionResult ProductFeature(ProductFeature feature)
        {
            if (ModelState.IsValid)
            {
                db.ProductFeatureRepository.Insert(feature);
                db.ProductFeatureRepository.Save();
                return RedirectToAction("ProductFeature");
            }
            return View(feature);
        }
        public void DeletePF(int id)
        {
            var feature = db.ProductFeatureRepository.GetById(id);
            db.ProductFeatureRepository.Delete(feature);
            db.ProductFeatureRepository.Save();
        }
        #endregion


        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.ProductRepository.GetById(id.Value);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.ProductRepository.GetById(id);
            if (products.Product_Galleries.Any(p => p.ProductID == products.ProductID))
            {
                List<Product_Galleries> gallery = db.GalleryRepository.Get(p => p.ProductID == products.ProductID).ToList();
                foreach(var item in gallery)
                {
                    System.IO.File.Delete(Server.MapPath("/Images/Product/Gallery/" + item.ImageName));
                    System.IO.File.Delete(Server.MapPath("/Images/Product//Gallery/thumb/" + item.ImageName));
                }
                
                db.GalleryRepository.Get(g => g.ProductID == products.ProductID).ToList().ForEach(g => db.GalleryRepository.Delete(g));
            }
            if (products.ProductFeature.Any(p => p.ProductID == products.ProductID))
            {
                db.ProductFeatureRepository.Get(g => g.ProductID == products.ProductID).ToList().ForEach(g => db.ProductFeatureRepository.Delete(g));
            }
            if (products.ProductSelectGroup.Any(p=>p.ProductID==products.ProductID))
            {
                db.SelectGroupRepository.Get(s => s.ProductID == products.ProductID).ToList().ForEach(s => db.SelectGroupRepository.Delete(s));
            }
            if (products.ProductTags.Any(p=>p.ProductID==products.ProductID))
            {
                db.TagsRepository.Get(t => t.ProductID == products.ProductID).ToList().ForEach(t => db.TagsRepository.Delete(t));
            }
            if(products.ProductComment.Any(c=>c.ProductID==products.ProductID))
            {
                db.ProductCommentRepository.Get(c => c.ProductID == products.ProductID).ToList().ForEach(c => db.ProductCommentRepository.Delete(c));
            }

            if (products.ImageName != "image.jpg")
            {
                System.IO.File.Delete(Server.MapPath("/Images/Product/" + products.ImageName));
                System.IO.File.Delete(Server.MapPath("/Images/Product/thumb/" + products.ImageName));
            }
            db.ProductRepository.Delete(products);
            db.ProductRepository.Save();
            return RedirectToAction("Index");
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
