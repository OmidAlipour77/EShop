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
    public class RolesController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        // GET: Admin/Roles
        public ActionResult Index()
        {
            return View(db.RoleRepository.Get());
        }

 

        // GET: Admin/Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleID,RoleTitle,RoleName")] Roles roles)
        {
            if (ModelState.IsValid)
            {
                db.RoleRepository.Insert(roles);
                db.RoleRepository.Save();
                return RedirectToAction("Index");
            }

            return View(roles);
        }

        // GET: Admin/Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles roles = db.RoleRepository.GetById(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        // POST: Admin/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleID,RoleTitle,RoleName")] Roles roles)
        {
            if (ModelState.IsValid)
            {
                db.RoleRepository.Update(roles);
                db.RoleRepository.Save();
                return RedirectToAction("Index");
            }
            return View(roles);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles roles = db.RoleRepository.GetById(id.Value);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }
        // POST: Admin/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Roles roles = db.RoleRepository.GetById(id);
            db.RoleRepository.Delete(roles);
            db.RoleRepository.Save();
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
