using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
  [Route("api/trips")]
  public class TripsController : Controller
  {
    private IWorldRepository _repository;


    private ILogger _logger;

    //public TripsController(IWorldRepository repository)
    public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
    {
      _repository = repository;
      _logger = logger;
    }


    //[HttpGet("api/trips")]
    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        var results = _repository.GetAllTrips();

        //return Ok(_repository.GetAllTrips());
        return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to get trips: {ex.Message}");
      }
      return BadRequest("erro");

    }

    //[HttpPost("api/trips")]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody]TripViewModel tripVM)
    {
      //return Ok();

      // Salvar na base de dados

      var trip = Mapper.Map<Trip>(tripVM);

      _repository.AddTrip(trip);

      if (await _repository.SaveChangesAsync())
      {
        return Created($"api/trips/{tripVM.Name}", Mapper.Map<TripViewModel>(trip));
      }

      //if (ModelState.IsValid)
      //{
      //  //return Created($"api/trips/{tripVM.Name}", trip);
        
      //}

      return BadRequest("Failed Save");
    }


    ////[HttpPost("api/trips")]
    //[HttpPost]
    //public IActionResult Post([FromBody]Trip trip)
    //{
    //  //return Ok();

    //  if (ModelState.IsValid)
    //  {
    //    return Created($"api/trips/{trip.Name}", trip);
    //  }

    //  return BadRequest("bad data");
    //}


    //[HttpGet("api/trips")]
    //public JsonResult Get()
    //{
    //  return Json(new Trip() { Name = "mytrip" });
    //}




    //[HttpGet("api/trips")]
    //public IActionResult Get()
    //{
    //  if (true)
    //  {
    //    return BadRequest("Bad...");
    //  }

    //  return Ok(new Trip() { Name = "mytrip" });
    //}
  }
}
