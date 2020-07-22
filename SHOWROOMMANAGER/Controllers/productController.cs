using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SHOWROOMMANAGER.Models;

namespace SHOWROOMMANAGER.Controllers
{
    public class productController : Controller
    {
        private ShowroomManagerEntities db = new ShowroomManagerEntities();
        

        // GET: product
        public ActionResult Index(string sortOrder , string currentFilter, string searchString , int? page)
        {
            //int page = 1;
            //var products = db.products.Include(p => p.brand).Include(p => p.Showroom);
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var products = from p in db.products select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.product_name.Contains(searchString));
                                       
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.product_name);
                    break;
                case "name":
                    products = products.OrderBy(p => p.product_name);
                    break;
                case "price":
                    products = products.OrderBy(p => p.product_price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.product_price);
                    break;

                default:
                    products = products.OrderBy(p => p.product_name);
                    break;
            }

            //return View(products.ToList());
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: product/Create
        public ActionResult Create()
        {
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name");
            ViewBag.showroom_id = new SelectList(db.Showrooms, "showroom_id", "showroom_name");
            return View();
        }

        // POST: product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_name,product_model,product_price,product_description,brand_id,showroom_id")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.showroom_id = new SelectList(db.Showrooms, "showroom_id", "showroom_name", product.showroom_id);
            return View(product);
        }

        // GET: product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.showroom_id = new SelectList(db.Showrooms, "showroom_id", "showroom_name", product.showroom_id);
            return View(product);
        }

        // POST: product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,product_name,product_model,product_price,product_description,brand_id,showroom_id")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.showroom_id = new SelectList(db.Showrooms, "showroom_id", "showroom_name", product.showroom_id);
            return View(product);
        }

        // GET: product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
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
