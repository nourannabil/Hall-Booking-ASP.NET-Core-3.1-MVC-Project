using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace First_Project2.Models
{
    public partial class HomePage
    {
        public HomePage()
        {
            AboutPages = new HashSet<AboutPage>();
            ContactUsPages = new HashSet<ContactUsPage>();
            TestimonialPages = new HashSet<TestimonialPage>();
        }

        public decimal Id { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }

        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }

        public virtual ICollection<AboutPage> AboutPages { get; set; }
        public virtual ICollection<ContactUsPage> ContactUsPages { get; set; }
        public virtual ICollection<TestimonialPage> TestimonialPages { get; set; }
    }
}
