using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace First_Project2.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            AboutPages = new HashSet<AboutPage>();
            Cards = new HashSet<Card>();
            ContactUsPages = new HashSet<ContactUsPage>();
            HallBookings = new HashSet<HallBooking>();
            Logins = new HashSet<Login>();
            Payments = new HashSet<Payment>();
            Reports = new HashSet<Report>();
            Requests = new HashSet<Request>();
            Statistics = new HashSet<Statistic>();
            TestimonialPages = new HashSet<TestimonialPage>();
        }

        public decimal Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public int? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImagePath { get; set; }

        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public virtual ICollection<AboutPage> AboutPages { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<ContactUsPage> ContactUsPages { get; set; }
        public virtual ICollection<HallBooking> HallBookings { get; set; }
        public virtual ICollection<Login> Logins { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Statistic> Statistics { get; set; }
        public virtual ICollection<TestimonialPage> TestimonialPages { get; set; }
    }
}
