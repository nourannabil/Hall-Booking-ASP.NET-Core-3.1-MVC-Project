using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace First_Project2.Models
{
    public partial class Request
    {
        public decimal Id { get; set; }
        public string Status { get; set; }
        public decimal? CategoryId { get; set; }
        public decimal? HallId { get; set; }
        public decimal? BookingId { get; set; }
        public decimal? PayId { get; set; }
        public decimal? UserId { get; set; }

        public virtual HallBooking Booking { get; set; }
        public virtual CategoryHall Category { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual Payment Pay { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
