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
        private readonly MenuHandler _menuHandler;
        public Menu(IBookingRepository repository, MenuHandler menuHandler)
        {
            _repository = repository;
            _menuHandler = menuHandler;
        }
        private void BookRoom()
        {
            var name = _menuHandler.ReadGuestName();
            if (name == null)
            {
                Console.WriteLine("Ogiltigt namn");
                return;
            }
            var checkIn = _menuHandler.ReadDate("Skriv in inchecknings-datum");
            if (checkIn == null)
            {
                Console.WriteLine("Ogiltigt datum");
                return;
            }
            if (_menuHandler.DayHasPassed(checkIn.Value))
            {
                Console.WriteLine("Datumet har redan passerat");
                return;
            }
            var checkOut = _menuHandler.ReadDate("Skriv in utchecknings-datum");
            if (checkOut == null)
            {
                Console.WriteLine("Ogiltigt datum");
                return;
            }
            if (!_menuHandler.IsDateValid(checkIn.Value, checkOut.Value))
            {
                Console.WriteLine("Ogiltigt datum");
                return;
            }

            var freeRooms = _repository.GetFreeRooms(checkIn.Value, checkOut.Value);
            if (freeRooms.Count == 0)
            {
                Console.WriteLine($"Finns inga lediga rum: {checkIn} - {checkOut}");
                return;
            }
            _menuHandler.PrintFreeRooms(freeRooms);
            Console.Write($"Välj rum (Rumsnummer): ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int roomNumber))
            {
                Console.WriteLine("Skriv endast nummer");
                return;
            }
            //användaren skriver in rummsnummer men databasen kräver rummets id
            var room = _repository.GetRoomByNumber(roomNumber);
            if (room == null)
            {
                Console.WriteLine("Rummet finns inte");
                return;
            }
            if (_repository.IsBooked(room.Id, checkIn.Value, checkOut.Value))
            {
                Console.WriteLine($"Rummet är tyvärr redan bokat dessa datum {checkIn} - {checkOut}");
                return;
            }
            var booking = new Booking()
            {
                Guest = new Guest { Name = name },
                Room = room,
                CheckIn = checkIn.Value,
                CheckOut = checkOut.Value
            };
            if (_repository.AddBooking(booking))
            {
                Console.WriteLine($"Bokning bekräftad! Totalt pris: {booking.TotalPrice():C}\n{booking.Nights()} nätter x {room.PricePerNight:C}");
            }
            else
            {
                Console.WriteLine("Bokningen kunde inte genomföras");
            }
        }
        private void ShowBookings()
        {
            var bookings = _repository.GetBookings();
            if (bookings.Count == 0)
            {
                Console.WriteLine("Inga bokningar");
                return;
            }
            else
            {
                _menuHandler.PrintRoomBookings(bookings);
            }
        }
        private void CancelBooking()
        {
            var bookings = _repository.GetBookings();
            if (bookings.Count == 0)
            {
                Console.WriteLine("Finns inga aktuella bokningar");
                return;
            }
            _menuHandler.PrintRoomBookings(bookings);

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
            var rooms = _repository.GetAllRooms();
            _menuHandler.PrintAllRooms(rooms);
        }
        private void ShowAllFreeRooms()
        {
            var inCheck = _menuHandler.ReadDate("Skriv in inchecknings-datum");
            if (inCheck == null)
            {
                Console.WriteLine("Ogiltigt datum");
                return;
            }
            if (_menuHandler.DayHasPassed(inCheck.Value))
            {
                Console.WriteLine("Datumet har redan passerat");
                return;
            }
            var outCheck = _menuHandler.ReadDate("Skriv in utchecknings-datum");
            if (outCheck == null)
            {
                Console.WriteLine("Ogiltigt datum");
                return;
            }
            if (!_menuHandler.IsDateValid(inCheck.Value, outCheck.Value))
            {
                Console.WriteLine("Ogiltigt datum");
                return;
            }

            var freeRooms = _repository.GetFreeRooms(inCheck.Value, outCheck.Value);

            if (freeRooms.Count == 0)
            {
                Console.WriteLine($"Inga lediga rum {inCheck} - {outCheck}");
                return;
            }

            Console.WriteLine("Lediga rum:");
            _menuHandler.PrintFreeRooms(freeRooms);
        }
        private void BookSpa()
        {
            var name = _menuHandler.ReadGuestName();
            if (name == null)
            {
                Console.WriteLine("Ogiltigt namn");
                return;
            }
            var date = _menuHandler.ReadDate("Skriv in datum");
            if (date == null)
            {
                Console.WriteLine("Ogiltigt datum");
                return;
            }

            if (_menuHandler.DayHasPassed(date.Value))
            {
                Console.WriteLine("Datumet har redan passerat");
                return;
            }
            var treatments = _repository.GetAllTreatments();
            foreach (var t in treatments)
            {
                Console.WriteLine($"Id: {t.Id}. {t.Name} - {t.DurationMinutes} min - {t.Price:C}");
            }
            Console.Write("Välj behandling (Id):"); var input2 = Console.ReadLine();
            if (!int.TryParse(input2, out int choice))
            {
                Console.WriteLine($"Ogiltigt val: {input2}");
                return;
            }
            var treatment = _repository.GetTreatmentById(choice);
            if (treatment == null)
            {
                Console.WriteLine("Ogiltigt val");
                return;
            }
            var spaBooking = new SpaBooking { Guest = new Guest { Name = name }, Date = date.Value, Treatment = treatment };
            if (_repository.AddSpaBooking(spaBooking))
            {
                Console.WriteLine("Bokning bekräftad!");
                Console.WriteLine($"{spaBooking.Guest.Name} - {spaBooking.Treatment.Name} Pris: {treatment.Price:C} - {spaBooking.Date} ");
                return;
            }
            else
            {
                Console.WriteLine($"Bokning misslyckades");
            }
        }

        public void StartMenu()
        {
            bool run = true;
            Console.WriteLine("=== Hotel Grandioso ===");
            while (run)
            {
                Console.WriteLine("1. Boka rum");
                Console.WriteLine("2. Avboka");
                Console.WriteLine("3. Visa bokningar");
                Console.WriteLine("4. Visa alla rum");
                Console.WriteLine("5. Visa lediga rum");
                Console.WriteLine("6. Boka spa");
                Console.WriteLine("7. Avsluta");
                Console.Write("Val:"); string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BookRoom();
                        break;
                    case "2":
                        CancelBooking();
                        break;
                    case "3":
                        ShowBookings();
                        break;
                    case "4":
                        ShowAllRooms();
                        break;
                    case "5":
                        ShowAllFreeRooms();
                        break;
                    case "6":
                        BookSpa();
                        break;
                    case "7":
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen");
                        break;
                }
            }
        }
    }
}
