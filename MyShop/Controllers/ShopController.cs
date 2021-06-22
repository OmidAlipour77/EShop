using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DataLayer;

namespace MyShop.Controllers
{
    public class ShopController : ApiController
    {
        // GET: api/Shop
        public int Get()
        {
            List<ShopCartItem> list = new List<ShopCartItem>();
            var session = HttpContext.Current.Session;
            if (session["ShopCart"] != null)
            {
                list = session["ShopCart"] as List<ShopCartItem>;
            }
            return list.Sum(l=>l.Count);
        }

        // GET: api/Shop/5
        public int Get(int id)
        {
            List<ShopCartItem> list = new List<ShopCartItem>();
            var session = HttpContext.Current.Session;
            if(session["ShopCart"]!=null)
            {
                list = session["ShopCart"] as List<ShopCartItem>;
            }
            if(list.Any(p=>p.ProductID==id))
            {
                int index = list.FindIndex(p => p.ProductID == id);
                list[index].Count += 1;
            }
            else
            {
                list.Add(new ShopCartItem() {
                Count=1,
                ProductID=id
                });
            }
            session["ShopCart"] = list;
            return Get();
        }
    }
}
