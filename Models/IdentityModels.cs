using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Co_nnecto.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<StudentParent> StudentParents { get; set; }

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
        public System.Data.Entity.DbSet<StudentParent> StudentParents { get; set; }

        protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentParent>()
                .HasKey(c => new { c.StudentID, c.ParentID });
            
            modelBuilder.Entity<StudentParent>()
        .HasOne(sp => sp.Student)
        .WithMany(s => s.StudentParents)
        .HasForeignKey(sp => sp.StudentID);
            
            modelBuilder.Entity<StudentParent>()
                .HasOne(sp => sp.Parent)
                .WithMany(p => p.StudentParents)
                .HasForeignKey(sp => sp.ParentID);
        }
        public ApplicationDbContext()
           : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


    }
}