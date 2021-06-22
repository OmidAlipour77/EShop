using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyShop.Areas.Admin.Controllers
{
    
    public class ProductGroupsController : Controller
    {
        UnitOfWork db = new UnitOfWork();
        // GET: Admin/ProductGroups
        public ActionResult Index()
        {
            var productGroups = db.ProductGroupsRepository.Get();
            return View(productGroups.ToList());
        }

        // GET: Admin/ProductGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroups productGroups = db.ProductGroupsRepository.GetById(id);
            if (productGroups == null)
            {
                return HttpNotFound();
            }
            return View(productGroups);
        }

        // GET: Admin/ProductGroups/Create
        public ActionResult Create(int? id)
        {
           
            return PartialView(new ProductGroups()
            {
                ParentID=id
            });
        }

        // POST: Admin/ProductGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,GroupTitle,ParentID")] ProductGroups productGroups)
        {
            if (ModelState.IsValid)
            {
                db.ProductGroupsRepository.Insert(productGroups);
                db.ProductGroupsRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ParentID = new SelectList(db.ProductGroupsRepository.Get(), "GroupID", "GroupTitle", productGroups.ParentID);
            return View(productGroups);
        }

        // GET: Admin/ProductGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroups productGroups = db.ProductGroupsRepository.GetById(id);
            if (productGroups == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentID = new SelectList(db.ProductGroupsRepository.Get(), "GroupID", "GroupTitle", productGroups.ParentID);
            return PartialView(productGroups);
        }

        // POST: Admin/ProductGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,GroupTitle,ParentID")] ProductGroups productGroups)
        {
            if (ModelState.IsValid)
            {
                db.ProductGroupsRepository.Update(productGroups);
                db.ProductGroupsRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(db.ProductGroupsRepository.Get(), "GroupID", "GroupTitle", productGroups.ParentID);
            return View(productGroups);
        }

        // GET: Admin/ProductGroups/Delete/5
        public void Delete(int id)
        {
            var group = db.ProductGroupsRepository.GetById(id);
            if(group.ProductGroups1.Any())
            {
                foreach(var subgroup in db.ProductGroupsRepository.Get(g=>g.ParentID==id))
                {
                    db.ProductGroupsRepository.Delete(subgroup);
                    db.ProductGroupsRepository.Delete(group);
                    db.ProductGroupsRepository.Save();
                }
            }
            else
            {
                db.ProductGroupsRepository.Delete(group);
                db.ProductGroupsRepository.Save();
            }
        }

        // POST: Admin/ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductGroups productGroups = db.ProductGroupsRepository.GetById(id);
            db.ProductGroupsRepository.Delete(productGroups);
            db.ProductGroupsRepository.Save();
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
