using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Slider()
        {
            return PartialView();
        }
        public ActionResult Menu()
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var menu = db.ProductGroupsRepository.Get();
                return PartialView(menu.ToList());
            }
        }
   
    }
}