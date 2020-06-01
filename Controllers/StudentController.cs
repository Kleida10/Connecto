using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Co_nnecto.Models;

namespace Co_nnecto.Controllers
{
    public class StudentController : ApplicationBaseController
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Student
        public ActionResult Index()
        {
            return View(context.Students.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = context.Students.Find(id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,MiddleName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                context.Students.Add(student);
                context.SaveChanges();
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
            Student student = context.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,MiddleName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                context.Entry(student).State = EntityState.Modified;
                context.SaveChanges();
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
            Student student = context.Students.Find(id);
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
            Student student = context.Students.Find(id);
            context.Students.Remove(student);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddParents()
        {
            var parentId = context.Roles.Where(x => x.Name.Equals("Parent")).Select(y => y.Id).FirstOrDefault();
            var parents = context.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(parentId))).ToList();
            ViewBag.Users = new SelectList(parents, "UserName", "UserName");
            return View();
           
        }

        [HttpPost]
        public ActionResult AddParents(int? id, ApplicationUser user)
        {
            Student student = context.Students.Find(id);

            //var parents = context.Students.Select(s => s.Parents).ToList();
            var studentParent = context.Users.Where(s => s.UserName == user.UserName).FirstOrDefault();
            //var studentParent = userId.
             student.Parents.Add(studentParent);           
            var parents = student.Parents.ToList();//??????

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult AddTeachers()
        {
            var teacherRole = context.Roles.Where(x => x.Name.Equals("Teacher")).Select(y => y.Id).FirstOrDefault();
            var teachers = context.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(teacherRole))).ToList();
            ViewBag.Users =new SelectList(teachers, "UserName", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult AddTeachers(Student student, ApplicationUser user)
        {
            //var teachers = student.Teachers.ToList();//????
            //var userId = context.Users.Where(i => i.UserName == user.UserName).FirstOrDefault();
            //teachers.Add(userId);

            return Redirect("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
