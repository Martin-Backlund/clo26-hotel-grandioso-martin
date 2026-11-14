using HotelGrandioso.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGrandioso.Services
{
    public interface IBookingRepository
    {
        List<Room> GetAllRooms();
        bool AddBooking(Booking booking);
        Room? GetRoomByNumber(int roomNumber);
    }
}
