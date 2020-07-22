using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SHOWROOMMANAGER.Models;

namespace SHOWROOMMANAGER.Controllers
{
    public class ShowroomController : Controller
    {
        private ShowroomManagerEntities db = new ShowroomManagerEntities();

        // GET: Showroom
        public ActionResult Index()
        {
            return View(db.Showrooms.ToList());
        }

        // GET: Showroom/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showroom showroom = db.Showrooms.Find(id);
            if (showroom == null)
            {
                return HttpNotFound();
            }
            return View(showroom);
        }

        // GET: Showroom/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Showroom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "showroom_id,showroom_name,showroom_phone,showroom_address")] Showroom showroom)
        {
            if (ModelState.IsValid)
            {
                db.Showrooms.Add(showroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(showroom);
        }

        // GET: Showroom/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showroom showroom = db.Showrooms.Find(id);
            if (showroom == null)
            {
                return HttpNotFound();
            }
            return View(showroom);
        }

        // POST: Showroom/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "showroom_id,showroom_name,showroom_phone,showroom_address")] Showroom showroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(showroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(showroom);
        }

        // GET: Showroom/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showroom showroom = db.Showrooms.Find(id);
            if (showroom == null)
            {
                return HttpNotFound();
            }
            return View(showroom);
        }

        // POST: Showroom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Showroom showroom = db.Showrooms.Find(id);
            db.Showrooms.Remove(showroom);
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
