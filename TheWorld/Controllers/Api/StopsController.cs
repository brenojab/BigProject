using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
  [Route("/api/trips/{tripName}/stops")]
  public class StopsController : Controller
  {
    private ILogger<StopsController> _logger;
    private IWorldRepository _repository;
    private GeoCoordsService _coordService;

    public StopsController(IWorldRepository repository, ILogger<StopsController> logger, GeoCoordsService coordResult)
    {
      _logger = logger;
      _repository = repository;
      _coordService = coordResult;

    }

    //[HttpGet("/api/trips/{tripName}/stops")]
    public IActionResult Get(string tripName)
    {
      try
      {
        var trip = _repository.GetTripByName(tripName);

        return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
      }
      catch (Exception ex)
      {

        _logger.LogError($"Failed get stops: {ex.Message}");

      }

      return BadRequest("Bad...");
    }

    [HttpPost("")]
    public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel stopVM)
    {
      try
      {
        // Se a VM é válida
        if (ModelState.IsValid)
        {
          var newStop = Mapper.Map<Stop>(stopVM);

          // Lookup as geolocalizações
          var result = await _coordService.GetCoordAsync(newStop.Name);

          if (!result.Success)
          {
            _logger.LogError(result.Message);
          }
          else
          {

            newStop.Longitude = result.Longitude;
            newStop.Latitude = result.Latitude;
          // salvar
          _repository.AddStop(tripName, newStop);

          if (await _repository.SaveChangesAsync())
          {
            return Created($"/api/trips/{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
          }
          }
        }
      }
      catch (Exception ex)
      {

        _logger.LogError("Faild...", ex);
      }
      return BadRequest("bad request");

    }
  }
}
