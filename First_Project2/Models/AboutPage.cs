using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace First_Project2.Models
{
    public partial class AboutPage
    {
        public decimal Id { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public decimal? HomeId { get; set; }
        public decimal? HallId { get; set; }
        public decimal? UserId { get; set; }

        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual HomePage Home { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
