using System;
using System.Collections.Generic;
using System.Text;
using HotelGrandioso.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelGrandioso.Data
{
     public class HotelContext : DbContext
    {
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<Guest> Guests => Set<Guest>();
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Connectionstring hämtas från miljövariabel, aldrig i koden
            string? connectionString = Environment.GetEnvironmentVariable("HOTEL_DB");

            // Vakt: stanna tidigt om den saknas
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Miljövariabeln HOTEL_DB saknas.");
            }

            options.UseSqlite(connectionString);
        }

    }
}
