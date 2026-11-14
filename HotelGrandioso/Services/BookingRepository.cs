using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
