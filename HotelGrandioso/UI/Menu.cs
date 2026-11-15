using HotelGrandioso.Models;
using HotelGrandioso.Services;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HotelGrandioso.UI
{
    public class Menu
    {
        private readonly IBookingRepository _repository;
        public Menu (IBookingRepository repository)
        {
            _repository = repository;
        }
        private void BookRoom()
        {
            Console.Write("Gästnamn: "); var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Bokningen kräver ett namn");
                return;
            }
            Console.Write("Inchecknings datum (YYYY-MM-DD): "); var input = Console.ReadLine();
            if (!DateOnly.TryParse(input, out DateOnly checkIn))
            {
                Console.WriteLine("Ogiltigt datum"); return;
            }
            Console.Write("Utchecknings datum (YYYY-MM-DD): "); var input2 = Console.ReadLine();
            if (!DateOnly.TryParse(input2, out DateOnly checkOut))
            {
                Console.WriteLine("Ogiltigt datum"); return;
            }
            else if (checkOut <= checkIn)
            {
                Console.WriteLine("Utcheckning måste vara efter Incheckning"); return;
            }
            foreach (var r in _repository.GetAllRooms())
            {
                Console.WriteLine($"{r.RoomNumber} - {r.RoomType.Name} Pris/natt: {r.PricePerNight:N0}");
            }
            Console.Write($"Välj rum: "); var input3 = Console.ReadLine();
            if (!int.TryParse(input3, out int roomNumber))
            {
                Console.WriteLine("Skriv endast nummer"); return;
            }
            var room = _repository.GetRoomByNumber(roomNumber);
            if (room == null)
            {
                Console.WriteLine("Rummet finns inte"); return;
            }
            if (_repository.IsBooked(room.Id, checkIn, checkOut))
            {
                Console.WriteLine($"Rummet är tyvärr redan bokat dessa datum {checkIn} - {checkOut}"); return;
            }
            var booking = new Booking()
            {
                Guest = new Guest { Name = name },
                Room = room,
                CheckIn = checkIn,
                CheckOut = checkOut
            };
            if (_repository.AddBooking(booking))
            {
                Console.WriteLine($"Bokning bekräftad! Totalt pris: {booking.TotalPrice():N0}kr\n{booking.Nights()} nätter x {room.PricePerNight:N0}kr");
            }
            else
            {
                Console.WriteLine("Bokningen kunde inte genomföras");
            }
        }
        private void ShowBookings()
        {
            var bookings = _repository.GetBooking();
            if (bookings.Count == 0)
            {
                Console.WriteLine("Inga bokningar");
                return;
            }
            else
            {
                foreach (var b in bookings)
                {
                    Console.WriteLine($"{b.Id} - {b.Guest.Name} - Rum {b.Room.RoomNumber} - {b.CheckIn} - {b.CheckOut}");
                }
            }
        }
        private void CancelBooking()
        {
           var bookings = _repository.GetBooking();
            if (bookings.Count == 0)
            {
                Console.WriteLine("Finns inga aktuella bokningar");
                return;
            }
            foreach (var b in bookings)
            {
                Console.WriteLine($"ID: {b.Id} - {b.Guest.Name} - Rum {b.Room.RoomNumber} - {b.CheckIn} - {b.CheckOut}");
            }
            Console.Write("Skriv in boknings-id för att avboka: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Skriv in endast nummer");
                return;
            }
           if (_repository.CancelBooking(id))
            {
                Console.WriteLine("Bokningen är avbokad!");
            }
           else
            {
                Console.WriteLine($"Hittade ingen bokning med id {id}");
            }
        }
        private void ShowAllRooms()
        {
            foreach (var r in _repository.GetAllRooms())
            {
                Console.WriteLine($"{r.RoomNumber} - {r.RoomType.Name} Pris/natt: {r.PricePerNight:N0}");
            }
        }
        public void ShowAllFreeRooms()
        {
            
           
        }
        
        public void Run()
        {
            bool run = true;
            while (run)
            {
                Console.WriteLine("1. Boka rum");
                Console.WriteLine("2. Avboka");
                Console.WriteLine("3. Visa bokningar");
                Console.WriteLine("4. Visa alla rum");
                Console.WriteLine("5. Visa lediga rum") ;
                Console.WriteLine("6. Boka spa") ;
                Console.WriteLine("7. Avsluta") ;
                Console.Write("Val:"); string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    BookRoom();
                }
                else if (choice == "2")
                {
                    CancelBooking();
                }
                else if (choice == "3")
                {
                    ShowBookings();
                }
                else if (choice == "4")
                {
                    ShowAllRooms();
                }
                else if (choice == "5")
                {
                    ShowAllFreeRooms();
                }
                else if (choice == "6")
                {

                }
                else if (choice == "7")
                {
                    run = false;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val, försök igen") ;
                }
            }
        }
    }
}
