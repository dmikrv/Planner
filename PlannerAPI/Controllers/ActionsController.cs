using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IEnumerable<ActionModel>> GetAllAsync([FromQuery, Required]ActionModel.ActionStateModel state, CancellationToken ct = default)
        {
            return await _db.Actions.Where(x => x.Account.UserName == User.Identity!.Name)
                .Where(x => x.State == _mapper.Map<Action.ActionState>(state))
                .ProjectTo<ActionModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
        }
        
        [HttpGet]
        [Route("focus")]
        public async Task<IEnumerable<ActionModel>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Actions.Where(x => x.Account.UserName == User.Identity!.Name)
                .Where(x => x.IsFocused)
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
                .Include(x => x.WaitingContact)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return Forbid();
        
            return _mapper.Map<ActionModel>(entity);
        }
        
        [HttpPost]
        public async Task<ActionResult<ActionModel>> AddAsync(ActionModel model, CancellationToken ct = default)
        {
            var result = Validate(model);
            if (result is not null)
                return result;
            
            var entity = _mapper.Map<Action>(model);
            entity.Id = default;
            entity.CreatedDate = DateTime.Now;
            entity.Account = await _userManager.FindByNameAsync(User.Identity!.Name);

            await ConnectTrackingChildren(entity, ct);

            _db.Actions.Add(entity);
            await _db.SaveChangesAsync(ct);
        
            model.Id = entity.Id;
            return CreatedAtAction(nameof(GetAsync), new { id = model.Id }, model);
        }
        
        [HttpPut]
        public async Task<ActionResult<ActionModel>> UpdateAsync(ActionModel model, CancellationToken ct = default)
        {
            var result = Validate(model);
            if (result is not null)
                return result;

            var entity = await _db.Actions.AsNoTracking().Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == model.Id, ct);

            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return await AddAsync(model, ct);
            
            var newEntity = _mapper.Map<Action>(model);
            _db.Update(newEntity);
            newEntity.CreatedDate = entity.CreatedDate;
            newEntity.Account = await _userManager.FindByNameAsync(User.Identity!.Name);

            await ConnectTrackingChildren(newEntity, ct);
            await _db.SaveChangesAsync(ct);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Actions.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return NoContent();
        
            _db.TrashActions.Add(new TrashAction
            {
                Account = entity.Account,
                Name = entity.Text
            });
            
            _db.Actions.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        private async Task ConnectTrackingChildren(Action entity, CancellationToken ct = default)
        {
            var labelTags = await _db.Tags.AsNoTracking().Where(x => x.Account == entity.Account).ToArrayAsync(ct);
            var areaTags = await _db.Areas.AsNoTracking().Where(x => x.Account == entity.Account).ToArrayAsync(ct);
            var contactTags = await _db.Contacts.AsNoTracking().Where(x => x.Account == entity.Account).ToArrayAsync(ct);

            foreach (var tag in entity.Tags)
            {
                if (labelTags.Select(x => x.Id).Contains(tag.Id))
                {
                    _db.Update(tag);
                }
                else
                {
                    tag.Id = default;
                    _db.Add(tag);
                }
                tag.Account = entity.Account;
            }
            foreach (var area in entity.Areas)
            {
                if (areaTags.Select(x => x.Id).Contains(area.Id))
                {
                    _db.Update(area);
                }
                else
                {
                    area.Id = default;
                    _db.Add(area);
                }
                area.Account = entity.Account;
            }
            foreach (var contact in entity.Contacts)
            {
                if (contactTags.Select(x => x.Id).Contains(contact.Id))
                {
                    _db.Update(contact);
                }
                else
                {
                    contact.Id = default;
                    _db.Add(contact);
                }
                contact.Account = entity.Account;
            }
            
            // waiting contact
            if (entity.WaitingContact is not null)
            {
                entity.WaitingContact.Account = entity.Account;
                // if the contact id was not specified
                if (entity.WaitingContact.Id == default)
                {
                    _db.Add(entity.WaitingContact);
                }
                // if the contact will be added or update by this transaction
                else if (entity.Contacts.Select(x => x.Id).Contains(entity.WaitingContact.Id))
                {
                    entity.WaitingContact = entity.Contacts.First(x => x.Id == entity.WaitingContact.Id);
                }
                // if the contact was already in the database
                else if (contactTags.Select(x => x.Id).Contains(entity.WaitingContact.Id))
                {
                    _db.Update(entity.WaitingContact);
                }
            }
        }

        private ActionResult? Validate(ActionModel model)
        {
            // validation
            if (model.State == ActionModel.ActionStateModel.Scheduled)
            {
                if (model.ScheduledDate is null)
                    return BadRequest();
                if (model.WaitingContact is not null)
                    return BadRequest();
            }
            else if (model.State == ActionModel.ActionStateModel.Waiting)
            {
                if (model.WaitingContact is null)
                    return BadRequest();
                if (model.ScheduledDate is not null)
                    return BadRequest();
            }
            else
            {
                if (model.WaitingContact is not null)
                    return BadRequest();
                if (model.ScheduledDate is not null)
                    return BadRequest();
            }

            return null;
        }
    }
}
