using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
  public class WorldRepository : IWorldRepository
  {
    private WorldContext _context;
    private ILogger<WorldRepository> _logger;

    public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<bool> SaveChangesAsync()
    {
      return (await _context.SaveChangesAsync()) > 0;
    }

    public void AddTrip(Trip trip)
    {
      _context.Add(trip);
    }

    public IEnumerable<Trip> GetAllTrips()
    {
      _logger.LogInformation("Getting All Tripps from the DataBase");

      return _context.Trips.ToList();
    }

    public Trip GetTripByName(string tripName)
    {
      // Permite adicionar acrregar a coleção de Stops quando retornada.
      return _context.Trips.Include(t => t.Stops).Where(n => n.Name == tripName).FirstOrDefault();

      //return _context.Trips.Where(n => n.Name == tripName);
    }
  }
}
