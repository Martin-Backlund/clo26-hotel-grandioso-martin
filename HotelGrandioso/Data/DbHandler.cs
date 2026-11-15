using HotelGrandioso.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace HotelGrandioso.Data
{// en klass som skapar och kopplar samman rummen för att hålla program.cs kort
    public static class DbHandler
    {
        public static void CreateRooms(HotelContext context)
        {
            if (context.RoomTypes.Any()) return; //Skapa rummen en gång endast för att inte dubblera alla rum
            var standard = new RoomType { Name = "Standard", Description = "Enkelbädd, inget fönster" };
            var lyxsvit = new RoomType { Name = "Lyxsvit", Description = "Queensize säng med härlig natur-utsikt" };
            var brollopsvit = new RoomType { Name = "Bröllopsvit", Description = "Kingsize säng med fantastisk havsutsikt, balkong samt tillgång till privat bubbelpool" };

           for (int i = 101; i <=105;i++)
            {
                context.Rooms.Add(new Room { RoomNumber = i, Floor = 1, SizeM2 = 80, PricePerNight = 800, RoomType = standard });
            }
            for (int i = 201; i <= 205; i++)
            {
                context.Rooms.Add(new Room { RoomNumber = i, Floor = 2, SizeM2 = 100, PricePerNight = 1000, RoomType = standard });
            }
            for (int i = 301; i<=303; i++)
            {
                context.Rooms.Add(new Room { RoomNumber = i, Floor = 3, SizeM2 = 120, PricePerNight = 1200, RoomType = standard });
            }
            context.Rooms.Add(new Room { RoomNumber = 304, Floor = 3, SizeM2 = 180, PricePerNight = 2800, RoomType = lyxsvit });
            context.Rooms.Add(new Room { RoomNumber = 305, Floor = 3, SizeM2 = 220, PricePerNight = 3500, RoomType = brollopsvit });
            context.SaveChanges();
            
        }
        public static void CreateTreatments(HotelContext context)
        {
            if (context.Treatments.Any()) return;
            context.Treatments.Add(new Treatment { Name = "Entré (per timme)", DurationMinutes = 60, Price = 150 });
            context.Treatments.Add(new Treatment { Name = "Massage", DurationMinutes = 60, Price = 950 });
            context.Treatments.Add(new Treatment { Name = "Pedikyr (obs endast stortån)", DurationMinutes = 30, Price = 350 });
            context.Treatments.Add(new Treatment { Name = "Pedikyr (Hela foten)", DurationMinutes = 60, Price = 650 });
            context.SaveChanges();
        }
    }
}
