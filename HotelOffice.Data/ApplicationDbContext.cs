using System.Collections.Generic;
using System.Reflection.Emit;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelOffice.Data
{
    public class ApplicationDbContext : DbContext
    {
        // هذا الكونستركتور ضروري لعمل حقن التبعية (Dependency Injection)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           
        }

        // كل DbSet يمثل جدولاً في قاعدة البيانات
        public DbSet<Floor> Floors { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Setting> Settings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تعريف العلاقة كثير-إلى-كثير بشكل صريح
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Amenities)
                .WithMany(a => a.Rooms)
                .UsingEntity(j => j.ToTable("AmenityRoom")); // سيتم إنشاء جدول وسيط بهذا الاسم
        }
    }
}
