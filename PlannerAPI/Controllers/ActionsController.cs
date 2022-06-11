using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Data.Entities;
using PlannerAPI.Models;
using Action = Planner.Data.Entities.Action;

namespace PlannerAPI.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly PlannerContext _db;
        private readonly UserManager<Account> _userManager;
        private readonly ILogger<ActionsController> _logger;
        private readonly IMapper _mapper;

        public ActionsController(ILogger<ActionsController> logger, UserManager<Account> userManager, PlannerContext db, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ActionModel>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Actions.Where(x => x.Account.UserName == User.Identity!.Name)
                .ProjectTo<ActionModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
        }
        
        [HttpGet("{id}")]
        [ActionName("GetAsync")]
        public async Task<ActionResult<ActionModel>> GetAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Actions
                .Include(x => x.Account)
                .Include(x => x.Tags)
                .Include(x => x.Contacts)
                .Include(x => x.Areas)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return Forbid();
        
            return _mapper.Map<ActionModel>(entity);
        }
        
        [HttpPost]
        public async Task<ActionResult<ActionModel>> AddAsync(ActionModel model, CancellationToken ct = default)
        {
            // TODO: validation 
            var entity = _mapper.Map<Action>(model);
            entity.Id = 0;
            entity.Account = await _userManager.FindByNameAsync(User.Identity!.Name);
            
            _db.Actions.Add(entity);
        
            // switch (entity.State)
            // {
            //     case Action.ActionState.Waiting:
            //         _db.WaitingActions.Add(new WaitingAction { Action = entity, Contact = _mapper.Map<Contact>(model.WaitingContact)});
            //         break;
            // }
            
            await _db.SaveChangesAsync(ct);
        
            model.Id = entity.Id;
            return CreatedAtAction(nameof(GetAsync), new { id = model.Id }, model);
        }
        
        // [HttpPut]
        // public async Task<ActionResult<LabelTagModel>> UpdateAsync(LabelTagModel model, CancellationToken ct = default)
        // {
        //     var entity = await _db.Tags.Include(x => x.Account)
        //         .FirstOrDefaultAsync(x => x.Id == model.Id, ct);
        //
        //     if (entity is null || entity.Account.UserName != User.Identity!.Name)
        //         return await AddAsync(model, ct);
        //
        //     entity.Name = model.Name;
        //     entity.Color = _mapper.Map<Color>(model.Color);
        //     
        //     _db.Tags.Update(entity);
        //     await _db.SaveChangesAsync(ct);
        //     
        //     return NoContent();
        // }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Actions.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return NoContent();
        
            _db.TrashActions.Add(new TrashAction
            {
                Account = await _userManager.FindByNameAsync(User.Identity!.Name),
                Name = entity.Text
            });
            
            _db.Actions.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
