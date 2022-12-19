using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace First_Project2.Models
{
    public partial class CategoryHall
    {
        public CategoryHall()
        {
            Features = new HashSet<Feature>();
            HallBookings = new HashSet<HallBooking>();
            HallPhotos = new HashSet<HallPhoto>();
            Halls = new HashSet<Hall>();
            Reports = new HashSet<Report>();
            Requests = new HashSet<Request>();
        }

        public decimal Id { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }

        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public virtual ICollection<Feature> Features { get; set; }
        public virtual ICollection<HallBooking> HallBookings { get; set; }
        public virtual ICollection<HallPhoto> HallPhotos { get; set; }
        public virtual ICollection<Hall> Halls { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
