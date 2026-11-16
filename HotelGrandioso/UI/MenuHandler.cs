using HotelGrandioso.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace HotelGrandioso.UI
{
    public class MenuHandler
    {
        public void PrintRoomBookings(List<Booking> bookings)
        {
            foreach (var b in bookings)
            {
                Console.WriteLine($"ID: {b.Id} - {b.Guest.Name} - Rum: {b.Room.RoomNumber} - {b.CheckIn} - {b.CheckOut}");
            }
        }
        public void PrintAllRooms(List<Room> rooms)
        {
            foreach (var r in rooms)
            {
                Console.WriteLine($"{r.RoomNumber} - {r.RoomType.Name} Pris/natt: {r.PricePerNight:C}");
            }
        }
        public DateOnly? ReadDate(string prompt)
        {
            Console.Write($"{prompt} (YYYY-MM-DD): "); var input = Console.ReadLine();
            if (!DateOnly.TryParse(input, out DateOnly date))
            {
                return null;
            }
            else
            {
                return date;
            }
        }
        public bool IsDateValid(DateOnly inCheck, DateOnly outCheck) 
        {
            //minst en natt krävs, kan inte checka ut innan man checkat in
            if ((inCheck >= outCheck))
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
        public bool DayHasPassed(DateOnly date)
        {
            if (date < DateOnly.FromDateTime(DateTime.Today))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void PrintFreeRooms(List<Room> freeRooms)
        {
            foreach (var r in freeRooms)
            {
                Console.WriteLine($"{r.RoomNumber} - {r.RoomType.Name} Pris/natt: {r.PricePerNight:C}");
            }
        }
        public string? ReadGuestName()
        {
            Console.Write("Gästnamn: "); var name = Console.ReadLine();
            //vakt som avbryter så ogitlig data inte sparas
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            else
            {
                return name;
            }
        }
    }
}
