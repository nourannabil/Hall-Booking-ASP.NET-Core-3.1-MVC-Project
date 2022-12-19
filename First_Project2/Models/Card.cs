using System;
using System.Collections.Generic;

#nullable disable

namespace First_Project2.Models
{
    public partial class Card
    {
        public Card()
        {
            Payments = new HashSet<Payment>();
        }

        public decimal Id { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public string Cvv { get; set; }
        public decimal Balance { get; set; }
        public string CardType { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? UserId { get; set; }

        public virtual UserInfo User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
