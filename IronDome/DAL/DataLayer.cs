using Microsoft.EntityFrameworkCore;
using IronDome.Models;
namespace IronDome.DAL
{
    public class DataLayer : DbContext
    {
        public DataLayer(string ConnectionString) : base(GetOptions(ConnectionString))
        {
            Database.EnsureCreated();
            Seed();
        }
        public DbSet<Missle> Missles { get; set; }
        public DbSet<TerrorOrg> TerrorOrgs { get; set; }
        public DbSet<DefenceAmmunition> DefenceAmmunitions { get; set; }
        public DbSet<Threat> Threats { get; set; }

        private void Seed()
        {
            if (!Missles.Any())
            {
                Missle drone = new Missle { missle_type = "Drone", travel_speed = 300 };
                Missle rocket = new Missle { missle_type = "Rocket", travel_speed = 880 };
                Missle balistic = new Missle { missle_type = "BalisticMissle", travel_speed = 18000 };
                Missles.AddRange(rocket, drone, balistic);
                SaveChanges();
            }

            if (!TerrorOrgs.Any()) 
            {
                TerrorOrgs.AddRange
                (
                    new TerrorOrg { distance = 1600, location = "Iran", name = "Iran" },
                    new TerrorOrg { distance = 100, location = "Lebanon", name = "Hezbulla" },
                    new TerrorOrg { distance = 70, location = "Lebanon", name = "Hamas" },
                    new TerrorOrg { distance = 2377, location = "Yemen", name = "Huthis" }
                );
                SaveChanges();
             }

            if (!DefenceAmmunitions.Any()) 
            {
                DefenceAmmunition defence1 = new DefenceAmmunition { name = "Iron Dome Missle", amount = 100 };
                DefenceAmmunition defence2 = new DefenceAmmunition { name = "Patriot Missle", amount = 50 };
                DefenceAmmunitions.AddRange(defence1, defence2);
                SaveChanges();
            }

            if (!Threats.Any()) 
            {
                TerrorOrg? hamas = TerrorOrgs.FirstOrDefault(t => t.name == "Hamas");
                Missle? rocket1 = Missles.FirstOrDefault(m => m.missle_type == "Rocket");

                if (hamas != null && rocket1 != null) 
                    Threats.AddRange
                    (
                        new Threat
                        {
                            Org = hamas,
                            missle_type = rocket1
                        }
                    );
                
                SaveChanges();

            }
        }
        
        private static DbContextOptions GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;
        }

        
        
    }
}
