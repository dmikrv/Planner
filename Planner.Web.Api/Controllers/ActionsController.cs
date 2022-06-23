using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner.Data;
using Planner.Data.Entities;
using Planner.Web.Api.Models;
using Action = Planner.Data.Entities.Action;

namespace Planner.Web.Api.Controllers
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
        public async Task<IEnumerable<ActionModel>> GetAllAsync(
            [FromQuery]ActionModel.ActionStateModel? state = null,
            [FromQuery]bool? done = null,
            [FromQuery]bool? focused = null,
            [FromQuery]long? projectId = null,
            CancellationToken ct = default)
        {
            var actions = _db.Actions.Include(x => x.Project)
                .Where(x => x.Account.UserName == User.Identity!.Name);

            if (state is not null)
                actions = actions.Where(x => x.State == _mapper.Map<Action.ActionState>(state));
            
            if (done is not null)
                actions = actions.Where(x => x.IsDone == done);

            if (focused is not null)
                actions = actions.Where(x => x.IsFocused == focused);
            
            if (projectId is not null)
            {
                var project = await _db.Projects.Where(x => x.Account.UserName == User.Identity!.Name)
                    .FirstOrDefaultAsync(x => x.Id == projectId, ct);
                if (project is null)
                    return Array.Empty<ActionModel>();
                
                actions = actions.Where(x => x.ProjectId == projectId);
            }
            
            return await actions.ProjectTo<ActionModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
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
            
            if (string.IsNullOrEmpty(model.Text))
                model.Text = "To do";

            if (model.DueDate is not null
                && DateTime.Parse(model.DueDate.Value.ToShortDateString()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
                model.DueDate = DateTime.Now;
        
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, [FromQuery]bool toTrashAction = true, CancellationToken ct = default)
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
            
            var includedIds = new List<long>();
            foreach (var tag in entity.Tags)
            {
                // // if several tags with the same id were sent
                // if (tag.Id != default && includedIds.Contains(tag.Id))
                // {
                //     entity.Tags.Remove(tag);
                //     continue;
                // }
                if (labelTags.Select(x => x.Id).Contains(tag.Id))
                {
                    includedIds.Add(tag.Id);
                    _db.Update(tag);
                }
                else
                {
                    tag.Id = default;
                    _db.Add(tag);
                }
                tag.Account = entity.Account;
            }

            includedIds = new List<long>();
            foreach (var area in entity.Areas)
            {
                // if several tags with the same id were sent
                // if (area.Id != default && includedIds.Contains(area.Id))
                // {
                //     entity.Areas.Remove(area);
                //     continue;
                // }
                if (areaTags.Select(x => x.Id).Contains(area.Id))
                {
                    includedIds.Add(area.Id);
                    _db.Update(area);
                }
                else
                {
                    area.Id = default;
                    _db.Add(area);
                }
                area.Account = entity.Account;
            }
            
            includedIds = new List<long>();
            foreach (var contact in entity.Contacts)
            {
                // if several tags with the same id were sent
                // if (contact.Id != default && includedIds.Contains(contact.Id))
                // {
                //     entity.Contacts.Remove(contact);
                //     continue;
                // }
                if (contactTags.Select(x => x.Id).Contains(contact.Id))
                {
                    includedIds.Add(contact.Id);
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
