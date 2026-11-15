using HotelGrandioso.Data;
using HotelGrandioso.Models;
using HotelGrandioso.Services;
using HotelGrandioso.UI;
using Microsoft.EntityFrameworkCore;
using var context = new HotelContext();
//Skapar databasen och kör nya migrationer vid start
context.Database.Migrate();
DbHandler.CreateRooms(context);
DbHandler.CreateTreatments(context);

IBookingRepository repository = new BookingRepository(context);
Console.WriteLine("=== Hotel Grandioso ===");
var menu = new Menu(repository);
menu.Run();