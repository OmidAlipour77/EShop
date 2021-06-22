using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Controllers
{
    [Authorize]
    public class testController : Controller
    {
        // GET: test
        public ActionResult test1()
        {
            return View();
        }
        public ActionResult test2()
        {
            return View();
        }
    }
}