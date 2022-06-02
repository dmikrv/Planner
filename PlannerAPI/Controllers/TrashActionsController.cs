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
using PlannerAPI.Database;
using PlannerAPI.Database.Entities;
using PlannerAPI.Models;
using PlannerAPI.Models.Actions;

namespace PlannerAPI.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class TrashActionsController : ControllerBase
    {
        private readonly PlannerContext _db;
        private readonly UserManager<Account> _userManager;
        private readonly ILogger<TrashActionsController> _logger;
        private readonly IMapper _mapper;

        public TrashActionsController(ILogger<TrashActionsController> logger, UserManager<Account> userManager, PlannerContext db, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<TrashActionModel>> GetAllAsync(CancellationToken ct = default)
        {
            // TODO: don't work
            return await _db.TrashActions.Where(x => x.Account.UserName == User.Identity!.Name)
                .Include(x=> x.Account)
                .ProjectTo<TrashActionModel>(_mapper.ConfigurationProvider).ToArrayAsync(ct);
        }
        
        [HttpDelete]
        public async Task<ActionResult> DeleteAllAsync(CancellationToken ct = default)
        {
            var entities = _db.TrashActions.Where(x => x.Account.UserName == User.Identity!.Name);
            _db.TrashActions.RemoveRange(entities);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.TrashActions.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id, ct);
            
            if (entity is null || entity.Account.UserName != User.Identity!.Name)
                return NoContent();
            
            _db.TrashActions.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
