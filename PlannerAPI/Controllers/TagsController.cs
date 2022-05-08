using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlannerAPI.Models;

namespace PlannerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TagsController : ControllerBase
{
    // private readonly PlannerContext _db;
    private readonly ILogger<WeatherForecastController> _logger;

    // public TagsController(ILogger<WeatherForecastController> logger, PlannerContext db)
    // {
    //     _db = db;
    //     _logger = logger;
    // }

    // [HttpGet]
    // public async Task<IEnumerable<TagModel>> GetAll()
    // {
    //     long id = 1;
    //     
    //     // var account = await _db.Accounts.FindAsync(userId);
    //
    //     // var x = account.Areas;
    //     return Task<Enumerable>;
    // }
}