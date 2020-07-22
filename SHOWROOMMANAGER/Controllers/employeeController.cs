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
    public class employeeController : Controller
    {
        private ShowroomManagerEntities db = new ShowroomManagerEntities();

        // GET: employee
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


            var employees = from p in db.employees select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.employee_name.Contains(searchString));

            }

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(p => p.employee_name);
                    break;
                case "name":
                    employees = employees.OrderBy(p => p.employee_name);
                    break;

                case "date_desc":
                    employees = employees.OrderByDescending(p => p.employee_birthday);
                    break;
                case "date":
                    employees = employees.OrderBy(p => p.employee_birthday);
                    break;

                case "salary_desc":
                    employees = employees.OrderByDescending(p => p.employee_salary);
                    break;
                case "salary":
                    employees = employees.OrderBy(p => p.employee_salary);
                    break;

                default:
                    employees = employees.OrderBy(p => p.employee_name);
                    break;
            }

            //return View(products.ToList());
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(employees.ToPagedList(pageNumber, pageSize));
            // var employees = db.employees.Include(e => e.Showroom);
            // return View(employees.ToList());

        }

        // GET: employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: employee/Create
        public ActionResult Create()
        {
            ViewBag.showroom_id = new SelectList(db.Showrooms, "showroom_id", "showroom_name");
            return View();
        }

        // POST: employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "employee_id,employee_name,employee_birthday,employee_salary,employee_phone,employee_email,showroom_id")] employee employee)
        {
            if (ModelState.IsValid)
            {
                db.employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.showroom_id = new SelectList(db.Showrooms, "showroom_id", "showroom_name", employee.showroom_id);
            return View(employee);
        }

        // GET: employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.showroom_id = new SelectList(db.Showrooms, "showroom_id", "showroom_name", employee.showroom_id);
            return View(employee);
        }

        // POST: employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "employee_id,employee_name,employee_birthday,employee_salary,employee_phone,employee_email,showroom_id")] employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.showroom_id = new SelectList(db.Showrooms, "showroom_id", "showroom_name", employee.showroom_id);
            return View(employee);
        }

        // GET: employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            employee employee = db.employees.Find(id);
            db.employees.Remove(employee);
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
