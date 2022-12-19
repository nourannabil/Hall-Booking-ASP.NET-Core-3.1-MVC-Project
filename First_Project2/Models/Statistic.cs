using System;
using System.Collections.Generic;

#nullable disable

namespace First_Project2.Models
{
    public partial class Statistic
    {
        public decimal Id { get; set; }
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }
        public decimal? TotBookingNum { get; set; }
        public decimal? TotUsersRegisteredNum { get; set; }
        public decimal Revenues { get; set; }
        public decimal Expenses { get; set; }
        public decimal? BookingId { get; set; }
        public decimal? PayId { get; set; }
        public decimal? UserId { get; set; }

        public virtual HallBooking Booking { get; set; }
        public virtual Payment Pay { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
