using HotelGrandioso.Data;
using HotelGrandioso.Models;
using HotelGrandioso.Services;
using Microsoft.EntityFrameworkCore;
using var context = new HotelContext();
context.Database.Migrate();
// TODO: Starta menyn (Boka rum, Avboka, Visa bokningar, Visa lediga rum, Avsluta)
DbHandler.Create(context);

Console.WriteLine("=== Hotel Grandioso ===");
