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
    public class FeaturesController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        // GET: Admin/Features
        public ActionResult Index()
        {
            return View(db.FeatureRepository.Get().ToList());
        }

        // GET: Admin/Features/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = db.FeatureRepository.GetById(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        // GET: Admin/Features/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/Features/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeatureID,FeatureTitle")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                db.FeatureRepository.Insert(feature);
                db.FeatureRepository.Save();
                return RedirectToAction("Index");
            }

            return View(feature);
        }

        // GET: Admin/Features/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = db.FeatureRepository.GetById(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return PartialView(feature);
        }

        // POST: Admin/Features/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeatureID,FeatureTitle")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                db.FeatureRepository.Update(feature);
                db.FeatureRepository.Save();
                return RedirectToAction("Index");
            }
            return View(feature);
        }

        // GET: Admin/Features/Delete/5
        public ActionResult Delete(int id)
        {
            var feature = db.FeatureRepository.GetById(id);
            db.FeatureRepository.Delete(feature);
            db.FeatureRepository.Save();
            return View(feature);
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
