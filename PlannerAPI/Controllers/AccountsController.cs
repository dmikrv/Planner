using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using PlannerAPI.Entities;
using PlannerAPI.Models;

namespace PlannerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    // private readonly PlannerContext _db;
    private readonly ILogger<AccountsController> _logger;

    // public AccountsController(ILogger<AccountsController> logger, PlannerContext db)
    // {
    //     _db = db;
    //     _logger = logger;
    // }

    // [HttpPost]
    // public async Task<AccountModel> Create(AccountModel account)
    // {
    //     // var x = await _db.Accounts.FindAsync(1);
    //     
    //     // _db.Areas.Add(new Area {})
    //
    //     // _db.Accounts.Add(account);
    //     // await _db.SaveChangesAsync();
    //     // return CreatedAtAction(nameof(GetAsync), new { id = user.Id }, user);
    // }

    // [HttpGet]
    // public async Task<IEnumerable<TagModel>> GetAll()
    // {
    //     // long id = 1;
    //     //
    //     // var account = await _db.Accounts.FindAsync(userId);
    //     //
    //     // var x = account.Areas;
    // }
}