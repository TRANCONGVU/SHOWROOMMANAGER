using SHOWROOMMANAGER.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SHOWROOMMANAGER.Controllers
{

    public class ClientController : Controller
    {
        private ShowroomManagerEntities db = new ShowroomManagerEntities();
        // GET: Client

        public ActionResult Index()
        {
           
            var products = from p in db.products select p;
            var brands = from b in db.brands select b;

            products = products.OrderByDescending(p => p.product_id).Take(6);
            brands = brands.OrderByDescending(b => b.brand_id).Take(3);


            HomePageViewModel mymodel = new HomePageViewModel();
            mymodel.Products = products.ToList();
            mymodel.Brands = brands.ToList();
            mymodel.Showrooms = db.Showrooms.ToList();
            return View(mymodel);            
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Contact(int id)
        {
            return View();
        }
    }
}