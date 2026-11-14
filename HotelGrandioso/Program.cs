using HotelGrandioso.Data;
using HotelGrandioso.Models;
using HotelGrandioso.Services;
using Microsoft.EntityFrameworkCore;
using var context = new HotelContext();
context.Database.Migrate();
// TODO: Starta menyn (Boka rum, Avboka, Visa bokningar, Visa lediga rum, Avsluta)
DbHandler.Create(context);

Console.WriteLine("=== Hotel Grandioso ===");
IBookingRepository repository = new BookingRepository(context);

foreach (Room room in repository.GetAllRooms())
{
    Console.WriteLine($"{room.RoomNumber} - {room.RoomType.Name} {room.SizeM2}m2 {room.PricePerNight}kr/natt");
}