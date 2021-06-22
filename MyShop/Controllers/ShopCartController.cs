using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Controllers
{
    public class ShopCartController : Controller
    {
        UnitOfWork db = new UnitOfWork();
        // GET: ShopCart
        public ActionResult ShowCart()
        {
            List<ShopCartItemViewModel> list = new List<ShopCartItemViewModel>();
            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> shopList = (List<ShopCartItem>)Session["ShopCart"];
                foreach (var item in shopList)
                {
                    var product = db.ProductRepository.Get(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.Title
                    }).Single();
                    list.Add(new ShopCartItemViewModel()
                    {
                        ProductID = item.ProductID,
                        Title = product.Title,
                        ImageName = product.ImageName,
                        Count = item.Count
                    });
                }
            }
            return PartialView(list);
        }
        public ActionResult Index()
        {
            return View();
        }
        List<ShowOrderViewModel> getListOrder()
        {
            List<ShowOrderViewModel> list = new List<ShowOrderViewModel>();
            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> listShop = Session["ShopCart"] as List<ShopCartItem>;
                foreach (var item in listShop)
                {
                    var product = db.ProductRepository.Get(p => p.ProductID == item.ProductID).Select(p => new {
                        p.Title,
                        p.Price,
                        p.ImageName
                    }).Single();
                    list.Add(new ShowOrderViewModel()
                    {
                        ProductID = item.ProductID,
                        Count = item.Count,
                        Title = product.Title,
                        ImageName = product.ImageName,
                        Price = product.Price,
                        Sum = item.Count * product.Price
                    });
                }
            }
            return list;
        }
        public ActionResult Order()
        {
            return PartialView(getListOrder());
        }
        public ActionResult CommandOrder(int id,int count)
        {
            List<ShopCartItem> listShop = Session["ShopCart"] as List<ShopCartItem>;
            int index = listShop.FindIndex(p => p.ProductID == id);
            if(count==0)
            {
                listShop.RemoveAt(index);
            }
            else
            {
                listShop[index].Count = count;
            }
            Session["ShopCart"] = listShop;
            return PartialView("Order", getListOrder());
        }
        [Authorize]
        public ActionResult Payment()
        {
            int userId = db.UserRepository.Get().Single(u => u.UserName == User.Identity.Name).UserID;
            Order order = new Order() {
            UserID=userId,
            Date=DateTime.Now,
            IsFinaly=false
            };
            db.OrderRepository.Insert(order);
            db.OrderRepository.Save();
            var listDetails = getListOrder();
            foreach(var item in listDetails)
            {
                db.OrderDetailsRepository.Insert(new OrderDetails() {
                Count=item.Count,
                OrderID=order.OrderID,
                Price=item.Price,
                ProductID=item.ProductID
                });
            }
            db.OrderDetailsRepository.Save();
            // online Payment
            return null;
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