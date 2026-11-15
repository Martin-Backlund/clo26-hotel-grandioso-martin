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
        List<Booking> GetBooking();
        bool CancelBooking(int bookingId);

        bool IsBooked(int roomId, DateOnly checkIn, DateOnly checkOut);
    }
}
