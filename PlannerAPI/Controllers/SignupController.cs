using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using PlannerAPI.Database;
using PlannerAPI.Database.Entities;
using PlannerAPI.Models;

using Action = PlannerAPI.Database.Entities.Action;

namespace PlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignupController : ControllerBase
{
    private readonly PlannerContext _db;
    private readonly ILogger<AccountsController> _logger;

    public SignupController(ILogger<AccountsController> logger, PlannerContext db)
    {
        _db = db;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> SignupAsync(RegistryModel accountModel)
    {
        // TODO: validation

        var account = new Account { Email = accountModel.Email };
        
        account.Areas.AddRange(new []
        {
            new Area {Name = "personal", Color = Color.Blue},
            new Area {Name = "work", Color = Color.Green},
        });
        account.Tags.AddRange(new []
        {
            new Tag {Name = "agendas"},
            new Tag {Name = "anywhere"},
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
                TimeRequired = TimeSpan.FromMinutes(10), 
                State = Action.ActionState.Next,
                Energy = Action.EnergyLevel.Middle,
                CreatedDate = DateTime.Now,
                DueDate = DateTime.Today + TimeSpan.FromDays(4),
                Areas = new List<Area>
                {
                    account.Areas.First(x => x.Name == "personal"),
                    account.Areas.First(x => x.Name == "work"),
                }
            }
        });
        account.Projects.AddRange(new []
        {
            new Project
            {
                Name = "My new project",
                Notes = "my project notes",
                State = Project.ProjectState.Active,
                CreatedDate = DateTime.Now,
            }
        });
        
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();
        
        return NoContent();
    }
}