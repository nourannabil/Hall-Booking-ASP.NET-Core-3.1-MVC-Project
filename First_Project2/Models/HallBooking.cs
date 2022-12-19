using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace First_Project2.Models
{
    public partial class HallBooking
    {
        public HallBooking()
        {
            Payments = new HashSet<Payment>();
            Reports = new HashSet<Report>();
            Requests = new HashSet<Request>();
            Statistics = new HashSet<Statistic>();
        }

        public decimal Id { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy - MM - dd }")]

        public DateTime? BookingDate { get; set; }
        public string BookingTime { get; set; }
        public decimal? CategoryId { get; set; }
        public decimal? HallId { get; set; }
        public decimal? UserId { get; set; }

        public virtual CategoryHall Category { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual UserInfo User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Statistic> Statistics { get; set; }
    }
}
