using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace First_Project2.Models
{
    public partial class HallPhoto
    {
        public HallPhoto()
        {
            Features = new HashSet<Feature>();
        }

        public decimal Id { get; set; }
        public string ImagePath { get; set; }
        public decimal? CategoryId { get; set; }
        public decimal? HallId { get; set; }

        [NotMapped]
        public virtual List<IFormFile> ImageFile { get; set; }

        public virtual CategoryHall Category { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
    }
}
