using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;
using HotelGrandioso.Data;
using HotelGrandioso.Models;
using Microsoft.EntityFrameworkCore;
namespace HotelGrandioso.Services
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelContext _context;
        public BookingRepository (HotelContext context)
        {
            _context = context;
        }
        public List<Room> GetAllRooms()
        {
            return _context.Rooms.Include(r => r.RoomType).ToList();
        }
        public bool AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Kunde inte genomföra bokningen pga: {ex.Message}");
                return false;
            }

        }
        public Room? GetRoomByNumber(int roomNumber)
        {
            return _context.Rooms.Include(r => r.RoomType).FirstOrDefault(r => r.RoomNumber == roomNumber);
        }
        public List <Booking> GetBooking()
        {
            return _context.Bookings.Include(b => b.Guest).
                Include(b => b.Room).
                Where(b=> b.CheckOut >= DateOnly.FromDateTime(DateTime.Today)).
                OrderBy(b => b.CheckIn).
                ToList();
        }
        public bool CancelBooking(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
            {
                return false;
            }
            else
            {
                _context.Bookings.Remove(booking);
                try
                {
                    _context.SaveChanges();
                    return true;
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine($"Något gick fel: {e.Message}");
                    return false;
                }
            }
        }
      
    }
}
