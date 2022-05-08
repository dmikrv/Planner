using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using PlannerAPI.Entities;
using PlannerAPI.Models;
using Action = PlannerAPI.Entities.Action;

namespace PlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignupController : ControllerBase
{
    private readonly PlannerContext _db;
    private readonly ILogger<AccountsController> _logger;
    private readonly IMapper _mapper;

    public SignupController(ILogger<AccountsController> logger, PlannerContext db, IMapper mapper)
    {
        _db = db;
        _logger = logger;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> SignupAsync(RegistryModel accountModel)
    {
        // TODO: validation

        var account = new Account { Email = accountModel.Email };

        account.Areas.AddRange(new []
        {
            new Area {Name = "personal", ColorId = (int)Colors.BLUE},
            new Area {Name = "work", ColorId = (int)Colors.GREEN},
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
                StateId = (int)ActionStates.NEXT,
                EnergyId = (int)ActionEnergies.MIDDLE,
                CreatedDate = DateTime.Now,
                DueDate = DateTime.Today + TimeSpan.FromDays(4),
                // Tags = 
            }
        });
        account.Projects.AddRange(new []
        {
            new Project
            {
                Name = "My new project",
                Notes = "my project notes",
                StateId = (int)ProjectStates.ACTIVE,
                CreatedDate = DateTime.Now,
            }
        });
        
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();
        
        return NoContent();
    }
}