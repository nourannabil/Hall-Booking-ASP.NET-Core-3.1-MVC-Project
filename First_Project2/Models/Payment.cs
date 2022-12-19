using System;
using System.Collections.Generic;

#nullable disable

namespace First_Project2.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Reports = new HashSet<Report>();
            Requests = new HashSet<Request>();
            Statistics = new HashSet<Statistic>();
        }

        public decimal Id { get; set; }
        public DateTime? PayDate { get; set; }
        public decimal? CardId { get; set; }
        public decimal? HallId { get; set; }
        public decimal? BookingId { get; set; }
        public decimal? UserId { get; set; }

        public virtual HallBooking Booking { get; set; }
        public virtual Card Card { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual UserInfo User { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Statistic> Statistics { get; set; }
    }
}
