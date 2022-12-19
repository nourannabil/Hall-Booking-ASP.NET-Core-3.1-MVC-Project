using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace First_Project2.Models
{
    public partial class Hall
    {
        public Hall()
        {
            AboutPages = new HashSet<AboutPage>();
            Features = new HashSet<Feature>();
            HallBookings = new HashSet<HallBooking>();
            HallPhotos = new HashSet<HallPhoto>();
            Payments = new HashSet<Payment>();
            Reports = new HashSet<Report>();
            Requests = new HashSet<Request>();
        }

        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Capacity { get; set; }
        public decimal Price { get; set; }
        public string Interval1 { get; set; }
        public string Interval2 { get; set; }
        public string Interval3 { get; set; }
        public string Interval4 { get; set; }
        public string Location { get; set; }
        public string Map { get; set; }
        public string ImagePath { get; set; }
        public decimal? CategoryId { get; set; }

        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public virtual CategoryHall Category { get; set; }
        public virtual ICollection<AboutPage> AboutPages { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
        public virtual ICollection<HallBooking> HallBookings { get; set; }
        public virtual ICollection<HallPhoto> HallPhotos { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
