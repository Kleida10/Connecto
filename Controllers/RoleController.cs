// <copyright file="RoleController.cs" company="Kleida Haxhaj">
// Copyright (c) Kleida Haxhaj. All rights reserved.
// </copyright>

namespace Co_nnecto.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Co_nnecto.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class RoleController : ApplicationBaseController
    {
        ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Role
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }

        public ActionResult Create()
        {
            var role = new IdentityRole();
            return View(role);
        }

        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        [HttpPost]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                context.Entry(role).State = EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetUsers(string roleName)
        {
            var roleId = context.Roles.Where(x => x.Name.Equals(roleName)).Select(y => y.Id).FirstOrDefault();
            var users = context.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(roleId))).ToList();
            ViewBag.Role = roleName;
            ViewBag.Users = users;
            return View(users);
        }

        public ActionResult ParentsView()
        {
            var roleId = context.Roles.Where(x => x.Name.Equals("Teacher")).Select(y => y.Id).FirstOrDefault();
            var teachers = context.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(roleId))).ToList();
            ViewBag.Teachers = teachers;
            return View(teachers);
        }

        public ActionResult TeachersView()
        {
            var roleId = context.Roles.Where(x => x.Name.Equals("Parent")).Select(y => y.Id).FirstOrDefault();
            var parents = context.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(roleId))).ToList();
            ViewBag.Parents = parents;
            return View(parents);
        }
    }
}