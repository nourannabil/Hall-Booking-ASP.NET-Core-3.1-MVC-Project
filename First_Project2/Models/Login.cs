using System;
using System.Collections.Generic;

#nullable disable

namespace First_Project2.Models
{
    public partial class Login
    {
        public decimal Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal? RoleId { get; set; }
        public decimal? UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
