namespace First_Project2.Models
{
    public class JoinTable
    {
        public Hall hall { get; set; }
        public CategoryHall categoryHall { get; set; }
        public HallBooking hallBooking { get; set; }
        public Request request { get; set; }
        public UserInfo userInfo { get; set; }
    }
}
