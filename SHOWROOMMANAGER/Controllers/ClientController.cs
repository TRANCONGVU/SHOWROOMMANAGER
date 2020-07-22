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
            var a = new ArrayList();
            var products = from p in db.products select p;
            var brands = from br in db.brands select br;
            products = products.OrderByDescending(p => p.product_id);
            brands = brands.OrderByDescending(br => br.brand_id);

            a.Add(brands);
            a.Add(products);
            return View(a.ToArray());
        }
    }
}