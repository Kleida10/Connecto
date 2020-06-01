using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Co_nnecto.Models
{
    public class Student
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public int ID { get; set; }
        public int ParentsID { get; set; }
        public List<ApplicationUser> Parents { get; set; }
        public int TeachersID { get; set; }
        public List<ApplicationUser> Teachers { get; set; }

        public Student()
        {
            Parents = new List<ApplicationUser>();
            Teachers = new List<ApplicationUser>();
        }

    }
}