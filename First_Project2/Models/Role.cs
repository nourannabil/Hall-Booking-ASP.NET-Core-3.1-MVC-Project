using System;
using System.Collections.Generic;

#nullable disable

namespace First_Project2.Models
{
    public partial class Role
    {
        public Role()
        {
            Logins = new HashSet<Login>();
        }

        public decimal Id { get; set; }
        public string Rolename { get; set; }

        public virtual ICollection<Login> Logins { get; set; }
    }
}
