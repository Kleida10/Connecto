using System;
using System.Collections.Generic;
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

        public ActionResult AddParents()
        {
            var parentId = db.Roles.Where(x => x.Name.Equals("Parent")).Select(y => y.Id).FirstOrDefault();
            var parentUsers = db.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(parentId))).ToList();
            ViewBag.Parents = new SelectList(parentUsers, "UserName", "UserName");

            return View();

        }

        [HttpPost]
        public ActionResult AddParents(int id, ApplicationUser user, StudentParent stParent)
        {
            Student student = db.Students.Find(id);
            var userId = db.Users.Where(u => u.UserName == user.UserName).Select(s => s.Id).FirstOrDefault();
            var parent = db.Users.Find(userId);
            var parents = db.StudentParents.Select(p => p.Parent).ToList();
            if (!parents.Contains(parent) || parents == null)
            {
                // parents.Add(parent);
                stParent = new StudentParent
                {
                    StudentID = id,
                    ParentID = userId,
                    Student = student,
                    Parent = parent
                };
                db.StudentParents.Add(stParent);
                db.SaveChanges();
                return RedirectToAction("StudentParents");

            }
            return RedirectToAction("Index");
        }

        public ActionResult StudentParents(int? id)
        {
            var student = db.StudentParents.Where(s => s.StudentID == id).FirstOrDefault();
            var parents = db.StudentParents.Where(s => s.StudentID == id).Select(p => p.Parent).ToList();

            ViewBag.Parents = parents;
            //ViewBag.Title = student.Student.FirstName + " " + student.Student.LastName;
            return View(parents);
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