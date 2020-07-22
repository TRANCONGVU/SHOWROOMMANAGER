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
    public class customerController : Controller
    {
        private ShowroomManagerEntities db = new ShowroomManagerEntities();

        // GET: customer
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // var customers = db.customers.Include(c => c.product);
            // return View(customers.ToList());
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


            var customers = from c in db.customers select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.customer_name.Contains(searchString));

            }

            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(c => c.customer_name);
                    break;
                case "name":
                    customers = customers.OrderBy(c => c.customer_name);
                    break;               

                default:
                    customers = customers.OrderBy(c => c.customer_name);
                    break;
            }

            //return View(products.ToList());
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(customers.ToPagedList(pageNumber, pageSize));

        }

        // GET: customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: customer/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name");
            return View();
        }

        // POST: customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "customer_id,customer_name,customer_address,customer_phone,customer_email,product_id")] customer customer)
        {
            if (ModelState.IsValid)
            {
                db.customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", customer.product_id);
            return View(customer);
        }

        // GET: customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", customer.product_id);
            return View(customer);
        }

        // POST: customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "customer_id,customer_name,customer_address,customer_phone,customer_email,product_id")] customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", customer.product_id);
            return View(customer);
        }

        // GET: customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
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
