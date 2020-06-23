// <copyright file="IdentityModels.cs" company="Kleida Haxhaj">
// Copyright (c) Kleida Haxhaj. All rights reserved.
// </copyright>

namespace Co_nnecto.Models
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.EntityFrameworkCore;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Students = new HashSet<Student>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Student> StudentTeachers { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Student> Students { get; set; }

        public System.Data.Entity.DbSet<Message> Messages { get; set; }

        public System.Data.Entity.DbSet<Reply> Replies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Parents)
                .WithMany(c => c.Students)
                .Map(cs =>
                {
                    cs.MapLeftKey("StudentID");
                    cs.MapRightKey("ParentID");
                    cs.ToTable("StudentParent");
                });

            modelBuilder.Entity<Student>()
                .HasMany(t => t.Teachers)
                .WithMany(s => s.StudentTeachers)
                .Map(st =>
                {
                    st.MapLeftKey("StudentID");
                    st.MapRightKey("TeacherID");
                    st.ToTable("StudentTeacher");
                });
        }

        public ApplicationDbContext()
           : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
    }
}