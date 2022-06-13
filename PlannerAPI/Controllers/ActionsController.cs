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
using NuGet.Packaging;
using Planner.Data;
using Planner.Data.Entities;
using PlannerAPI.Models;
using Action = Planner.Data.Entities.Action;

namespace PlannerAPI.Controllers
{
    public static class TT
    {
        public static void TryUpdateManyToMany<T, TKey>(this DbContext db, IEnumerable<T> currentItems, IEnumerable<T> newItems, Func<T, TKey> getKey) where T : class { db.Set<T>().RemoveRange(currentItems.Except(newItems, getKey)); db.Set<T>().AddRange(newItems.Except(currentItems, getKey)); } public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> getKeyFunc) { return items .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems }) .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp }) .Where(t => ReferenceEquals(null, t.temp) || t.temp.Equals(default(T))) .Select(t => t.t.item); }

    } 

    
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
            var result = await Validate(model, ct);
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

        // [HttpPut]
        // public async Task<ActionResult<ActionModel>> UpdateAsync(ActionModel model, CancellationToken ct = default)
        // {
        //     var result = await Validate(model, ct);
        //     if (result is not null)
        //         return result;
        //     
        //     var entity = await _db.Actions.Include(x => x.Account)
        //         .Include(x => x.Tags)
        //         .Include(x => x.Areas)
        //         .Include(x => x.Contacts)
        //         .FirstOrDefaultAsync(x => x.Id == model.Id, ct);
        //     
        //     if (entity is not null && entity.Account.UserName == User.Identity!.Name)
        //         _db.Actions.Remove(entity);
        //     _db.Entry(entity).State = EntityState.Detached;
        //
        //     return await AddAsync(model, ct);
        //
        //
        //
        //     // var entity = await _db.Actions.Include(x => x.Account)
        //     //     .Include(x => x.Tags)
        //     //     .Include(x => x.Areas)
        //     //     .Include(x => x.Contacts)
        //     //     .FirstOrDefaultAsync(x => x.Id == model.Id, ct);
        //     //
        //     // // if (entity is null || entity.Account.UserName != User.Identity!.Name)
        //     // //     return await AddAsync(model, ct);
        //     //
        //     // if (!model.LabelTagIds.All(x => entity.Account.Tags.Select(y => y.Id).Contains(x)))
        //     //     return BadRequest("tag id is not your");
        //     //
        //     // entity.Text = model.Text;
        //     // entity.Notes = model.Notes;
        //     // entity.State = _mapper.Map<Action.ActionState>(model.State);
        //     // entity.IsDone = model.IsDone;
        //     // entity.IsFocused = model.IsFocused;
        //     // entity.TimeRequired = model.TimeRequired;
        //     // entity.Energy =_mapper.Map<Action.EnergyLevel>(model.Energy);
        //     // entity.DueDate = model.DueDate;
        //     // entity.Tags = await _db.Tags.Where(x => x.Account == entity.Account && model.LabelTagIds.Contains(x.Id)).ToArrayAsync(ct);
        //     // // entity.Areas.AddRange(_mapper.Map<Area[]>(model.AreaTags));
        //     // // entity.Contacts.AddRange(_mapper.Map<Contact[]>(model.ContactTags));
        //     // entity.ProjectId = model.ProjectId;
        //     // entity.ScheduledDate = model.ScheduledDate;
        //     // entity.WaitingContact = _mapper.Map<Contact>(model.WaitingContact);
        //     //
        //     // return NoContent();
        // }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, [FromQuery]bool toTrashAction = false, CancellationToken ct = default)
        {
            var entity = await _db.Actions.Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return NoContent();
            
            if (toTrashAction)
            {
                _db.TrashActions.Add(new TrashAction
                {
                    Account = entity.Account,
                    Name = entity.Text
                });
            }
            
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

        private async Task<ActionResult?> Validate(ActionModel model, CancellationToken ct = default)
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

            if (model.ProjectId is not null)
            {
                var project = await _db.Projects.AsNoTracking().Include(x => x.Account)
                    .FirstOrDefaultAsync(x => x.Id == model.ProjectId, ct);
                if (project is null || project.Account.UserName != User.Identity!.Name)
                    return BadRequest();
            }
            
            return null;
        }
    }
}
