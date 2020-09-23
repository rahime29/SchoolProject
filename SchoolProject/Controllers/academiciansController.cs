using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class academiciansController : Controller
    {
        private schooldbEntities db = new schooldbEntities();

        // GET: academicians
        public ActionResult Index()
        {
            var academicians = db.academicians.Include(a => a.titles);
            return View(academicians.ToList());
        }

        // GET: academicians/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            academicians academicians = db.academicians.Find(id);
            if (academicians == null)
            {
                return HttpNotFound();
            }
            return View(academicians);
        }

        // GET: academicians/Create
        public ActionResult Create()
        {
            ViewBag.title_id = new SelectList(db.titles, "id", "degree");
            return View();
        }

        // POST: academicians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,surname,title_id")] academicians academicians)
        {
            if (ModelState.IsValid)
            {
                db.academicians.Add(academicians);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.title_id = new SelectList(db.titles, "id", "degree", academicians.title_id);
            return View(academicians);
        }

        // GET: academicians/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            academicians academicians = db.academicians.Find(id);
            if (academicians == null)
            {
                return HttpNotFound();
            }
            ViewBag.title_id = new SelectList(db.titles, "id", "degree", academicians.title_id);
            return View(academicians);
        }

        // POST: academicians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,surname,title_id")] academicians academicians)
        {
            if (ModelState.IsValid)
            {
                db.Entry(academicians).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.title_id = new SelectList(db.titles, "id", "degree", academicians.title_id);
            return View(academicians);
        }

        // GET: academicians/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            academicians academicians = db.academicians.Find(id);
            if (academicians == null)
            {
                return HttpNotFound();
            }
            return View(academicians);
        }

        // POST: academicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            academicians academicians = db.academicians.Find(id);
            db.academicians.Remove(academicians);
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
