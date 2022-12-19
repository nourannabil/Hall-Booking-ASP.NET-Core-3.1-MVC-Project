using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace First_Project2.Models
{
    public partial class TestimonialPage
    {
        public decimal Id { get; set; }
        public string Opinion { get; set; }
        public decimal? HomeId { get; set; }
        public decimal? UserId { get; set; }

        [NotMapped]
        public string Status { get; set; }

        public virtual HomePage Home { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
