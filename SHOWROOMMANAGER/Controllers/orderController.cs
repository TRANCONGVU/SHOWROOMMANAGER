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
    public class orderController : Controller
    {
        private ShowroomManagerEntities db = new ShowroomManagerEntities();

        // GET: order
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
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


            var orders = from o in db.orders select o;

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.order_name.Contains(searchString));

            }

            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(o => o.order_name);
                    break;
                case "name":
                    orders = orders.OrderBy(o => o.order_name);
                    break;
                

                default:
                    orders = orders.OrderBy(o => o.order_name);
                    break;
            }

            //return View(products.ToList());
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        // GET: order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: order/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "customer_name");
            ViewBag.employee_id = new SelectList(db.employees, "employee_id", "employee_name");
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name");
            return View();
        }

        // POST: order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "order_id,order_name,order_description,order_time,product_id,customer_id,employee_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "customer_name", order.customer_id);
            ViewBag.employee_id = new SelectList(db.employees, "employee_id", "employee_name", order.employee_id);
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", order.product_id);
            return View(order);
        }

        // GET: order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "customer_name", order.customer_id);
            ViewBag.employee_id = new SelectList(db.employees, "employee_id", "employee_name", order.employee_id);
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", order.product_id);
            return View(order);
        }

        // POST: order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "order_id,order_name,order_description,order_time,product_id,customer_id,employee_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "customer_name", order.customer_id);
            ViewBag.employee_id = new SelectList(db.employees, "employee_id", "employee_name", order.employee_id);
            ViewBag.product_id = new SelectList(db.products, "product_id", "product_name", order.product_id);
            return View(order);
        }

        // GET: order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
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
