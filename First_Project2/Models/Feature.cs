using System;
using System.Collections.Generic;

#nullable disable

namespace First_Project2.Models
{
    public partial class Feature
    {
        public decimal Id { get; set; }
        public string Feat { get; set; }
        public decimal? CategoryId { get; set; }
        public decimal? HallId { get; set; }
        public decimal? PhotoId { get; set; }

        public virtual CategoryHall Category { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual HallPhoto Photo { get; set; }
    }
}
