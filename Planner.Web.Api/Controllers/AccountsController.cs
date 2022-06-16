using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Planner.Data;
using Planner.Data.Entities;
using Planner.Web.Api.Models;
using Action = Planner.Data.Entities.Action;

namespace Planner.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly PlannerContext _db;
    private readonly ILogger<AccountsController> _logger;
    private readonly UserManager<Account> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountsController(ILogger<AccountsController> logger, PlannerContext db, UserManager<Account> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }
  
    [AllowAnonymous]
    [HttpPost]
    [Route("signup")]
    public async Task<ActionResult> SignupAsync(RegisterModel accountModel)
    {
        var search = await _userManager.FindByNameAsync(accountModel.Email);
        if (search is not null)
            return Conflict();

        var account = new Account { UserName = accountModel.Email, Email = accountModel.Email };
        var result = await _userManager.CreateAsync(account, accountModel.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        if (await _roleManager.RoleExistsAsync("user"))
            await _userManager.AddToRoleAsync(account, "user");

        var area1 = new Area {Name = "personal", Color = Color.Blue};
        var area2 = new Area {Name = "work", Color = Color.Green};
        account.Areas.AddRange(new []
        {
            area1,
            area2
        });
        account.Tags.AddRange(new []
        {
            new Tag {Name = "agendas"},
            new Tag {Name = "anywhere", Color = Color.Purple},
            new Tag {Name = "calls"},
            new Tag {Name = "computer"},
            new Tag {Name = "errands"},
            new Tag {Name = "home"},
            new Tag {Name = "office"},
        });
        account.Actions.AddRange(new []
        {
            new Action {
                Text = "Welcome to Planner",
                IsFocused = true,
                Notes = "notes", 
                TimeRequired = "10 minutes", 
                State = Action.ActionState.Next,
                Energy = Action.EnergyLevel.Middle,
                CreatedDate = DateTime.Now,
                DueDate = DateTime.Today + TimeSpan.FromDays(4),
                Areas = new Area[]
                {
                    area1, area2
                }
            }
        });
        account.Projects.AddRange(new []
        {
            new Project
            {
                Name = "My new project",
                Notes = "my project notes",
                // State = Project.ProjectState.Active,
                CreatedDate = DateTime.Now,
            }
        });
        await _db.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpGet]
    [Authorize]
    [Route("me")]
    public ActionResult MeAsync()
    {
        var user = User.Identity;

        if (user.IsAuthenticated)
            return Ok(new { name = user.Name, type = user.AuthenticationType });

        throw new Exception("identity is empty, but user is authorized");
    }
}