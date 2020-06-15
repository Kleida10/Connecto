using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Co_nnecto.Models
{
    public class Student
    {
        public Student()
        {
            Parents = new HashSet<ApplicationUser>();
        }

        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public int ID { get; set; }
        //public ICollection<StudentParent> StudentParents { get; set; }
        public ICollection<ApplicationUser> Parents { get; set; }
    }
}