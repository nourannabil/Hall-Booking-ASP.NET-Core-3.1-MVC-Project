using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace First_Project2.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AboutPage> AboutPages { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<CategoryHall> CategoryHalls { get; set; }
        public virtual DbSet<ContactUsPage> ContactUsPages { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<Hall> Halls { get; set; }
        public virtual DbSet<HallBooking> HallBookings { get; set; }
        public virtual DbSet<HallPhoto> HallPhotos { get; set; }
        public virtual DbSet<HomePage> HomePages { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Statistic> Statistics { get; set; }
        public virtual DbSet<TestimonialPage> TestimonialPages { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("USER ID=JOR15_User42;PASSWORD=nourannabil12;DATA SOURCE=94.56.229.181:3488/traindb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("JOR15_USER42")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<AboutPage>(entity =>
            {
                entity.ToTable("ABOUT_PAGE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Description1)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION1");

                entity.Property(e => e.Description2)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION2");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.HomeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HOME_ID");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Title)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.AboutPages)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("ABOUT_FK2");

                entity.HasOne(d => d.Home)
                    .WithMany(p => p.AboutPages)
                    .HasForeignKey(d => d.HomeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("ABOUT_FK1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AboutPages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("ABOUT_FK3");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("CARD");

                entity.HasIndex(e => e.Cvv, "CARD_UQ")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Balance)
                    .HasColumnType("FLOAT")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.CardNumber)
                    .HasMaxLength(19)
                    .IsUnicode(false)
                    .HasColumnName("CARD_NUMBER");

                entity.Property(e => e.CardType)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("CARD_TYPE");

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CVV")
                    .IsFixedLength(true);

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.NameOnCard)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME_ON_CARD");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("CARD_FK1");
            });

            modelBuilder.Entity<CategoryHall>(entity =>
            {
                entity.ToTable("CATEGORY_HALL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORY_NAME");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");
            });

            modelBuilder.Entity<ContactUsPage>(entity =>
            {
                entity.ToTable("CONTACT_US_PAGE");

                entity.HasIndex(e => e.PhoneNumber, "CONTACT_UQ1")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "CONTACT_UQ2")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Description1)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION1");

                entity.Property(e => e.Description2)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION2");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Fname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FNAME");

                entity.Property(e => e.HomeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HOME_ID");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Lname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LNAME");

                entity.Property(e => e.Map)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("MAP");

                entity.Property(e => e.Message)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MESSAGE");

                entity.Property(e => e.PhoneNumber)
                    .HasPrecision(10)
                    .HasColumnName("PHONE_NUMBER");

                entity.Property(e => e.Title)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Home)
                    .WithMany(p => p.ContactUsPages)
                    .HasForeignKey(d => d.HomeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("CONTACT_FK1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ContactUsPages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("CONTACT_FK2");
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.ToTable("FEATURE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.Feat)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("FEAT");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.PhotoId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PHOTO_ID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Features)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FEATURE_FK1");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Features)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FEATURE_FK2");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.Features)
                    .HasForeignKey(d => d.PhotoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FEATURE_FK3");
            });

            modelBuilder.Entity<Hall>(entity =>
            {
                entity.ToTable("HALL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Capacity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CAPACITY");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Interval1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("INTERVAL_1");

                entity.Property(e => e.Interval2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("INTERVAL_2");

                entity.Property(e => e.Interval3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("INTERVAL_3");

                entity.Property(e => e.Interval4)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("INTERVAL_4");

                entity.Property(e => e.Location)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.Map)
                    .HasMaxLength(3000)
                    .IsUnicode(false)
                    .HasColumnName("MAP");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Price)
                    .HasColumnType("FLOAT")
                    .HasColumnName("PRICE");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Halls)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("HALL_FK");
            });

            modelBuilder.Entity<HallBooking>(entity =>
            {
                entity.ToTable("HALL_BOOKING");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BOOKING_DATE");

                entity.Property(e => e.BookingTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BOOKING_TIME");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.HallBookings)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("BOOKING_FK1");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.HallBookings)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("BOOKING_FK2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HallBookings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("BOOKING_FK3");
            });

            modelBuilder.Entity<HallPhoto>(entity =>
            {
                entity.ToTable("HALL_PHOTO");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.HallPhotos)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("HALL_PHOTO_FK1");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.HallPhotos)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("HALL_PHOTO_FK2");
            });

            modelBuilder.Entity<HomePage>(entity =>
            {
                entity.ToTable("HOME_PAGE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Description1)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION1");

                entity.Property(e => e.Description2)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION2");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Title)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("LOGIN");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.RoleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("USER_NAME");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("LOGIN_FK1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("LOGIN_FK2");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("PAYMENT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.BookingId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOOKING_ID");

                entity.Property(e => e.CardId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CARD_ID");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.PayDate)
                    .HasPrecision(6)
                    .HasColumnName("PAY_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("PAYMENT_FK3");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("PAYMENT_FK1");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("PAYMENT_FK2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("PAYMENT_FK4");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("REPORT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.BookingId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOOKING_ID");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.PayId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAY_ID");

                entity.Property(e => e.ReportName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("REPORT_NAME");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REPORT_FK3");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REPORT_FK1");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REPORT_FK2");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.PayId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REPORT_FK4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REPORT_FK5");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("REQUEST");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.BookingId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOOKING_ID");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.HallId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HALL_ID");

                entity.Property(e => e.PayId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAY_ID");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REQUEST_FK3");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REQUEST_FK1");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REQUEST_FK2");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.PayId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REQUEST_FK4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("REQUEST_FK5");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLES");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Rolename)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ROLENAME");
            });

            modelBuilder.Entity<Statistic>(entity =>
            {
                entity.ToTable("STATISTIC");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.BookingId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOOKING_ID");

                entity.Property(e => e.EndDay)
                    .HasColumnType("DATE")
                    .HasColumnName("END_DAY");

                entity.Property(e => e.Expenses)
                    .HasColumnType("FLOAT")
                    .HasColumnName("EXPENSES");

                entity.Property(e => e.PayId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAY_ID");

                entity.Property(e => e.Revenues)
                    .HasColumnType("FLOAT")
                    .HasColumnName("REVENUES");

                entity.Property(e => e.StartDay)
                    .HasColumnType("DATE")
                    .HasColumnName("START_DAY");

                entity.Property(e => e.TotBookingNum)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOT_BOOKING_NUM");

                entity.Property(e => e.TotUsersRegisteredNum)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOT_USERS_REGISTERED_NUM");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Statistics)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("STATISTIC_FK1");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.Statistics)
                    .HasForeignKey(d => d.PayId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("STATISTIC_FK2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Statistics)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("STATISTIC_FK3");
            });

            modelBuilder.Entity<TestimonialPage>(entity =>
            {
                entity.ToTable("TESTIMONIAL_PAGE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.HomeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HOME_ID");

                entity.Property(e => e.Opinion)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("OPINION");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Home)
                    .WithMany(p => p.TestimonialPages)
                    .HasForeignKey(d => d.HomeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("TESTIMONIAL_FK1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TestimonialPages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("TESTIMONIAL_FK3");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("USER_INFO");

                entity.HasIndex(e => e.PhoneNumber, "USER_INFO_UQ1")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "USER_INFO_UQ2")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_OF_BIRTH");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Fname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FNAME");

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GENDER");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Lname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LNAME");

                entity.Property(e => e.PhoneNumber)
                    .HasPrecision(10)
                    .HasColumnName("PHONE_NUMBER");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
