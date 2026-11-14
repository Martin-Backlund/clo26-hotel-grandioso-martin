using HotelGrandioso.Data;
using HotelGrandioso.Models;
using HotelGrandioso.Services;
using  HotelGrandioso.UI;
using Microsoft.EntityFrameworkCore;
using var context = new HotelContext();

context.Database.Migrate();
DbHandler.Create(context);

IBookingRepository repository = new BookingRepository(context);
Console.WriteLine("=== Hotel Grandioso ===");
var manu = new Menu(repository);
manu.Run();