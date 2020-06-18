using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Co_nnecto.Models;

namespace Co_nnecto.Controllers
{
    public class StudentController : ApplicationBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Student
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        [Authorize(Roles="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,MiddleName,LastName,Parents,Teachers")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Student/Edit/5
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,MiddleName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Delete/5
        [Authorize(Roles="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles="Admin")]
        public ActionResult AddParents()
        {
            var parentId = db.Roles.Where(x => x.Name.Equals("Parent")).Select(y => y.Id).FirstOrDefault();
            var parentUsers = db.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(parentId))).ToList();
            ViewBag.Parents = new SelectList(parentUsers, "UserName", "UserName");

            return View();

        }

        [HttpPost]
        public ActionResult AddParents(int? id, string parents)
        {
            Student student = db.Students.Find(id);
            student.ID = (int)id;
            var parentId = db.Users.Where(u => u.UserName == parents).Select(p => p.Id).FirstOrDefault();
            var parent = db.Users.Find(parentId);
            if (!student.Parents.Contains(parent) || student.Parents == null)
            {
                student.Parents.Add(parent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [Authorize(Roles="Admin,Teacher")]

        public ActionResult StudentParents(int? id)
        {
            Student student = db.Students.Find(id);
            student.ID = (int)id;
            var stParents = db.Students.Where(s => s.ID == id).SelectMany(p => p.Parents).ToList();
            ViewBag.StParents = stParents.ToList();
            ViewBag.Title = student.FirstName + " " + student.LastName;
            return View(stParents);
        }
        
        [Authorize(Roles="Admin")]
        public ActionResult AddTeachers()
        {
            var teacherId = db.Roles.Where(x => x.Name.Equals("Teacher")).Select(y => y.Id).FirstOrDefault();
            var teacherUsers = db.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(teacherId))).ToList();
            ViewBag.Teachers = new SelectList(teacherUsers, "UserName", "UserName");

            return View();

        }

        [HttpPost]
        public ActionResult AddTeachers(int? id, string teachers)
        {
            Student student = db.Students.Find(id);
            student.ID = (int)id;
            var teacherId = db.Users.Where(u => u.UserName == teachers).Select(p => p.Id).FirstOrDefault();
            var teacher = db.Users.Find(teacherId);
            if (!student.Teachers.Contains(teacher) || student.Teachers == null)///????
            {
                student.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [Authorize(Roles="Admin,Parent")]
        public ActionResult StudentTeachers(int? id)
        {
            Student student = db.Students.Find(id);
            student.ID = (int)id;
            var stTeachers = db.Students.Where(s => s.ID == id).SelectMany(p => p.Teachers).ToList();
            ViewBag.StTeachers = stTeachers.ToList();
            ViewBag.Title = student.FirstName + " " + student.LastName;
            return View(stTeachers);
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