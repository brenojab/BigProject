using Microsoft.EntityFrameworkCore;

class WorldContext : DbContext
{
 public WorldContext()
 {
     
 }   

 public DbSet<Trip> Trips { get; set; }
 public DbSet<Stop> Stops { get; set; }
}