using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;
using HotelGrandioso.Data;
using HotelGrandioso.Models;
using Microsoft.EntityFrameworkCore;
namespace HotelGrandioso.Services
{
    //All databas kod här, resten av programmet pratar bara med interfacet
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelContext _context;
        public BookingRepository(HotelContext context)
        {
            _context = context;
        }
        public List<Room> GetAllRooms()
        {
            //Include för att RoomType inte ska bli null
            return _context.Rooms.Include(r => r.RoomType).OrderBy(r => r.RoomNumber).ToList();
        }
        public bool AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            //Databasen kan vara otillgänglig 
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Room? GetRoomByNumber(int roomNumber)
        {
            return _context.Rooms.Include(r => r.RoomType).FirstOrDefault(r => r.RoomNumber == roomNumber);
        }
        public List<Booking> GetBookings()
        {
            return _context.Bookings.Include(b => b.Guest).
                Include(b => b.Room).
                Where(b => b.CheckOut >= DateOnly.FromDateTime(DateTime.Today)).//visar bara bokningr som inte redan är utcheckade
                OrderBy(b => b.CheckIn).
                ToList();
        }
        public bool CancelBooking(int bookingId)
        {
            //Find söker på PrimaryKey och ger null om bokningen inte finns
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
                catch
                {
                    return false;
                }
            }
        }
        //Två bokningar krockar om den ena börjar innan den andra slutar
        // "<" istället för "<=" så att en gäst kan checka in samma dag som en annan gäst checkar ut
        public bool IsBooked(int roomId, DateOnly checkIn, DateOnly checkOut)
        {
            return _context.Bookings.Any(b => b.RoomId == roomId && b.CheckIn < checkOut && b.CheckOut > checkIn);

        }
        //Alla rum som inte är bokade på datumen
        public List<Room> GetFreeRooms(DateOnly checkIn, DateOnly checkOut)
        {
            return _context.Rooms.Include(r => r.RoomType).
                Where(r => !_context.Bookings.
                Any(b => b.RoomId == r.Id && b.CheckIn < checkOut && b.CheckOut > checkIn)).
                OrderBy(r => r.RoomNumber).ToList();
        }
        public List<Treatment> GetAllTreatments()
        {
            return _context.Treatments.ToList(); //ingen include då Treatment inte har någon koppling som ska laddas
        }
        public Treatment? GetTreatmentById(int id)
        {
            return _context.Treatments.Find(id);
        }
        public bool AddSpaBooking(SpaBooking spaBooking)
        {
            _context.SpaBookings.Add(spaBooking);
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
