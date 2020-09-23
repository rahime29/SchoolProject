using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class studentsController : Controller
    {
        private schooldbEntities db = new schooldbEntities();

        // GET: students
        public ActionResult Index()
        {
            return View(db.students.ToList());
        }

        // GET: students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = db.students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // GET: students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "no,name,surname")] students students)
        {
            if (ModelState.IsValid)
            {
                var student = new students();
                student.no = students.no;
                student.name = students.name;
                student.surname = students.surname;
                student.gano = 2.00;
                db.students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(students);
        }

        // GET: students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = db.students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "no,name,surname,gano")] students students)
        {
            if (ModelState.IsValid)
            {
                db.Entry(students).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(students);
        }

        // GET: students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = db.students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            students students = db.students.Find(id);
            db.students.Remove(students);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Subjects (int no)
        {
            var sub = db.students_subjects.Where(w => w.student_no == no).ToList();
            return View(sub);
        }

        public ActionResult UpdateNotes(int no, string code)
        {
            var year = DateTime.Now.Year.ToString();
            var response = db.students_subjects.Include(s => s.students).Include(sb => sb.subjects).Where(w => w.student_no == no && w.subjects_code == code && w.year == year).FirstOrDefault();
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateNotes([Bind(Include = "student_no,subjects_code,midterm,final,resit")] students_subjects sb)
        {
            var year = DateTime.Now.Year.ToString();
            var ss = db.students_subjects.Where(w => w.student_no == sb.student_no && w.subjects_code == sb.subjects_code && w.year == year ).FirstOrDefault();
            ss.midterm = sb.midterm;
            ss.final = sb.final==0?null:sb.final;
            ss.resit = sb.resit==0?null:sb.resit;
            if(ss.resit != null)
            {
                ss.avg= (ss.midterm * 0.4) + (ss.resit * 0.6);
            }
            else
            {
                if (ss.final != null)
                {
                    ss.avg = (ss.midterm * 0.4) + (ss.final * 0.6);   
                } 
            }
            db.SaveChanges();

            
            if(ss.avg != null)
            {
                var toplamKredi = 0;
                double agirlik = 0.0;
                var stsb = db.students_subjects.Where(w => w.student_no == sb.student_no && w.year == year);
                foreach(var item in stsb)
                {
                    toplamKredi += item.subjects.credit;
                    agirlik += NoteConverter(Convert.ToDouble(item.avg)) * Convert.ToDouble(item.subjects.credit);
                }
                var student = db.students.Find(sb.student_no);
                student.gano = agirlik / toplamKredi;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
        private double NoteConverter(double note)
        {
            if(note>=0 && note < 25)
            {
                return 0.0;
            }else if(note>=25 && note < 35)
            {
                return 0.5;
            }
            else if (note >= 25 && note < 35)
            {
                return 0.5;
            }
            else if (note >= 35 && note < 45)
            {
                return 1.0;
            }
            else if (note >= 45 && note < 50)
            {
                return 1.5;
            }
            else if (note >= 50 && note < 58)
            {
                return 2;
            }
            else if (note >= 58 && note < 65)
            {
                return 2.5;
            }
            else if (note >= 65 && note < 75)
            {
                return 3.0;
            }
            else if (note >= 75 && note < 85)
            {
                return 3.5;
            }
            else 
            {
                return 4.0;
            }
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
